using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Logging
{
    public class DefaultInitLogger : IInitLogger
    {
        public List<AbpInitLogEntry> Entries { get; }

        public DefaultInitLogger()
        {
            Entries = new List<AbpInitLogEntry>();
        }
        
        public void Log(
            LogLevel logLevel,
            string message,
            Exception exception = null)
        {
            Entries.Add(new AbpInitLogEntry(logLevel, message, exception));
        }
    }
}