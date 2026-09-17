using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Public.Web.Pages.Public;

public abstract class CmsKitDynamicRouteValueTransformerBase : DynamicRouteValueTransformer, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }
    protected ITenantConfigurationProvider TenantConfigurationProvider { get; }

    protected CmsKitDynamicRouteValueTransformerBase(ICurrentTenant currentTenant, ITenantConfigurationProvider tenantConfigurationProvider)
    {
        CurrentTenant = currentTenant;
        TenantConfigurationProvider = tenantConfigurationProvider;
    }

    public async override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        TenantConfiguration tenant = null;
        try
        {
            tenant = await TenantConfigurationProvider.GetAsync(saveResolveResult: false);
        }
        catch (Exception)
        {
            //Ignore
        }

        using (CurrentTenant.Change(tenant?.Id, tenant?.Name))
        {
            return await DoTransformAsync(httpContext, values);
        }
    }

    protected abstract ValueTask<RouteValueDictionary> DoTransformAsync(HttpContext httpContext, RouteValueDictionary values);
}
