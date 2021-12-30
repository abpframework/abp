using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features;

public class MethodInvocationFeatureCheckerService : IMethodInvocationFeatureCheckerService, ITransientDependency
{
    private readonly IFeatureChecker _featureChecker;

    public MethodInvocationFeatureCheckerService(
        IFeatureChecker featureChecker)
    {
        _featureChecker = featureChecker;
    }

    public async Task CheckAsync(MethodInvocationFeatureCheckerContext context)
    {
        if (IsFeatureCheckDisabled(context))
        {
            return;
        }

        foreach (var requiresFeatureAttribute in GetRequiredFeatureAttributes(context.Method))
        {
            await _featureChecker.CheckEnabledAsync(requiresFeatureAttribute.RequiresAll, requiresFeatureAttribute.Features);
        }
    }

    protected virtual bool IsFeatureCheckDisabled(MethodInvocationFeatureCheckerContext context)
    {
        return context.Method
            .GetCustomAttributes(true)
            .OfType<DisableFeatureCheckAttribute>()
            .Any();
    }

    protected virtual IEnumerable<RequiresFeatureAttribute> GetRequiredFeatureAttributes(MethodInfo methodInfo)
    {
        var attributes = methodInfo
            .GetCustomAttributes(true)
            .OfType<RequiresFeatureAttribute>();

        if (methodInfo.IsPublic)
        {
            attributes = attributes
                .Union(
                    methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<RequiresFeatureAttribute>()
                );
        }

        return attributes;
    }
}
