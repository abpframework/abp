using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Data;

public static class ConnectionStringResolverExtensions
{
    [NotNull]
    [Obsolete("Use ResolveAsync method")]
    public static string Resolve<T>(this IConnectionStringResolver resolver)
    {
        return resolver.Resolve(typeof(T));
    }

    [NotNull]
    public static Task<string> ResolveAsync<T>(this IConnectionStringResolver resolver)
    {
        return resolver.ResolveAsync(typeof(T));
    }

    [NotNull]
    [Obsolete("Use ResolveAsync method")]
    public static string Resolve(this IConnectionStringResolver resolver, Type type)
    {
        return resolver.Resolve(ConnectionStringNameAttribute.GetConnStringName(type));
    }

    [NotNull]
    public static Task<string> ResolveAsync(this IConnectionStringResolver resolver, Type type)
    {
        return resolver.ResolveAsync(ConnectionStringNameAttribute.GetConnStringName(type));
    }
}
