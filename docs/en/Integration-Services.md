# Integration Services

ABP Framework provides **Integration Services** for inter-module (or inter-microservice) communications. 

**Integration Services** were created to distinguish the [application services](Application-Services.md) that are built for inter-module (or inter-microservice) communications from the application services that are intended to be consumed from a UI or a client application.

## Marking an Application Service as Integration Service

You can mark an application service as an integration service by using the `[IntegrationService]` attribute.

Assume that you have an application service named `ProductAppService`, if you want to mark this application service as an integration service, you should use the `[IntegrationService]` attribute top of the application service class as shown below:

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

That's all. If you are using the [Auto API Controllers](API/Auto-API-Controllers.md) feature in your application, the URL prefix will be `/integration-api` instead of `/api` for your services. Thus, you can distinguish internal and external service communications and take additional actions, such as preventing REST API calls for integration services out of API Gateway.

## Enabling/Disabling Audit Logging

Audit Logging is disabled by default for integration services but it can be enabled by configuring the `AbpAuditingOptions`:

```csharp
Configure<AbpAuditingOptions>(options =>
{
    options.IsEnabledForIntegrationService = true; //enable audit logging for integration services
});
```

* `IsEnabledForIntegrationService` (default: `false`): Disables/enables audit logging for integration services.
* `AlwaysLogSelectors`: A list of selectors to save the audit logs for the matched criteria. 

> Please refer to the [audit logging document](Audit-Logging.md) for other options and details.

## Filtering Auto API Controllers

You can filter integration services (or non-integration services) while creating [Auto API Controllers](API/Auto-API-Controllers.md), using the `ApplicationServiceTypes` option of the `ConventionalControllerSetting` by configuring the `AbpAspNetCoreMvcOptions` as shown below:

```csharp
Configure<AbpAspNetCoreMvcOptions>(options =>
{
    options.ConventionalControllers.Create(typeof(MyApplicationModule).Assembly, conventionalControllerSetting =>
    {
        conventionalControllerSetting.ApplicationServiceTypes = ApplicationServiceTypes.IntegrationServices;
    });
});
```

> Please refer to the [Auto API Controllers document](API/Auto-API-Controllers.md) for details.

## See Also

* [Application Services](Application-Services.md)
* [Auto API Controllers](API/Auto-API-Controllers.md)
* [Audit Logging](Audit-Logging.md)