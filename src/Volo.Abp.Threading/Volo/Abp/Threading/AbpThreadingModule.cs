using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Threading
{
    public class AbpThreadingModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);

            services.AddAssemblyOf<AbpThreadingModule>();
        }
    }
}
