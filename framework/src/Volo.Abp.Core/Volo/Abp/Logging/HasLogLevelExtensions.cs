using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Logging
{
    public static class HasLogLevelExtensions
    {
        public static TException WithLogLevel<TException>([NotNull] this TException exception, LogLevel logLevel)
            where TException : IHasLogLevel
        {
            Check.NotNull(exception, nameof(exception));

            exception.LogLevel = logLevel;

            return exception;
        }
    }
}
