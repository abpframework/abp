﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Auditing;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.Features;
using Volo.Abp.AspNetCore.Mvc.GlobalFeatures;
using Volo.Abp.AspNetCore.Mvc.ModelBinding;
using Volo.Abp.AspNetCore.Mvc.Response;
using Volo.Abp.AspNetCore.Mvc.Uow;
using Volo.Abp.AspNetCore.Mvc.Validation;

namespace Volo.Abp.AspNetCore.Mvc
{
    internal static class AbpMvcOptionsExtensions
    {
        public static void AddAbp(this MvcOptions options, IServiceCollection services)
        {
            AddConventions(options, services);
            AddActionFilters(options);
            AddPageFilters(options);
            AddModelBinders(options);
            AddMetadataProviders(options, services);
        }

        private static void AddConventions(MvcOptions options, IServiceCollection services)
        {
            options.Conventions.Add(new AbpServiceConventionWrapper(services));
        }

        private static void AddActionFilters(MvcOptions options)
        {
            options.Filters.AddService(typeof(GlobalFeatureActionFilter));
            options.Filters.AddService(typeof(AbpAuditActionFilter));
            options.Filters.AddService(typeof(AbpNoContentActionFilter));
            options.Filters.AddService(typeof(AbpFeatureActionFilter));
            options.Filters.AddService(typeof(AbpValidationActionFilter));
            options.Filters.AddService(typeof(AbpUowActionFilter));
            options.Filters.AddService(typeof(AbpExceptionFilter));
        }

        private static void AddPageFilters(MvcOptions options)
        {
            options.Filters.AddService(typeof(GlobalFeaturePageFilter));
            options.Filters.AddService(typeof(AbpExceptionPageFilter));
            options.Filters.AddService(typeof(AbpAuditPageFilter));
            options.Filters.AddService(typeof(AbpFeaturePageFilter));
            options.Filters.AddService(typeof(AbpUowPageFilter));
        }

        private static void AddModelBinders(MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new AbpDateTimeModelBinderProvider());
            options.ModelBinderProviders.Insert(0, new AbpExtraPropertiesDictionaryModelBinderProvider());
        }

        private static void AddMetadataProviders(MvcOptions options, IServiceCollection services)
        {
            options.ModelMetadataDetailsProviders.Add(
                new AbpDataAnnotationAutoLocalizationMetadataDetailsProvider(services)
            );
        }
    }
}
