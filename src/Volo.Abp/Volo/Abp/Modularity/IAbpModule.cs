using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public interface IAbpModule : ISingletonDependency
    {
        void ConfigureServices([NotNull] IServiceCollection services);
    }
}
