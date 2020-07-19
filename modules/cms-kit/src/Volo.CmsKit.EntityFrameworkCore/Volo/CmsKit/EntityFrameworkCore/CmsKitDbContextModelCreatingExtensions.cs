using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit.EntityFrameworkCore
{
    public static class CmsKitDbContextModelCreatingExtensions
    {
        public static void ConfigureCmsKit(
            this ModelBuilder builder,
            Action<CmsKitModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new CmsKitModelBuilderConfigurationOptions(
                CmsKitDbProperties.DbTablePrefix,
                CmsKitDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<UserReaction>(b =>
            {
                b.ToTable(options.TablePrefix + "UserReactions", options.Schema);
                b.ConfigureByConvention();

                b.Property(x => x.EntityType).IsRequired().HasMaxLength(UserReactionConsts.EntityTypeLength);
                b.Property(x => x.EntityId).IsRequired().HasMaxLength(UserReactionConsts.EntityIdLength);
                b.Property(x => x.ReactionName).IsRequired().HasMaxLength(UserReactionConsts.ReactionNameLength);
                b.Property(x => x.CreationTime);

                b.HasIndex(x => new { x.EntityType, x.EntityId });
                b.HasIndex(x => new { x.CreatorId, x.EntityType, x.EntityId, x.ReactionName });
            });
        }
    }
}
