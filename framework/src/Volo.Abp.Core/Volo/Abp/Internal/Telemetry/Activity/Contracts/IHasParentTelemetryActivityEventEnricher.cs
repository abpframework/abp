using Volo.Abp.Internal.Telemetry.Activity.Providers;

namespace Volo.Abp.Internal.Telemetry.Activity.Contracts;

public interface IHasParentTelemetryActivityEventEnricher<out TParent> where TParent: TelemetryActivityEventEnricher
{
}