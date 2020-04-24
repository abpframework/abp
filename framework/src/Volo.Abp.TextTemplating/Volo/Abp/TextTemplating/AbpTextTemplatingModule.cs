using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating
{
    [DependsOn(
        typeof(AbpVirtualFileSystemModule)
        )]
    public class AbpTextTemplatingModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddDefinitionProviders(context.Services);
        }

        private static void AutoAddDefinitionProviders(IServiceCollection services)
        {
            var definitionProviders = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(ITemplateDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                {
                    definitionProviders.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpTextTemplatingOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            //TODO: Consider to move to the TemplateContentProvider and invoke lazy (with making it singleton)
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var templateDefinitionManager = scope.ServiceProvider
                        .GetRequiredService<ITemplateDefinitionManager>();

                foreach (var templateDefinition in templateDefinitionManager.GetAll())
                {
                    var contributorInitializationContext = new TemplateContentContributorInitializationContext(
                        templateDefinition,
                        scope.ServiceProvider
                    );

                    foreach (var contributor in templateDefinition.ContentContributors)
                    {
                        contributor.Initialize(contributorInitializationContext);
                    }
                }
            }
        }
    }
}
