# Migration Identity Server to OpenIddict Guide

This document explains how to migrate to [OpenIddict](https://github.com/openiddict/openiddict-core) from Identity Server. From now on the ABP startup templates uses `OpenIddict` as the auth server by default since version v6.0.0.

## History
We are not removing Identity Server packages and we will continue to release new versions of Identity Server related NuGet/NPM packages. That means you won't have an issue while upgrading to v6.0 when the stable version releases. We will continue to fix bugs in our packages for a while. ABP 7.0 will be based on .NET 7. If Identity Server continues to work with .NET 7, we will also continue to ship NuGet packages for our IDS integration.

On the other hand, Identity Server ends support for the open-source Identity Server in the end of 2022. The Identity Server team has decided to move to Duende IDS and ABP will not be migrated to the commercial Duende IDS. You can see the Duende Identity Server announcement from [this link](https://blog.duendesoftware.com/posts/20220111_fair_trade). 

## OpenIddict Migration Steps

* Update all `Volo's` packages to `6.x`.
* Replace all `Volo's` `IdentityServer.*` packages with corresponding `OpenIddict.*` packages. eg `Volo.Abp.IdentityServer.Domain` to `Volo.Abp.OpenIddict.Domain`, `Volo.Abp.Account.Web.IdentityServer` to `Volo.Abp.Account.Web.OpenIddict`. 
* Replace all `IdentityServer` modules with corresponding `OpenIddict` modules. eg `AbpIdentityServerDomainModule` to `AbpOpenIddictDomainModule`, `AbpAccountWebIdentityServerModule` to `AbpAccountWebOpenIddictModule`.
* Rename the `ConfigureIdentityServer` to `ConfigureOpenIddict` in your `ProjectNameDbContext` class.
* Remove the `UseIdentityServer` and add `UseAbpOpenIddictValidation` after `UseAuthentication`.
* Add follow code to your startup module.

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

* If your project is not separate AuthServer please also add `ForwardIdentityAuthenticationForBearer`

```cs
private void ConfigureAuthentication(ServiceConfigurationContext context)
{
    context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
}
```

* Remove the `IdentityServerDataSeedContributor` from the `Domain` project.
* Create a new version of the project, with the same name as your existing project.
* Copy the `ProjectName.Domain\OpenIddict\OpenIddictDataSeedContributor.cs` of new project into your project and update `appsettings.json` base on `ProjectName.DbMigrator\appsettings.json`, Be careful to change the port number.
* Copy the `Index.cshtml.cs` and `Index.cs` of new project to your project if you're using `IClientRepository` in `IndexModel`.
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
