using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Docs.MongoDB;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Volo.Docs.Projects
{
    public class MongoProjectRepository : MongoDbRepository<IDocsMongoDbContext, Project, Guid>, IProjectRepository
    {
        public MongoProjectRepository(IMongoDbContextProvider<IDocsMongoDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public async Task<List<Project>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            var projects = await (await GetMongoQueryableAsync()).OrderBy(sorting ?? "Id desc").As<IMongoQueryable<Project>>()
                .PageBy<Project, IMongoQueryable<Project>>(skipCount, maxResultCount)
                .ToListAsync();

            return projects;
        }

        public async Task<Project> GetByShortNameAsync(string shortName)
        {
            var normalizeShortName = NormalizeShortName(shortName);

            var project = await (await GetMongoQueryableAsync()).FirstOrDefaultAsync(p => p.ShortName == normalizeShortName);

            if (project == null)
            {
                throw new EntityNotFoundException($"Project with the name {shortName} not found!");
            }

            return project;
        }

        public async Task<bool> ShortNameExistsAsync(string shortName)
        {
            var normalizeShortName = NormalizeShortName(shortName);

            return await (await GetMongoQueryableAsync()).AnyAsync(x => x.ShortName == normalizeShortName);
        }

        private string NormalizeShortName(string shortName)
        {
            return shortName.ToLower();
        }
    }
}
