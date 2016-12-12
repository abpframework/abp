using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Repositories.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore
{
    public class AbpEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //TODO: This will be changed!
            services.TryAddTransient(typeof(IDbContextProvider<>), typeof(DefaultDbContextProvider<>));
        }
    }
}
