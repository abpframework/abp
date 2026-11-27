using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Volo.Abp.Castle.DynamicProxy;

internal static class CastleByRefParameterChecker
{
    private static readonly ConcurrentDictionary<MethodInfo, bool> MethodsWithByRefParameters = new();

    public static bool HasByRefParameters(MethodInfo method)
    {
        return MethodsWithByRefParameters.GetOrAdd(method, m => m.GetParameters().Any(p => p.ParameterType.IsByRef));
    }
}
