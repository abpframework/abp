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
        public MongoProjectRepository(IMongoDbContextProvider<IDocsMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Project>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            var projects = await GetMongoQueryable().OrderBy(sorting ?? "Id desc").As<IMongoQueryable<Project>>()
                .PageBy<Project, IMongoQueryable<Project>>(skipCount, maxResultCount)
                .ToListAsync();

            return projects;
        }

        public async Task<Project> GetByShortNameAsync(string shortName)
        {
            var project = await GetMongoQueryable().FirstOrDefaultAsync(p => p.ShortName == shortName);

            if (project == null)
            {
                throw new EntityNotFoundException($"Project with the name {shortName} not found!");
            }

            return project;
        }
    }
}
