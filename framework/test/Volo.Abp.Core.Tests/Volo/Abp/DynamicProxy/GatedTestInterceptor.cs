using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class GatedTestInterceptor : AbpInterceptor
{
    public TaskCompletionSource<bool> Gate { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        await Gate.Task;
        await invocation.ProceedAsync();
    }
}
