using System;
using System.Reflection;

namespace Volo.Abp.DynamicProxy
{
    public interface IAbpMethodInvocation
    {
        object[] Arguments { get; }

        Type[] GenericArguments { get; }

        object InvocationTarget { get; }

        MethodInfo MethodInvocationTarget { get; }

        object ReturnValue { get; set; }

        void Proceed();
    }
}