using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Castle.DynamicProxy;

public class AbpAsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor
    where TInterceptor : IAbpInterceptor
{
    private readonly TInterceptor _abpInterceptor;

    public AbpAsyncDeterminationInterceptor(TInterceptor abpInterceptor)
        : base(new CastleAsyncAbpInterceptorAdapter<TInterceptor>(abpInterceptor))
    {
        _abpInterceptor = abpInterceptor;
    }

    public override void Intercept(IInvocation invocation)
    {
        if (CastleAsyncEnumerableAbpInterceptor.TryIntercept(invocation, _abpInterceptor))
        {
            return;
        }

        base.Intercept(invocation);
    }
}
