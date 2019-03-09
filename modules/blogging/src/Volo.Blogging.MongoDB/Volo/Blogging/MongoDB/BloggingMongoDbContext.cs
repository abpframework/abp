using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Users;

namespace Volo.Blogging.MongoDB
{
    [ConnectionStringName("Blogging")]
    public class BloggingMongoDbContext : AbpMongoDbContext, IBloggingMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = BloggingConsts.DefaultDbTablePrefix;

        public IMongoCollection<BlogUser> Users => Collection<BlogUser>();

        public IMongoCollection<Blog> Blogs => Collection<Blog>();

        public IMongoCollection<Post> Posts => Collection<Post>();

        public IMongoCollection<Tagging.Tag> Tags => Collection<Tagging.Tag>();

        public IMongoCollection<Comment> Comments => Collection<Comment>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureBlogging(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}
