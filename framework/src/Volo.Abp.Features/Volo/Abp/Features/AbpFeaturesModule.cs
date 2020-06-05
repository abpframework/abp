using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;

namespace Volo.Abp.Features
{
    [DependsOn(
        typeof(AbpLocalizationAbstractionsModule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpValidationModule)
        )]
    public class AbpFeaturesModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(FeatureInterceptorRegistrar.RegisterIfNeeded);
            AutoAddDefinitionProviders(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpFeatureOptions>(options =>
            {
                options.ValueProviders.Add<DefaultValueFeatureValueProvider>();
                options.ValueProviders.Add<EditionFeatureValueProvider>();
                options.ValueProviders.Add<TenantFeatureValueProvider>();
            });
        }

        private static void AutoAddDefinitionProviders(IServiceCollection services)
        {
            var definitionProviders = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(IFeatureDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                {
                    definitionProviders.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpFeatureOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }
    }
}
