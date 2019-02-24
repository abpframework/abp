using Serilog.Core;
using Serilog.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Tracing;

namespace Volo.AbpWebSite
{
    //This is for trial for now
    public class CorrelationIdLogEventEnricher : ILogEventEnricher, ITransientDependency
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public CorrelationIdLogEventEnricher(ICorrelationIdProvider correlationIdProvider)
        {
            _correlationIdProvider = correlationIdProvider;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(
                new LogEventProperty(
                    "CorrelationId",
                    new ScalarValue("CorrId:" + _correlationIdProvider.Get())
                )
            );
        }
    }
}