using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Volo.Abp.Http.Client.Web.Conventions;

public class AbpHttpClientProxyControllerFeatureProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        return AbpHttpClientProxyHelper.IsClientProxyService(typeInfo);
    }
}
