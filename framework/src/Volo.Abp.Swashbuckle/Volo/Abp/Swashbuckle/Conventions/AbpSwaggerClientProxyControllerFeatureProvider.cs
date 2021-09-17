using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Volo.Abp.Swashbuckle.Conventions
{
    public class AbpSwaggerClientProxyControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            return AbpSwaggerClientProxyHelper.IsClientProxyService(typeInfo);
        }
    }
}
