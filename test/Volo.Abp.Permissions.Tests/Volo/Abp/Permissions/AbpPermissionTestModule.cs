using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions.EntityFrameworkCore;
using Volo.Abp.Session;
using Volo.Abp.Uow;

namespace Volo.Abp.Permissions
{
    [DependsOn(
        typeof(AbpPermissionsEntityFrameworkCoreModule), 
        typeof(AbpSessionModule),
        typeof(AbpAutofacModule))]
    public class AbpPermissionTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionTestModule>();

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
                options.DefinitionProviders.Add<TestPermissionDefinitionProvider>();
            });
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
                    .GetRequiredService<PermissionTestDataBuilder>()
                    .Build();
            }
        }
    }
}
