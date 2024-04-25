# Application Configuration Endpoint

ABP Framework provides a pre-built and standard endpoint that contains some useful information about the application/service. Here, the list of some fundamental information at this endpoint:

* Granted [policies](../../fundamentals/authorization.md) (permissions) for the current user.
* [Setting](../../fundamentals/settings.md) values for the current user.
* Info about the [current user](../../infrastructure/current-user.md) (like id and user name).
* Info about the current [tenant](../../architecture/multi-tenancy/index.md) (like id and name).
* [Time zone](../../infrastructure/timing.md) information for the current user and the [clock](../../infrastructure/timing.md) type of the application.

> If you have started with ABP's startup solution templates and using one of the official UI options, then all these are set up for you and you don't need to know these details. However, if you are building a UI application from scratch, you may want to know this endpoint.

## HTTP API

If you navigate to the `/api/abp/application-configuration` URL of an ABP Framework based web application or HTTP Service, you can access the configuration as a JSON object. This endpoint is useful to create the client of your application.

## Script

For ASP.NET Core MVC (Razor Pages) applications, the same configuration values are also available on the JavaScript side. `/Abp/ApplicationConfigurationScript` is the URL of the script that is auto-generated based on the HTTP API above.

See the [JavaScript API document](../../ui/mvc-razor-pages/javascript-api/index.md) for the ASP.NET Core UI.

Other UI types provide services native to the related platform. For example, see the [Angular UI settings documentation](../../ui/angular/settings.md) to learn how to use the setting values exposes by this endpoint.

