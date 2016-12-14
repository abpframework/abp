using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public class AbpDbContextConfigurationContext
    {
        public string ConnectionString { get; }

        public string ModuleName { get; } //TODO: Use a SchemaName/DatabaseName instead?

        public virtual DbContextOptionsBuilder DbContextOptions { get; protected set; }

        public AbpDbContextConfigurationContext(string connectionString, [CanBeNull] string moduleName)
        {
            ConnectionString = connectionString;
            ModuleName = moduleName;
            DbContextOptions = new DbContextOptionsBuilder();
        }
    }

    public class AbpDbContextConfigurationContext<TDbContext> : AbpDbContextConfigurationContext
        where TDbContext : AbpDbContext<TDbContext>
    {
        public new DbContextOptionsBuilder<TDbContext> DbContextOptions => (DbContextOptionsBuilder<TDbContext>)base.DbContextOptions;

        public AbpDbContextConfigurationContext(string connectionString, [CanBeNull] string moduleName)
            : base(connectionString, moduleName)
        {
            base.DbContextOptions = new DbContextOptionsBuilder<TDbContext>();
        }
    }
}