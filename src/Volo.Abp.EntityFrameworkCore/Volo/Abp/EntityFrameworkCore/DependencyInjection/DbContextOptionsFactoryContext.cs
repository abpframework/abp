using System;
using System.Threading;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    internal class DbContextOptionsFactoryContext
    {
        public static DbContextOptionsFactoryContext Current => _current.Value;
        private static readonly AsyncLocal<DbContextOptionsFactoryContext> _current = new AsyncLocal<DbContextOptionsFactoryContext>();

        public string ConnectionStringName { get; }

        public string ConnectionString { get; }

        public DbContextOptionsFactoryContext(string connectionStringName, string connectionString)
        {
            ConnectionStringName = connectionStringName;
            ConnectionString = connectionString;
        }

        public static IDisposable Use(DbContextOptionsFactoryContext context)
        {
            _current.Value = context;
            return new DisposeAction(() => _current.Value = null);
        }
    }
}