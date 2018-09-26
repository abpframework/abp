using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    [ConnectionStringName("AbpIdentity")]
    public interface IAbpIdentityMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<IdentityUser> Users { get; }

        IMongoCollection<IdentityRole> Roles { get; }

        IMongoCollection<IdentityClaimType> ClaimTypes { get; }
    }
}