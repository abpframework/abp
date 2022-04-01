using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpOpenIddictDbProperties.ConnectionStringName)]
public interface IOpenIddictDbContext : IEfCoreDbContext
{
    DbSet<OpenIddictApplication> Applications { get; }

    DbSet<OpenIddictAuthorization> Authorizations { get; }

    DbSet<OpenIddictScope> Scopes { get; }

    DbSet<OpenIddictToken> Tokens { get; }
}
