using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging.Tagging
{
    public class EfCoreTagRepository : EfCoreRepository<IBloggingDbContext, Tag, Guid>, ITagRepository
    {
        public EfCoreTagRepository(IDbContextProvider<IBloggingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Tag>> GetListAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Tag> GetByNameAsync(string name)
        {
            return await DbSet.FirstOrDefaultAsync(t=>t.Name == name);
        }

        public async Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await DbSet.Where(c => ids.Contains(c.Id)).ToListAsync();
        }

        public void DecreaseUsageCountOfTags(List<Guid> ids)
        {
            var tags = DbSet.Where(t => ids.Any(id => id == t.Id));

            foreach (var tag in tags)
            {
                tag.DecreaseUsageCount();
            }
        }
    }
}
