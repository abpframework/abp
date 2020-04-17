using System;
using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Users;

namespace Volo.Blogging.MongoDB
{
    public static class BloggingMongoDbContextExtensions
    {
        public static void ConfigureBlogging(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BloggingMongoModelBuilderConfigurationOptions(
                BloggingDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<BlogUser>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Users";
            });

            builder.Entity<Blog>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Blogs";
            });

            builder.Entity<Post>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Posts";
            });

            builder.Entity<Tagging.Tag>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Tags";
            });

            builder.Entity<Comment>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Comments";
            });
        }
    }
}
