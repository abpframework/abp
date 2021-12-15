using Shouldly;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Http.Client.IdentityModel.Web.Tests;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

public class HttpContextIdentityModelRemoteServiceHttpClientAuthenticator_Tests : AbpIntegratedTest<AbpHttpClientIdentityModelWebTestModule>
{
    private readonly IRemoteServiceHttpClientAuthenticator _remoteServiceHttpClientAuthenticator;

    public HttpContextIdentityModelRemoteServiceHttpClientAuthenticator_Tests()
    {
        _remoteServiceHttpClientAuthenticator = GetRequiredService<IRemoteServiceHttpClientAuthenticator>();
    }

    [Fact]
    public void Implementation_Should_Be_Type_Of_HttpContextIdentityModelRemoteServiceHttpClientAuthenticator()
    {
        ProxyHelper.UnProxy(_remoteServiceHttpClientAuthenticator)
            .ShouldBeOfType(typeof(HttpContextIdentityModelRemoteServiceHttpClientAuthenticator));
    }
}
