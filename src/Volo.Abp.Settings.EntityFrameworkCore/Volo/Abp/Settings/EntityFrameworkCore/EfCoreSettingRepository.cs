using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Settings.EntityFrameworkCore
{
    public class EfCoreSettingRepository : EfCoreRepository<IAbpSettingsDbContext, Setting, Guid>, ISettingRepository
    {
        public EfCoreSettingRepository(IDbContextProvider<IAbpSettingsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Setting> FindAsync(string name, string entityType, string entityId)
        {
            return await DbSet.FirstOrDefaultAsync(s => s.Name == name && s.EntityType == entityType && s.EntityId == entityId);
        }

        public async Task<List<Setting>> GetListAsync(string entityType, string entityId)
        {
            return await DbSet.Where(s => s.EntityType == entityType && s.EntityId == entityId).ToListAsync();
        }
    }
}
