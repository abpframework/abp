namespace DistDemoApp;

public class DistEventScenarioProfile
{
    public string Name { get; set; } = "default";

    public string DynamicOnlyEventName { get; set; } = "dist-demo.dynamic-only";

    public string DynamicOnlyMessage { get; set; } = "hello-dynamic";

    public int TypedFromTypedValue { get; set; } = 7;

    public int TypedFromDynamicValue { get; set; } = 11;

    public bool EnableTypedFromTypedScenario { get; set; } = true;

    public bool EnableTypedFromDynamicScenario { get; set; } = true;

    public bool EnableDynamicOnlyScenario { get; set; } = true;

    public bool OnUnitOfWorkComplete { get; set; } = true;

    public bool UseOutbox { get; set; } = true;

    public bool UseUnitOfWork { get; set; } = true;

    public int WarmupDelayMs { get; set; } = 1500;

    public int TimeoutSeconds { get; set; } = 60;

    public static DistEventScenarioProfile Default()
    {
        return new DistEventScenarioProfile();
    }

    public static DistEventScenarioProfile DaprWeb()
    {
        return new DistEventScenarioProfile
        {
            Name = "dapr-web",
            DynamicOnlyEventName = "dist-demo.dapr.dynamic-only",
            DynamicOnlyMessage = "hello-dapr-web",
            EnableTypedFromTypedScenario = false,
            EnableTypedFromDynamicScenario = false,
            EnableDynamicOnlyScenario = false
        };
    }

    public static DistEventScenarioProfile AzureEmulator()
    {
        return new DistEventScenarioProfile
        {
            Name = "azure-emulator",
            DynamicOnlyEventName = "DistDemoApp.Azure.DynamicOnly",
            DynamicOnlyMessage = "hello-azure-emulator",
            TypedFromTypedValue = 21,
            TypedFromDynamicValue = 34
        };
    }
}
