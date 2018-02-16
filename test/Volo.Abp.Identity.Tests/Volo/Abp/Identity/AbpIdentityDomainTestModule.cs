using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions;
using Volo.Abp.Permissions.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityDomainModule), 
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpPermissionsEntityFrameworkCoreModule),
        typeof(AbpAutofacModule))]
    public class AbpIdentityDomainTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase();

            var databaseName = Guid.NewGuid().ToString();

            services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(context =>
                {
                    context.DbContextOptions.UseInMemoryDatabase(databaseName);
                });
            });

            services.Configure<UnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled; //EF in-memory database does not support transactions
            });

            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<IdentityTestPermissionDefinitionProvider>();
            });

            services.AddAssemblyOf<AbpIdentityDomainTestModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<AbpIdentityTestDataBuilder>()
                    .Build();
            }
        }
    }
}
