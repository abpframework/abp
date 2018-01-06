using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer.Clients
{
    public class IdentityResourceStore_Tests : AbpIdentityServerTestBase
    {
        private readonly IResourceStore _persistedGrantStore;

        public IdentityResourceStore_Tests()
        {
            _persistedGrantStore = ServiceProvider.GetRequiredService<IResourceStore>();
        }
 
        //too:WRITE TESTS
    }
}
