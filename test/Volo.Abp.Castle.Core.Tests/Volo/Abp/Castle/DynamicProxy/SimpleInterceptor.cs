using System.Threading.Tasks;
using Volo.Abp.DynamicProxy;
using Volo.Abp.TestBase.Logging;
using Volo.DependencyInjection;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class SimpleInterceptor : IAbpInterceptor, ITransientDependency
    {
        public async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            //await Task.Delay(5);
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("SimpleInterceptor_BeforeInvocation");
            await invocation.ProceedAsync();
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("SimpleInterceptor_AfterInvocation");
            await Task.Delay(5);
        }
    }

    public class SimpleInterceptor2 : IAbpInterceptor, ITransientDependency
    {
        public async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            //await Task.Delay(5);
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("SimpleInterceptor2_BeforeInvocation");
            await invocation.ProceedAsync();
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add("SimpleInterceptor2_AfterInvocation");
            await Task.Delay(5);
        }
    }
}