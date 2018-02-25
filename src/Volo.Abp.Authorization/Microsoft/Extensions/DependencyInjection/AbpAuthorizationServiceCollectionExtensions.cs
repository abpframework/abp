using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Authorization.Permissions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpAuthorizationServiceCollectionExtensions
    {
        public static IServiceCollection AddAlwaysAllowPermissionChecker(this IServiceCollection services)
        {
            return services.Replace(ServiceDescriptor.Singleton<IPermissionChecker, AlwaysAllowPermissionChecker>());
        }
    }
}
