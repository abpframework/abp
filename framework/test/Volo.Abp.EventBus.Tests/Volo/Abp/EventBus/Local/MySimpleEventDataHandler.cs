using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Local;

public class MySimpleEventDataHandler : ILocalEventHandler<MySimpleEventData>, ISingletonDependency
{
    public int TotalData { get; private set; }

    public Task HandleEventAsync(MySimpleEventData eventData)
    {
        TotalData += eventData.Value;
        return Task.CompletedTask;
    }
}
