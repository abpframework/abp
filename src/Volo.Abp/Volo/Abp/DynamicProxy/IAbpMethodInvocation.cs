using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy
{
    public interface IAbpMethodInvocation
    {
        object[] Arguments { get; }

        Type[] GenericArguments { get; }

        object TargetObject { get; }

        MethodInfo Method { get; }

        object ReturnValue { get; set; }

        Task ProceedAsync();
    }
}