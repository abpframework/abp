using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Authorization.Permissions.Resources;

namespace Microsoft.Extensions.DependencyInjection;

public static class KeyedObjectResourcePermissionExtenstions
{
    public static IServiceCollection AddKeyedObjectResourcePermissionAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, KeyedObjectResourcePermissionRequirementHandler>();
        return services;
    }
}
