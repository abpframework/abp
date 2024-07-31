# Application Configuration Endpoint

ABP provides a pre-built and standard endpoint that contains some useful information about the application/service. Here, is the list of some fundamental information at this endpoint:

* Granted [policies](../../fundamentals/authorization.md) (permissions) for the current user.
* [Setting](../../infrastructure/settings.md) values for the current user.
* Info about the [current user](../../infrastructure/current-user.md) (like id and user name).
* Info about the current [tenant](../../architecture/multi-tenancy) (like id and name).
* [Time zone](../../infrastructure/timing.md) information for the current user and the [clock](../../infrastructure/timing.md) type of the application.

> If you have started with ABP's startup solution templates and using one of the official UI options, then all these are set up for you and you don't need to know these details. However, if you are building a UI application from scratch, you may want to know this endpoint.

## HTTP API

If you navigate to the `/api/abp/application-configuration` URL of an ABP based web application or HTTP Service, you can access the configuration as a JSON object. This endpoint is useful to create the client of your application.

## Script

For ASP.NET Core MVC (Razor Pages) applications, the same configuration values are also available on the JavaScript side. `/Abp/ApplicationConfigurationScript` is the URL of the script that is auto-generated based on the HTTP API above.

See the [JavaScript API document](../../ui/mvc-razor-pages/javascript-api) for the ASP.NET Core UI.

Other UI types provide services native to the related platform. For example, see the [Angular UI settings documentation](../../ui/angular/settings.md) to learn how to use the setting values exposes by this endpoint.

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

* `IApplicationConfigurationContributor` defines the `ContributeAsync` method to extend the **application-configuration** endpoint with the specified additional data.
* You can inject services and perform any logic needed to extend the endpoint as you wish.

> Application configuration contributors are automatically discovered by the ABP and executed as a part of the application configuration initialization process.
