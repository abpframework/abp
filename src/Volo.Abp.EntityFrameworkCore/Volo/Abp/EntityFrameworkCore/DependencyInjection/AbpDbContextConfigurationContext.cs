using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    public class AbpDbContextConfigurationContext
    {
        public string ConnectionString { get; }

        public string ConnectionStringName { get; }

        public DbContextOptionsBuilder DbContextOptions { get; protected set; }

        public AbpDbContextConfigurationContext(string connectionString, [CanBeNull] string connectionStringName)
        {
            ConnectionString = connectionString;
            ConnectionStringName = connectionStringName;
            DbContextOptions = new DbContextOptionsBuilder();
        }
    }

    public class AbpDbContextConfigurationContext<TDbContext> : AbpDbContextConfigurationContext
        where TDbContext : AbpDbContext<TDbContext>
    {
        public new DbContextOptionsBuilder<TDbContext> DbContextOptions => (DbContextOptionsBuilder<TDbContext>)base.DbContextOptions;

        public AbpDbContextConfigurationContext(string connectionString, [CanBeNull] string connectionStringName)
            : base(connectionString, connectionStringName)
        {
            base.DbContextOptions = new DbContextOptionsBuilder<TDbContext>();
        }
    }
}