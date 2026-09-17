using System.Threading.Tasks;

namespace Volo.Abp.Internal.Telemetry.Activity.Contracts;

public interface ITelemetryActivityEventEnricher
{
    int ExecutionOrder { get; } 
    
    Task EnrichAsync(ActivityContext context);
    
}