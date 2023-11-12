using System.Collections.Generic;
using System.Security.Claims;
using Volo.Abp.Collections;

namespace Volo.Abp.Security.Claims;

public class AbpClaimsPrincipalFactoryOptions
{
    public ITypeList<IAbpClaimsPrincipalContributor> Contributors { get; }

    public ITypeList<IAbpDynamicClaimsPrincipalContributor> DynamicContributors { get; }

    public List<string> DynamicClaims { get; }

    public string RemoteUrl { get; set; }

    public Dictionary<string, List<string>> ClaimsMap { get; set; }

    public AbpClaimsPrincipalFactoryOptions()
    {
        Contributors = new TypeList<IAbpClaimsPrincipalContributor>();
        DynamicContributors = new TypeList<IAbpDynamicClaimsPrincipalContributor>();

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

        ClaimsMap = new Dictionary<string, List<string>>()
        {
            { AbpClaimTypes.UserName, new List<string> { "preferred_username", "unique_name", ClaimTypes.Name }},
            { AbpClaimTypes.Name, new List<string> { "given_name", ClaimTypes.GivenName }},
            { AbpClaimTypes.SurName, new List<string> { "family_name", ClaimTypes.Surname }},
            { AbpClaimTypes.Role, new List<string> { "role", "roles", ClaimTypes.Role }},
            { AbpClaimTypes.Email, new List<string> { "email", ClaimTypes.Email }},
        };
    }
}
