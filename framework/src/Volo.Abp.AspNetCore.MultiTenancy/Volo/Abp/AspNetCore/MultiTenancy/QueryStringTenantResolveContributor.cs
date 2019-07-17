using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class QueryStringTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "QueryString";

        public override string Name => ContributorName;

        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            if (httpContext.Request == null || !httpContext.Request.QueryString.HasValue)
            {
                return null;
            }

            var tenantIdOrName = httpContext.Request.Query[context.GetAspNetCoreMultiTenancyOptions().TenantKey];
            if (!tenantIdOrName.IsNullOrEmpty())
            {
                return null;
            }
            return WebUtility.UrlDecode(tenantIdOrName);
        }
    }
}
