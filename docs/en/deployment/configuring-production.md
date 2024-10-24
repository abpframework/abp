# Configuring Your Application for Production Environments

ABP predefines the configurations for the best performance for common deployment scenarios. 
However, depending on how you have structured your deployment environment, you may need to consider some extra options. This document highlights these options. 
It is highly recommended that you read it to prevent unexpected behaviors in your production environment.

## Distributed Cache Prefix

ABP's [distributed cache infrastructure](../framework/fundamentals/caching.md) provides an option to set a key prefix for all your data saved into your distributed cache provider. 
The default value of this option is not set (it is `null`). If you are using a distributed cache server that is shared by different applications, then you can set a prefix value to isolate an application's cache data from others.

````csharp
Configure<AbpDistributedCacheOptions>(options =>
{
    options.KeyPrefix = "MyCrmApp";
});
````

That's all! This prefix will be added to all cache keys in your application, as long as you use ABP's `IDistributedCache<TCacheItem>` or `IDistributedCache<TCacheItem,TKey>` services. 
If you are new to distributed caching, check out the [caching documentation](../framework/fundamentals/caching.md).


> **Warning**: If you use ASP.NET Core's standard `IDistributedCache` [service](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.distributed.idistributedcache), it is your responsibility to add the key prefix (you can get the value by injecting `IOptions<AbpDistributedCacheOptions>`). ABP can not do it anymore.

> **Warning**: Even if you have never used distributed caching in your own codebase, ABP still uses it for some features. So, you should always configure this prefix if your caching server is shared among multiple systems.

> **Warning**: If you are building a microservice architecture, then you will have multiple applications that share the same distributed cache server. All applications (sub-services) in these systems should use the same cache prefix and cache values to have application-wide consistency.

> **Warning**: Some of ABP's startup templates are pre-configured to set a prefix value for the distributed cache. If your application code is already configured, please check it.

## Distributed Lock Prefix

ABP's [distributed locking infrastructure](../framework/infrastructure/distributed-locking.md) provides an option to set a prefix for all the keys you use in the distributed lock server. The default value of this option is not set (it is `null`). If you use a distributed lock server shared by different applications, you can set a prefix value to isolate an application's lock from others.

````csharp
Configure<AbpDistributedLockOptions>(options =>
{
    options.KeyPrefix = "MyCrmApp";
});
````

That's all! ABP will add this prefix to all of your keys in your application. See the [Distributed Locking documentation](../framework/infrastructure/distributed-locking.md) if you are new to distributed locking.

> **Warning**: Even if you have never used distributed locking in your own codebase, ABP still uses it for some features. So, you should always configure this prefix if your distributed lock server is shared among multiple systems.

> **Warning**: If you are building a microservice system, then you will have multiple applications that share the same distributed locking server. In such systems, all applications (or services) should normally use the same lock prefix, because you want to globally lock your resources in your system.

> **Warning**: Some of ABP's startup templates are pre-configured to set a prefix value for distributed locking. Please check your application code to see if it has already been configured.

## Email Sender

ABP's [Email Sending](../framework/infrastructure/emailing.md) system abstracts sending emails from your application and module code and allows you to configure the email provider and settings in a single place.

For the development environment, ABP will not send real emails. The email service is configured to write email contents in the development environment's standard [application log](../framework/fundamentals/logging.md). By default, it will write to the `Logs.txt` in your host project's `Logs` folder. You should configure the email settings to be able to send emails to users in your production environment.

See the [Sending Email](../framework/infrastructure/emailing.md) document to learn what settings to configure to send actual email without writing to the logs.

> **Warning**: If you don't configure the email settings, you will get errors while trying to send emails. For example, the [Account module](../modules/account.md)'s *Password Reset* feature sends email to the users to reset their passwords if they forget it.

## SMS Sender

ABP's [SMS Sending abstraction](../framework/infrastructure/sms-sending.md) provides a unified interface for sending SMS to users. 
However, its implementation is left to you. Typically, a paid SMS service is used to send SMS, and ABP doesn't depend on a specific SMS provider.
> PRO users can use the [Twilio SMS module](../modules/twilio-sms.md) to send SMS.

In the following code-block you can see where to implement the `ISmsSender` service:

````csharp
public class MySmsSender : ISmsSender, ITransientDependency
{
    public async Task SendAsync(SmsMessage smsMessage)
    {
        // TODO: Send it using your provider...
    }
}
````

> [ABP](https://abp.io/) provides a [Twilio SMS Module](../modules/twilio-sms.md) as a pre-built integration with the popular [Twilio](https://www.twilio.com/) platform.

## BLOB Provider

If you use ABP's [BLOB Storing](../framework/infrastructure/blob-storing) infrastructure, you should care about the BLOB provider in your production environment. For example, if you use the [File System](../framework/infrastructure/blob-storing/file-system.md) provider and your application is running in a Docker container, you should configure a volume mapping for the BLOB storage path. Otherwise, your data will be lost when the container is restarted. Also, the File System is not a good provider for production if you have a [clustered deployment](./clustered-environment.md) or a microservice system.

Check the [BLOB Storing](../framework/infrastructure/blob-storing) document to see all the available BLOB storage providers.

> **Warning**: Even if you don't directly use the BLOB Storage system, a module you are depending on may use it. For example, ABP's [File Management](../modules/file-management.md) module stores file contents, and the [Account](../modules/account.md) module stores user profile pictures in the BLOB Storage system. So, be careful with the BLOB Storing configuration in production. Note that ABP uses the [Database Provider](../framework/infrastructure/blob-storing/database.md) as a pre-configured BLOB storage provider, which works in production without any problem, but you may still want to use another provider.

## String Encryption

ABP's [`IStringEncryptionService` Service](../framework/infrastructure/string-encryption.md) simply encrypts and decrypts given strings based on a password phrase. You should configure the `AbpStringEncryptionOptions` options for the production with a strong password and keep it as a secret. You can also configure the other properties of those options class. See the following example:

````csharp
Configure<AbpStringEncryptionOptions>(options =>
{
    options.DefaultPassPhrase = "gs5nTT042HAL4it1";
});
````

Note that ABP CLI automatically sets the password to a random value on new project creation. However, it is stored in the `appsettings.json` file and is generally added to your source control. It is suggested to use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or [Environment Variables](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration) to set that value.

## Logging

ABP uses .NET's standard [Logging services](../framework/fundamentals/logging.md). So, it is compatible with any logging provider that works with .NET. ABP's startup solution templates come with [Serilog](https://serilog.net/) pre-installed and configured for you. It writes logs to the file system and console with the initial configuration. The file system is useful for the development environment, but it is suggested you use a different provider for your production environment, like Elasticsearch, database or any other provider that can properly work.

## The Swagger UI

ABP's startup solution templates come with [Swagger UI](https://swagger.io/) pre-installed. Swagger is a pretty standard and useful tool to discover and test your HTTP APIs on a built-in UI that is embedded into your application or service. It is typically used in development environment, but you may want to enable it on staging or production environments too.

While you will always secure your HTTP APIs with other techniques (like the [Authorization](../framework/fundamentals/authorization.md) system), allowing malicious software and people to easily discover your HTTP API endpoint details can be considered as a security problem for some systems. So, be careful while taking the decision of enabling or disabling Swagger for the production environment.

> You may also want to see the [ABP Swagger integration](../framework/api-development/swagger.md) document.
