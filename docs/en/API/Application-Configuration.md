# Application Configuration Endpoint

ABP Framework provides a pre-built and standard endpoint that contains some useful information about the application/service. Here, the list of some fundamental information at this endpoint:

* [Localization](../Localization.md) values, supported and the current language of the application.
* Available and granted [policies](../Authorization.md) (permissions) for the current user.
* [Setting](../Settings.md) values for the current user.
* Info about the [current user](../CurrentUser.md) (like id and user name).
* Info about the current [tenant](../Multi-Tenancy.md) (like id and name).
* [Time zone](../Timing.md) information for the current user and the [clock](../Timing.md) type of the application.

## HTTP API

If you navigate to the `/api/abp/application-configuration` URL of an ABP Framework based web application or HTTP Service, you can access the configuration as a JSON object. This endpoint is useful to create the client of your application.

## Script

For ASP.NET Core MVC (Razor Pages) applications, the same configuration values are also available on the JavaScript side. `/Abp/ApplicationConfigurationScript` is the URL of the script that is auto-generated based on the HTTP API above.

See the [JavaScript API document](../UI/AspNetCore/JavaScript-API/Index.md) for the ASP.NET Core UI.

Other UI types provide services native to the related platform. For example, see the [Angular UI localization documentation](../UI/Angular/Localization.md) to learn how to use the localization values exposes by this endpoint.

