```json
//[doc-seo]
{
    "Description": "Learn how to migrate from Identity Server to OpenIddict in ABP Framework with this comprehensive guide, ensuring a smooth transition."
}
```

# Migration Identity Server to OpenIddict Guide

This document explains how to migrate an application from IdentityServer4 to [OpenIddict](https://github.com/openiddict/openiddict-core). ABP startup templates have used OpenIddict as the authentication server by default since v6.0.0.

> The checklist below describes the v6.0 transition. For a layer-by-layer migration, use the [IdentityServer to OpenIddict step-by-step guide](openiddict-step-by-step.md) and apply the package version that matches the ABP version of your application.

## History
IdentityServer4 is archived and no longer maintained by its owners. ABP did not migrate its integration to the commercial Duende IdentityServer product. The ABP IdentityServer integration packages are still shipped for existing applications, while new applications use OpenIddict. See the [Duende IdentityServer announcement](https://blog.duendesoftware.com/posts/20220111_fair_trade) for the background.

## OpenIddict Migration Steps

* Update all `Volo's` packages to `6.x`.
* Replace all `Volo's` `IdentityServer.*` packages with corresponding `OpenIddict.*` packages. eg `Volo.Abp.IdentityServer.Domain` to `Volo.Abp.OpenIddict.Domain`, `Volo.Abp.Account.Web.IdentityServer` to `Volo.Abp.Account.Web.OpenIddict`. 
* Replace all `IdentityServer` modules with corresponding `OpenIddict` modules. eg `AbpIdentityServerDomainModule` to `AbpOpenIddictDomainModule`, `AbpAccountWebIdentityServerModule` to `AbpAccountWebOpenIddictModule`.
* Rename the `ConfigureIdentityServer` to `ConfigureOpenIddict` in your `ProjectNameDbContext` class.
* Remove the `UseIdentityServer` and add `UseAbpOpenIddictValidation` after `UseAuthentication`.
* Add the following code to your startup module.

```cs
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictBuilder>(builder =>
    {
        builder.AddValidation(options =>
        {
            options.AddAudiences("ProjectName"); // Change ProjectName to your project name.
            options.UseLocalServer();
            options.UseAspNetCore();
        });
    });
}
```

* If your project does not have a separate AuthServer, also add `ForwardIdentityAuthenticationForBearer`.

```cs
private void ConfigureAuthentication(ServiceConfigurationContext context)
{
    context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
}
```

* Remove the `IdentityServerDataSeedContributor` from the `Domain` project.
* Generate a temporary project with the same name and architecture as the existing project so you can compare the current OpenIddict setup.
* Copy the generated `ProjectName.Domain\OpenIddict\OpenIddictDataSeedContributor.cs` into your project and update `appsettings.json` based on `ProjectName.DbMigrator\appsettings.json`. Adjust the ports and client URLs for your application.
* Copy the generated `Index.cshtml.cs` and `Index.cshtml` files into your project if your `IndexModel` still uses `IClientRepository`.
* Update the scope name from `role` to `roles` in `AddAbpOpenIdConnect` method.
* Remove `options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);` from `HttpApi.Host` project. 
* AuthServer no longer requires `JWT bearer authentication`. Please remove it. eg `AddJwtBearer` and `UseJwtTokenMiddleware`.
* Try compiling the project in the IDE and following the errors to remove and reference the code and namespaces.
* Add migrations and update the database if you are using EF Core as the database provider.

## Module packages
### Open source side
* Volo.Abp.OpenIddict.Domain (`AbpOpenIddictDomainModule`)
* Volo.Abp.OpenIddict.Domain.Shared (`AbpOpenIddictDomainSharedModule`)
* Volo.Abp.OpenIddict.EntityFrameworkCore (`AbpOpenIddictEntityFrameworkCoreModule`)
* Volo.Abp.OpenIddict.AspNetCore (`AbpOpenIddictAspNetCoreModule`)
* Volo.Abp.OpenIddict.MongoDB (`AbpOpenIddictMongoDbModule`)
* Volo.Abp.Account.Web.OpenIddict (`AbpAccountWebOpenIddictModule`)
* Volo.Abp.PermissionManagement.Domain.OpenIddict (`AbpPermissionManagementDomainOpenIddictModule`)

### Commercial side
* Volo.Abp.OpenIddict.Pro.Application.Contracts (`AbpOpenIddictProApplicationContractsModule`)
* Volo.Abp.OpenIddict.Pro.Application (`AbpOpenIddictProApplicationModule`)
* Volo.Abp.OpenIddict.Pro.HttpApi.Client (`AbpOpenIddictProHttpApiClientModule`)
* Volo.Abp.OpenIddict.Pro.HttpApi (`AbpOpenIddictProHttpApiModule`)
* Volo.Abp.OpenIddict.Pro.Blazor(`AbpOpenIddictProBlazorModule`)
* Volo.Abp.OpenIddict.Pro.Blazor.Server (`AbpOpenIddictProBlazorServerModule`)
* Volo.Abp.OpenIddict.Pro.Blazor.WebAssembly (`AbpOpenIddictProBlazorWebAssemblyModule`)
* Volo.Abp.OpenIddict.Pro.Web (`AbpOpenIddictProWebModule`)

## Source code of samples and module

* [Open source tiered & separate auth server application migrate Identity Server to OpenIddict](https://github.com/abpframework/abp-samples/tree/master/Ids2OpenId)
* [Commercial tiered & separate auth server application migrate Identity Server to OpenIddict](https://abp.io/Account/Login?returnUrl=/api/download/samples/Ids2OpenId)
* [OpenIddict module document](https://docs.abp.io/en/abp/6.0/Modules/OpenIddict)
* [OpenIddict module source code](https://github.com/abpframework/abp/tree/rel-6.0/modules/openiddict)
