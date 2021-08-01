using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.Account.Web.AbpGrantTypes
{
    public class AbpClientCredentialsGrantTypeProvider : IGrantTypeProvider, ITransientDependency
    {
        public string GrantType => GrantTypes.ClientCredentials;

        protected IOpenIddictApplicationManager ApplicationManager { get; }

        protected IOpenIddictScopeManager ScopeManager { get; }

        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }

        protected IOpenIddictDestinationService OpenIddictDestinationService { get; }

        protected IAbpLazyServiceProvider LazyServiceProvider { get; }

        protected IStringLocalizer<AbpOpenIddictWebResource> L { get; }

        public ILogger<AbpClientCredentialsGrantTypeProvider> Logger { get; set; }

        public AbpClientCredentialsGrantTypeProvider(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOpenIddictDestinationService openIddictDestinationService,
            IAbpLazyServiceProvider lazyServiceProvider,
            IStringLocalizer<AbpOpenIddictWebResource> localizer)
        {
            ApplicationManager = applicationManager;
            ScopeManager = scopeManager;
            IdentitySecurityLogManager = identitySecurityLogManager;
            OpenIddictDestinationService = openIddictDestinationService;
            LazyServiceProvider = lazyServiceProvider;
            L = localizer;
            Logger = NullLogger<AbpClientCredentialsGrantTypeProvider>.Instance;
        }

        public async Task<GrantTypeResult> HandleAsync(OpenIddictRequest request)
        {
            // Note: the client credentials are automatically validated by OpenIddict:
            // if client_id or client_secret are invalid, this action won't be invoked.

            var application =
                await ApplicationManager.FindByClientIdAsync(request.ClientId) ??
                throw new AbpException("The application cannot be found.");

            // Create a new ClaimsIdentity containing the claims that
            // will be used to create an id_token, a token or a code.
            var identity = new ClaimsIdentity(
                TokenValidationParameters.DefaultAuthenticationType,
                Claims.Name, Claims.Role);

            var clientId = await ApplicationManager.GetClientIdAsync(application);

            // Use the client_id as the subject identifier.
            identity.AddClaim(Claims.Subject,
                clientId,
                Destinations.AccessToken, Destinations.IdentityToken);

            identity.AddClaim(AbpClaimTypes.ClientId,
                clientId,
                Destinations.AccessToken, Destinations.IdentityToken);

            identity.AddClaim(Claims.Name,
                await ApplicationManager.GetDisplayNameAsync(application),
                Destinations.AccessToken, Destinations.IdentityToken);

            var principal = new ClaimsPrincipal(identity);

            principal.SetScopes(request.GetScopes());
            principal.SetResources(await ScopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

            return GrantTypeResult.SuccessResult(principal);
        }
    }
}
