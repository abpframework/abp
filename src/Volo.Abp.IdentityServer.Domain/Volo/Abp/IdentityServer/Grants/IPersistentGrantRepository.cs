using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.Grants
{
    public interface IPersistentGrantRepository : IRepository<PersistedGrant, Guid>
    {
        Task<PersistedGrant> FindByKeyAsync(string key);

        Task<List<PersistedGrant>> GetListBySubjectIdAsync(string key);

        Task DeleteAsync(string subjectId, string clientId);

        Task DeleteAsync(string subjectId, string clientId, string type);
    }
}