# Application Configuration Endpoint

ABP Framework provides a pre-built and standard endpoint that contains some useful information about the application/service. Here, is the list of some fundamental information at this endpoint:

* Granted [policies](../Authorization.md) (permissions) for the current user.
* [Setting](../Settings.md) values for the current user.
* Info about the [current user](../CurrentUser.md) (like id and user name).
* Info about the current [tenant](../Multi-Tenancy.md) (like id and name).
* [Time zone](../Timing.md) information for the current user and the [clock](../Timing.md) type of the application.

> If you have started with ABP's startup solution templates and using one of the official UI options, then all these are set up for you and you don't need to know these details. However, if you are building a UI application from scratch, you may want to know this endpoint.

## HTTP API

If you navigate to the `/api/abp/application-configuration` URL of an ABP Framework based web application or HTTP Service, you can access the configuration as a JSON object. This endpoint is useful to create the client of your application.

## Script

For ASP.NET Core MVC (Razor Pages) applications, the same configuration values are also available on the JavaScript side. `/Abp/ApplicationConfigurationScript` is the URL of the script that is auto-generated based on the HTTP API above.

See the [JavaScript API document](../UI/AspNetCore/JavaScript-API/Index.md) for the ASP.NET Core UI.

Other UI types provide services native to the related platform. For example, see the [Angular UI settings documentation](../UI/Angular/Settings.md) to learn how to use the setting values exposes by this endpoint.

## Extending the Endpoint

The **application-configuration** endpoint contains some useful information about the application, such as _localization values_, _current user information_, _granted permissions_, etc. Even most of the time these provided values are sufficient to use in your application to perform common requirements such as getting the logged-in user's ID or its granted permissions. You may still want to extend this endpoint and provide additional information for your application/service. At that point, you can use the `IApplicationConfigurationContributor` endpoint.

### IApplicationConfigurationContributor

`IApplicationConfigurationContributor` is the interface that should be implemented to add additional information to the **application-configuration** endpoint.

**Example: Setting the deployment version**

```csharp
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Data;

namespace Acme.BookStore.Web
{
    public class MyApplicationConfigurationContributor : IApplicationConfigurationContributor
    {
        public Task ContributeAsync(ApplicationConfigurationContributorContext context)
        {
            //for simplicity, it's a static number, you can inject any service to this class and perform your logic...
            var deploymentVersion = "v1.0.0"; 

            //setting the deploymentVersion
            context.ApplicationConfiguration.SetProperty("deploymentVersion", deploymentVersion);

            return Task.CompletedTask;
        }
    }
}
```

Add your contributor instance to the `AbpApplicationConfigurationOptions`

```csharp
Configure<AbpApplicationConfigurationOptions>(options =>
{
    options.Contributors.AddIfNotContains(new MyApplicationConfigurationContributor());
});
```

* `IApplicationConfigurationContributor` defines the `ContributeAsync` method to extend the **application-configuration** endpoint with the specified additional data.
* You can inject services and perform any logic needed to extend the endpoint as you wish.

> Application configuration contributors are executed as a part of the application configuration initialization process.
