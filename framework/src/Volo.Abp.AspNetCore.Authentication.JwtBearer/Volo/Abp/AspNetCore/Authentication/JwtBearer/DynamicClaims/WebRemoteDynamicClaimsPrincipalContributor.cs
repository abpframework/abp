using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Authentication.JwtBearer.DynamicClaims;

[DisableConventionalRegistration]
public class WebRemoteDynamicClaimsPrincipalContributor : RemoteDynamicClaimsPrincipalContributorBase<WebRemoteDynamicClaimsPrincipalContributor, WebRemoteDynamicClaimsPrincipalContributorCache>
{

}
