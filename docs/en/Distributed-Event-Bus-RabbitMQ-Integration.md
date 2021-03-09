# Distributed Event Bus RabbitMQ Integration

> This document explains **how to configure the [RabbitMQ](https://www.rabbitmq.com/)** as the distributed event bus provider. See the [distributed event bus document](Distributed-Event-Bus.md) to learn how to use the distributed event bus system

## Installation

Use the ABP CLI to add [Volo.Abp.EventBus.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.EventBus.RabbitMQ) NuGet package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.EventBus.RabbitMQ` package.
* Run `abp add-package Volo.Abp.EventBus.RabbitMQ` command.

If you want to do it manually, install the [Volo.Abp.EventBus.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.EventBus.RabbitMQ) NuGet package to your project and add `[DependsOn(typeof(AbpEventBusRabbitMqModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

## Configuration

You can configure using the standard [configuration system](Configuration.md), like using the `appsettings.json` file, or using the [options](Options.md) classes.

### `appsettings.json` file configuration

This is the simplest way to configure the RabbitMQ settings. It is also very strong since you can use any other configuration source (like environment variables) that is [supported by the AspNet Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/).

**Example: The minimal configuration to connect to a local RabbitMQ server with default configurations**

````json
{
  "RabbitMQ": {
    "EventBus": {
      "ClientName": "MyClientName",
      "ExchangeName": "MyExchangeName"
    }
  }
}
````

* `ClientName` is the name of this application, which is used as the **queue name** on the RabbitMQ.
* `ExchangeName` is the **exchange name**.

See [the RabbitMQ document](https://www.rabbitmq.com/dotnet-api-guide.html#exchanges-and-queues) to understand these options better.

#### Connections

If you need to connect to another server than the localhost, you need to configure the connection properties.

**Example: Specify the host name (as an IP address)**

````json
{
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "123.123.123.123"
      }
    },
    "EventBus": {
      "ClientName": "MyClientName",
      "ExchangeName": "MyExchangeName"
    }
  }
}
````

Defining multiple connections is allowed. In this case, you can specify the connection that is used for the event bus.

**Example: Declare two connections and use one of them for the event bus**

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
    },
    "EventBus": {
      "ClientName": "MyClientName",
      "ExchangeName": "MyExchangeName",
      "ConnectionName": "SecondConnection"
    }
  }
}
````

This allows you to use multiple RabbitMQ server in your application, but select one of them for the event bus.

You can use any of the [ConnectionFactry](http://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.ConnectionFactory.html#properties) properties as the connection properties.

**Example: Specify the connection port**

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

If you need to connect to the RabbitMQ cluster, you can use the `;` character to separate the host names.

**Example: Connect to the RabbitMQ cluster**

```json
{
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "123.123.123.123;234.234.234.234"
      }
    },
    "EventBus": {
      "ClientName": "MyClientName",
      "ExchangeName": "MyExchangeName"
    }
  }
}
```

### The Options Classes

`AbpRabbitMqOptions` and `AbpRabbitMqEventBusOptions` classes can be used to configure the connection strings and event bus options for the RabbitMQ.

You can configure this options inside the `ConfigureServices` of your [module](Module-Development-Basics.md).

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

**Example: Configure the client and exchange names**

````csharp
Configure<AbpRabbitMqEventBusOptions>(options =>
{
    options.ClientName = "TestApp1";
    options.ExchangeName = "TestMessages";
});
````

Using these options classes can be combined with the `appsettings.json` way. Configuring an option property in the code overrides the value in the configuration file.