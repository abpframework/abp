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
            return httpContext.Request.QueryString.HasValue
                ? Task.FromResult(httpContext.Request.Query[context.GetAbpAspNetCoreMultiTenancyOptions().TenantKey].ToString())
                : null;
        }
    }
}
