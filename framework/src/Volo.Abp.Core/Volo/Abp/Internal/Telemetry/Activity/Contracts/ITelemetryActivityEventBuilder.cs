using System.Threading.Tasks;

namespace Volo.Abp.Internal.Telemetry.Activity.Contracts;

public interface ITelemetryActivityEventBuilder
{
    Task<ActivityEvent?> BuildAsync(ActivityContext context);
}