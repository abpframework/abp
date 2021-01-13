using Volo.Abp.Collections;

namespace Volo.Abp.Security.Claims
{
    public class AbpClaimOptions
    {
        public ITypeList<IClaimsIdentityContributor> ClaimsIdentityContributors { get; set; }

        public AbpClaimOptions()
        {
            ClaimsIdentityContributors = new TypeList<IClaimsIdentityContributor>();
        }
    }
}
