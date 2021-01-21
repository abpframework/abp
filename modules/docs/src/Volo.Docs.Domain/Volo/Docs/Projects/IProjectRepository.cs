using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Docs.Projects
{
    public interface IProjectRepository : IBasicRepository<Project, Guid>
    {
        Task<List<Project>> GetListAsync(string sorting, int maxResultCount, int skipCount, CancellationToken cancellationToken = default);

        Task<Project> GetByShortNameAsync(string shortName, CancellationToken cancellationToken = default);

        Task<bool> ShortNameExistsAsync(string shortName, CancellationToken cancellationToken = default);
    }
}
