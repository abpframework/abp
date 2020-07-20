using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    [ConnectionStringName(AbpIdentityDbProperties.ConnectionStringName)]
    public class AbpIdentityMongoDbContext : AbpMongoDbContext, IAbpIdentityMongoDbContext
    {
        public IMongoCollection<IdentityUser> Users => Collection<IdentityUser>();

        public IMongoCollection<IdentityRole> Roles => Collection<IdentityRole>();

        public IMongoCollection<IdentityClaimType> ClaimTypes => Collection<IdentityClaimType>();

        public IMongoCollection<OrganizationUnit> OrganizationUnits => Collection<OrganizationUnit>();

        public IMongoCollection<IdentitySecurityLog> IdentitySecurityLogs => Collection<IdentitySecurityLog>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureIdentity();
        }
    }
}
