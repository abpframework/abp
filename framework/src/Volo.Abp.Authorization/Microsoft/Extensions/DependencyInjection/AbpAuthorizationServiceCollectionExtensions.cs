using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpAuthorizationServiceCollectionExtensions
    {
        //TODO: Remove this and use AddAlwaysAllowAuthorization
        [Obsolete("Use AddAlwaysAllowAuthorization instead")]
        public static IServiceCollection AddAlwaysAllowPermissionChecker(this IServiceCollection services)
        {
            return services.Replace(ServiceDescriptor.Singleton<IPermissionChecker, AlwaysAllowPermissionChecker>());
        }

        public static IServiceCollection AddAlwaysAllowAuthorization(this IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton<IAuthorizationService, AlwaysAllowAuthorizationService>());
            services.Replace(ServiceDescriptor.Singleton<IAbpAuthorizationService, AlwaysAllowAuthorizationService>());
            return services.Replace(ServiceDescriptor.Singleton<IPermissionChecker, AlwaysAllowPermissionChecker>());
        }
    }
}
