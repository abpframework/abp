namespace DistDemoApp;

public class DistEventScenarioProfile
{
    public string Name { get; set; } = "default";

    public string AnonymousOnlyEventName { get; set; } = "dist-demo.anonymous-only";

    public string AnonymousOnlyMessage { get; set; } = "hello-anonymous";

    public int TypedFromTypedValue { get; set; } = 7;

    public int TypedFromAnonymousValue { get; set; } = 11;

    public bool EnableTypedFromTypedScenario { get; set; } = true;

    public bool EnableTypedFromAnonymousScenario { get; set; } = true;

    public bool EnableAnonymousOnlyScenario { get; set; } = true;

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
            AnonymousOnlyEventName = "dist-demo.dapr.anonymous-only",
            AnonymousOnlyMessage = "hello-dapr-web",
            EnableTypedFromTypedScenario = false,
            EnableTypedFromAnonymousScenario = false,
            EnableAnonymousOnlyScenario = false
        };
    }

    public static DistEventScenarioProfile AzureEmulator()
    {
        return new DistEventScenarioProfile
        {
            Name = "azure-emulator",
            AnonymousOnlyEventName = "DistDemoApp.Azure.AnonymousOnly",
            AnonymousOnlyMessage = "hello-azure-emulator",
            TypedFromTypedValue = 21,
            TypedFromAnonymousValue = 34
        };
    }
}
