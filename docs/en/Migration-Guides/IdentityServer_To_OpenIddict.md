# Migration Identity Server to OpenIddict Guides

1. Replace all `Volo.Abp.IdentityServer.*` packages with corresponding `Volo.Abp.OpenIddict.*` packages. eg `Volo.Abp.IdentityServer.Domain` to `Volo.Abp.OpenIddict.Domain`. 
2. Replace all `IdentityServer` modules with corresponding `OpenIddict` modules. eg `AbpIdentityServerDomainModule` to `AbpOpenIddictDomainModule`. 
3. Rename the `ConfigureIdentityServer` to `ConfigureOpenIddict` in your `ProjectNameDbContext` class.
4. Remove the `UseIdentityServer` and add `UseAbpOpenIddictValidation` after `UseAuthentication`.
5. Add follow code to your startup module.
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
6. If your project is not separate AuthServer please also add `ForwardIdentityAuthenticationForBearer`
```cs
private void ConfigureAuthentication(ServiceConfigurationContext context)
{
    context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
}
```
7. Try compiling the project in the IDE and following the errors to remove and reference the code and namespaces.
8. Create a new version of the project, then copy `MyProjectName.Domain\OpenIddict\OpenIddictDataSeedContributor.cs` into your project, and update your `\appsettings.json` base on `MyProjectName.DbMigrator\appsettings.json`
9. Add migrations and update the database if you are using EF Core as the database provider.

If in doubt with the above steps, create a new project to compare the code, or refer to the test project in the module


[OpenIddict module document](https://docs.abp.io/en/abp/6.0/Modules/OpenIddict)

[OpenIddict module source code](https://github.com/abpframework/abp/tree/rel-6.0/modules/openiddict)