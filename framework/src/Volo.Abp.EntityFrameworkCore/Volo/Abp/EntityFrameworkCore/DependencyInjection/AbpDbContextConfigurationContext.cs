using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection;

public class AbpDbContextConfigurationContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public string ConnectionString { get; }

    public string ConnectionStringName { get; }

    public DbConnection ExistingConnection { get; }

    public DbContextOptionsBuilder DbContextOptions { get; protected set; }

    public AbpDbContextConfigurationContext(
        [NotNull] string connectionString,
        [NotNull] IServiceProvider serviceProvider,
        [CanBeNull] string connectionStringName,
        [CanBeNull] DbConnection existingConnection)
    {
        ConnectionString = connectionString;
        ServiceProvider = serviceProvider;
        ConnectionStringName = connectionStringName;
        ExistingConnection = existingConnection;

        DbContextOptions = new DbContextOptionsBuilder()
            .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
            .UseApplicationServiceProvider(serviceProvider);
    }
}

public class AbpDbContextConfigurationContext<TDbContext> : AbpDbContextConfigurationContext
    where TDbContext : AbpDbContext<TDbContext>
{
    public new DbContextOptionsBuilder<TDbContext> DbContextOptions => (DbContextOptionsBuilder<TDbContext>)base.DbContextOptions;

    public AbpDbContextConfigurationContext(
        string connectionString,
        [NotNull] IServiceProvider serviceProvider,
        [CanBeNull] string connectionStringName,
        [CanBeNull] DbConnection existingConnection)
        : base(
              connectionString,
              serviceProvider,
              connectionStringName,
              existingConnection)
    {
        base.DbContextOptions = new DbContextOptionsBuilder<TDbContext>()
            .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
            .UseApplicationServiceProvider(serviceProvider); ;
    }
}
