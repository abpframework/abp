# gRPC - Health Checks

In this article we will show how to use gRPC health checks with the ABP Framework.

## Health Checks

ASP.NET Core 7 supports gRPC health checks. Health Checks allow us to determine the overall health and availability of our application infrastructure. They are exposed as HTTP endpoints and can be configured to provide information for various monitoring scenarios, such as the response time and memory usage of our application, or whether our application can communicate with our database provider.

### gRPC Health Checks

The [gRPC health checking protocol](https://github.com/grpc/grpc/blob/master/doc/health-checking.md) is a standard for reporting the health of gRPC server apps. An app exposes health checks as a gRPC service. They are typically used with an external monitoring service to check the status of an app.

### Grpc.AspNetCore.HealthChecks

ASP.NET Core supports the gRPC health checking protocol with the [Grpc.AspNetCore.HealthChecks](https://www.nuget.org/packages/Grpc.AspNetCore.HealthChecks) package. Results from .NET health checks are reported to callers.

## Using gRPC Health Checks with the ABP Framework

In this article, I'm assuming you've used gRPC with ABP before. If you are still having problems with this, it may be good for you to review this article.
https://community.abp.io/posts/using-grpc-with-the-abp-framework-2dgaxzw3

### Set up gRPC Health Checks

In this solution, `*.HttpApi.Host` is the project that configures and runs the server-side application. So, we will make changes in that project.

* Add the `Grpc.AspNetCore.HealthChecks` package to your project.

```bash
dotnet add package Grpc.AspNetCore.HealthChecks
```

* `AddGrpcHealthChecks` to register services that enable health checks.

```csharp	
public override void ConfigureServices(ServiceConfigurationContext context)
{
    // Other configurations...

    context.Services.AddGrpcHealthChecks()
        .AddCheck("SampleHealthCheck", () => HealthCheckResult.Healthy());
}
```
* `MapGrpcHealthChecksService` to add a health check service endpoint.

```csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    // Other middlewares...

    app.UseConfiguredEndpoints(builder =>
    {
        builder.MapGrpcHealthChecksService();
    });
}
```

### Calling Health Checks From a Client

Now that our server is configured for gRPC health checks, we can test it by creating a basic console client. 

```csharp	
var channel = GrpcChannel.ForAddress("https://localhost:44357");
var client = new Health.HealthClient(channel);

var response = await client.CheckAsync(new HealthCheckRequest());
var status = response.Status;

Console.WriteLine($"Health Status: {status}");
```

## References

- https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-7.0?view=aspnetcore-7.0#grpc-health-checks-in-aspnet-core
