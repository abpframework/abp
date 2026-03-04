using Volo.Abp.EventBus;

namespace DistDemoApp;

[EventName("DistDemoApp.ProviderScenarioEvent")]
public class ProviderScenarioEvent
{
    public int Value { get; set; }
}
