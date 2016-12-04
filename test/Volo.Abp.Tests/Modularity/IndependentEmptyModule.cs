using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Tests.Modularity
{
    public class IndependentEmptyModule : IAbpModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            
        }
    }
}
