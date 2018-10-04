using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Models;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public static class AbpIdentityServerMongoDbContextExtensions
    {
        public static void ConfigureIdentityServer(
            this IMongoModelBuilder builder,
            Action<IdentityServerMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new IdentityServerMongoModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<ApiResource>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "ApiResources";
            });

            builder.Entity<Client>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Clients";
            });

            builder.Entity<IdentityResource>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "IdentityResources";
            });

            builder.Entity<PersistedGrant>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "PersistedGrants";
            });
        }
    }
}
