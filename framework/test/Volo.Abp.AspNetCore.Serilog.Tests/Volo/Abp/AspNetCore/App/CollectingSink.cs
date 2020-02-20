using System.Collections.Generic;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

namespace Volo.Abp.AspNetCore.App
{
    public class CollectingSink : ILogEventSink
    {
        public List<LogEvent> Events { get; } = new List<LogEvent>();

        public LogEvent SingleEvent => Events.Single();

        public void Emit(LogEvent logEvent)
        {
            Events.Add(logEvent);
        }
    }
}