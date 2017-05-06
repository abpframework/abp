using System;
using System.Reflection;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Castle.DynamicProxy
{
    public abstract class CastleAbpMethodInvocationAdapterBase : IAbpMethodInvocationCore
    {
        public object[] Arguments => Invocation.Arguments;

        public Type[] GenericArguments => Invocation.GenericArguments;

        public object TargetObject => Invocation.InvocationTarget;

        public MethodInfo Method => Invocation.MethodInvocationTarget;

        public object ReturnValue
        {
            get => Invocation.ReturnValue;
            set => Invocation.ReturnValue = value;
        }

        protected IInvocation Invocation { get; }

        protected CastleAbpMethodInvocationAdapterBase(IInvocation invocation)
        {
            Invocation = invocation;
        }
    }
}