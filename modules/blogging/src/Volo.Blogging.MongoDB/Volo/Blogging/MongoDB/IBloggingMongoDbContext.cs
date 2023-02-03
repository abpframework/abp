﻿using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Users;

namespace Volo.Blogging.MongoDB
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpBloggingDbProperties.ConnectionStringName)]
    public interface IBloggingMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<BlogUser> Users { get; }

        IMongoCollection<Blog> Blogs { get; }

        IMongoCollection<Post> Posts { get; }

        IMongoCollection<Tagging.Tag> Tags { get; }

        IMongoCollection<Comment> Comments { get; }

    }
}
