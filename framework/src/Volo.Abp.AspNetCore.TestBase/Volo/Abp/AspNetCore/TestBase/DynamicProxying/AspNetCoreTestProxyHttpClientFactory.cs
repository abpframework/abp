using System.Net.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Client.Proxying;

namespace Volo.Abp.AspNetCore.TestBase.DynamicProxying
{
    [Dependency(ReplaceServices = true)]
    public class AspNetCoreTestProxyHttpClientFactory : IProxyHttpClientFactory, ITransientDependency
    {
        private readonly ITestServerAccessor _testServerAccessor;

        public AspNetCoreTestProxyHttpClientFactory(
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
