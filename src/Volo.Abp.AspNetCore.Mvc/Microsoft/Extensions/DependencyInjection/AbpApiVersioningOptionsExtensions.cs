using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Volo.Abp.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpApiVersioningOptionsExtensions
    {
        public static void ConfigureAbpModules(this ApiVersioningOptions options, IServiceCollection services)
        {
            //TODO: Use new builder will be released with Api Versioning 2.1 instead of reflection!

            services.Configure<AbpAspNetCoreMvcOptions>(op =>
            {
                //TODO: Configuring api version should be done directly inside ConfigureAbpModules,
                //TODO: not in a callback that will be called by MVC later! For that, we immediately need to controllerAssemblySettings

                foreach (var setting in op.AppServiceControllers.ControllerAssemblySettings)
                {
                    if (setting.ApiVersionConfigurer == null)
                    {
                        ConfigureApiVersionsByConvention(options, setting);
                    }
                    else
                    {
                        setting.ApiVersionConfigurer.Invoke(options);
                    }
                }
            });
        }

        private static void ConfigureApiVersionsByConvention(ApiVersioningOptions options, AbpControllerAssemblySetting setting)
        {
            foreach (var controllerType in setting.ControllerTypes)
            {
                var controllerBuilder = typeof(ApiVersionConventionBuilder)
                    .GetMethod(nameof(ApiVersionConventionBuilder.Controller),
                        BindingFlags.Instance | BindingFlags.Public)
                    .MakeGenericMethod(controllerType)
                    .Invoke(options.Conventions, null);

                if (setting.ApiVersions.Any())
                {
                    foreach (var apiVersion in setting.ApiVersions)
                    {
                        typeof(ControllerApiVersionConventionBuilder<>)
                            .MakeGenericType(controllerType)
                            .GetMethod("HasApiVersion")
                            .Invoke(controllerBuilder, new object[] {apiVersion});
                    }
                }
                else
                {
                    if (!controllerType.IsDefined(typeof(ApiVersionAttribute), true))
                    {
                        typeof(ControllerApiVersionConventionBuilder<>)
                            .MakeGenericType(controllerType)
                            .GetMethod("IsApiVersionNeutral")
                            .Invoke(controllerBuilder, null);
                    }
                }
            }
        }
    }
}
