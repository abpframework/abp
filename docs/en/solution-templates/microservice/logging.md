# Microservice Solution: Logging

````json
//[doc-nav]
{
  "Next": {
    "Name": "Monitoring in the Microservice solution",
    "Path": "solution-templates/microservice/monitoring"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

The ABP Studio [microservice solution template](index.md) is fully configured for [logging](../../framework/fundamentals/logging.md). All the services, applications and gateways are configured to use the [Serilog](https://serilog.net/) library for structured logging. They are configured in a common way for logging. This document explains that common logging structure.

## The Serilog Sinks

The Serilog library is configured so it writes the logs to the following targets (a.k.a. [sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks)) in parallel:

* **[Console](https://github.com/serilog/serilog-sinks-console)**: Logs are written to the standard output of the executing application. Logging to console is useful when you want to see logs easily while it is running in a container.
* **[File](https://github.com/serilog/serilog-sinks-file)**: Logs are written to a file named `logs.txt` located under the `Logs` folder of the executing application. File logging is useful when you run the application on your local computer. You can check logs easily when you have a trouble. This sinks is only configured for DEBUG mode. It won't be available in your production environment (you can change the behavior in your `Program.cs` file).
* **[ElasticSearch](https://github.com/serilog-contrib/serilog-sinks-elasticsearch)**: Logs are sent to an [Elasticsearch](https://www.elastic.co/) server. Elasticsearch is especially useful when you want to search and trace the logs. This microservice solution is also includes a Kibana container configuration, so you can visualize and search your logs easily. See your `appsettings.json` file for the Elasticsearch configuration.
* **ABP Studio**: This is a Sink provided by ABP Studio. It sends all logs to ABP Studio, so you can easily monitor your logs in real-time on your ABP Studio Application Monitoring panel.

The solution can work with [any sink](https://github.com/serilog/serilog/wiki/Provided-Sinks) supported by Serilog. You can add more sinks, remove pre-installed sinks or fine tune their configuration for your solution.

## Program.cs

The `Program.cs` file is the main point that configures the logging system. It is done here, because we want to initialize and start the logging in the very beginning of the application.

## Additional Information

You can easily understand the Serilog configuration when you check your `Program.cs`. However, there are a few things worth mentioning here:

* We are adding an `Application` property to every log record, so you can filter logs by the application name. It is done in the `Program.cs` file with the `.Enrich.WithProperty("Application", applicationName)` line. The `applicationName` value is taken from [the `IApplicationInfoAccessor` service](../../framework/fundamentals/application-startup.md#the-applicationname-option) of ABP. By default, it is the name of the entrance assembly (that contains the `Program.cs` file) of the application.
* We are using ABP Serilog Enrichers in the module class of the application. It is done by the `app.UseAbpSerilogEnrichers();` line in the `OnApplicationInitialization` method of your module class. That ASP.NET Core middleware adds current [tenant](../../framework/architecture/multi-tenancy/index.md), [user](../../framework/infrastructure/current-user.md), client and correlation id information to the log records.