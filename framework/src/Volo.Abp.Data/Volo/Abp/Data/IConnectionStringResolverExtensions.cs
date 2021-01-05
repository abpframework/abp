using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Data
{
    public static class ConnectionStringResolverExtensions
    {
        [NotNull]
        [Obsolete("Use ResolveAsync method")]
        public static string Resolve<T>(this IConnectionStringResolver resolver)
        {
            return resolver.Resolve(ConnectionStringNameAttribute.GetConnStringName<T>());
        }

        [NotNull]
        public static Task<string> ResolveAsync<T>(this IConnectionStringResolver resolver)
        {
            return resolver.ResolveAsync(ConnectionStringNameAttribute.GetConnStringName<T>());
        }
    }
}
