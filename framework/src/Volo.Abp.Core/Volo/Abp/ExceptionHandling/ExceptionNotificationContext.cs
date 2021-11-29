using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.ExceptionHandling
{
    public class ExceptionNotificationContext
    {
        /// <summary>
        /// The exception object.
        /// </summary>
        [NotNull]
        public Exception Exception { get; }

        public LogLevel LogLevel { get; }

        /// <summary>
        /// True, if it is handled.
        /// </summary>
        public bool Handled { get; }
        
        public ExceptionNotificationContext(
            [NotNull] Exception exception,
            LogLevel? logLevel = null,
            bool handled = true)
        {
            Exception = Check.NotNull(exception, nameof(exception));
            LogLevel = logLevel ?? exception.GetLogLevel();
            Handled = handled;
        }
    }
}