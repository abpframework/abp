using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore.TestApp.SecondContext;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    [DependsOn(typeof(TestAppModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpEfCoreTestSecondContextModule))]
    public class AbpEntityFrameworkCoreTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpEntityFrameworkCoreTestModule>();

            services.AddAbpDbContext<TestAppDbContext>(options =>
            {
                options.AddDefaultRepositories();
                options.ReplaceDbContext<IThirdDbContext>();
            });

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            inMemorySqlite.Open();

            services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(context =>
                {
                    context.DbContextOptions.UseSqlite(inMemorySqlite);
                });
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider.GetRequiredService<TestAppDbContext>().Database.Migrate();
            context.ServiceProvider.GetRequiredService<SecondDbContext>().Database.Migrate();
        }
    }
}
