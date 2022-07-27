# Migration Identity Server to OpenIddict Guides

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
            options.AddAudiences("MyProjectName");
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
* Update all `Volo.Abp.*` packages to 6.x.
* Remove the `IdentityServerDataSeedContributor` from the `Domain` project.
* Create a new version of the project, then copy `MyProjectName.Domain\OpenIddict\OpenIddictDataSeedContributor.cs` into your project, and update your `\appsettings.json` base on `MyProjectName.DbMigrator\appsettings.json`, Be careful to change the port number.
* Copy the `Index.cshtml.cs` and `Index.cs` of new project to your project if you're using `IClientRepository` in `IndexModel`.
* Update the scope name from `role` to `roles` in `AddAbpOpenIdConnect` method.
* Remove `options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);` from `HttpApi.Host` project. 
* Try compiling the project in the IDE and following the errors to remove and reference the code and namespaces.
* Add migrations and update the database if you are using EF Core as the database provider.

There is a sample that [migrate Identity Server to OpenIddct](https://github.com/abpframework/abp-samples/commit/c6b28246021935566ab2b58e539a1b9602ee5341) for tiered and separate auth server project.

[OpenIddict module document](https://docs.abp.io/en/abp/6.0/Modules/OpenIddict)

[OpenIddict module source code](https://github.com/abpframework/abp/tree/rel-6.0/modules/openiddict)