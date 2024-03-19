using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.PermissionManagement;

public static class PermissionFinderExtensions
{
    public async static Task<bool> IsGrantedAsync(this IPermissionFinder permissionFinder, Guid userId, string permissionName)
    {
        return await permissionFinder.IsGrantedAsync(userId, new[] { permissionName });
    }

    public async static Task<bool> IsGrantedAsync(this IPermissionFinder permissionFinder, Guid userId, string[] permissionNames)
    {
        return (await permissionFinder.IsGrantedAsync(new List<IsGrantedRequest>
        {
            new IsGrantedRequest
            {
                UserId = userId,
                PermissionNames = permissionNames
            }
        })).Any(x => x.UserId == userId && x.Permissions.All(p => permissionNames.Contains(p.Key) && p.Value));
    }
}
