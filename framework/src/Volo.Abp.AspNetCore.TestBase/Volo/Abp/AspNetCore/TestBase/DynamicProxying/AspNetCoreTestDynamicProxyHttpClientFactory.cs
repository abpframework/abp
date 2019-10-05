using System.Net.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;

namespace Volo.Abp.AspNetCore.TestBase.DynamicProxying
{
    [Dependency(ReplaceServices = true)]
    public class AspNetCoreTestDynamicProxyHttpClientFactory : IDynamicProxyHttpClientFactory, ITransientDependency
    {
        private readonly ITestServerAccessor _testServerAccessor;

        public AspNetCoreTestDynamicProxyHttpClientFactory(
            ITestServerAccessor testServerAccessor)
        {
            _testServerAccessor = testServerAccessor;
        }

        public HttpClient Create()
        {
            return _testServerAccessor.Server.CreateClient();
        }

        public HttpClient Create(string name)
        {
            return Create();
        }
    }
}
