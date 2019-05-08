using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Docs.EntityFrameworkCore;

namespace Volo.Docs.Projects
{
    public class EfCoreProjectRepository : EfCoreRepository<IDocsDbContext, Project, Guid>, IProjectRepository
    {
        public EfCoreProjectRepository(IDbContextProvider<IDocsDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {

        }

        public async Task<List<Project>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            var projects = await DbSet.OrderBy(sorting ?? "Id desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync();

            return projects;
        }

        public async Task<Project> GetByShortNameAsync(string shortName)
        {
            var project = await DbSet.FirstOrDefaultAsync(p => p.ShortName == shortName);

            if (project == null)
            {
                throw new EntityNotFoundException($"Project with the name {shortName} not found!");
            }

            return project;
        }
    }
}
