using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    [ConnectionStringName(AbpIdentityDbProperties.ConnectionStringName)]
    public interface IIdentityDbContext : IEfCoreDbContext
    {
        DbSet<IdentityUser> Users { get; }

        DbSet<IdentityRole> Roles { get; }

        DbSet<IdentityClaimType> ClaimTypes { get; }

        DbSet<OrganizationUnit> OrganizationUnits { get; }

        DbSet<IdentitySecurityLog> SecurityLogs { get; }

        DbSet<IdentityLinkUser> LinkUsers { get; }
    }
}
