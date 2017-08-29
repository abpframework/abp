using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.MemoryDb;
using Volo.Abp.Data;
using Volo.Abp.Autofac;
using Volo.Abp.TestApp;

namespace Volo.Abp.MemoryDb
{
    [DependsOn(typeof(TestAppModule), typeof(AbpMemoryDbModule), typeof(AbpAutofacModule))]
    public class AbpMemoryDbTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var connStr = Guid.NewGuid().ToString();

            services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connStr;
            });

            services.AddMemoryDbContext<TestAppMemoryDbContext>(options =>
            {
                options.WithDefaultRepositories();
            });

            services.AddAssemblyOf<AbpMemoryDbModule>();
        }
    }
}
