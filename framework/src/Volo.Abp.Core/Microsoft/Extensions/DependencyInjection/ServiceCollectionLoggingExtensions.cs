using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionLoggingExtensions
{
    public static ILogger<T> GetInitLogger<T>(this IServiceCollection services)
    {
        var loggerFactory = services.GetSingletonInstanceOrNull<IInitLoggerFactory>();
        return loggerFactory == null ? NullLogger<T>.Instance : loggerFactory.Create<T>();
    }
}
