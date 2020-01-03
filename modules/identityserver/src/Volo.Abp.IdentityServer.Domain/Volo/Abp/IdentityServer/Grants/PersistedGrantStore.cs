using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.IdentityServer.Grants
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IPersistentGrantRepository _persistentGrantRepository;
        private readonly IObjectMapper<AbpIdentityServerDomainModule> _objectMapper;
        private readonly IGuidGenerator _guidGenerator;

        public PersistedGrantStore(IPersistentGrantRepository persistentGrantRepository,
            IObjectMapper<AbpIdentityServerDomainModule> objectMapper, IGuidGenerator guidGenerator)
        {
            _persistentGrantRepository = persistentGrantRepository;
            _objectMapper = objectMapper;
            _guidGenerator = guidGenerator;
        }

        public virtual async Task StoreAsync(IdentityServer4.Models.PersistedGrant grant)
        {
            var entity = await _persistentGrantRepository.FindByKeyAsync(grant.Key).ConfigureAwait(false);
            if (entity == null)
            {
                entity = _objectMapper.Map<IdentityServer4.Models.PersistedGrant, PersistedGrant>(grant);
                EntityHelper.TrySetId(entity, () => _guidGenerator.Create());
                await _persistentGrantRepository.InsertAsync(entity).ConfigureAwait(false);
            }
            else
            {
                _objectMapper.Map(grant, entity);
                await _persistentGrantRepository.UpdateAsync(entity).ConfigureAwait(false);
            }
        }

        public virtual async Task<IdentityServer4.Models.PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = await _persistentGrantRepository.FindByKeyAsync(key).ConfigureAwait(false);
            return _objectMapper.Map<PersistedGrant, IdentityServer4.Models.PersistedGrant>(persistedGrant);
        }

        public virtual async Task<IEnumerable<IdentityServer4.Models.PersistedGrant>> GetAllAsync(string subjectId)
        {
            var persistedGrants = await _persistentGrantRepository.GetListBySubjectIdAsync(subjectId).ConfigureAwait(false);
            return persistedGrants.Select(x => _objectMapper.Map<PersistedGrant, IdentityServer4.Models.PersistedGrant>(x));
        }

        public virtual async Task RemoveAsync(string key)
        {
            var persistedGrant = await _persistentGrantRepository.FindByKeyAsync(key).ConfigureAwait(false);
            if (persistedGrant == null)
            {
                return;
            }

            await _persistentGrantRepository.DeleteAsync(persistedGrant).ConfigureAwait(false);
        }

        public virtual async Task RemoveAllAsync(string subjectId, string clientId)
        {
            await _persistentGrantRepository.DeleteAsync(subjectId, clientId).ConfigureAwait(false);
        }

        public virtual async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            await _persistentGrantRepository.DeleteAsync(subjectId, clientId, type).ConfigureAwait(false);
        }
    }
}
