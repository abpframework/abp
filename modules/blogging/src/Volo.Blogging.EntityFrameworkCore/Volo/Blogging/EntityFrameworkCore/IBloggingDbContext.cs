using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Users;

namespace Volo.Blogging.EntityFrameworkCore
{
    [ConnectionStringName(BloggingDbProperties.ConnectionStringName)]
    public interface IBloggingDbContext : IEfCoreDbContext
    {
        DbSet<BlogUser> Users { get; }

        DbSet<Blog> Blogs { get; set; }

        DbSet<Post> Posts { get; set; }

        DbSet<Comment> Comments { get; set; }

        DbSet<PostTag> PostTags { get; set; }

        DbSet<Tag> Tags { get; set; }
    }
}