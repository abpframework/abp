using Microsoft.AspNetCore.TestHost;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.TestBase
{
    public class TestServerAccessor : ITestServerAccessor, ISingletonDependency
    {
        public TestServer Server { get; set; }
    }
}