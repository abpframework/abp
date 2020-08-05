using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.EntityFrameworkCore
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public interface ICmsKitDbContext : IEfCoreDbContext
    {
         DbSet<UserReaction> UserReactions { get; }

         DbSet<Comment> Comments { get; }

         DbSet<CmsUser> CmsUsers { get; set; }
    }
}
