using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging.EntityFrameworkCore
{
    [ConnectionStringName("Blogging")]
    public interface IBloggingDbContext : IEfCoreDbContext
    {
        DbSet<Blog> Blogs { get; set; }

        DbSet<Post> Posts { get; set; }
    }
}