# RabbitMQ Background Job Manager

RabbitMQ is an industry standard message broker. While it is typically used for inter-process communication (messaging / distributed events), it is pretty useful to store and execute background jobs in FIFO (First In First Out) order.

ABP Framework provides the [Volo.Abp.BackgroundJobs.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.RabbitMQ) NuGet package to use the RabbitMQ for background job execution.

> See the [background jobs document](Background-Jobs.md) to learn how to use the background job system. This document only shows how to install and configure the RabbitMQ integration.

## Installation

Use the ABP CLI to add [Volo.Abp.BackgroundJobs.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.RabbitMQ) NuGet package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.BackgroundJobs.RabbitMQ` package.
* Run `abp add-package Volo.Abp.BackgroundJobs.RabbitMQ` command.

If you want to do it manually, install the [Volo.Abp.BackgroundJobs.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.RabbitMQ) NuGet package to your project and add `[DependsOn(typeof(AbpBackgroundJobsRabbitMqModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

## Configuration

### Default Configuration

The default configuration automatically connects to the local RabbitMQ server (localhost) with the standard port. **In this case, no configuration needed.**

### RabbitMQ Connection(s)

You can configure the RabbitMQ connections using the standard [configuration system](Configuration.md), like using the `appsettings.json` file, or using the [options](Options.md) classes.

#### `appsettings.json` file configuration

This is the simplest way to configure the RabbitMQ connections. It is also very strong since you can use any other configuration source (like environment variables) that is [supported by the AspNet Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/).

**Example:  Configuring the Default RabbitMQ Connection**

````json
{
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "123.123.123.123",
        "Port": "5672"
      }
    }
  }
}
````

You can use any of the [ConnectionFactry](http://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.ConnectionFactory.html#properties) properties as the connection properties. See [the RabbitMQ document](https://www.rabbitmq.com/dotnet-api-guide.html#exchanges-and-queues) to understand these options better.

Defining multiple connections is allowed. In this case, you can use different connections for different background job types (see the `AbpRabbitMqBackgroundJobOptions` section below).

**Example: Declare two connections**

````json
{
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "123.123.123.123"
      },
      "SecondConnection": {
        "HostName": "321.321.321.321"
      }
    }
  }
}
````

#### AbpRabbitMqOptions

`AbpRabbitMqOptions` class can be used to configure the connection strings for the RabbitMQ. You can configure this options inside the `ConfigureServices` of your [module](Module-Development-Basics.md).

**Example: Configure the connection**

````csharp
Configure<AbpRabbitMqOptions>(options =>
{
    options.Connections.Default.UserName = "user";
    options.Connections.Default.Password = "pass";
    options.Connections.Default.HostName = "123.123.123.123";
    options.Connections.Default.Port = 5672;
});
````

Using these options classes can be combined with the `appsettings.json` way. Configuring an option property in the code overrides the value in the configuration file.

### AbpRabbitMqBackgroundJobOptions

#### Job Queue Names

By default, each job type uses a separate queue. Queue names are calculated by combining a standard prefix and the job name. Default prefix is `AbpBackgroundJobs.` So, if the job name is `EmailSending` then the queue name in the RabbitMQ becomes `AbpBackgroundJobs.EmailSending`

> Use `BackgroundJobName` attribute on the background **job argument** class to specify the job name. Otherwise, the job name will be the full name (with namespace) of the job class.

#### Job Connections

By default, all the job types use the `Default` RabbitMQ connection.

#### Customization

`AbpRabbitMqBackgroundJobOptions` can be used to customize the queue names and the connections used by the jobs.

**Example:**

````csharp
Configure<AbpRabbitMqBackgroundJobOptions>(options =>
{
    options.DefaultQueueNamePrefix = "my_app_jobs.";
    options.JobQueues[typeof(EmailSendingArgs)] =
        new JobQueueConfiguration(
            typeof(EmailSendingArgs),
            queueName: "my_app_jobs.emails",
            connectionName: "SecondConnection"
        );
});
````

* This example sets the default queue name prefix to `my_app_jobs.`. If different applications use the same RabbitMQ server, it would be important to use different prefixes for each application to not consume jobs of each other.
* Also specifies a different connection string for the `EmailSendingArgs`.

`JobQueueConfiguration` class has some additional options in its constructor;

* `queueName`: The queue name that is used for this job. The prefix is not added, so you need to specify the full name of the queue.
* `connectionName`: The RabbitMQ connection name (see the connection configuration above). This is optional and the default value is `Default`.
* `durable` (optional, default: `true`).
* `exclusive` (optional, default: `false`).
* `autoDelete` (optional, default: `false`)

See the RabbitMQ documentation if you want to understand the `durable`, `exclusive` and `autoDelete` options better, while most of the times the default configuration is what you want. 

## See Also

* [Background Jobs](Background-Jobs.md)