using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public async Task<List<Tag>> GetListAsync(Guid blogId)
        {
            return await (await GetDbSetAsync()).Where(t=>t.BlogId == blogId).ToListAsync();
        }

        public async Task<Tag> GetByNameAsync(Guid blogId, string name)
        {
            return await (await GetDbSetAsync()).FirstAsync(t=> t.BlogId == blogId && t.Name == name);
        }

        public async Task<Tag> FindByNameAsync(Guid blogId, string name)
        {
            return await (await GetDbSetAsync()).FirstOrDefaultAsync(t => t.BlogId == blogId && t.Name == name);
        }

        public async Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await (await GetDbSetAsync()).Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public async Task DecreaseUsageCountOfTagsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            var tags = await (await GetDbSetAsync())
                .Where(t => ids.Any(id => id == t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var tag in tags)
            {
                tag.DecreaseUsageCount();
            }
        }
    }
}
