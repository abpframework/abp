using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.DependencyInjection;

namespace Volo.Abp.Tests.Modularity
{
    public class TestModule : IAbpModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAssembly(typeof(TestModule).GetTypeInfo().Assembly);
        }
    }
}
