using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing
{
    [DependsOn(
        typeof(SettingsModule),
        typeof(VirtualFileSystemModule),
        typeof(BackgroundJobsAbstractionsModule),
        typeof(LocalizationModule)
        )]
    public class EmailingModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddDefinitionProviders(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<EmailingModule>();
            });

            Configure<BackgroundJobOptions>(options =>
            {
                options.AddJob<BackgroundEmailSendingJob>();
            });
        }

        private static void AutoAddDefinitionProviders(IServiceCollection services)
        {
            var definitionProviders = new List<Type>();

            services.OnRegistred(context =>
            {

                if (typeof(IEmailTemplateDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                {
                    definitionProviders.Add(context.ImplementationType);
                }
            });

            services.Configure<EmailTemplateOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var emailTemplateDefinitionManager =
                    scope.ServiceProvider.GetRequiredService<IEmailTemplateDefinitionManager>();

                foreach (var templateDefinition in emailTemplateDefinitionManager.GetAll())
                {
                    foreach (var contributor in templateDefinition.Contributors)
                    {
                        contributor.Initialize(new EmailTemplateInitializationContext(templateDefinition, scope.ServiceProvider));
                    }
                }
            }
        }
    }
}
