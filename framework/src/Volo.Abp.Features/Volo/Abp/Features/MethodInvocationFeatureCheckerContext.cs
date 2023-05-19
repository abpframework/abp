using System.Reflection;

namespace Volo.Abp.Features;

public class MethodInvocationFeatureCheckerContext
{
    public MethodInfo Method { get; }

    public MethodInvocationFeatureCheckerContext(MethodInfo method)
    {
        Method = method;
    }
}
