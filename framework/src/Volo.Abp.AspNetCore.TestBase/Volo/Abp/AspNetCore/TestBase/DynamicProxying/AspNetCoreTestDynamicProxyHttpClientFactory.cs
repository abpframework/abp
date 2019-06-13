using System.Net.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;

namespace Volo.Abp.AspNetCore.TestBase.DynamicProxying
{
    [Dependency(ReplaceServices = true)]
    public class AspNetCoreTestDynamicProxyHttpClientFactory : IDynamicProxyHttpClientFactory, ITransientDependency
    {
        private readonly ITestServerAccessor _testServerAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public AspNetCoreTestDynamicProxyHttpClientFactory(ITestServerAccessor testServerAccessor, IHttpClientFactory httpClientFactory) {
            _testServerAccessor = testServerAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient Create()
        {
            return _testServerAccessor.Server.CreateClient();
        }

        public HttpClient Create(string name) {
            return Create();
        }
    }
}
