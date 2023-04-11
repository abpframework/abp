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
            this IMongoModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<BlogUser>(b =>
            {
                b.CollectionName = AbpBloggingDbProperties.DbTablePrefix + "Users";
            });

            builder.Entity<Blog>(b =>
            {
                b.CollectionName = AbpBloggingDbProperties.DbTablePrefix + "Blogs";
            });

            builder.Entity<Post>(b =>
            {
                b.CollectionName = AbpBloggingDbProperties.DbTablePrefix + "Posts";
            });

            builder.Entity<Tagging.Tag>(b =>
            {
                b.CollectionName = AbpBloggingDbProperties.DbTablePrefix + "Tags";
            });

            builder.Entity<Comment>(b =>
            {
                b.CollectionName = AbpBloggingDbProperties.DbTablePrefix + "Comments";
            });
        }
    }
}
