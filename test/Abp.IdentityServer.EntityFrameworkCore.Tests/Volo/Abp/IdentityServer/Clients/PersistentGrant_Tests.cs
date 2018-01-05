using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer.Clients
{
    public class PersistentGrantStore_Tests : AbpIdentityServerTestBase
    {
        private readonly IPersistedGrantStore _persistedGrantStore;

        public PersistentGrantStore_Tests()
        {
            _persistedGrantStore = ServiceProvider.GetRequiredService<IPersistedGrantStore>();
        }

        [Fact]
        public async Task FindClientByIdAsync_Should_Return_Null_If_Not_Found()
        {
            var persistentGrant = await _persistedGrantStore.GetAsync("not-existing-id");
            persistentGrant.ShouldBeNull();
        }

        [Fact]
        public async Task FindPersistentGrantByIdAsync_Should_Return_The_PersistentGrant_If_Found()
        {
            //Act
            var client = await _persistedGrantStore.GetAsync("38");

            //Assert
            client.ShouldNotBeNull();
            client.ClientId.ShouldBe("TestClientId-38");
            client.SubjectId.ShouldBe("TestSubject-38");
            client.Data.ShouldContain("TestData-38");
            client.Type.ShouldContain("TestType-38");
        }
    }
}
