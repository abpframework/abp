using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.IdentityServer.Grants;

public class PersistedGrantStore : IPersistedGrantStore
{
    protected IPersistentGrantRepository PersistentGrantRepository { get; }
    protected IObjectMapper<AbpIdentityServerDomainModule> ObjectMapper { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public PersistedGrantStore(IPersistentGrantRepository persistentGrantRepository,
        IObjectMapper<AbpIdentityServerDomainModule> objectMapper, IGuidGenerator guidGenerator)
    {
        PersistentGrantRepository = persistentGrantRepository;
        ObjectMapper = objectMapper;
        GuidGenerator = guidGenerator;
    }

    public virtual async Task StoreAsync(IdentityServer4.Models.PersistedGrant grant)
    {
        var entity = await PersistentGrantRepository.FindByKeyAsync(grant.Key);
        if (entity == null)
        {
            entity = ObjectMapper.Map<IdentityServer4.Models.PersistedGrant, PersistedGrant>(grant);
            EntityHelper.TrySetId(entity, () => GuidGenerator.Create());
            await PersistentGrantRepository.InsertAsync(entity);
        }
        else
        {
            ObjectMapper.Map(grant, entity);
            await PersistentGrantRepository.UpdateAsync(entity);
        }
    }

    public virtual async Task<IdentityServer4.Models.PersistedGrant> GetAsync(string key)
    {
        var persistedGrant = await PersistentGrantRepository.FindByKeyAsync(key);
        return ObjectMapper.Map<PersistedGrant, IdentityServer4.Models.PersistedGrant>(persistedGrant);
    }

    public virtual async Task<IEnumerable<IdentityServer4.Models.PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
    {
        var persistedGrants = await PersistentGrantRepository.GetListAsync(filter.SubjectId, filter.SessionId, filter.ClientId, filter.Type);
        return ObjectMapper.Map<List<PersistedGrant>, List<IdentityServer4.Models.PersistedGrant>>(persistedGrants);
    }

    public virtual async Task RemoveAsync(string key)
    {
        var persistedGrant = await PersistentGrantRepository.FindByKeyAsync(key);
        if (persistedGrant == null)
        {
            return;
        }

        await PersistentGrantRepository.DeleteAsync(persistedGrant);
    }

    public virtual async Task RemoveAllAsync(PersistedGrantFilter filter)
    {
        await PersistentGrantRepository.DeleteAsync(filter.SubjectId, filter.SessionId, filter.ClientId, filter.Type);
    }
}
