using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.Data
{
    [DependsOn(
        typeof(AbpUnitOfWorkModule)
        )]
    public class AbpDataModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddDataSeedContributors(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpDbConnectionOptions>(configuration);

            context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        }

        private static void AutoAddDataSeedContributors(IServiceCollection services)
        {
            var contributors = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(IDataSeedContributor).IsAssignableFrom(context.ImplementationType))
                {
                    contributors.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpDataSeedOptions>(options =>
            {
                options.Contributors.AddIfNotContains(contributors);
            });
        }
    }
}
