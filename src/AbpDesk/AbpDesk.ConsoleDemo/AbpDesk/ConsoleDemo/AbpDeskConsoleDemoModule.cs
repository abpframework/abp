using AbpDesk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace AbpDesk.ConsoleDemo
{
    [DependsOn(typeof(AbpDeskApplicationModule), typeof(AbpDeskEntityFrameworkCoreModule))]
    public class AbpDeskConsoleDemoModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AbpDeskDbContext>(options =>
            {
                options.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
            });

            services.AddAssemblyOf<AbpDeskConsoleDemoModule>();
        }
    }
}
