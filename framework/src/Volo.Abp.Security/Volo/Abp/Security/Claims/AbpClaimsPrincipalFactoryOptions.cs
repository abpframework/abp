using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.Security.Claims;

public class AbpClaimsPrincipalFactoryOptions
{
    public ITypeList<IAbpClaimsPrincipalContributor> Contributors { get; }

    public ITypeList<IAbpClaimsPrincipalContributor> DynamicContributors { get; }

    public List<string> DynamicClaims { get; }

    public string RemoteUrl { get; set; }

    public AbpClaimsPrincipalFactoryOptions()
    {
        Contributors = new TypeList<IAbpClaimsPrincipalContributor>();
        DynamicContributors = new TypeList<IAbpClaimsPrincipalContributor>();

        DynamicClaims = new List<string>
        {
            AbpClaimTypes.UserName,
            AbpClaimTypes.Name,
            AbpClaimTypes.SurName,
            AbpClaimTypes.Role,
            AbpClaimTypes.Email,
            AbpClaimTypes.EmailVerified,
            AbpClaimTypes.PhoneNumber,
            AbpClaimTypes.PhoneNumberVerified
        };

        RemoteUrl = "/api/account/dynamic-claims";
    }
}
