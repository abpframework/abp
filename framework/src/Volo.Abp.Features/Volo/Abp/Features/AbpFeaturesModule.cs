using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Features
{
    [DependsOn(
        typeof(AbpLocalizationAbstractionsModule),
        typeof(AbpMultiTenancyAbstractionsModule)
        )]
    public class AbpFeaturesModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddProviders(context.Services);
        }

        private static void AutoAddProviders(IServiceCollection services)
        {
            var featureDefinitionProviders = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(IFeatureDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                {
                    featureDefinitionProviders.Add(context.ImplementationType);
                }
            });

            services.Configure<FeatureOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(featureDefinitionProviders);
            });
        }
    }
}
