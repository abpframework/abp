using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpIdentityServerEntityFrameworkCoreModule))]
    public class AbpIdentityServerTestEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityServerTestEntityFrameworkCoreModule>();

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
                    .GetRequiredService<AbpIdentityServerTestDataBuilder>()
                    .Build();
            }
        }
    }
}