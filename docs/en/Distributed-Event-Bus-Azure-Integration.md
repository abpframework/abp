# Distributed Event Bus Azure Integration

> This document explains **how to configure the [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/)** as the distributed event bus provider. See the [distributed event bus document](Distributed-Event-Bus.md) to learn how to use the distributed event bus system

## Installation

Use the ABP CLI to add [Volo.Abp.EventBus.Azure](https://www.nuget.org/packages/Volo.Abp.EventBus.Azure) NuGet package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.EventBus.Azure` package.
* Run `abp add-package Volo.Abp.EventBus.Azure` command.

If you want to do it manually, install the [Volo.Abp.EventBus.Azure](https://www.nuget.org/packages/Volo.Abp.EventBus.Azure) NuGet package to your project and add `[DependsOn(typeof(AbpEventBusAzureModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

## Configuration

You can configure using the standard [configuration system](Configuration.md), like using the `appsettings.json` file, or using the [options](Options.md) classes.

### `appsettings.json` file configuration

This is the simplest way to configure the Azure Service Bus settings. It is also very strong since you can use any other configuration source (like environment variables) that is [supported by the AspNet Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/).

**Example: The minimal configuration to connect to Azure Service Bus Namespace with default configurations**

````json
{
  "Azure": {
    "ServiceBus": {
      "Connections": {
        "Default": {
          "ConnectionString": "Endpoint=sb://sb-my-app.servicebus.windows.net/;SharedAccessKeyName={%{{{Policy Name}}}%};SharedAccessKey={};EntityPath=marketing-consent"
        }
      }
    },
    "EventBus": {
      "ConnectionName": "Default",
      "SubscriberName": "MySubscriberName",
      "TopicName": "MyTopicName"
    }
  }
}
````

* `MySubscriberName` is the name of this subscription, which is used as the **Subscriber** on the Azure Service Bus.
* `MyTopicName` is the **topic name**.

See [the Azure Service Bus document](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-queues-topics-subscriptions) to understand these options better.

#### Connections

If you need to connect to another Azure Service Bus Namespace the Default, you need to configure the connection properties.

**Example: Declare two connections and use one of them for the event bus**

````json
{
  "Azure": {
    "ServiceBus": {
      "Connections": {
        "Default": {
          "ConnectionString": "Endpoint=sb://sb-my-app.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey={%{{{SharedAccessKey}}}%}"
        },
        "SecondConnection": {
          "ConnectionString": "Endpoint=sb://sb-my-app.servicebus.windows.net/;SharedAccessKeyName={%{{{Policy Name}}}%};SharedAccessKey={%{{{SharedAccessKey}}}%}"
        }
      }
    },
    "EventBus": {
      "ConnectionName": "SecondConnection",
      "SubscriberName": "MySubscriberName",
      "TopicName": "MyTopicName"
    }
  }
}
````

This allows you to use multiple Azure Service Bus namespaces in your application, but select one of them for the event bus.

You can use any of the [ServiceBusAdministrationClientOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.administration.servicebusadministrationclientoptions?view=azure-dotnet), [ServiceBusClientOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusclientoptions?view=azure-dotnet), [ServiceBusProcessorOptions](https://docs.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusprocessoroptions?view=azure-dotnet) properties for the connection.

**Example: Specify the Admin, Client and Processor options**

````json
{
  "Azure": {
    "ServiceBus": {
      "Connections": {
        "Default": {
          "ConnectionString": "Endpoint=sb://sb-my-app.servicebus.windows.net/;SharedAccessKeyName={%{{{Policy Name}}}%};SharedAccessKey={};EntityPath=marketing-consent",
          "Admin": {
            "Retry": {
              "MaxRetries": 3
            }
          },
          "Client": {
            "RetryOptions": {
              "MaxRetries": 1
            }
          },
          "Processor": {
            "AutoCompleteMessages": true,
            "ReceiveMode": "ReceiveAndDelete"
          }
        }
      }
    },
    "EventBus": {
      "ConnectionName": "Default",
      "SubscriberName": "MySubscriberName",
      "TopicName": "MyTopicName"
    }
  }
}
````

### The Options Classes

`AbpAzureServiceBusOptions` and `AbpAzureEventBusOptions` classes can be used to configure the connection strings and event bus options for Azure Service Bus.

You can configure this options inside the `ConfigureServices` of your [module](Module-Development-Basics.md).

**Example: Configure the connection**

````csharp
Configure<AbpAzureServiceBusOptions>(options =>
{
    options.Connections.Default.ConnectionString = "Endpoint=sb://sb-my-app.servicebus.windows.net/;SharedAccessKeyName={%{{{Policy Name}}}%};SharedAccessKey={}";
    options.Connections.Default.Admin.Retry.MaxRetries = 3;
    options.Connections.Default.Client.RetryOptions.MaxRetries = 1;
});
````

Using these options classes can be combined with the `appsettings.json` way. Configuring an option property in the code overrides the value in the configuration file.
