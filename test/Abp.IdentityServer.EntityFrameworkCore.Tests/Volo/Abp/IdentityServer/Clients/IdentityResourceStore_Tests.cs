using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer.Clients
{
    public class IdentityResourceStore_Tests : AbpIdentityServerTestBase
    {
        private readonly IResourceStore _resourceStore;

        public IdentityResourceStore_Tests()
        {
            _resourceStore = ServiceProvider.GetRequiredService<IResourceStore>();
        }

        [Fact]
        public async Task FindApiResourceAsync_Should_Return_Null_If_Not_Found()
        {
            var resource = await _resourceStore.FindApiResourceAsync("non-existing-name");
            resource.ShouldBeNull();
        }
         
    }
}
