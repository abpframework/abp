using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrganizationService.EntityFrameworkCore
{
    [ConnectionStringName("OrganizationService")]
    public interface IOrganizationServiceDbContext : IEfCoreDbContext
    {
         DbSet<Organization> AbpOrganizations { get; }

        DbSet<UserOrganization> UserOrganizations { get; set; }
    }
}