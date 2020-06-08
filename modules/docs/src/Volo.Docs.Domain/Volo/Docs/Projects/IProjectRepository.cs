using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Docs.Projects
{
    public interface IProjectRepository : IBasicRepository<Project, Guid>
    {
        Task<List<Project>> GetListAsync(string sorting, int maxResultCount, int skipCount);

        Task<Project> GetByShortNameAsync(string shortName);

        Task<bool> ShortNameExistsAsync(string shortName);
    }
}