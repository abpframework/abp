using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    public class AbpDbContextConfigurationContext : IServiceProviderAccessor
    {
        public IServiceProvider ServiceProvider { get; }

        public string ConnectionString { get; }

        public string ConnectionStringName { get; }

        public DbContextOptionsBuilder DbContextOptions { get; protected set; }

        public AbpDbContextConfigurationContext(string connectionString, [CanBeNull] string connectionStringName, [NotNull] IServiceProvider serviceProvider)
        {
            ConnectionString = connectionString;
            ConnectionStringName = connectionStringName;
            ServiceProvider = serviceProvider;

            DbContextOptions = new DbContextOptionsBuilder();
        }
    }

    public class AbpDbContextConfigurationContext<TDbContext> : AbpDbContextConfigurationContext
        where TDbContext : AbpDbContext<TDbContext>
    {
        public new DbContextOptionsBuilder<TDbContext> DbContextOptions => (DbContextOptionsBuilder<TDbContext>)base.DbContextOptions;

        public AbpDbContextConfigurationContext(string connectionString, [CanBeNull] string connectionStringName, [NotNull] IServiceProvider serviceProvider)
            : base(connectionString, connectionStringName, serviceProvider)
        {
            base.DbContextOptions = new DbContextOptionsBuilder<TDbContext>();
        }
    }
}