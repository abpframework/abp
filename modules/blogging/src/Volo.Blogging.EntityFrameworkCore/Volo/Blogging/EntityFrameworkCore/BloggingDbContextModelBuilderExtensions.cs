﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Users.EntityFrameworkCore;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Users;

namespace Volo.Blogging.EntityFrameworkCore
{
    public static class BloggingDbContextModelBuilderExtensions
    {
        public static void ConfigureBlogging(
            [NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            if (builder.IsTenantOnlyDatabase())
            {
                return;
            }

            builder.Entity<BlogUser>(b =>
            {
                b.ToTable(AbpBloggingDbProperties.DbTablePrefix + "Users", AbpBloggingDbProperties.DbSchema);

                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<Blog>(b =>
            {
                b.ToTable(AbpBloggingDbProperties.DbTablePrefix + "Blogs", AbpBloggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(BlogConsts.MaxNameLength).HasColumnName(nameof(Blog.Name));
                b.Property(x => x.ShortName).IsRequired().HasMaxLength(BlogConsts.MaxShortNameLength).HasColumnName(nameof(Blog.ShortName));
                b.Property(x => x.Description).IsRequired(false).HasMaxLength(BlogConsts.MaxDescriptionLength).HasColumnName(nameof(Blog.Description));

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<Post>(b =>
            {
                b.ToTable(AbpBloggingDbProperties.DbTablePrefix + "Posts", AbpBloggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.BlogId).HasColumnName(nameof(Post.BlogId));
                b.Property(x => x.Title).IsRequired().HasMaxLength(PostConsts.MaxTitleLength).HasColumnName(nameof(Post.Title));
                b.Property(x => x.CoverImage).IsRequired().HasColumnName(nameof(Post.CoverImage));
                b.Property(x => x.Url).IsRequired().HasMaxLength(PostConsts.MaxUrlLength).HasColumnName(nameof(Post.Url));
                b.Property(x => x.Content).IsRequired(false).HasMaxLength(PostConsts.MaxContentLength).HasColumnName(nameof(Post.Content));
                b.Property(x => x.Description).IsRequired(false).HasMaxLength(PostConsts.MaxDescriptionLength).HasColumnName(nameof(Post.Description));

                b.HasMany(p => p.Tags).WithOne().HasForeignKey(qt => qt.PostId);

                b.HasOne<Blog>().WithMany().IsRequired().HasForeignKey(p => p.BlogId);

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<Comment>(b =>
            {
                b.ToTable(AbpBloggingDbProperties.DbTablePrefix + "Comments", AbpBloggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Text).IsRequired().HasMaxLength(CommentConsts.MaxTextLength).HasColumnName(nameof(Comment.Text));
                b.Property(x => x.RepliedCommentId).HasColumnName(nameof(Comment.RepliedCommentId));
                b.Property(x => x.PostId).IsRequired().HasColumnName(nameof(Comment.PostId));

                b.HasOne<Comment>().WithMany().HasForeignKey(p => p.RepliedCommentId);
                b.HasOne<Post>().WithMany().IsRequired().HasForeignKey(p => p.PostId);

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<Tag>(b =>
            {
                b.ToTable(AbpBloggingDbProperties.DbTablePrefix + "Tags", AbpBloggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(TagConsts.MaxNameLength).HasColumnName(nameof(Tag.Name));
                b.Property(x => x.Description).HasMaxLength(TagConsts.MaxDescriptionLength).HasColumnName(nameof(Tag.Description));
                b.Property(x => x.UsageCount).HasColumnName(nameof(Tag.UsageCount));

                b.HasMany<PostTag>().WithOne().HasForeignKey(qt => qt.TagId);

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<PostTag>(b =>
            {
                b.ToTable(AbpBloggingDbProperties.DbTablePrefix + "PostTags", AbpBloggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.PostId).HasColumnName(nameof(PostTag.PostId));
                b.Property(x => x.TagId).HasColumnName(nameof(PostTag.TagId));

                b.HasKey(x => new { x.PostId, x.TagId });

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<BloggingDbContext>();
        }
    }
}
