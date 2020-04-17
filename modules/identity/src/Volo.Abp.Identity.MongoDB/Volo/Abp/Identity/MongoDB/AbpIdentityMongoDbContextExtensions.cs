using System;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public static class AbpIdentityMongoDbContextExtensions
    {
        public static void ConfigureIdentity(
            this IMongoModelBuilder builder,
            Action<IdentityMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new IdentityMongoModelBuilderConfigurationOptions(
                AbpIdentityDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<IdentityUser>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Users";
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Roles";
            });

            builder.Entity<IdentityClaimType>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "ClaimTypes";
            });
        }
    }
}