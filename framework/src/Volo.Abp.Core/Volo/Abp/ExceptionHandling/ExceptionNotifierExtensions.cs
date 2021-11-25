using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.ExceptionHandling;

public static class ExceptionNotifierExtensions
{
    public static Task NotifyAsync(
        [NotNull] this IExceptionNotifier exceptionNotifier,
        [NotNull] Exception exception,
        LogLevel? logLevel = null,
        bool handled = true)
    {
        Check.NotNull(exceptionNotifier, nameof(exceptionNotifier));

        return exceptionNotifier.NotifyAsync(
            new ExceptionNotificationContext(
                exception,
                logLevel,
                handled
            )
        );
    }
}
