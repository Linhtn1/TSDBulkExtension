using System.Linq.Expressions;
using System.Reflection;

namespace TSD.BulkExtensions.Metadata;

/// <summary>
/// Compiled getters and setters for a member path such as <c>Name</c> or <c>Address.City</c>, built from the
/// <see cref="MemberInfo"/>s EF mapped (so private setters on base classes and field-backed properties work).
/// Reflection is paid once per column, not once per row.
/// </summary>
internal static class PropertyAccessor
{
    /// <summary>
    /// Builds <c>entity => (object?)((TDeclaring)entity)?.A?.B</c>. Returns <c>null</c> when the entity is not an instance of
    /// <paramref name="declaringType"/> (derived-type property on a base-typed list) or when any segment is null.
    /// </summary>
    public static Func<object, object?> CreateGetter(Type declaringType, IReadOnlyList<MemberInfo> path)
    {
        ArgumentNullException.ThrowIfNull(declaringType);
        ArgumentNullException.ThrowIfNull(path);

        var entity = Expression.Parameter(typeof(object), "entity");
        Expression typed = declaringType.IsValueType ? Expression.Convert(entity, declaringType) : Expression.TypeAs(entity, declaringType);
        var body = BuildNullSafeAccess(typed, path, 0);

        return Expression.Lambda<Func<object, object?>>(body, entity).Compile();
    }

    private static Expression BuildNullSafeAccess(Expression instance, IReadOnlyList<MemberInfo> path, int index)
    {
        var access = Expression.MakeMemberAccess(instance, path[index]);
        Expression next = index == path.Count - 1
            ? Expression.Convert(access, typeof(object))
            : BuildNullSafeAccess(access, path, index + 1);

        if (!instance.Type.IsValueType || Nullable.GetUnderlyingType(instance.Type) is not null)
        {
            return Expression.Condition(
                Expression.Equal(instance, Expression.Constant(null, instance.Type)),
                Expression.Constant(null, typeof(object)),
                next);
        }

        return next;
    }

    /// <summary>
    /// Builds <c>(entity, value) => { if (entity is TDeclaring d) d.A.B = (TProp)value; }</c>. Entities that are not instances
    /// of <paramref name="declaringType"/> are skipped; a null intermediate throws, since writing a generated value into a
    /// missing owned instance is a caller bug.
    /// </summary>
    public static Action<object, object?> CreateSetter(Type declaringType, IReadOnlyList<MemberInfo> path)
    {
        ArgumentNullException.ThrowIfNull(declaringType);
        ArgumentNullException.ThrowIfNull(path);

        var entity = Expression.Parameter(typeof(object), "entity");
        var value = Expression.Parameter(typeof(object), "value");
        var typed = Expression.Variable(declaringType, "typed");

        Expression instance = typed;
        for (var i = 0; i < path.Count - 1; i++)
        {
            instance = Expression.MakeMemberAccess(instance, path[i]);
        }

        var last = path[^1];
        Expression assign = last switch
        {
            PropertyInfo property => Expression.Call(
                instance,
                property.GetSetMethod(nonPublic: true) ?? throw new InvalidOperationException($"Property '{property.DeclaringType?.Name}.{property.Name}' has no setter."),
                Expression.Convert(value, property.PropertyType)),
            FieldInfo field => Expression.Assign(Expression.Field(instance, field), Expression.Convert(value, field.FieldType)),
            _ => throw new InvalidOperationException($"Member '{last.Name}' is neither a property nor a field."),
        };

        Expression body;
        if (declaringType.IsValueType)
        {
            body = Expression.Block(new[] { typed }, Expression.Assign(typed, Expression.Convert(entity, declaringType)), assign);
        }
        else
        {
            body = Expression.Block(
                new[] { typed },
                Expression.Assign(typed, Expression.TypeAs(entity, declaringType)),
                Expression.IfThen(Expression.NotEqual(typed, Expression.Constant(null, declaringType)), assign));
        }

        return Expression.Lambda<Action<object, object?>>(body, entity, value).Compile();
    }

    /// <summary>Whether the last member of the path can be assigned (any setter, including private, or a non-readonly field).</summary>
    public static bool CanWrite(MemberInfo member) => member switch
    {
        PropertyInfo property => property.GetSetMethod(nonPublic: true) is not null,
        FieldInfo field => !field.IsInitOnly,
        _ => false,
    };
}
