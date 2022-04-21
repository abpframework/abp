using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.OpenIddict;

public static class AbpOpenIddictQueryableExtensions
{
    public static TQueryable SkipIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, int? count)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        if (condition && count.HasValue)
        {
            return (TQueryable)query.Skip(count.Value);
        }

        return query;
    }

    public static TQueryable TakeIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, int? count)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        if (condition && count.HasValue)
        {
            return (TQueryable)query.Take(count.Value);
        }

        return query;
    }
}
