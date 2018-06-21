using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging.EntityFrameworkCore
{
    public static class BloggingDbContextModelBuilderExtensions
    {
        public static void ConfigureBlogging(
            [NotNull] this ModelBuilder builder,
            Action<BloggingModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BloggingModelBuilderConfigurationOptions();
            optionsAction?.Invoke(options);

            builder.Entity<Blog>(b =>
            {
                b.ToTable(options.TablePrefix + "Blogs", options.Schema);

                b.ConfigureFullAudited();

                b.Property(x => x.Name).IsRequired().HasMaxLength(BlogConsts.MaxNameLength).HasColumnName(nameof(Blog.Name));
                b.Property(x => x.ShortName).IsRequired().HasMaxLength(BlogConsts.MaxShortNameLength).HasColumnName(nameof(Blog.ShortName));
                b.Property(x => x.Description).IsRequired(false).HasMaxLength(BlogConsts.MaxDescriptionLength).HasColumnName(nameof(Blog.Description));
            });

            builder.Entity<Post>(b =>
            {
                b.ToTable(options.TablePrefix + "Posts", options.Schema);

                b.ConfigureFullAudited();
                
                b.Property(x => x.BlogId).HasColumnName(nameof(Post.BlogId));
                b.Property(x => x.Title).IsRequired().HasMaxLength(PostConsts.MaxTitleLength).HasColumnName(nameof(Post.Title));
                b.Property(x => x.Content).IsRequired(false).HasMaxLength(PostConsts.MaxContentLength).HasColumnName(nameof(Post.Content));

                b.HasOne<Blog>().WithMany().IsRequired().HasForeignKey(p => p.BlogId);
            });
        }
    }
}
