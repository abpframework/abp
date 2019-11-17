using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.Users.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpUsersTestsSharedModule),
        typeof(AbpUsersEntityFrameworkCoreModule)
        )]
    public class AbpUsersEntityFrameworkCoreTestModule : AbpModule
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

            services.Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled; //EF in-memory database does not support transactions
            });

            services.AddAssemblyOf<AbpUsersEntityFrameworkCoreTestModule>();
        }
    }
}
