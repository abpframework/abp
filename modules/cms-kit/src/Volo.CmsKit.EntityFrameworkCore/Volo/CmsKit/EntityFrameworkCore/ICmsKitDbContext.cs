using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit.EntityFrameworkCore
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public interface ICmsKitDbContext : IEfCoreDbContext
    {
         DbSet<UserReaction> UserReactions { get; }
    }
}
