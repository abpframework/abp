using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.Proxying;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying
{
    public class RegularTestControllerClientProxy_AbpRemoteCallException_Tests : AbpHttpClientTestBase
    {
        private readonly IRegularTestController _controller;

        public RegularTestControllerClientProxy_AbpRemoteCallException_Tests()
        {
            _controller = ServiceProvider.GetRequiredService<IRegularTestController>();
        }

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton<IProxyHttpClientFactory, TestProxyHttpClientFactory>());
        }

        [Fact]
        public async Task AbpRemoteCallException_On_SendAsync_Test()
        {
            var exception = await Assert.ThrowsAsync<AbpRemoteCallException>(async () => await _controller.AbortRequestAsync(default));
            exception.Message.ShouldContain("An error occurred during the ABP remote HTTP request.");
        }

        class TestProxyHttpClientFactory : IProxyHttpClientFactory
        {
            private readonly ITestServerAccessor _testServerAccessor;

            private int _count;

            public TestProxyHttpClientFactory(ITestServerAccessor testServerAccessor)
            {
                _testServerAccessor = testServerAccessor;
            }

            public HttpClient Create(string name) => Create();

            public HttpClient Create()
            {
                if (_count++ > 0)
                {
                    //Will get an error on the SendAsync method.
                    return new HttpClient();
                }

                // for DynamicHttpProxyInterceptor.GetActionApiDescriptionModel
                return _testServerAccessor.Server.CreateClient();
            }
        }
    }
}
