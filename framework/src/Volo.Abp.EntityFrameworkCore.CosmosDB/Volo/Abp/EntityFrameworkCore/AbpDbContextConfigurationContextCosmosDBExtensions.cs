using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using System.Linq;
using System.Collections.Generic;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextConfigurationContextCosmosDBExtensions
    {
        public static DbContextOptionsBuilder UseCosmos(
           [NotNull] this AbpDbContextConfigurationContext context,
           [CanBeNull] Action<CosmosDbContextOptionsBuilder> cosmosOptionsAction = null)
        {
            //context.ConnectionStringName = "AbpBackgroundJobs" hits all the time
            if (context.ExistingConnection != null)
            {
                var connStringParts = context.ExistingConnection.ConnectionString.ParseConnectionString();

                var accountEndpoint = connStringParts["AccountEndpoint"];
                if (string.IsNullOrEmpty(accountEndpoint))
                    throw new AbpException($"CosmosDb AccountName can not be null or empty!");

                var accountKey = connStringParts["AccountKey"];
                if (string.IsNullOrEmpty(accountEndpoint))
                    throw new AbpException($"CosmosDb AccountKey can not be null or empty!");

                var databaseName = connStringParts["DatabaseName"];
                if (string.IsNullOrEmpty(accountEndpoint))
                    throw new AbpException($"CosmosDb DatabaseName can not be null or empty!");

                return context.DbContextOptions.UseCosmos(accountEndpoint: accountEndpoint, accountKey: accountKey, databaseName: databaseName, cosmosOptionsAction);
            }
            else
            {
                var connStringParts = context.ConnectionString.ParseConnectionString();

                var accountEndpoint = connStringParts["AccountEndpoint"];
                if (string.IsNullOrEmpty(accountEndpoint))
                    throw new AbpException($"CosmosDb AccountName can not be null or empty!");

                var accountKey = connStringParts["AccountKey"];
                if (string.IsNullOrEmpty(accountEndpoint))
                    throw new AbpException($"CosmosDb AccountKey can not be null or empty!");

                var databaseName = connStringParts["DatabaseName"];
                if (string.IsNullOrEmpty(accountEndpoint))
                    throw new AbpException($"CosmosDb DatabaseName can not be null or empty!");

                return context.DbContextOptions.UseCosmos(accountEndpoint: accountEndpoint, accountKey: accountKey, databaseName: databaseName, cosmosOptionsAction);
            }
        }

        static Dictionary<string, string> ParseConnectionString(this string connString)
        {
            return connString.Split(';')
            .Select(t => t.Split(new char[] { '=' }, 2))
            .ToDictionary(t => t[0].Trim(), t => t[1].Trim(), StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
