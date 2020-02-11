using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Data;
using Volo.Abp.DependencyInjection;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    [Dependency(ReplaceServices = true)]
    public class EntityFrameworkCoreMyProjectNameDbSchemaMigrator 
        : IMyProjectNameDbSchemaMigrator, ITransientDependency
    {
        private readonly MyProjectNameMigrationsDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreMyProjectNameDbSchemaMigrator(
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            await _serviceProvider
                .GetRequiredService<MyProjectNameMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}