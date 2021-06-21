using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;
using Tag = Volo.CmsKit.Tags.Tag;

namespace Volo.CmsKit.MongoDB
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public class CmsKitMongoDbContext : AbpMongoDbContext, ICmsKitMongoDbContext
    {
        public IMongoCollection<Comment> Comments => Collection<Comment>();

        public IMongoCollection<UserReaction> UserReactions => Collection<UserReaction>();

        public IMongoCollection<CmsUser> CmsUsers => Collection<CmsUser>();

        public IMongoCollection<Rating> Ratings => Collection<Rating>();
        
        public IMongoCollection<Tag> Tags => Collection<Tag>();
        
        public IMongoCollection<EntityTag> EntityTags => Collection<EntityTag>();
        
        public IMongoCollection<Page> Pages => Collection<Page>();

        public IMongoCollection<Blog> Blogs => Collection<Blog>();

        public IMongoCollection<BlogPost> BlogPosts => Collection<BlogPost>();

        public IMongoCollection<BlogFeature> BlogFeatures => Collection<BlogFeature>();

        public IMongoCollection<MediaDescriptor> MediaDescriptors => Collection<MediaDescriptor>();

        public IMongoCollection<Menu> Menus => Collection<Menu>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureCmsKit();
        }
    }
}
