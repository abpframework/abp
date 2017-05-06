using System.Threading.Tasks;
using Volo.Abp.DynamicProxy;
using Volo.Abp.TestBase.Logging;
using Volo.DependencyInjection;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class SimpleInterceptor : IAbpInterceptor, /*IAbpAsyncInterceptor,*/ ITransientDependency
    {
        public void Intercept(IAbpMethodInvocation invocation)
        {
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("SimpleInterceptor_BeforeInvocation");
            invocation.Proceed();
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("SimpleInterceptor_AfterInvocation");
        }

        //public async Task InterceptAsync(IAbpAsyncMethodInvocation invocation)
        //{
        //    (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("SimpleInterceptor_BeforeInvocation");
        //    await invocation.ProceedAsync();
        //    (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("SimpleInterceptor_AfterInvocation");
        //}
    }
}