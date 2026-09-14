using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TSD.BulkExtensions.Metadata;

/// <summary>
/// Answers, for one entity type and one <see cref="BulkConfig"/>, which columns go to the staging table, which decide
/// whether a matched row changed, which are updated, and which the server generates. Built from EF Core metadata only;
/// contains no SQL and holds no per-operation state, so it is cached per model, CLR type, provider and the config
/// options that affect the shape.
/// </summary>
public sealed class EntityTableMap
{
    private static readonly ConditionalWeakTable<IModel, ConcurrentDictionary<string, EntityTableMap>> Cache = new();

    private EntityTableMap(IEntityType entityType, Type clrType, string? schema, string tableName, IReadOnlyList<ColumnMap> columns)
    {
        EntityType = entityType;
        ClrType = clrType;
        Schema = schema;
        TableName = tableName;
        Columns = columns;
        PrimaryKeyColumns = columns.Where(c => c.IsPrimaryKey).ToArray();
        MatchColumns = PrimaryKeyColumns;
        WriteColumns = CompareColumns = UpdateColumns = ReadColumns = ServerGeneratedColumns = DefaultValueColumns = Array.Empty<ColumnMap>();
    }

    /// <summary>The mapped EF entity type (the list's declared type, or the runtime type when the list is base-typed).</summary>
    public IEntityType EntityType { get; }

    /// <summary>CLR type the accessors were compiled against.</summary>
    public Type ClrType { get; }

    /// <summary>Unquoted schema, or <c>null</c> when the model has none (the provider then resolves the login's default schema, as EF does).</summary>
    public string? Schema { get; }

    /// <summary>Unquoted table name. Honours <see cref="BulkConfig.CustomDestinationTableName"/>.</summary>
    public string TableName { get; }

    /// <summary>Every mapped column, including ones excluded from the current operation. Model order.</summary>
    public IReadOnlyList<ColumnMap> Columns { get; }

    /// <summary>Primary key columns of the table.</summary>
    public IReadOnlyList<ColumnMap> PrimaryKeyColumns { get; }

    /// <summary>Columns rows are matched on: the primary key, or <see cref="BulkConfig.UpdateByProperties"/>.</summary>
    public IReadOnlyList<ColumnMap> MatchColumns { get; private set; }

    /// <summary>Server-assigned identity column, if the table has one.</summary>
    public ColumnMap? IdentityColumn { get; private set; }

    /// <summary>Rowversion / timestamp column, if any and not ignored.</summary>
    public ColumnMap? RowVersionColumn { get; private set; }

    /// <summary>TPH discriminator column, if the hierarchy uses one.</summary>
    public ColumnMap? DiscriminatorColumn { get; private set; }

    /// <summary>
    /// Columns copied to the staging table: match columns plus the include/exclude selection, without computed and
    /// rowversion columns. The identity column is present for every operation except a plain Insert without KeepIdentity,
    /// because Update/Upsert/Sync match on it. Use <see cref="GetInsertColumns{T}"/> for the INSERT column list.
    /// </summary>
    public IReadOnlyList<ColumnMap> WriteColumns { get; private set; }

    /// <summary>Columns whose difference makes a matched row count as changed.</summary>
    public IReadOnlyList<ColumnMap> CompareColumns { get; private set; }

    /// <summary>Columns assigned in the UPDATE branch. Never contains match or identity columns.</summary>
    public IReadOnlyList<ColumnMap> UpdateColumns { get; private set; }

    /// <summary>Columns a BulkRead copies into the entities: the include/exclude selection (keys included) that can be written back.</summary>
    public IReadOnlyList<ColumnMap> ReadColumns { get; private set; }

    /// <summary>Columns the server may generate: identity, server defaults, computed, rowversion. Read back when <see cref="BulkConfig.SetOutputIdentity"/> is set.</summary>
    public IReadOnlyList<ColumnMap> ServerGeneratedColumns { get; private set; }

    /// <summary>Write columns with a server default; skipped on insert when every entity leaves them at the CLR default.</summary>
    public IReadOnlyList<ColumnMap> DefaultValueColumns { get; private set; }

    /// <summary>
    /// True when the operation resolves to nothing (an Update whose include/exclude lists leave no column and
    /// <see cref="BulkConfig.IgnoreUnknownPropertyNames"/> is set); adapters return without touching the database.
    /// </summary>
    public bool IsNoOp { get; private set; }

    /// <summary>Whether the identity column is written explicitly (SqlBulkCopyOptions.KeepIdentity).</summary>
    public bool KeepIdentity { get; private set; }

    /// <summary>
    /// The columns an INSERT (or the insert branch of a merge) assigns for these particular entities: the write columns
    /// without the identity column (unless KeepIdentity), and without default-valued columns that no entity supplies a
    /// value for. Columns left to the server are also returned so the caller can read them back.
    /// </summary>
    public (IReadOnlyList<ColumnMap> Insert, IReadOnlyList<ColumnMap> LeftToServer) GetInsertColumns<T>(IList<T> entities, BulkValueContext values)
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(values);

        var leftToServer = new List<ColumnMap>();
        if (IdentityColumn is not null && !KeepIdentity && WriteColumns.Contains(IdentityColumn))
        {
            leftToServer.Add(IdentityColumn);
        }

        foreach (var column in DefaultValueColumns)
        {
            var allDefault = true;
            foreach (var entity in entities)
            {
                if (entity is not null && !column.IsDefaultProviderValue(column.GetProviderValue(entity, values)))
                {
                    allDefault = false;
                    break;
                }
            }

            if (allDefault)
            {
                leftToServer.Add(column);
            }
        }

        if (leftToServer.Count == 0)
        {
            return (WriteColumns, leftToServer);
        }

        return (WriteColumns.Where(c => !leftToServer.Contains(c)).ToArray(), leftToServer);
    }

    /// <summary>Finds a column by property path (case-insensitive).</summary>
    public ColumnMap? FindColumn(string propertyPath)
        => Columns.FirstOrDefault(c => string.Equals(c.PropertyPath, propertyPath, StringComparison.OrdinalIgnoreCase));

    /// <summary>Builds (or returns the cached) map for the entity type under the given options.</summary>
    /// <param name="context">Context whose model describes the entity.</param>
    /// <param name="clrType">Entity CLR type as mapped in the model.</param>
    /// <param name="config">Effective options; include/exclude lists and match keys shape the map.</param>
    /// <param name="operationType">Delete and Read only carry the match key; other operations carry all writable columns.</param>
    /// <param name="isIdentity">Provider-specific identity detection, normally <see cref="IBulkAdapter.IsIdentityColumn"/>.</param>
    public static EntityTableMap Create(DbContext context, Type clrType, BulkConfig config, OperationType operationType, Func<IProperty, bool> isIdentity)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(clrType);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(isIdentity);

        var model = context.Model;
        var perModel = Cache.GetValue(model, _ => new ConcurrentDictionary<string, EntityTableMap>(StringComparer.Ordinal));
        var key = BuildCacheKey(context, clrType, config, operationType);

        return perModel.GetOrAdd(key, _ => Build(context, clrType, config, operationType, isIdentity));
    }

    private static string BuildCacheKey(DbContext context, Type clrType, BulkConfig config, OperationType operationType)
    {
        static string Join(List<string>? list) => list is null ? "" : string.Join(",", list);

        return string.Join("|",
            context.Database.ProviderName,
            clrType.AssemblyQualifiedName,
            operationType,
            config.CustomDestinationTableName,
            Join(config.PropertiesToInclude), Join(config.PropertiesToExclude),
            Join(config.PropertiesToIncludeOnCompare), Join(config.PropertiesToExcludeOnCompare),
            Join(config.PropertiesToIncludeOnUpdate), Join(config.PropertiesToExcludeOnUpdate),
            Join(config.UpdateByProperties),
            config.EnableShadowProperties, config.IgnoreRowVersion, config.IgnoreUnknownPropertyNames,
            config.KeepIdentity);
    }

    private static EntityTableMap Build(DbContext context, Type clrType, BulkConfig config, OperationType operationType, Func<IProperty, bool> isIdentity)
    {
        var entityType = context.Model.FindRuntimeEntityType(clrType)
            ?? throw new InvalidOperationException($"DbContext does not contain an entity type for '{clrType.Name}'.");

        var (schema, tableName) = ResolveTable(entityType, config);
        var storeObject = StoreObjectIdentifier.Table(entityType.GetTableName()!, entityType.GetSchema());

        var columns = new List<ColumnMap>();
        AddColumns(entityType, entityType.ClrType, storeObject, config, isIdentity, navigationPath: Array.Empty<MemberInfo>(), pathPrefix: null, columns, seenColumns: new HashSet<string>(StringComparer.OrdinalIgnoreCase));

        var map = new EntityTableMap(entityType, clrType, schema, tableName, columns);
        map.IdentityColumn = columns.FirstOrDefault(c => c.IsIdentity);
        map.RowVersionColumn = config.IgnoreRowVersion ? null : columns.FirstOrDefault(c => c.IsRowVersion);
        map.DiscriminatorColumn = columns.FirstOrDefault(c => c.IsDiscriminator);
        map.KeepIdentity = config.KeepIdentity;

        map.MatchColumns = ResolveMatchColumns(map, config);
        map.SelectColumns(config, operationType);
        return map;
    }

    private static (string? Schema, string Table) ResolveTable(IEntityType entityType, BulkConfig config)
    {
        var table = entityType.GetTableName()
            ?? throw new InvalidOperationException($"Entity type '{entityType.DisplayName()}' is not mapped to a table.");
        var schema = entityType.GetSchema();

        if (string.IsNullOrWhiteSpace(config.CustomDestinationTableName))
        {
            return (schema, table);
        }

        var custom = config.CustomDestinationTableName!;
        var dot = custom.IndexOf('.', StringComparison.Ordinal);
        return dot < 0 ? (schema, custom) : (custom[..dot], custom[(dot + 1)..]);
    }

    private static void AddColumns(
        IEntityType entityType,
        Type ownerClrType,
        StoreObjectIdentifier storeObject,
        BulkConfig config,
        Func<IProperty, bool> isIdentity,
        IReadOnlyList<MemberInfo> navigationPath,
        string? pathPrefix,
        List<ColumnMap> columns,
        HashSet<string> seenColumns)
    {
        var discriminator = pathPrefix is null ? entityType.FindDiscriminatorProperty() : null;

        // Own properties, then properties declared only on derived types (base-typed lists of a TPH hierarchy).
        var properties = entityType.GetProperties().ToList();
        if (pathPrefix is null)
        {
            foreach (var derived in entityType.GetDerivedTypes())
            {
                foreach (var p in derived.GetDeclaredProperties())
                {
                    if (!properties.Contains(p))
                    {
                        properties.Add(p);
                    }
                }
            }
        }

        foreach (var property in properties)
        {
            var columnName = property.GetColumnName(storeObject);
            if (columnName is null || !seenColumns.Add(columnName))
            {
                continue; // not mapped to this table, or already added via another path
            }

            var isShadow = property.IsShadowProperty();
            var isDiscriminator = discriminator is not null && property == discriminator;
            if (isShadow && !isDiscriminator && !config.EnableShadowProperties)
            {
                seenColumns.Remove(columnName);
                continue;
            }

            if (pathPrefix is not null && property.IsPrimaryKey())
            {
                seenColumns.Remove(columnName);
                continue; // owned type's key mirrors the owner's key; the owner column already covers it
            }

            columns.Add(CreateColumn(property, ownerClrType, columnName, navigationPath, pathPrefix, isShadow, isDiscriminator, isIdentity));
        }

        // Owned types sharing the table are flattened into dotted paths.
        foreach (var navigation in entityType.GetNavigations())
        {
            var target = navigation.TargetEntityType;
            if (!target.IsOwned() || navigation.IsCollection)
            {
                continue;
            }

            if (target.GetTableName() != entityType.GetTableName() || target.GetSchema() != entityType.GetSchema())
            {
                continue; // owned type in its own table: its rows are not columns of this table
            }

            var member = (MemberInfo?)navigation.PropertyInfo ?? navigation.FieldInfo;
            if (member is null)
            {
                continue; // shadow navigation: nothing to read on the CLR object
            }

            var childPath = new List<MemberInfo>(navigationPath) { member };
            var childPrefix = pathPrefix is null ? navigation.Name : $"{pathPrefix}.{navigation.Name}";
            AddColumns(target, ownerClrType, storeObject, config, isIdentity, childPath, childPrefix, columns, seenColumns);
        }
    }

    private static ColumnMap CreateColumn(
        IProperty property,
        Type ownerClrType,
        string columnName,
        IReadOnlyList<MemberInfo> navigationPath,
        string? pathPrefix,
        bool isShadow,
        bool isDiscriminator,
        Func<IProperty, bool> isIdentity)
    {
        var path = pathPrefix is null ? property.Name : $"{pathPrefix}.{property.Name}";
        // HasConversion<TProvider>() materialises its converter on the type mapping, not on the property itself.
        var mapping = property.FindTypeMapping();
        var converter = property.GetValueConverter() ?? mapping?.Converter;
        var modelClrType = property.ClrType;
        var providerClrType = converter?.ProviderClrType ?? mapping?.ClrType ?? modelClrType;
        providerClrType = Nullable.GetUnderlyingType(providerClrType) ?? providerClrType;
        // A type mapping whose CLR type is still the enum means the provider handles it natively (Npgsql enum mapping);
        // without any mapping information the enum is sent as its underlying number.
        var nativeEnum = converter is null && mapping is not null && providerClrType.IsEnum;
        if (providerClrType.IsEnum && !nativeEnum)
        {
            providerClrType = Enum.GetUnderlyingType(providerClrType);
        }

        var flags = ColumnFlags.None;
        if (property.IsPrimaryKey()) flags |= ColumnFlags.PrimaryKey;
        if (isIdentity(property)) flags |= ColumnFlags.Identity;
        if (property.GetComputedColumnSql() is not null) flags |= ColumnFlags.Computed;
        if (property.IsConcurrencyToken && property.ValueGenerated == ValueGenerated.OnAddOrUpdate) flags |= ColumnFlags.RowVersion;
        if (isShadow) flags |= ColumnFlags.Shadow;
        if (isDiscriminator) flags |= ColumnFlags.Discriminator;
        if (property.IsNullable) flags |= ColumnFlags.Nullable;
        if (HasServerDefault(property)) flags |= ColumnFlags.ServerDefault;

        Func<object, BulkValueContext, object?> getter;
        Action<object, BulkValueContext, object?>? setter;

        if (isDiscriminator)
        {
            // Proxy types (lazy loading) derive from the mapped type; FindRuntimeEntityType walks up to it.
            var byRuntimeType = new ConcurrentDictionary<Type, object?>();
            getter = (entity, values) => byRuntimeType.GetOrAdd(entity.GetType(), t => values.Context.Model.FindRuntimeEntityType(t)?.GetDiscriminatorValue());
            setter = null;
        }
        else if (isShadow)
        {
            var name = property.Name;
            getter = (entity, values) => ToProvider(values.ShadowValue is not null ? values.ShadowValue(entity, name) : values.Context.Entry(entity).Property(name).CurrentValue, converter, nativeEnum);
            setter = (entity, values, value) => values.Context.Entry(entity).Property(name).CurrentValue = FromProvider(value, converter, modelClrType);
        }
        else
        {
            var member = (MemberInfo?)property.PropertyInfo ?? property.FieldInfo
                ?? throw new InvalidOperationException($"Property '{property.DeclaringType.DisplayName()}.{property.Name}' has no CLR member.");
            var memberPath = new List<MemberInfo>(navigationPath) { member };
            // A root-level property declared on a derived TPH type is compiled against that type; base instances read null.
            var declaringType = pathPrefix is null ? property.DeclaringType.ClrType : ownerClrType;

            var rawGetter = PropertyAccessor.CreateGetter(declaringType, memberPath);
            getter = (entity, _) => ToProvider(rawGetter(entity), converter, nativeEnum);

            if (PropertyAccessor.CanWrite(member))
            {
                var rawSetter = PropertyAccessor.CreateSetter(declaringType, memberPath);
                setter = (entity, _, value) => rawSetter(entity, FromProvider(value, converter, modelClrType));
            }
            else
            {
                setter = null;
            }
        }

        return new ColumnMap(property, path, columnName, property.GetColumnType(), modelClrType, providerClrType, converter, flags, getter, setter);
    }

    private static bool HasServerDefault(IProperty property)
    {
        if (property.GetDefaultValueSql() is not null)
        {
            return true;
        }

        // EF gives Guid properties an implicit default; only an explicit one counts.
        return property.GetDefaultValue() is not null
            && property.ValueGenerated != ValueGenerated.Never
            && property.ClrType != typeof(Guid);
    }

    private static object? ToProvider(object? value, ValueConverter? converter, bool nativeEnum)
    {
        if (value is null)
        {
            return null;
        }

        if (converter is not null)
        {
            return converter.ConvertToProvider(value);
        }

        // Enums normally carry a converter from the type mapping; without one they go as numbers unless the provider maps them natively.
        return value is Enum e && !nativeEnum
            ? Convert.ChangeType(e, Enum.GetUnderlyingType(e.GetType()), System.Globalization.CultureInfo.InvariantCulture)
            : value;
    }

    private static object? FromProvider(object? value, ValueConverter? converter, Type modelClrType)
    {
        if (value is null || value is DBNull)
        {
            return null;
        }

        if (converter is not null)
        {
            return converter.ConvertFromProvider(value);
        }

        var target = Nullable.GetUnderlyingType(modelClrType) ?? modelClrType;
        if (target.IsInstanceOfType(value))
        {
            return value;
        }

        return target.IsEnum
            ? Enum.ToObject(target, value)
            : Convert.ChangeType(value, target, System.Globalization.CultureInfo.InvariantCulture);
    }

    private static IReadOnlyList<ColumnMap> ResolveMatchColumns(EntityTableMap map, BulkConfig config)
    {
        if (config.UpdateByProperties is not { Count: > 0 })
        {
            return map.PrimaryKeyColumns;
        }

        var resolved = map.ResolveNamed(config.UpdateByProperties, nameof(BulkConfig.UpdateByProperties), config);
        return resolved.Count > 0 ? resolved : map.PrimaryKeyColumns;
    }

    private IReadOnlyList<ColumnMap> ResolveNamed(List<string> names, string optionName, BulkConfig config)
    {
        var result = new List<ColumnMap>(names.Count);
        var unknown = new List<string>();
        foreach (var name in names)
        {
            var column = FindColumn(name);
            if (column is null)
            {
                unknown.Add(name);
            }
            else if (!result.Contains(column))
            {
                result.Add(column);
            }
        }

        if (unknown.Count > 0 && !config.IgnoreUnknownPropertyNames)
        {
            throw new InvalidBulkConfigException(
                $"{optionName} names properties that are not mapped on '{EntityType.DisplayName()}': {string.Join(", ", unknown)}.");
        }

        return result;
    }

    private void SelectColumns(BulkConfig config, OperationType operationType)
    {
        var keys = MatchColumns;

        // The include/exclude selection, keys always kept; applies to writes and to what a BulkRead copies back.
        IEnumerable<ColumnMap> selected = Columns;
        if (config.PropertiesToInclude is { Count: > 0 })
        {
            var included = ResolveNamed(config.PropertiesToInclude, nameof(BulkConfig.PropertiesToInclude), config);
            selected = selected.Where(c => keys.Contains(c) || included.Contains(c) || c.IsDiscriminator);
        }
        else if (config.PropertiesToExclude is { Count: > 0 })
        {
            var excluded = ResolveNamed(config.PropertiesToExclude, nameof(BulkConfig.PropertiesToExclude), config);
            selected = selected.Where(c => keys.Contains(c) || !excluded.Contains(c));
        }

        var selectedList = selected.ToArray();
        ReadColumns = selectedList.Where(c => c.CanWriteBack).ToArray();

        // Delete and Read only need the key to find rows; everything else starts from the selection minus what cannot be written.
        // The identity column is the match key of Update/Upsert/Sync, so it must reach the staging table for those;
        // a plain Insert never sends it unless KeepIdentity is requested.
        var identityAllowed = KeepIdentity || operationType != OperationType.Insert;
        WriteColumns = operationType is OperationType.Delete or OperationType.Read
            ? keys
            : selectedList.Where(c => !c.IsComputed && !(c.IsRowVersion && !config.IgnoreRowVersion) && (!c.IsIdentity || identityAllowed)).ToArray();

        IEnumerable<ColumnMap> compare = WriteColumns.Where(c => !c.IsIdentity);
        if (config.PropertiesToIncludeOnCompare is { Count: > 0 })
        {
            var included = ResolveNamed(config.PropertiesToIncludeOnCompare, nameof(BulkConfig.PropertiesToIncludeOnCompare), config);
            compare = compare.Where(included.Contains);
        }
        else if (config.PropertiesToExcludeOnCompare is { Count: > 0 })
        {
            var excluded = ResolveNamed(config.PropertiesToExcludeOnCompare, nameof(BulkConfig.PropertiesToExcludeOnCompare), config);
            compare = compare.Where(c => !excluded.Contains(c));
        }
        CompareColumns = compare.ToArray();

        IEnumerable<ColumnMap> update = WriteColumns.Where(c => !c.IsIdentity && !keys.Contains(c) && !c.IsDiscriminator);
        if (config.PropertiesToIncludeOnUpdate is { Count: > 0 })
        {
            var included = ResolveNamed(config.PropertiesToIncludeOnUpdate, nameof(BulkConfig.PropertiesToIncludeOnUpdate), config);
            update = update.Where(included.Contains);
        }
        else if (config.PropertiesToExcludeOnUpdate is { Count: > 0 })
        {
            var excluded = ResolveNamed(config.PropertiesToExcludeOnUpdate, nameof(BulkConfig.PropertiesToExcludeOnUpdate), config);
            update = update.Where(c => !excluded.Contains(c));
        }
        UpdateColumns = update.ToArray();

        if (operationType == OperationType.Update && UpdateColumns.Count == 0)
        {
            // A graph pass whose type has none of the listed properties has nothing to do; a direct call is a configuration error.
            IsNoOp = config.IgnoreUnknownPropertyNames
                ? true
                : throw new InvalidBulkConfigException("BulkUpdate has no columns to update after applying the include/exclude options.");
        }

        // Non-key columns with a default, plus a Guid primary key the server fills (newsequentialid / gen_random_uuid).
        DefaultValueColumns = WriteColumns
            .Where(c => c.HasServerDefault && !c.IsIdentity)
            .Where(c => !keys.Contains(c) || (c.IsPrimaryKey && c.ProviderClrType == typeof(Guid)))
            .ToArray();

        ServerGeneratedColumns = Columns
            .Where(c => c.IsIdentity || c.IsComputed || c.IsRowVersion || c.HasServerDefault)
            .Where(c => c.CanWriteBack)
            .ToArray();
    }
}
