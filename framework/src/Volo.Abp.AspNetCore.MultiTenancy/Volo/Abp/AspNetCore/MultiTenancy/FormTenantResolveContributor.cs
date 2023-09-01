using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy;

[Obsolete("This may make some features of ASP NET Core unavailable, Will be removed in future versions.")]
public class FormTenantResolveContributor : HttpTenantResolveContributorBase
{
    public const string ContributorName = "Form";

    public override string Name => ContributorName;

    protected override async Task<string?> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
    {
        if (!httpContext.Request.HasFormContentType)
        {
            return null;
        }

        var form = await httpContext.Request.ReadFormAsync();
        return form[context.GetAbpAspNetCoreMultiTenancyOptions().TenantKey];
    }
}
