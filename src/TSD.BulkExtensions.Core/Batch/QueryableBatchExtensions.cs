using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
#if NET8_0_OR_GREATER
using Microsoft.EntityFrameworkCore.Query;
#endif

namespace TSD.BulkExtensions;

/// <summary>
/// Set-based DELETE / UPDATE from a LINQ query, with the EFCore.BulkExtensions method names.
/// On EF Core 8 and later these are thin wrappers over <c>ExecuteDelete</c> / <c>ExecuteUpdate</c>. EF Core 6 has no
/// equivalent, so on the net6.0 build the methods are marked obsolete-as-error: a consumer that still calls them fails
/// at compile time with a pointer to the ABP EFPlus repository methods.
/// </summary>
public static class QueryableBatchExtensions
{
#if !NET8_0_OR_GREATER
    private const string NotOnEf6 = "Batch operations on IQueryable need EF Core 8 or later (ExecuteDelete/ExecuteUpdate). " +
                                    "On EF Core 6 use the ABP EFPlus repository methods, e.g. repository.BatchDeleteAsync(predicate).";
#endif

    /// <summary>Deletes every row the query selects. Returns the number of rows affected.</summary>
#if !NET8_0_OR_GREATER
    [Obsolete(NotOnEf6, error: true)]
#endif
    public static int BatchDelete<T>(this IQueryable<T> query) where T : class
    {
        ArgumentNullException.ThrowIfNull(query);
#if NET8_0_OR_GREATER
        return query.ExecuteDelete();
#else
        throw new NotSupportedException(NotOnEf6);
#endif
    }

    /// <summary>Deletes every row the query selects. Returns the number of rows affected.</summary>
#if !NET8_0_OR_GREATER
    [Obsolete(NotOnEf6, error: true)]
#endif
    public static Task<int> BatchDeleteAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(query);
#if NET8_0_OR_GREATER
        return query.ExecuteDeleteAsync(cancellationToken);
#else
        throw new NotSupportedException(NotOnEf6);
#endif
    }

    /// <summary>
    /// Updates every row the query selects. <paramref name="updateExpression"/> is a member initialiser such as
    /// <c>x =&gt; new Item { Quantity = x.Quantity + 1, Status = ItemStatus.Archived }</c>; only the assigned members change.
    /// </summary>
#if !NET8_0_OR_GREATER
    [Obsolete(NotOnEf6, error: true)]
#endif
    public static int BatchUpdate<T>(this IQueryable<T> query, Expression<Func<T, T>> updateExpression) where T : class
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(updateExpression);
#if NET10_0_OR_GREATER
        var setters = ParseSetters(updateExpression);
        return query.ExecuteUpdate(builder => Apply(builder, setters));
#elif NET8_0_OR_GREATER
        return query.ExecuteUpdate(ToSetPropertyCalls(updateExpression));
#else
        throw new NotSupportedException(NotOnEf6);
#endif
    }

    /// <summary>
    /// Updates every row the query selects. <paramref name="updateExpression"/> is a member initialiser such as
    /// <c>x =&gt; new Item { Quantity = x.Quantity + 1, Status = ItemStatus.Archived }</c>; only the assigned members change.
    /// </summary>
#if !NET8_0_OR_GREATER
    [Obsolete(NotOnEf6, error: true)]
#endif
    public static Task<int> BatchUpdateAsync<T>(this IQueryable<T> query, Expression<Func<T, T>> updateExpression, CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(updateExpression);
#if NET10_0_OR_GREATER
        var setters = ParseSetters(updateExpression);
        return query.ExecuteUpdateAsync(builder => Apply(builder, setters), cancellationToken);
#elif NET8_0_OR_GREATER
        return query.ExecuteUpdateAsync(ToSetPropertyCalls(updateExpression), cancellationToken);
#else
        throw new NotSupportedException(NotOnEf6);
#endif
    }

#if NET8_0_OR_GREATER
    /// <summary>One <c>Property = value</c> assignment of the member initialiser, both sides as lambdas over the source parameter.</summary>
    internal readonly record struct Setter(Type PropertyType, LambdaExpression Property, LambdaExpression Value);

    /// <summary>Splits <c>x =&gt; new T { A = a(x), B = b(x) }</c> into <c>(x =&gt; x.A, x =&gt; a(x))</c>, <c>(x =&gt; x.B, x =&gt; b(x))</c>.</summary>
    internal static List<Setter> ParseSetters<T>(Expression<Func<T, T>> updateExpression)
    {
        if (updateExpression.Body is not MemberInitExpression init)
        {
            throw new ArgumentException("The update expression must be a member initialiser: x => new T { Property = value, ... }.", nameof(updateExpression));
        }

        var source = updateExpression.Parameters[0];
        var setters = new List<Setter>(init.Bindings.Count);
        foreach (var binding in init.Bindings)
        {
            if (binding is not MemberAssignment assignment)
            {
                throw new ArgumentException($"Only direct assignments are supported in the update expression ('{binding.Member.Name}' is not).", nameof(updateExpression));
            }

            var propertyType = assignment.Member is PropertyInfo p ? p.PropertyType : ((FieldInfo)assignment.Member).FieldType;
            var property = Expression.Lambda(Expression.MakeMemberAccess(source, assignment.Member), source);
            var value = Expression.Lambda(assignment.Expression, source);
            setters.Add(new Setter(propertyType, property, value));
        }

        return setters;
    }
#endif

#if NET10_0_OR_GREATER
    /// <summary>Typed call site we own, so overload resolution against EF happens at compile time; reflection only closes the generic arguments.</summary>
    private static void Set<TSource, TProperty>(UpdateSettersBuilder<TSource> builder, Expression<Func<TSource, TProperty>> property, Expression<Func<TSource, TProperty>> value)
        => builder.SetProperty(property, value);

    private static readonly MethodInfo SetDefinition = typeof(QueryableBatchExtensions)
        .GetMethod(nameof(Set), BindingFlags.NonPublic | BindingFlags.Static)!;

    private static void Apply<T>(UpdateSettersBuilder<T> builder, List<Setter> setters)
    {
        foreach (var setter in setters)
        {
            // Expression.Lambda(body, x) already produced Expression<Func<T, TProperty>>, which is what Set takes.
            SetDefinition.MakeGenericMethod(typeof(T), setter.PropertyType).Invoke(null, new object[] { builder, setter.Property, setter.Value });
        }
    }
#elif NET8_0_OR_GREATER
    /// <summary>Builds <c>s =&gt; s.SetProperty(x =&gt; x.A, x =&gt; a(x)).SetProperty(...)</c> for EF Core 8's expression-based ExecuteUpdate.</summary>
    internal static Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> ToSetPropertyCalls<T>(Expression<Func<T, T>> updateExpression)
    {
        var settersParameter = Expression.Parameter(typeof(SetPropertyCalls<T>), "setters");
        Expression chain = settersParameter;

        var setProperty = typeof(SetPropertyCalls<T>).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(m => m.Name == nameof(SetPropertyCalls<T>.SetProperty)
                         && m.GetParameters()[1].ParameterType.IsGenericType
                         && m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>));

        foreach (var setter in ParseSetters(updateExpression))
        {
            chain = Expression.Call(chain, setProperty.MakeGenericMethod(setter.PropertyType), setter.Property, setter.Value);
        }

        return Expression.Lambda<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>>(chain, settersParameter);
    }
#endif
}
