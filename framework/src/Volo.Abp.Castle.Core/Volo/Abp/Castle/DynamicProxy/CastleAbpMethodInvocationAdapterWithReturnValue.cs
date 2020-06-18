using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class CastleAbpMethodInvocationAdapterWithReturnValue<TResult> : CastleAbpMethodInvocationAdapterBase, IAbpMethodInvocation
    {
        protected IInvocationProceedInfo ProceedInfo { get; }
        protected Func<IInvocation, IInvocationProceedInfo, Task<TResult>> Proceed { get; }

        public CastleAbpMethodInvocationAdapterWithReturnValue(IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
            : base(invocation)
        {
            ProceedInfo = proceedInfo;
            Proceed = proceed;
        }

        public override async Task ProceedAsync()
        {
            ReturnValue = await Proceed(Invocation, ProceedInfo);
        }
    }
}