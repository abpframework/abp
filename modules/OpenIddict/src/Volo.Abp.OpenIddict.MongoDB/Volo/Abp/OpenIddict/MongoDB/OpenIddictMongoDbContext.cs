using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.MongoDB;

[ConnectionStringName(AbpOpenIddictDbProperties.ConnectionStringName)]
public class OpenIddictMongoDbContext : AbpMongoDbContext, IOpenIddictMongoDbContext
{
    public IMongoCollection<OpenIddictApplication> Applications => Collection<OpenIddictApplication>();

    public IMongoCollection<OpenIddictAuthorization> Authorizations => Collection<OpenIddictAuthorization>();

    public IMongoCollection<OpenIddictScope> Scopes => Collection<OpenIddictScope>();

    public IMongoCollection<OpenIddictToken> Tokens => Collection<OpenIddictToken>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureOpenIddict();
    }
}
