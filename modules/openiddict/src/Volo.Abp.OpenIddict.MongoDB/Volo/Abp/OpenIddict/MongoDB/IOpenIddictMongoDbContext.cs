using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.MongoDB
{
    [ConnectionStringName(AbpOpenIddictDbProperties.ConnectionStringName)]
    public interface IOpenIddictMongoDbContext : IAbpMongoDbContext
    {
        public IMongoCollection<OpenIddictApplication> Applications { get; }

        public IMongoCollection<OpenIddictAuthorization> Authorizations { get; }

        public IMongoCollection<OpenIddictScope> Scopes { get; }

        public IMongoCollection<OpenIddictToken> Tokens { get; }
    }
}
