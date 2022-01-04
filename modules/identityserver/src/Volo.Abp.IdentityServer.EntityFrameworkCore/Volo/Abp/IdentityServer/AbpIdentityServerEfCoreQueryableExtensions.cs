using System.Linq;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer;

public static class AbpIdentityServerEfCoreQueryableExtensions
{
    public static IQueryable<ApiResource> IncludeDetails(this IQueryable<ApiResource> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Secrets)
            .Include(x => x.UserClaims)
            .Include(x => x.Scopes)
            .Include(x => x.Properties);
    }

    public static IQueryable<ApiScope> IncludeDetails(this IQueryable<ApiScope> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.UserClaims)
            .Include(x => x.Properties);
    }


    public static IQueryable<IdentityResource> IncludeDetails(this IQueryable<IdentityResource> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.UserClaims)
            .Include(x => x.Properties);
    }

    public static IQueryable<Client> IncludeDetails(this IQueryable<Client> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.AllowedGrantTypes)
            .Include(x => x.RedirectUris)
            .Include(x => x.PostLogoutRedirectUris)
            .Include(x => x.AllowedScopes)
            .Include(x => x.ClientSecrets)
            .Include(x => x.Claims)
            .Include(x => x.IdentityProviderRestrictions)
            .Include(x => x.AllowedCorsOrigins)
            .Include(x => x.Properties);
    }
}
