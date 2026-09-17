using System.Threading.Tasks;

namespace Volo.Abp.Internal.Telemetry;

public interface ITelemetryActivitySender
{
    Task TrySendQueuedActivitiesAsync();
}