using System;
using AbpDesk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    //TODO: Remove AbpAutofacModule and add it to AbpDesk application's Core module!
    [DependsOn(typeof(AbpDeskApplicationModule), typeof(AbpDeskEntityFrameworkCoreModule), typeof(AbpAutofacModule))]
    public class AbpDeskApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase();

            services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = Guid.NewGuid().ToString();
            });

            services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(context =>
                {
                    context.DbContextOptions.UseInMemoryDatabase(context.ConnectionString);
                });
            });
        }
    }
}
