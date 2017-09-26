using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.Uow;
using Volo.Abp.AspNetCore.Mvc.Validation;

namespace Volo.Abp.AspNetCore.Mvc
{
    internal static class AbpMvcOptionsExtensions
    {
        public static void AddAbp(this MvcOptions options, IServiceCollection services)
        {
            AddConventions(options, services);
            AddFilters(options);
            AddModelBinders(options);
        }

        private static void AddConventions(MvcOptions options, IServiceCollection services)
        {
            options.Conventions.Add(new AbpAppServiceConventionWrapper(services));
        }

        private static void AddFilters(MvcOptions options)
        {
            //options.Filters.AddService(typeof(AbpAuthorizationFilter));
            //options.Filters.AddService(typeof(AbpAuditActionFilter));
            options.Filters.AddService(typeof(AbpValidationActionFilter));
            options.Filters.AddService(typeof(AbpUowActionFilter));
            options.Filters.AddService(typeof(AbpExceptionFilter));
        }

        private static void AddModelBinders(MvcOptions options)
        {
            //options.ModelBinderProviders.Add(new AbpDateTimeModelBinderProvider());
        }
    }
}