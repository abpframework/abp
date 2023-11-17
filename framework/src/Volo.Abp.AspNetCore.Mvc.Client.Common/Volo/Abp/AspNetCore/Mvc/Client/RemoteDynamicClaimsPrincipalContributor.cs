using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Mvc.Client;

[DisableConventionalRegistration]
public class RemoteDynamicClaimsPrincipalContributor : RemoteDynamicClaimsPrincipalContributorBase<RemoteDynamicClaimsPrincipalContributor, RemoteDynamicClaimsPrincipalContributorCache>
{

}
