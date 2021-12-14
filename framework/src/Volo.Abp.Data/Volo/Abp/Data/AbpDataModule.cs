using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.EventBus.Abstractions;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Uow;

namespace Volo.Abp.Data;

[DependsOn(
    typeof(AbpObjectExtendingModule),
    typeof(AbpUnitOfWorkModule),
    typeof(AbpEventBusAbstractionsModule)
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

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.Databases.RefreshIndexes();
        });
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
