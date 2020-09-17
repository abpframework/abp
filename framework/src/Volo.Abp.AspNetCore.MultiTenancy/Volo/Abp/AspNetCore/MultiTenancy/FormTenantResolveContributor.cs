using System.Linq;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class FormTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "Form";

        public override string Name => ContributorName;

        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            if (httpContext.Request == null || !httpContext.Request.HasFormContentType || !httpContext.Request.Form.Any())
            {
                return null;
            }

            return httpContext.Request.Form[context.GetAbpAspNetCoreMultiTenancyOptions().TenantKey];
        }
    }
}
