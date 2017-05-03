using Volo.Abp.DynamicProxy;
using Volo.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkInterceptor : IAbpInterceptor, ITransientDependency
    {
        public void Intercept(IAbpMethodInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}
