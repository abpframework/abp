using System;
using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.MongoDB
{
    public static class OpenIddictMongoDbContextExtensions
    {
        public static void ConfigureOpenIddict(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new OpenIddictMongoModelBuilderConfigurationOptions(
                AbpOpenIddictDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<OpenIddictApplication>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "OpenIddictApplications";
            });

            builder.Entity<OpenIddictAuthorization>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "OpenIddictAuthorizations";
            });

            builder.Entity<OpenIddictScope>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "OpenIddictScopes";
            });

            builder.Entity<OpenIddictToken>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "OpenIddictTokens";
            });
        }
    }
}