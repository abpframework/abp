using System;
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
            if (httpContext.Request.QueryString.HasValue)
            {
                var tenantKey = context.GetAbpAspNetCoreMultiTenancyOptions().TenantKey;
                if (httpContext.Request.Query.ContainsKey(tenantKey))
                {
                    var tenantValue = httpContext.Request.Query[tenantKey].ToString();
                    if (tenantValue.IsNullOrWhiteSpace())
                    {
                        context.Handled = true;
                        return Task.FromResult<string>(null);
                    }

                    return Task.FromResult(tenantValue);
                }
            }

            return Task.FromResult<string>(null);
        }
    }
}
