using System.Linq;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;

namespace Volo.Abp.OpenIddict;

public static class AbpOpenIddictEfCoreQueryableExtensions
{
    public static IQueryable<OpenIddictApplication> IncludeDetails(this IQueryable<OpenIddictApplication> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Authorizations).ThenInclude(x => x.Tokens)
            .Include(x => x.Tokens);
    }

    public static IQueryable<OpenIddictAuthorization> IncludeDetails(this IQueryable<OpenIddictAuthorization> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable.Include(x => x.Tokens);
    }
}
