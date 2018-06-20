using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpTenantManagementTestBaseModule)
        )]
    public class AbpTenantManagementEntityFrameworkCoreTestModule : AbpModule
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

            services.AddAssemblyOf<AbpTenantManagementEntityFrameworkCoreTestModule>();
        }
    }
}
