using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;

namespace Volo.Abp.Internal.Telemetry.Activity.Providers;

public class TelemetryActivityEventBuilder : ITelemetryActivityEventBuilder, ISingletonDependency
{
    private readonly List<ITelemetryActivityEventEnricher> _activityEnrichers;

    public TelemetryActivityEventBuilder(IEnumerable<ITelemetryActivityEventEnricher> activityDataEnrichers)
    {
        _activityEnrichers = activityDataEnrichers
            .Where(FilterEnricher)
            .OrderByDescending(x => x.ExecutionOrder)
            .ToList();
    }
    public virtual async Task<ActivityEvent?> BuildAsync(ActivityContext context)
    {
        foreach (var enricher in _activityEnrichers)
        {
            try
            {
                await enricher.EnrichAsync(context);
            }
            catch
            {
               //ignored
            }
            
            if (context.IsTerminated)
            {
                return null;
            }
        }

        return context.Current;
    }
    
    private static bool FilterEnricher(ITelemetryActivityEventEnricher enricher)
    {
        return ProxyHelper.GetUnProxiedType(enricher).Assembly.FullName!.StartsWith(TelemetryConsts.VoloNameSpaceFilter) && 
               enricher is not IHasParentTelemetryActivityEventEnricher<TelemetryActivityEventEnricher>;
    }
}