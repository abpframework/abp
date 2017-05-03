using System;
using System.Reflection;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class CastleAbpMethodInvocationAdapter : IAbpMethodInvocation
    {
        public object[] Arguments => _invocation.Arguments;

        public Type[] GenericArguments => _invocation.GenericArguments;

        public object TargetObject => _invocation.InvocationTarget;

        public MethodInfo Method => _invocation.MethodInvocationTarget;

        public object ReturnValue
        {
            get => _invocation.ReturnValue;
            set => _invocation.ReturnValue = value;
        }

        private readonly IInvocation _invocation;

        public CastleAbpMethodInvocationAdapter(IInvocation invocation)
        {
            _invocation = invocation;
        }

        public void Proceed()
        {
            _invocation.Proceed();
        }
    }
}