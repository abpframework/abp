using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DashboardDemo.Data;
using Volo.Abp.DependencyInjection;

namespace DashboardDemo.EntityFrameworkCore
{
    [Dependency(ReplaceServices = true)]
    public class EntityFrameworkCoreDashboardDemoDbSchemaMigrator 
        : IDashboardDemoDbSchemaMigrator, ITransientDependency
    {
        private readonly DashboardDemoMigrationsDbContext _dbContext;

        public EntityFrameworkCoreDashboardDemoDbSchemaMigrator(DashboardDemoMigrationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}