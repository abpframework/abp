using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Reflection;

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
            var type = typeInfo.AsType();

            if (!typeof(IRemoteService).IsAssignableFrom(type) ||
                !typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
            {
                return false;
            }

            var remoteServiceAttr = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(typeInfo);

            if (remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type))
            {
                return false;
            }

            //TODO: Move this to a lazy loaded field for efficiency.
            var configuration = _application.ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>().Value.AppServiceControllers.ControllerAssemblySettings.GetSettingOrNull(type);
            return configuration != null && configuration.TypePredicate(type);
        }
    }
}