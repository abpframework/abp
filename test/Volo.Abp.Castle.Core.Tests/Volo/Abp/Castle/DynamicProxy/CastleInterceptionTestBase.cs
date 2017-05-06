using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.Castle.DynamicProxy
{
    //TODO: There is no Castle Dependency here.. We can move this base class directly to Volo.Abp.Tests
    public abstract class CastleInterceptionTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {

    }
}