using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Users;

namespace Volo.Blogging.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpBloggingDbProperties.ConnectionStringName)]
    public interface IBloggingDbContext : IEfCoreDbContext
    {
        DbSet<BlogUser> Users { get; }

        DbSet<Blog> Blogs { get; }

        DbSet<Post> Posts { get; }

        DbSet<Comment> Comments { get; }

        DbSet<PostTag> PostTags { get; }

        DbSet<Tag> Tags { get; }
    }
}
