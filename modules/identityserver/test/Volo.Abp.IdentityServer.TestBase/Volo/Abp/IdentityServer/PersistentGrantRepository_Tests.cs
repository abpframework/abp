using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.IdentityServer
{
    public abstract class PersistentGrantRepository_Tests<TStartupModule> : AbpIdentityServerTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IPersistentGrantRepository _persistentGrantRepository;

        protected PersistentGrantRepository_Tests()
        {
            _persistentGrantRepository = GetRequiredService<IPersistentGrantRepository>();
        }

        [Fact]
        public async Task FindByKeyAsync()
        {
            (await _persistentGrantRepository.FindByKeyAsync("PersistedGrantKey1")).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetListBySubjectIdAsync()
        {
            var persistedGrants = await _persistentGrantRepository.GetListBySubjectIdAsync("PersistedGrantSubjectId1");
            persistedGrants.ShouldNotBeEmpty();
            persistedGrants.ShouldContain(x => x.Key == "PersistedGrantKey1");
        }

        [Fact]
        public async Task DeleteBySubjectIdAndClientId()
        {
            await _persistentGrantRepository.DeleteAsync("PersistedGrantSubjectId1", "PersistedGrantSessionId1", "PersistedGrantClientId1");

            var persistedGrants = await _persistentGrantRepository.GetListAsync();
            persistedGrants.ShouldNotBeEmpty();
            persistedGrants.ShouldNotContain(x =>
                x.Key == "PersistedGrantKey1" && x.SubjectId == "PersistedGrantSubjectId1" &&
                x.ClientId == "PersistedGrantClientId1");
        }

        [Fact]
        public async Task DeleteBySubjectIdAndClientIdAndType()
        {
            await _persistentGrantRepository.DeleteAsync("PersistedGrantSubjectId1", "PersistedGrantSessionId1", "PersistedGrantClientId1",
                "PersistedGrantClientId1");

            var persistedGrants = await _persistentGrantRepository.GetListAsync();
            persistedGrants.ShouldNotBeEmpty();
            persistedGrants.ShouldNotContain(x =>
                x.Key == "PersistedGrantKey1" && x.SubjectId == "PersistedGrantSubjectId1" &&
                x.ClientId == "PersistedGrantClientId1" && x.Type == "PersistedGrantClientId1");

        }
    }
}
