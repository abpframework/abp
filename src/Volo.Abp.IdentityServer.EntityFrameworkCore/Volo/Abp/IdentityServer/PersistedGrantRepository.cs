using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.IdentityServer.Grants;

namespace Volo.Abp.IdentityServer
{
    public class PersistentGrantRepository : EfCoreRepository<IIdentityServerDbContext, PersistedGrant, Guid>, IPersistentGrantRepository
    {
        public PersistentGrantRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public Task<PersistedGrant> FindByKeyAsync(string key)
        {
            return DbSet.FirstOrDefaultAsync(x => x.Key == key);
        }

        public Task<List<PersistedGrant>> GetListBySubjectIdAsync(string subjectId)
        {
            return DbSet.Where(x => x.SubjectId == subjectId).ToListAsync();
        }

        public Task DeleteAsync(string subjectId, string clientId)
        {
            DbSet.RemoveRange(
                DbSet.Where(x => x.SubjectId == subjectId && x.ClientId == clientId)
            );

            return Task.FromResult(0);
        }

        public Task DeleteAsync(string subjectId, string clientId, string type)
        {
            DbSet.RemoveRange(
                DbSet.Where(x =>
                    x.SubjectId == subjectId &&
                    x.ClientId == clientId &&
                    x.Type == type)
            );

            return Task.FromResult(0);
        }
    }
}
