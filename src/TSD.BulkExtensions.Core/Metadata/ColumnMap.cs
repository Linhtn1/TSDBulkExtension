using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TSD.BulkExtensions.Metadata;

/// <summary>
/// One table column of an <see cref="EntityTableMap"/>: where it lives in the database, which CLR property feeds it,
/// and how to move a value between the two (value converter, enum, shadow property, discriminator).
/// </summary>
public sealed class ColumnMap
{
    private readonly Func<object, BulkValueContext, object?> _getter;
    private readonly Action<object, BulkValueContext, object?>? _setter;
    private readonly object? _providerDefault;

    internal ColumnMap(
        IProperty property,
        string propertyPath,
        string columnName,
        string storeType,
        Type modelClrType,
        Type providerClrType,
        ValueConverter? converter,
        ColumnFlags flags,
        Func<object, BulkValueContext, object?> getter,
        Action<object, BulkValueContext, object?>? setter)
    {
        Property = property;
        PropertyPath = propertyPath;
        ColumnName = columnName;
        StoreType = storeType;
        ModelClrType = modelClrType;
        ProviderClrType = providerClrType;
        Converter = converter;
        Flags = flags;
        _getter = getter;
        _setter = setter;
        _providerDefault = providerClrType.IsValueType ? Activator.CreateInstance(providerClrType) : null;
    }

    /// <summary>The EF property this column is mapped from.</summary>
    public IProperty Property { get; }

    /// <summary>CLR property path from the entity, dotted for owned types (<c>Address.City</c>). Shadow properties use the EF name.</summary>
    public string PropertyPath { get; }

    /// <summary>Unquoted column name in the table.</summary>
    public string ColumnName { get; }

    /// <summary>Store type as EF reports it, for example <c>nvarchar(200)</c> or <c>numeric(18,2)</c>.</summary>
    public string StoreType { get; }

    /// <summary>CLR type of the property (nullable kept).</summary>
    public Type ModelClrType { get; }

    /// <summary>CLR type the database driver receives after conversion, with <see cref="Nullable{T}"/> unwrapped.</summary>
    public Type ProviderClrType { get; }

    /// <summary>Value converter in effect for the property (explicit or from the type mapping), if any.</summary>
    public ValueConverter? Converter { get; }

    /// <summary>Characteristics that decide whether the column is written, compared, updated or read back.</summary>
    public ColumnFlags Flags { get; }

    /// <summary>Part of the table's primary key.</summary>
    public bool IsPrimaryKey => Flags.HasFlag(ColumnFlags.PrimaryKey);

    /// <summary>Server-assigned identity/serial column.</summary>
    public bool IsIdentity => Flags.HasFlag(ColumnFlags.Identity);

    /// <summary>Computed column; never written.</summary>
    public bool IsComputed => Flags.HasFlag(ColumnFlags.Computed);

    /// <summary>Rowversion / timestamp concurrency column maintained by the server.</summary>
    public bool IsRowVersion => Flags.HasFlag(ColumnFlags.RowVersion);

    /// <summary>EF shadow property without a CLR member.</summary>
    public bool IsShadow => Flags.HasFlag(ColumnFlags.Shadow);

    /// <summary>TPH discriminator column; value derived from the entity's runtime type.</summary>
    public bool IsDiscriminator => Flags.HasFlag(ColumnFlags.Discriminator);

    /// <summary>Column has a server-side default and may be left to the server on insert.</summary>
    public bool HasServerDefault => Flags.HasFlag(ColumnFlags.ServerDefault);

    /// <summary>Column accepts NULL.</summary>
    public bool IsNullable => Flags.HasFlag(ColumnFlags.Nullable);

    /// <summary>Whether a generated value can be written back into the entity after the operation.</summary>
    public bool CanWriteBack => _setter is not null;

    /// <summary>Reads the property and converts it to the provider representation. <c>null</c> stands for SQL NULL.</summary>
    public object? GetProviderValue(object entity, BulkValueContext values) => _getter(entity, values);

    /// <summary>Converts a provider value back to the model type and assigns it to the entity.</summary>
    public void SetFromProviderValue(object entity, BulkValueContext values, object? providerValue)
    {
        if (_setter is null)
        {
            throw new InvalidOperationException($"Column '{ColumnName}' ({PropertyPath}) cannot be written back to the entity.");
        }

        _setter(entity, values, providerValue);
    }

    /// <summary>True when the value equals the CLR default of the provider type (or is null), meaning "let the server fill it".</summary>
    public bool IsDefaultProviderValue(object? providerValue)
    {
        if (providerValue is null || providerValue is DBNull)
        {
            return true;
        }

        return _providerDefault is not null && Equals(providerValue, _providerDefault);
    }

    /// <inheritdoc />
    public override string ToString() => $"{PropertyPath} -> [{ColumnName}] {StoreType}";
}

/// <summary>Column characteristics. Combinable.</summary>
[Flags]
public enum ColumnFlags
{
    /// <summary>Plain column.</summary>
    None = 0,

    /// <summary>Part of the primary key.</summary>
    PrimaryKey = 1,

    /// <summary>Server-assigned identity/serial.</summary>
    Identity = 2,

    /// <summary>Computed column.</summary>
    Computed = 4,

    /// <summary>Rowversion / timestamp.</summary>
    RowVersion = 8,

    /// <summary>EF shadow property.</summary>
    Shadow = 16,

    /// <summary>TPH discriminator.</summary>
    Discriminator = 32,

    /// <summary>Has a server-side default.</summary>
    ServerDefault = 64,

    /// <summary>Accepts NULL.</summary>
    Nullable = 128,
}
