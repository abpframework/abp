# Integration Services

The *Integration Service* concept was created to distinguish the [application services](Application-Services.md) that are built for inter-module (or inter-microservice) communication from the application services that are intended to be consumed from a UI or a client application.

The following figure shows a few microservices behind an API Gateway that is consumed by a UI application and 3rd-party client applications:

![integration-services](images/integration-services.png)

HTTP requests coming from out of the API Gateway can be called as *external request*, while the HTTP requests performed between microservices can be considered as *internal requests*. The application services that are designed to respond to these internal requests are called as *integration services*, because their purpose is to integrate microservices in the system, rather than respond to user requests.

## Marking an Application Service as Integration Service

Assume that you have an application service named `ProductAppService`, and you want to use that application service as an integration service. In that case, you can use the `[IntegrationService]` attribute on top of the application service class as shown below:

```csharp
[IntegrationService]
public class ProductAppService : ApplicationService, IProductAppService
{
    // ...
}
```

If your application service has an interface, like `IProductService` in this example, you can use it on the service interface:

```csharp
[IntegrationService]
public interface IProductAppService : IApplicationService
{
    // ...
}
```

> If you've used the `[IntegrationService]` on top of your service interface, it is *not needed* to use on the service class too.

That's all. From now, ABP will handle your application service as integration service and implement the followings by convention:

* If you are using the [Auto API Controllers](API/Auto-API-Controllers.md) feature in your application, the URL prefix will be `/integration-api` instead of `/api` for your integration services. Thus, you can distinguish internal and external service communications and take additional actions, such as preventing REST API calls for integration services out of API Gateway.
* Audit logging is disabled by default for the integration services. See the next section if you want to enable it.

## Configuration

### Enabling/Disabling the Audit Logging

Audit Logging is disabled by default for integration services but it can be enabled by configuring the `AbpAuditingOptions` [options class](Options.md) in the `ConfigureServices` method of your [module class](Module-Development-Basics.md):

```csharp
Configure<AbpAuditingOptions>(options =>
{
    options.IsEnabledForIntegrationService = true;
});
```

> Please refer to the [audit logging document](Audit-Logging.md) for other options and details.

### Filtering Auto API Controllers

You can filter integration services (or non-integration services) while creating [Auto API Controllers](API/Auto-API-Controllers.md), using the `ApplicationServiceTypes` option of the `ConventionalControllerSetting` by configuring the `AbpAspNetCoreMvcOptions` as shown below:

```csharp
Configure<AbpAspNetCoreMvcOptions>(options =>
{
    options.ConventionalControllers.Create(
        typeof(MyApplicationModule).Assembly,
        conventionalControllerSetting =>
        {
            conventionalControllerSetting.ApplicationServiceTypes = 
                ApplicationServiceTypes.IntegrationServices;
        });
});
```

Tip: You can call the `options.ConventionalControllers.Create` multiple times to configure regular application services and integration services with different options.

> Please refer to the [Auto API Controllers document](API/Auto-API-Controllers.md) for more information about the Auto API Controller system.

## See Also

* [Application Services](Application-Services.md)
* [Auto API Controllers](API/Auto-API-Controllers.md)
* [Audit Logging](Audit-Logging.md)
