using Volo.Abp.DynamicProxy;
using Volo.Abp.TestBase.Logging;
using Volo.DependencyInjection;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class SimpleInterceptor : IAbpInterceptor, ITransientDependency
    {
        public void Intercept(IAbpMethodInvocation invocation)
        {
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("BeforeInvocation");
            invocation.Proceed();
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("AfterInvocation");
        }
    }
}