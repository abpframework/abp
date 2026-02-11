# Microservice Solution: Health Check Configuration

```json
//[doc-nav]
{
  "Next": {
    "Name": "Communication in the Microservice solution",
    "Path": "solution-templates/microservice/communication"
  }
}
```

Health Check is a feature that allows applications to monitor their health and diagnose potential issues. The Microservice solution template comes with pre-configured Health Check system.

In the Microservice solution template, Health Check configuration is applied in all the services, gateways and UI applications (except Blazor Wasm & Blazor WebApp applications UI applications).

### Configuration in `HealthChecksBuilderExtensions.cs`

Health Checks are configured in the `HealthChecksBuilderExtensions` class. This class extends `IServiceCollection` to register health check services and configure health check UI endpoints.

#### Default Configuration

The default setup is as follows:

```csharp
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace MyCompanyName.MyProjectName.HealthChecks;

public static class HealthChecksBuilderExtensions
{
    public static void AddMyProjectNameHealthChecks(this IServiceCollection services)
    {
        // Add your health checks here
        var healthChecksBuilder = services.AddHealthChecks();
        healthChecksBuilder.AddCheck<MyProjectNameDatabaseCheck>("MyProjectName DbContext Check", tags: new string[] { "database" });

        // Read configuration for health check URL
        var configuration = services.GetConfiguration();
        var healthCheckUrl = configuration["App:HealthCheckUrl"] ?? "/health-status";
        
        services.ConfigureHealthCheckEndpoint(healthCheckUrl);

        // Configure HealthChecks UI
        var healthChecksUiBuilder = services.AddHealthChecksUI(settings =>
        {
            settings.AddHealthCheckEndpoint("MyProjectName Health Status", healthCheckUrl);
        });

        // Set HealthCheck UI storage
        healthChecksUiBuilder.AddInMemoryStorage();

        services.MapHealthChecksUiEndpoints(options =>
        {
            options.UIPath = "/health-ui";
            options.ApiPath = "/health-api";
        });
    }

    private static IServiceCollection ConfigureHealthCheckEndpoint(this IServiceCollection services, string path)
    {
       ....
    }

    private static IServiceCollection MapHealthChecksUiEndpoints(this IServiceCollection services, Action<global::HealthChecks.UI.Configuration.Options>? setupOption = null)
    {
       ....
    }
}
```

### Database Health Check Implementation

The `MyProjectNameDatabaseCheck` class is a custom implementation of a health check that verifies database connectivity in the applications with database connection. Example:

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace MyCompanyName.MyProjectName.HealthChecks;

public class MyProjectNameDatabaseCheck : IHealthCheck, ITransientDependency
{
    protected readonly IIdentityRoleRepository IdentityRoleRepository;

    public MyProjectNameDatabaseCheck(IIdentityRoleRepository identityRoleRepository)
    {
        IdentityRoleRepository = identityRoleRepository;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await IdentityRoleRepository.GetListAsync(sorting: nameof(IdentityRole.Id), maxResultCount: 1, cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy($"Could connect to database and get record.");
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy($"Error when trying to get database record. ", e);
        }
    }
}
```

