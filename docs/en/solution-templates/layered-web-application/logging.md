```json
//[doc-seo]
{
    "Description": "Learn how to implement structured logging with Serilog in your ABP Framework applications, ensuring a consistent and efficient logging setup."
}
```

# Layered Solution: Logging

```json
//[doc-nav]
{
  "Previous": {
    "Name": "Database configurations",
    "Path": "solution-templates/layered-web-application/database-configurations"
  },
  "Next": {
    "Name": "Swagger integration",
    "Path": "solution-templates/layered-web-application/swagger-integration"
  }
}
```

The ABP Studio [layered solution template](index.md) is fully configured for [logging](../../framework/fundamentals/logging.md). All the applications are configured to use the [Serilog](https://serilog.net/) library for structured logging. They are configured in a common way for logging. This document explains that common logging structure.

## The Serilog Sinks

The Serilog library is configured so it writes the logs to the following targets (a.k.a. [sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks)) in parallel:

* **[Console](https://github.com/serilog/serilog-sinks-console)**: Logs are written to the standard output of the executing application. Logging to console is useful when you want to see logs easily while it is running in a container.
* **[File](https://github.com/serilog/serilog-sinks-file)**: Logs are written to a file named `logs.txt` located under the `Logs` folder of the executing application. File logging is useful when you run the application on your local computer. You can check logs easily when you have a trouble. This sinks is only configured for DEBUG mode. It won't be available in your production environment (you can change the behavior in your `Program.cs` file).
* **ABP Studio**: This is a Sink provided by ABP Studio. It sends all logs to ABP Studio, so you can easily monitor your logs in real-time on your ABP Studio Application Monitoring panel.

The solution can work with [any sink](https://github.com/serilog/serilog/wiki/Provided-Sinks) supported by Serilog. You can add more sinks, remove pre-installed sinks or fine tune their configuration for your solution.

## Program.cs

The `Program.cs` file is the main point that configures the logging system. It is done here, because we want to initialize and start the logging in the very beginning of the application.

## Additional Information

We are using ABP Serilog Enrichers in the module class of the application. It is done by the `app.UseAbpSerilogEnrichers();` line in the `OnApplicationInitialization` method of your module class. That ASP.NET Core middleware adds current [tenant](../../framework/architecture/multi-tenancy/index.md), [user](../../framework/infrastructure/current-user.md), client and correlation id information to the log records.