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
            return Task.FromResult(httpContext.Request.QueryString.HasValue
                ? httpContext.Request.Query[context.GetAbpAspNetCoreMultiTenancyOptions().TenantKey].ToString()
                : null);
        }
    }
}
