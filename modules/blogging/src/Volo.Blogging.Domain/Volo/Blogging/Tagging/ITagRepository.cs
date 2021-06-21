using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Tagging
{
    public interface ITagRepository : IBasicRepository<Tag, Guid>
    {
        Task<List<Tag>> GetListAsync(Guid blogId, CancellationToken cancellationToken = default);

        Task<Tag> GetByNameAsync(Guid blogId, string name, CancellationToken cancellationToken = default);

        Task<Tag> FindByNameAsync(Guid blogId, string name, CancellationToken cancellationToken = default);

        Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);

        Task DecreaseUsageCountOfTagsAsync(List<Guid> id, CancellationToken cancellationToken = default);
    }
}
