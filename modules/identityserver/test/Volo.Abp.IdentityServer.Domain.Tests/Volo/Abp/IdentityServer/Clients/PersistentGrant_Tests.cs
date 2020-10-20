using System;
using System.Linq;
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
            client.SubjectId.ShouldBe("TestSubject");
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
                SubjectId = "TestSubject",
                Data = "TestData-39",
                Expiration = new DateTime(2018, 1, 6, 21, 22, 23),
                CreationTime = new DateTime(2018, 1, 5, 19, 20, 21)
            });

            //Assert
            var persistedGrant = await _persistedGrantStore.GetAsync("39");
            persistedGrant.Key.ShouldBe("39");
            persistedGrant.ClientId.ShouldBe("TestClientId-39");
            persistedGrant.Type.ShouldBe("TestType-39");
            persistedGrant.SubjectId.ShouldBe("TestSubject");
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
        public async Task StoreAsync_Should_Store_PersistedGrant_When_Exists()
        {
            //Act
            await _persistedGrantStore.StoreAsync(new PersistedGrant
            {
                Key = "PersistedGrantKey1",
                ClientId = "TestClientId-PersistedGrantKey1",
                Type = "TestType-PersistedGrantKey1",
                SubjectId = "TestSubject",
                Data = "TestData-PersistedGrantKey1",
                Expiration = new DateTime(2018, 1, 6, 21, 22, 23),
                CreationTime = new DateTime(2018, 1, 5, 19, 20, 21)
            });

            //Assert
            var persistedGrant = await _persistedGrantStore.GetAsync("PersistedGrantKey1");
            persistedGrant.Key.ShouldBe("PersistedGrantKey1");
            persistedGrant.ClientId.ShouldBe("TestClientId-PersistedGrantKey1");
            persistedGrant.Type.ShouldBe("TestType-PersistedGrantKey1");
            persistedGrant.SubjectId.ShouldBe("TestSubject");
            persistedGrant.Data.ShouldBe("TestData-PersistedGrantKey1");

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
        public async Task GetAllAsync_Should_Get_All_PersistedGrants_For_A_Given_SubjectId()
        {
            //Act
            var persistentGrants = await _persistedGrantStore.GetAllAsync(new PersistedGrantFilter()
            {
                SubjectId = "TestSubject"
            });

            //Assert
            var persistedGrants = persistentGrants as PersistedGrant[] ?? persistentGrants.ToArray();
            persistedGrants.ShouldNotBe(null);
            persistedGrants.Length.ShouldBe(2);
            persistedGrants[0].SubjectId.ShouldBe("TestSubject");
            persistedGrants[1].SubjectId.ShouldBe("TestSubject");
        }

        [Fact]
        public async Task RemoveAsync_Should_Remove_PeristedGrant()
        {
            //Arrange
            await _persistedGrantStore.StoreAsync(new PersistedGrant
            {
                Key = "#1P3R",
                Type = "Type",
                ClientId = "ClientId",
                Data = ""
            });

            //Act
            await _persistedGrantStore.RemoveAsync("#1P3R");

            //Assert
            var persistedGrant = await _persistedGrantStore.GetAsync("#1P3R");
            persistedGrant.ShouldBe(null);
        }

        [Fact]
        public async Task RemoveAllAsync_Should_RemoveAll_PeristedGrants_For_A_Given_Subject_And_ClientId()
        {
            //Arrange
            var persistedGrantsWithTestSubjectX = await _persistedGrantStore.GetAllAsync(new PersistedGrantFilter()
            {
                SubjectId = "TestSubject-X"
            });
            var persistedGrantsWithTestSubjectXBeforeLength = persistedGrantsWithTestSubjectX.ToArray().Length;

            //Act
            await _persistedGrantStore.RemoveAllAsync(new PersistedGrantFilter()
            {
                SubjectId = "TestSubject-X",
                ClientId =  "TestClientId-X"
            });

            //Assert
            persistedGrantsWithTestSubjectXBeforeLength.ShouldBe(2);

            var persistedGrants = (await _persistedGrantStore.GetAllAsync(new PersistedGrantFilter()
            {
                SubjectId = "TestClientId-37"
            })).ToArray();

            persistedGrants.ShouldNotBe(null);
            persistedGrants.Length.ShouldBe(0);
        }
    }
}
