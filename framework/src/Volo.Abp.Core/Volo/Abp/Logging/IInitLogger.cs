using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Logging
{
    public interface IInitLogger
    {
        public List<AbpInitLogEntry> Entries { get; }

        void Log(
            LogLevel logLevel,
            string message,
            Exception exception = null);
    }
}