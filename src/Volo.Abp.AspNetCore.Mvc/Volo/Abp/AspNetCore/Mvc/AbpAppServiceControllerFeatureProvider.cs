using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpAppServiceControllerFeatureProvider : ControllerFeatureProvider
    {
        private readonly IAbpApplication _application;

        public AbpAppServiceControllerFeatureProvider(IAbpApplication application)
        {
            _application = application;
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            //TODO: Move this to a lazy loaded field for efficiency.
            var configuration = _application.ServiceProvider
                .GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>().Value
                .AppServiceControllers
                .ControllerAssemblySettings
                .GetSettingOrNull(typeInfo.AsType());

            return configuration != null;
        }
    }
}