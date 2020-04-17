using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class CookieTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "Cookie";

        public override string Name => ContributorName;

        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            return httpContext.Request?.Cookies[context.GetAbpAspNetCoreMultiTenancyOptions().TenantKey];
        }
    }
}