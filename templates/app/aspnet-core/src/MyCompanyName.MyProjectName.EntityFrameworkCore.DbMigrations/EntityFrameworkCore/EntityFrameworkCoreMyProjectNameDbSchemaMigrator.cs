using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyCompanyName.MyProjectName.Data;
using Volo.Abp.DependencyInjection;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    [Dependency(ReplaceServices = true)]
    public class EntityFrameworkCoreMyProjectNameDbSchemaMigrator 
        : IMyProjectNameDbSchemaMigrator, ITransientDependency
    {
        private readonly MyProjectNameMigrationsDbContext _dbContext;

        public EntityFrameworkCoreMyProjectNameDbSchemaMigrator(MyProjectNameMigrationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}