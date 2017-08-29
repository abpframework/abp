using System.Collections.Concurrent;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public static class MemoryDatabaseManager
    {
        private static ConcurrentDictionary<string, IMemoryDatabase> _databases;

        static MemoryDatabaseManager()
        {
            _databases = new ConcurrentDictionary<string, IMemoryDatabase>();
        }

        public static IMemoryDatabase Get(string databaseName)
        {
            return _databases.GetOrAdd(databaseName, _ => new MemoryDatabase());
        }
    }
}