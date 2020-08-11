using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Users;
using Volo.Abp.Users.EntityFrameworkCore;

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

            builder.Entity<CmsUser>(b =>
            {
                b.ToTable(options.TablePrefix + "Users", options.Schema);

                b.ConfigureByConvention();
                b.ConfigureAbpUser();
            });

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

            builder.Entity<Comment>(b =>
            {
                b.ToTable(options.TablePrefix + "Comments", options.Schema);
                b.ConfigureByConvention();

                b.Property(x => x.EntityType).IsRequired().HasMaxLength(CommentConsts.EntityTypeLength);
                b.Property(x => x.EntityId).IsRequired().HasMaxLength(CommentConsts.EntityIdLength);
                b.Property(x => x.Text).IsRequired().HasMaxLength(CommentConsts.MaxTextLength);
                b.Property(x => x.RepliedCommentId);
                b.Property(x => x.CreationTime);

                b.HasIndex(x => new { x.EntityType, x.EntityId });
                b.HasIndex(x => new { x.RepliedCommentId });
            });
        }
    }
}
