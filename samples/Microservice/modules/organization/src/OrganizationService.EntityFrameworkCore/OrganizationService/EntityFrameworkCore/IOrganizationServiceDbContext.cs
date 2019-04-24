using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrganizationService.EntityFrameworkCore
{
    [ConnectionStringName("OrganizationService")]
    public interface IOrganizationServiceDbContext : IEfCoreDbContext
    {
         DbSet<AbpOrganization> AbpOrganizations { get; }
    }
}