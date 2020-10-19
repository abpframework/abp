using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class QueryStringTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "QueryString";

        public override string Name => ContributorName;

        protected override Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantKey = context.GetAbpAspNetCoreMultiTenancyOptions().TenantKey;
            if (!httpContext.Request.QueryString.HasValue)
            {
                return null;
            }

            if (httpContext.Request.Query.TryGetValue(tenantKey, out var tenant))
            {
                Task.FromResult(tenant);
            }

            return null;
        }
    }
}
