using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.MongoDB;

[ConnectionStringName(OpenIddictDbProperties.ConnectionStringName)]
public interface IOpenIddictMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<OpenIddictApplication> Applications { get; }

    IMongoCollection<OpenIddictAuthorization> Authorizations { get; }

    IMongoCollection<OpenIddictScope> Scopes { get; }

    IMongoCollection<OpenIddictToken> Tokens { get; }
}
