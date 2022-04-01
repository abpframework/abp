using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.OpenIddict;

internal static class AbpOpenIddictQueryableExtensions
{
    public static TQueryable SkipIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, int count)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? (TQueryable)query.Skip(count)
            : query;
    }

    public static TQueryable TakeIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, int count)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? (TQueryable)query.Take(count)
            : query;
    }
}
