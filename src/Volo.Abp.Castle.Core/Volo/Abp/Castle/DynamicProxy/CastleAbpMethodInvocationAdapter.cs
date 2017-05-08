using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class CastleAbpMethodInvocationAdapter : IAbpMethodInvocation
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

        public CastleAbpMethodInvocationAdapter(IInvocation invocation)
        {
            Invocation = invocation;
        }

	    public void Proceed()
	    {
			Invocation.Proceed();

			if (Invocation.Method.IsAsync())
			{
				AsyncHelper.RunSync(() => (Task) Invocation.ReturnValue);
			}
	    }

	    public Task ProceedAsync()
	    {
		    Invocation.Proceed();

		    return Invocation.Method.IsAsync()
			    ? (Task) Invocation.ReturnValue
			    : Task.FromResult(Invocation.ReturnValue);
	    }
	}
}