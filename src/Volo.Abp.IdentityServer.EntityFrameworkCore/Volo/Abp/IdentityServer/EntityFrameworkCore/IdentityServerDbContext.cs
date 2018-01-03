using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    public class IdentityServerDbContext : AbpDbContext<IdentityServerDbContext>
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        public DbSet<ApiResource> ApiResources { get; set; }

        public DbSet<IdentityResource> IdentityResources { get; set; }

        //TODO: Add child entity types too...

        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options) 
            : base(options)
        {

        }
    }
}
