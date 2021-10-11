using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.EntityFrameworkCore
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public class CmsKitDbContext : AbpDbContext<CmsKitDbContext>, ICmsKitDbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CmsUser> User { get; set; }
        public DbSet<UserReaction> Reactions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EntityTag> EntityTags { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogFeature> BlogFeatures { get; set; }
        public DbSet<MediaDescriptor> MediaDescriptors { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        public CmsKitDbContext(DbContextOptions<CmsKitDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureCmsKit();
        }
    }
}
