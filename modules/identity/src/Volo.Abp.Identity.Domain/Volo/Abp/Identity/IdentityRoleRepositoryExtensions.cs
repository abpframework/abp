using Volo.Abp.Threading;

namespace Volo.Abp.Identity
{
    public static class IdentityRoleRepositoryExtensions
    {
        public static IdentityRole FindByNormalizedName(this IIdentityRoleRepository roleRepository, string normalizedRoleName)
        {
            return AsyncHelper.RunSync(() => roleRepository.FindByNormalizedNameAsync(normalizedRoleName));
        }

        //TODO: Other sync extension methods
    }
}