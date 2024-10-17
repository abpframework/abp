using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectExtending;

public class ExtensionPropertyPolicyChecker : ITransientDependency
{
    public virtual async Task<bool> CheckPolicyAsync([NotNull] ExtensionPropertyPolicyConfiguration policy)
    {
        if (!await CheckAsync(policy.GlobalFeatures.Features, policy.GlobalFeatures.RequiresAll, CheckGlobalFeaturesAsync))
        {
            return false;
        }

        if (!await CheckAsync(policy.Features.Features, policy.Features.RequiresAll, CheckFeaturesAsync))
        {
            return false;
        }

        return await CheckAsync(policy.Permissions.PermissionNames, policy.Permissions.RequiresAll, CheckPermissionsAsync);
    }

    protected virtual async Task<bool> CheckAsync(string[] names, bool requiresAll, Func<string, Task<bool>> checkFunc)
    {
        if (names.IsNullOrEmpty())
        {
            return true;
        }

        var hasAny = false;
        foreach (var name in names)
        {
            if (!await checkFunc(name))
            {
                if (requiresAll)
                {
                    return false;
                }
            }
            else
            {
                hasAny = true;
                if (!requiresAll)
                {
                    break;
                }
            }
        }

        return hasAny;
    }

    protected virtual Task<bool> CheckGlobalFeaturesAsync(string featureName)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> CheckFeaturesAsync(string featureName)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> CheckPermissionsAsync(string permissionName)
    {
        return Task.FromResult(true);
    }
}
