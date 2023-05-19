using System;
using Microsoft.Extensions.Hosting;

namespace Volo.Abp;

public static class AbpHostEnvironmentExtensions
{
    public static bool IsDevelopment(this IAbpHostEnvironment hostEnvironment)
    {
        Check.NotNull(hostEnvironment, nameof(hostEnvironment));

        return hostEnvironment.IsEnvironment(Environments.Development);
    }

    public static bool IsStaging(this IAbpHostEnvironment hostEnvironment)
    {
        Check.NotNull(hostEnvironment, nameof(hostEnvironment));

        return hostEnvironment.IsEnvironment(Environments.Staging);
    }

    public static bool IsProduction(this IAbpHostEnvironment hostEnvironment)
    {
        Check.NotNull(hostEnvironment, nameof(hostEnvironment));

        return hostEnvironment.IsEnvironment(Environments.Production);
    }

    public static bool IsEnvironment(this IAbpHostEnvironment hostEnvironment, string environmentName)
    {
        Check.NotNull(hostEnvironment, nameof(hostEnvironment));

        return string.Equals(
            hostEnvironment.EnvironmentName,
            environmentName,
            StringComparison.OrdinalIgnoreCase);
    }
}
