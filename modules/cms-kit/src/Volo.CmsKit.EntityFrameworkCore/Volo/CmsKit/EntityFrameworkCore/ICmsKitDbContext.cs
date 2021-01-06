using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.EntityFrameworkCore
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public interface ICmsKitDbContext : IEfCoreDbContext
    {
        DbSet<Content> Contents { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<CmsUser> User { get; set; }
        DbSet<UserReaction> Reactions { get; set; }
        DbSet<Rating> Ratings { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<EntityTag> EntityTags { get; set; }
        DbSet<Page> Pages { get; set; }
    }
}