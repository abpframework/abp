using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDatabaseManager : ISingletonDependency
    {
        private readonly ConcurrentDictionary<string, IMemoryDatabase> _databases =
            new ConcurrentDictionary<string, IMemoryDatabase>();

        private readonly IServiceProvider _serviceProvider;

        public MemoryDatabaseManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMemoryDatabase Get(string databaseName)
        {
            return _databases.GetOrAdd(databaseName, _ => _serviceProvider.GetRequiredService<IMemoryDatabase>());
        }
    }
}