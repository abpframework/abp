using AbpDesk.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Repositories.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    [DependsOn(typeof(AbpDeskDomainModule))]
    public class AbpDeskEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpDeskEntityFrameworkCoreModule>();

            services.AddTransient<IRepository<Ticket, int>, EfCoreRepository<AbpDeskDbContext, Ticket, int>>();

            //services.AddDbContext<AbpDeskDbContext>(options =>
            //{
            //    options.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
            //});
        }
    }
}
