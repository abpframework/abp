using Volo.Abp.Collections;

namespace Volo.Abp.Security.Claims;

public class AbpClaimsPrincipalFactoryOptions
{
    public ITypeList<IAbpClaimsPrincipalContributor> Contributors { get; }

    public ITypeList<IAbpClaimsPrincipalContributor> DynamicContributors { get; }

    public AbpClaimsPrincipalFactoryOptions()
    {
        Contributors = new TypeList<IAbpClaimsPrincipalContributor>();
        DynamicContributors = new TypeList<IAbpClaimsPrincipalContributor>();
    }
}
