using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Tagging
{
    public interface ITagRepository : IBasicRepository<Tag, Guid>
    {
        Task<List<Tag>> GetListAsync(Guid blogId);

        Task<Tag> GetByNameAsync(Guid blogId, string name);

        Task<Tag> FindByNameAsync(Guid blogId, string name);

        Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids);

        Task DecreaseUsageCountOfTagsAsync(List<Guid> id, CancellationToken cancellationToken = default);
    }
}
