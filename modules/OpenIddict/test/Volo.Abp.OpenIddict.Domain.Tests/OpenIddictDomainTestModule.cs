using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.OpenIddict;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(OpenIddictEntityFrameworkCoreTestModule)
    )]
public class OpenIddictDomainTestModule : AbpModule
{

}
