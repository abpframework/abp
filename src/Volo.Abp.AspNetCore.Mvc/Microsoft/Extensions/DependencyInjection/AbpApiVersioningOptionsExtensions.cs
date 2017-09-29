using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Volo.Abp.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpApiVersioningOptionsExtensions
    {
        public static void AddAbpModules(this ApiVersioningOptions options, IServiceCollection services)
        {
            services.Configure<AbpAspNetCoreMvcOptions>(op =>
            {
                foreach (var setting in op.AppServiceControllers.ControllerAssemblySettings)
                {
                    foreach (var controllerType in setting.ControllerTypes)
                    {
                        var controllerBuilder = typeof(ApiVersionConventionBuilder)
                            .GetMethod(nameof(ApiVersionConventionBuilder.Controller), BindingFlags.Instance | BindingFlags.Public)
                            .MakeGenericMethod(controllerType)
                            .Invoke(options.Conventions, null);

                        typeof(ControllerApiVersionConventionBuilder<>)
                            .MakeGenericType(controllerType)
                            .GetMethod("HasApiVersion")
                            .Invoke(controllerBuilder, new object[] { setting.ApiVersion });
                    }
                }
            });
        }
    }
}
