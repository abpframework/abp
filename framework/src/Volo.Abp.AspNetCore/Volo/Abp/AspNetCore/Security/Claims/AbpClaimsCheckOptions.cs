using Volo.Abp.Collections;

namespace Volo.Abp.AspNetCore.Security.Claims;

public class AbpClaimsCheckOptions
{
    public ITypeList<IAbpClaimsChecker> Checker { get; }

    public AbpClaimsCheckOptions()
    {
        Checker = new TypeList<IAbpClaimsChecker>();
    }
}
