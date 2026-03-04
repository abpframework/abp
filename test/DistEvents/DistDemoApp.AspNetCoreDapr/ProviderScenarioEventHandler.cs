using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace DistDemoApp;

public class ProviderScenarioEventHandler :
    IDistributedEventHandler<ProviderScenarioEvent>,
    ITransientDependency
{
    public Task HandleEventAsync(ProviderScenarioEvent eventData)
    {
        Console.WriteLine($"Dapr ASP.NET Core handler received ProviderScenarioEvent: {eventData.Value}");
        return Task.CompletedTask;
    }
}
