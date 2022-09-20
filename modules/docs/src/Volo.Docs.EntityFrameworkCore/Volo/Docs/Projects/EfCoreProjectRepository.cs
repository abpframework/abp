using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
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

        public async Task<List<Project>> GetListAsync(string sorting, int maxResultCount, int skipCount, CancellationToken cancellationToken = default)
        {
            var projects = await (await GetDbSetAsync()).OrderBy(sorting.IsNullOrEmpty() ? "Id desc" : sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));

            return projects;
        }

        public async Task<Project> GetByShortNameAsync(string shortName, CancellationToken cancellationToken = default)
        {
            var normalizeShortName = NormalizeShortName(shortName);

            var project = await (await GetDbSetAsync()).FirstOrDefaultAsync(p => p.ShortName == normalizeShortName, GetCancellationToken(cancellationToken));

            if (project == null)
            {
                throw new EntityNotFoundException($"Project with the name {shortName} not found!");
            }

            return project;
        }

        public async Task<bool> ShortNameExistsAsync(string shortName, CancellationToken cancellationToken = default)
        {
            var normalizeShortName = NormalizeShortName(shortName);

            return await (await GetDbSetAsync()).AnyAsync(x => x.ShortName == normalizeShortName, GetCancellationToken(cancellationToken));
        }

        private string NormalizeShortName(string shortName)
        {
            return shortName.ToLower();
        }
    }
}
