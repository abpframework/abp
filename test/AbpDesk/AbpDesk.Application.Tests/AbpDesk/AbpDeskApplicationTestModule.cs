using AbpDesk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    [DependsOn(typeof(AbpDeskApplicationModule), typeof(AbpDeskEntityFrameworkCoreModule))]
    public class AbpDeskApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase();

            services.AddDbContext<AbpDeskDbContext>(options =>
            {
                options.UseInMemoryDatabase();
            });
        }
    }
}
