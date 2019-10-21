using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Linq;
using Volo.Abp.Modularity;

namespace Volo.Abp.Threading
{
    public class AbpThreadingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IAsyncQueryableExecuter>(DefaultAsyncQueryableExecuter.Instance);
            context.Services.AddSingleton<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);
            context.Services.AddSingleton(typeof(IAmbientScopeProvider<>), typeof(AmbientDataContextAmbientScopeProvider<>));
        }
    }
}
