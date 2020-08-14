using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.SecurityLog;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    [ConnectionStringName(AbpIdentityDbProperties.ConnectionStringName)]
    public interface IIdentityDbContext : IEfCoreDbContext
    {
        DbSet<IdentityUser> Users { get; set; }

        DbSet<IdentityRole> Roles { get; set; }

        DbSet<IdentityClaimType> ClaimTypes { get; set; }

        DbSet<OrganizationUnit> OrganizationUnits { get; set; }

        DbSet<IdentitySecurityLog> IdentitySecurityLogs { get; set; }
    }
}
