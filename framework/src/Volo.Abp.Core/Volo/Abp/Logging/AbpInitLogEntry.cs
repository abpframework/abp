using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Logging
{
    public class AbpInitLogEntry
    {
        public LogLevel Level { get; }
        
        public string Message { get; }
        
        [CanBeNull]
        public Exception Exception { get; }
        
        public AbpInitLogEntry(
            LogLevel level,
            string message,
            Exception exception)
        {
            Level = level;
            Message = message;
            Exception = exception;
        }
    }
}