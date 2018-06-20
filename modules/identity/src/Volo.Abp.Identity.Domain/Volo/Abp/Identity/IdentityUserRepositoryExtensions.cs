using JetBrains.Annotations;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity
{
    public static class IdentityUserRepositoryExtensions
    {
        public static IdentityUser FindByNormalizedUserName(this IIdentityUserRepository repository, [NotNull] string normalizedUserName)
        {
            return AsyncHelper.RunSync(() => repository.FindByNormalizedUserNameAsync(normalizedUserName));
        }

        //TODO: Other sync extension methods
    }
}