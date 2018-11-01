using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Docs.Projects
{
    public interface IProjectRepository : IBasicRepository<Project, Guid>
    {
        Task<Project> GetByShortNameAsync(string shortName);
    }
}