using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    [ConnectionStringName("AbpIdentity")]
    public interface IIdentityDbContext : IEfCoreDbContext
    {
        DbSet<IdentityUser> Users { get; set; }

        DbSet<IdentityUserClaim> UserClaims { get; set; }

        DbSet<IdentityUserLogin> UserLogins { get; set; }

        DbSet<IdentityUserRole> UserRoles { get; set; }

        DbSet<IdentityUserToken> UserTokens { get; set; }

        DbSet<IdentityRole> Roles { get; set; }

        DbSet<IdentityRoleClaim> RoleClaims { get; set; }
    }
}