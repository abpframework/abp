using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;

namespace Volo.Abp.Account.Web;

[DependsOn(
    typeof(AbpAccountWebTestModule),
    typeof(AbpAccountWebIdentityServerModule)
)]
public class AbpAccountWebIdentityServerTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // The other tests of the assembly run in the same process, so the static claim types are not changed.
        PreConfigure<AbpIdentityServerBuilderOptions>(options =>
        {
            options.UpdateAbpClaimTypes = false;
            options.UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap = false;
            options.AddDeveloperSigningCredential = false;
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // There is no authorize request in the tests.
        var interaction = Substitute.For<IIdentityServerInteractionService>();
        interaction.GetAuthorizationContextAsync(Arg.Any<string>()).Returns((AuthorizationRequest)null);
        context.Services.Replace(ServiceDescriptor.Singleton(interaction));

        // IdentityServer decorates the authentication service, the substitute of the tests is used instead.
        context.Services.RemoveAll<IAuthenticationService>();
        context.Services.AddSingleton(Substitute.For<IAuthenticationService>());
    }
}
