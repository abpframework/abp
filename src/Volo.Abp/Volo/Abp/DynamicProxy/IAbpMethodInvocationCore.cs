using System;
using System.Reflection;

namespace Volo.Abp.DynamicProxy
{
    public interface IAbpMethodInvocationCore
    {
        object[] Arguments { get; }

        Type[] GenericArguments { get; }

        object TargetObject { get; }

        MethodInfo Method { get; }

        object ReturnValue { get; set; }
    }
}