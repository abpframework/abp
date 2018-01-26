using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.IdentityServer.Grants
{
    public class PersistedGrantStore : IPersistedGrantStore, ITransientDependency
    {
        private readonly IPersistentGrantRepository _persistentGrantRepository;
        private readonly IObjectMapper _objectMapper;

        public PersistedGrantStore(IPersistentGrantRepository persistentGrantRepository, IObjectMapper objectMapper)
        {
            _persistentGrantRepository = persistentGrantRepository;
            _objectMapper = objectMapper;
        }

        public virtual async Task StoreAsync(IdentityServer4.Models.PersistedGrant grant)
        {
            var entity = _objectMapper.Map<IdentityServer4.Models.PersistedGrant, PersistedGrant>(grant);
            var existing =  await _persistentGrantRepository.FindByKeyAsync(grant.Key);
            if (existing == null)
            {
                await _persistentGrantRepository.InsertAsync(entity);
            }
            else
            {
                await _persistentGrantRepository.UpdateAsync(entity);
            }
        }

        public virtual async Task<IdentityServer4.Models.PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = await _persistentGrantRepository.FindByKeyAsync(key);
            return _objectMapper.Map<PersistedGrant, IdentityServer4.Models.PersistedGrant>(persistedGrant);
        }

        public virtual async Task<IEnumerable<IdentityServer4.Models.PersistedGrant>> GetAllAsync(string subjectId)
        {
            var persistedGrants = await _persistentGrantRepository.GetListBySubjectIdAsync(subjectId);
            return persistedGrants.Select(x => _objectMapper.Map<PersistedGrant, IdentityServer4.Models.PersistedGrant>(x));
        }

        public virtual async Task RemoveAsync(string key)
        {
            var persistedGrant = await _persistentGrantRepository.FindByKeyAsync(key);
            if (persistedGrant == null)
            {
                return;
            }

            await _persistentGrantRepository.DeleteAsync(persistedGrant);
        }

        public virtual async Task RemoveAllAsync(string subjectId, string clientId)
        {
            await _persistentGrantRepository.DeleteAsync(subjectId, clientId);
        }

        public virtual async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            await _persistentGrantRepository.DeleteAsync(subjectId, clientId, type);
        }
    }
}
