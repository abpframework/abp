using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    [ConnectionStringName("AbpIdentity")]
    public class AbpIdentityMongoDbContext : AbpMongoDbContext, IAbpIdentityMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = AbpIdentityConsts.DefaultDbTablePrefix;

        public IMongoCollection<IdentityUser> Users => Collection<IdentityUser>();

        public IMongoCollection<IdentityRole> Roles => Collection<IdentityRole>();

        public IMongoCollection<IdentityClaimType> ClaimTypes => Collection<IdentityClaimType>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureIdentity(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}