using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Users.EntityFrameworkCore;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

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

            if (GlobalFeatureManager.Instance.IsEnabled<CmsUserFeature>())
            {
                builder.Entity<CmsUser>(b =>
                {
                    b.ToTable(options.TablePrefix + "Users", options.Schema);

                    b.ConfigureByConvention();
                    b.ConfigureAbpUser();

                    b.HasIndex(x => new { x.TenantId, x.UserName });
                    b.HasIndex(x => new { x.TenantId, x.Email });
                });
            }
            else
            {
                builder.Ignore<CmsUser>();
            }

            if (GlobalFeatureManager.Instance.IsEnabled<ReactionsFeature>())
            {
                builder.Entity<UserReaction>(b =>
                {
                    b.ToTable(options.TablePrefix + "UserReactions", options.Schema);

                    b.ConfigureByConvention();

                    b.Property(x => x.EntityType).IsRequired().HasMaxLength(UserReactionConsts.MaxEntityTypeLength);
                    b.Property(x => x.EntityId).IsRequired().HasMaxLength(UserReactionConsts.MaxEntityIdLength);
                    b.Property(x => x.ReactionName).IsRequired().HasMaxLength(UserReactionConsts.MaxReactionNameLength);

                    b.HasIndex(x => new { x.TenantId, x.EntityType, x.EntityId, x.ReactionName });
                    b.HasIndex(x => new { x.TenantId, x.CreatorId, x.EntityType, x.EntityId, x.ReactionName });
                });
            }
            else
            {
                builder.Ignore<UserReaction>();
            }

            if (GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>())
            {
                builder.Entity<Comment>(b =>
                {
                    b.ToTable(options.TablePrefix + "Comments", options.Schema);

                    b.ConfigureByConvention();

                    b.Property(x => x.EntityType).IsRequired().HasMaxLength(CommentConsts.MaxEntityTypeLength);
                    b.Property(x => x.EntityId).IsRequired().HasMaxLength(CommentConsts.MaxEntityIdLength);
                    b.Property(x => x.Text).IsRequired().HasMaxLength(CommentConsts.MaxTextLength);
                    b.Property(x => x.RepliedCommentId);

                    b.HasIndex(x => new { x.TenantId, x.EntityType, x.EntityId });
                    b.HasIndex(x => new { x.TenantId, x.RepliedCommentId });
                });
            }
            else
            {
                builder.Ignore<Comment>();
            }

            if (GlobalFeatureManager.Instance.IsEnabled<RatingsFeature>())
            {
                builder.Entity<Rating>(r =>
                {
                    r.ToTable(options.TablePrefix + "Ratings", options.Schema);

                    r.ConfigureByConvention();

                    r.Property(x => x.StarCount).IsRequired();
                    r.Property(x => x.EntityType).IsRequired().HasMaxLength(RatingConsts.MaxEntityTypeLength);
                    r.Property(x => x.EntityId).IsRequired().HasMaxLength(RatingConsts.MaxEntityIdLength);

                    r.HasIndex(x => new { x.TenantId, x.EntityType, x.EntityId, x.CreatorId });
                });
            }
            else
            {
                builder.Ignore<Rating>();
            }

            if (GlobalFeatureManager.Instance.IsEnabled<TagsFeature>())
            {
                builder.Entity<Tag>(b =>
                {
                    b.ToTable(options.TablePrefix + "Tags", options.Schema);

                    b.ConfigureByConvention();

                    b.Property(x => x.EntityType).IsRequired().HasMaxLength(TagConsts.MaxEntityTypeLength);
                    b.Property(x => x.Name).IsRequired().HasMaxLength(TagConsts.MaxNameLength);

                    b.HasIndex(x => new
                    {
                        x.TenantId,
                        x.Name
                    });
                });

                builder.Entity<EntityTag>(b =>
                {
                    b.ToTable(options.TablePrefix + "EntityTags", options.Schema);

                    b.ConfigureByConvention();

                    b.HasKey(x => new { x.EntityId, x.TagId });

                    b.Property(x => x.EntityId).IsRequired();
                    b.Property(x => x.TagId).IsRequired();

                    b.HasIndex(x => new { x.TenantId, x.EntityId, x.TagId });
                });
            }
            else
            {
                builder.Ignore<EntityTag>();
                builder.Ignore<Tag>();
            }

            if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
            {
                builder.Entity<Page>(b =>
                {
                    b.ToTable(options.TablePrefix + "Pages", options.Schema);

                    b.ConfigureByConvention();

                    b.Property(x => x.Title).IsRequired().HasMaxLength(PageConsts.MaxTitleLength);
                    b.Property(x => x.Slug).IsRequired().HasMaxLength(PageConsts.MaxSlugLength);
                    b.Property(x => x.Content).HasMaxLength(PageConsts.MaxContentLength);

                    b.HasIndex(x => new { x.TenantId, Url = x.Slug });
                });
            }
            else
            {
                builder.Ignore<Page>();
            }

            if (GlobalFeatureManager.Instance.IsEnabled<BlogsFeature>())
            {
                builder.Entity<Blog>(b =>
                {
                    b.ToTable(options.TablePrefix + "Blogs", options.Schema);

                    b.ConfigureByConvention();

                    b.Property(p => p.Name).IsRequired().HasMaxLength(BlogConsts.MaxNameLength);

                    b.Property(p => p.Slug).IsRequired().HasMaxLength(BlogConsts.MaxSlugLength);
                });

                builder.Entity<BlogPost>(b =>
                {
                    b.ToTable(options.TablePrefix + "BlogPosts", options.Schema);

                    b.ConfigureByConvention();

                    b.Property(p => p.AuthorId).IsRequired();
                    b.Property(p => p.Title).IsRequired().HasMaxLength(BlogPostConsts.MaxTitleLength);
                    b.Property(p => p.Slug).IsRequired().HasMaxLength(BlogPostConsts.MaxSlugLength);
                    b.Property(p => p.ShortDescription).HasMaxLength(BlogPostConsts.MaxShortDescriptionLength);
                    b.Property(p => p.Content).HasMaxLength(BlogPostConsts.MaxContentLength);

                    b.HasIndex(x => new { x.Slug, x.BlogId });
                });

                builder.Entity<BlogFeature>(b =>
                {
                    b.ToTable(options.TablePrefix + "BlogFeatures", options.Schema);

                    b.ConfigureByConvention();

                    b.Property(p => p.FeatureName).IsRequired().HasMaxLength(BlogFeatureConsts.MaxFeatureNameLenth);
                });
            }
            else
            {
                builder.Ignore<Blog>();
                builder.Ignore<BlogPost>();
                builder.Ignore<BlogFeature>();
            }

            if (GlobalFeatureManager.Instance.IsEnabled<MediaFeature>())
            {
                builder.Entity<MediaDescriptor>(b =>
                {
                    b.ToTable(options.TablePrefix + "MediaDescriptors", options.Schema);

                    b.ConfigureByConvention();

                    b.Property(x => x.EntityType).IsRequired().HasMaxLength(MediaDescriptorConsts.MaxEntityTypeLength);
                    b.Property(x => x.Name).IsRequired().HasMaxLength(MediaDescriptorConsts.MaxNameLength);
                    b.Property(x => x.MimeType).IsRequired().HasMaxLength(MediaDescriptorConsts.MaxMimeTypeLength);
                    b.Property(x => x.Size).HasMaxLength(MediaDescriptorConsts.MaxSizeLength);
                });
            }
            else
            {
                builder.Ignore<MediaDescriptor>();
            }

            builder.TryConfigureObjectExtensions<CmsKitDbContext>();
        }
    }
}
