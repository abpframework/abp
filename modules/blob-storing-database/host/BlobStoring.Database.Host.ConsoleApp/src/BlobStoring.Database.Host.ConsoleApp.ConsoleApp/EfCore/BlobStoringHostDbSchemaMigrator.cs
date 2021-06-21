using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace BlobStoring.Database.Host.ConsoleApp.ConsoleApp.EfCore
{
    public class BlobStoringHostDbSchemaMigrator : IBlobStoringHostDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public BlobStoringHostDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            await _serviceProvider
                .GetRequiredService<BlobStoringHostDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}