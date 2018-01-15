using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy.EntityFrameworkCore;

namespace Volo.Abp.MultiTenancy
{
    public class EfCoreTenantRepository : EfCoreRepository<IMultiTenancyDbContext, Tenant, Guid>, ITenantRepository
    {
        public EfCoreTenantRepository(IDbContextProvider<IMultiTenancyDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<Tenant> FindByNameIncludeDetailsAsync(string name)
        {
            return DbSet
                .Include(t => t.ConnectionStrings) //TODO: Why not creating a virtual Include method in EfCoreRepository and override to add included properties to be available for every query..?
                .FirstOrDefaultAsync(t => t.Name == name);
        }

        public Task<Tenant> FindWithIncludeDetailsAsync(Guid id)
        {
            return DbSet
                .Include(t => t.ConnectionStrings) //TODO: Why not creating a virtual Include method in EfCoreRepository and override to add included properties to be available for every query..?
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
