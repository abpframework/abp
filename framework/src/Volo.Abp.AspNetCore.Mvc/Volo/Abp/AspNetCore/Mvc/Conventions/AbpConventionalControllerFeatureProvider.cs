using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.Conventions
{
    public class AbpConventionalControllerFeatureProvider : ControllerFeatureProvider
    {
        private readonly IAbpApplication _application;

        public AbpConventionalControllerFeatureProvider(IAbpApplication application)
        {
            _application = application;
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            //TODO: Move this to a lazy loaded field for efficiency.
            if (_application.ServiceProvider == null)
            {
                return false;
            }

            var configuration = _application.ServiceProvider
                .GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>().Value
                .ConventionalControllers
                .ConventionalControllerSettings
                .GetSettingOrNull(typeInfo.AsType());

            return configuration != null;
        }
    }
}