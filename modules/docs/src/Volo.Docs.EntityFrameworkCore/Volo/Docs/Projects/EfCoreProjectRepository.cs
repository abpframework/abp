using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Project> FindByShortNameAsync(string shortName)
        {
            return await DbSet.FirstOrDefaultAsync(p => p.ShortName == shortName);
        }
    }
}
