using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
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

        [Fact]
        public async Task StoreAsync_Should_Store_PersistedGrant()
        {
            //Act
            await _persistedGrantStore.StoreAsync(new PersistedGrant
            {
                Key = "39",
                ClientId = "TestClientId-39",
                Type = "TestType-39",
                SubjectId = "TestSubject-39",
                Data = "TestData-39",
                Expiration = new DateTime(2018, 1, 6, 21, 22, 23),
                CreationTime = new DateTime(2018, 1, 5, 19, 20, 21)
            });

            //Assert
            var persistedGrant = await _persistedGrantStore.GetAsync("39");
            persistedGrant.Key.ShouldBe("39");
            persistedGrant.ClientId.ShouldBe("TestClientId-39");
            persistedGrant.Type.ShouldBe("TestType-39");
            persistedGrant.SubjectId.ShouldBe("TestSubject-39");
            persistedGrant.Data.ShouldBe("TestData-39");

            persistedGrant.Expiration.HasValue.ShouldBe(true);
            persistedGrant.Expiration.Value.Year.ShouldBe(2018);
            persistedGrant.Expiration.Value.Month.ShouldBe(1);
            persistedGrant.Expiration.Value.Day.ShouldBe(6);
            persistedGrant.Expiration.Value.Hour.ShouldBe(21);
            persistedGrant.Expiration.Value.Minute.ShouldBe(22);
            persistedGrant.Expiration.Value.Second.ShouldBe(23);

            persistedGrant.CreationTime.Year.ShouldBe(2018);
            persistedGrant.CreationTime.Month.ShouldBe(1);
            persistedGrant.CreationTime.Day.ShouldBe(5);
            persistedGrant.CreationTime.Hour.ShouldBe(19);
            persistedGrant.CreationTime.Minute.ShouldBe(20);
            persistedGrant.CreationTime.Second.ShouldBe(21);
        }

        [Fact]
        public async Task FindClientByIdAsync_Should_Return_Null_If_Not_Found()
        {
            var persistentGrant = await _persistedGrantStore.GetAllAsync("not-existing-id");
            persistentGrant.ShouldBeNull();
        }

    }
}
