using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.MongoDB;

public static class OpenIddictMongoDbContextExtensions
{
    public static void ConfigureOpenIddict(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
        
        builder.Entity<OpenIddictApplication>(b =>
        {
            b.CollectionName = AbpOpenIddictDbProperties.DbTablePrefix + "Applications";
        });
        
        builder.Entity<OpenIddictAuthorization>(b =>
        {
            b.CollectionName = AbpOpenIddictDbProperties.DbTablePrefix + "Authorizations";
        });
        
        builder.Entity<OpenIddictScope>(b =>
        {
            b.CollectionName = AbpOpenIddictDbProperties.DbTablePrefix + "Scopes";
        });
        
        builder.Entity<OpenIddictToken>(b =>
        {
            b.CollectionName = AbpOpenIddictDbProperties.DbTablePrefix + "Tokens";
        });
    }
}
