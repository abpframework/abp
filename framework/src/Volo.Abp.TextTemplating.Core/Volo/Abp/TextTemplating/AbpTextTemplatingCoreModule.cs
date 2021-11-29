using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating
{
    [DependsOn(
        typeof(AbpVirtualFileSystemModule),
        typeof(AbpLocalizationAbstractionsModule)
        )]
    public class AbpTextTemplatingCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddProvidersAndContributors(context.Services);
        }

        private static void AutoAddProvidersAndContributors(IServiceCollection services)
        {
            var definitionProviders = new List<Type>();
            var contentContributors = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(ITemplateDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                {
                    definitionProviders.Add(context.ImplementationType);
                }

                if (typeof(ITemplateContentContributor).IsAssignableFrom(context.ImplementationType))
                {
                    contentContributors.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpTextTemplatingOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
                options.ContentContributors.AddIfNotContains(contentContributors);
            });
        }
    }
}
