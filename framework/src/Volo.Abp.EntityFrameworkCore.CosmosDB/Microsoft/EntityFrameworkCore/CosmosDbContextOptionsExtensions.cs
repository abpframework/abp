using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
// Microsoft.EntityFrameworkCore.Cosmos doesn't have single connectionString parameter DbContextOptionsBuilder in {ApplicationName}.EntityFrameworkCore.DbMigrations\{ApplicationName}MigrationsDbContextFactory
namespace Microsoft.EntityFrameworkCore
{
    public static class CosmosDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseCosmos<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
            [NotNull] string connectionString,
            [CanBeNull] Action<CosmosDbContextOptionsBuilder> cosmosOptionsAction = null)
            where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)UseCosmos(
                (DbContextOptionsBuilder)optionsBuilder,
                connectionString,
                cosmosOptionsAction
                );

        public static DbContextOptionsBuilder UseCosmos(
            [NotNull] this DbContextOptionsBuilder optionsBuilder,
            [NotNull] string connectionString,
            [CanBeNull] Action<CosmosDbContextOptionsBuilder> cosmosOptionsAction = null)
        {
            var connStringParts = connectionString.ParseConnectionString();
            var accountEndpoint = connStringParts["AccountEndpoint"];
            if (string.IsNullOrEmpty(accountEndpoint))
                throw new AbpException($"CosmosDb AccountName can not be null or empty!");

            var accountKey = connStringParts["AccountKey"];
            if (string.IsNullOrEmpty(accountEndpoint))
                throw new AbpException($"CosmosDb AccountKey can not be null or empty!");

            var databaseName = connStringParts["DatabaseName"];
            if (string.IsNullOrEmpty(accountEndpoint))
                throw new AbpException($"CosmosDb DatabaseName can not be null or empty!");

            return optionsBuilder.UseCosmos(accountEndpoint: accountEndpoint, accountKey: accountKey, databaseName: databaseName, cosmosOptionsAction);
        }

        static Dictionary<string, string> ParseConnectionString(this string connString)
        {
            return connString.Split(';')
            .Select(t => t.Split(new char[] { '=' }, 2))
            .ToDictionary(t => t[0].Trim(), t => t[1].Trim(), StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
