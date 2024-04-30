# Tenant impersonation & User impersonation

User impersonation allows you to temporarily sign in as a different user in your tenant's users. This article introduces how to enable impersonation in ABP. Impersonation is enabled by defautl in ABP v5.0 and above.

## Introduction

In some cases, users need to sign in as another user and perform operations on behalf of the target user without sharing the target user's password. 

## How to enable impersonation feature?

If your ABP version is lower than 5.0, you can implement the impersonation feature by following the steps below.

> Please remember to configure the `ImpersonationTenantPermission` and `ImpersonationUserPermission` permissions!!!

### MVC

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();

    //For impersonation in Saas module
    context.Services.Configure<AbpSaasHostWebOptions>(options =>
    {
        options.EnableTenantImpersonation = true;
    });

    //For impersonation in Identity module
    context.Services.Configure<AbpIdentityWebOptions>(options =>
    {
        options.EnableUserImpersonation = true;
    });

    context.Services.Configure<AbpAccountOptions>(options =>
    {
        //For impersonation in Saas module
        options.TenantAdminUserName = "admin";
        options.ImpersonationTenantPermission = SaasHostPermissions.Tenants.Impersonation;

        //For impersonation in Identity module
        options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
    });
}
```

### MVC Tiered

#### AuthServer

1. Depends `AbpAccountPublicWebImpersonationModule(Volo.Abp.Account.Pro.Public.Web.Impersonation)` and `SaasHostApplicationContractsModule` on your `AuthServerModule`
2. Configure the `AbpAccountOptions`.

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<AbpAccountOptions>(options =>
    {
        //For impersonation in Saas module
        options.TenantAdminUserName = "admin";
        options.ImpersonationTenantPermission = SaasHostPermissions.Tenants.Impersonation;

        //For impersonation in Identity module
        options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
    });
}
```
#### HttpApi.Host

No need to do anything here.

#### Web

1. Depends `AbpAccountPublicWebImpersonationModule(Volo.Abp.Account.Pro.Public.Web.Impersonation)` on your `WebModule`
2. Chnage the base class of `AccountController` to `AbpAccountImpersonationChallengeAccountController`

```cs
public class AccountController : AbpAccountImpersonationChallengeAccountController
{

}
```

3. Add `ImpersonationViewComponent` to `\Components\Toolbar\Impersonation` folder

```cs
public class ImpersonationViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Components/Toolbar/Impersonation/Default.cshtml");
    }
}
```
```cs
@using Microsoft.AspNetCore.Mvc.Localization
@using Volo.Abp.Account.Localization
@inject IHtmlLocalizer<AccountResource> L
<form method="post" data-ajaxForm="false" action="~/Account/BackToImpersonator">
    @Html.AntiForgeryToken()
    <button type="submit" class="btn text-danger" data-bs-toggle="tooltip" data-bs-placement="left" title="@L["BackToImpersonator"]">
        <i class="fa fa-undo"></i>
    </button>
</form>
```

4. Add `ImpersonationViewComponent` to `ToolbarContributor`.

```cs
if (context.ServiceProvider.GetRequiredService<ICurrentUser>().FindImpersonatorUserId() != null)
{
    context.Toolbar.Items.Add(new ToolbarItem(typeof(ImpersonationViewComponent), order: -1));
}
```

5. Configure `AbpSaasHostWebOptions` and `AbpIdentityWebOptions`

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();

    //For impersonation in Saas module
    context.Services.Configure<AbpSaasHostWebOptions>(options =>
    {
        options.EnableTenantImpersonation = true;
    });

    //For impersonation in Identity module
    context.Services.Configure<AbpIdentityWebOptions>(options =>
    {
        options.EnableUserImpersonation = true;
    });
}
```

### Blazor Server

1. Depends `AbpAccountPublicWebImpersonationModule(Volo.Abp.Account.Pro.Public.Web.Impersonation)` and `AbpAccountPublicBlazorServerModule(Volo.Abp.Account.Pro.Public.Blazor.Server)` on your `BlazorModule`
2. Configure `SaasHostBlazorOptions` and `AbpAccountOptions`

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();

    //For impersonation in Saas module
    context.Services.Configure<SaasHostBlazorOptions>(options =>
    {
        options.EnableTenantImpersonation = true;
    });

    //For impersonation in Identity module
    context.Services.Configure<AbpIdentityProBlazorOptions>(options =>
    {
        options.EnableUserImpersonation = true;
    });

    context.Services.Configure<AbpAccountOptions>(options =>
    {
        //For impersonation in Saas module
        options.TenantAdminUserName = "admin";
        options.ImpersonationTenantPermission = SaasHostPermissions.Tenants.Impersonation;

        //For impersonation in Identity module
        options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
    });
}
```

### Blazor Server Tiered

#### AuthServer

1. Depends `AbpAccountPublicWebImpersonationModule(Volo.Abp.Account.Pro.Public.Web.Impersonation)` and `SaasHostApplicationContractsModule` on your `AuthServerModule`
2. Configure the `AbpAccountOptions`.

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<AbpAccountOptions>(options =>
    {
        //For impersonation in Saas module
        options.TenantAdminUserName = "admin";
        options.ImpersonationTenantPermission = SaasHostPermissions.Tenants.Impersonation;

        //For impersonation in Identity module
        options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
    });
}
```

#### HttpApi.Host

No need to do anything here.

#### Blazor

1. Depends `AbpAccountPublicWebImpersonationModule(Volo.Abp.Account.Pro.Public.Web.Impersonation)` and `AbpAccountPublicBlazorServerModule(Volo.Abp.Account.Pro.Public.Blazor.Server)` on your `BlazorModule`

2. Chnage the base class of `AccountController` to `AbpAccountImpersonationChallengeAccountController`
```cs
public class AccountController : AbpAccountImpersonationChallengeAccountController
{

}
```

3. Configure `SaasHostBlazorOptions` and `AbpAccountOptions`

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    //For impersonation in Saas module
    context.Services.Configure<SaasHostBlazorOptions>(options =>
    {
        options.EnableTenantImpersonation = true;
    });

    //For impersonation in Identity module
    context.Services.Configure<AbpIdentityProBlazorOptions>(options =>
    {
        options.EnableUserImpersonation = true;
    });
}
```

### Angular

Add `Impersonation` to the Angular grant types.

```cs
//Console Test / Angular Client
var consoleAndAngularClientId = configurationSection["MyProjectName_App:ClientId"];
if (!consoleAndAngularClientId.IsNullOrWhiteSpace())
{
    var consoleAndAngularClientRootUrl = configurationSection["MyProjectName_App:RootUrl"]?.TrimEnd('/');
    await CreateApplicationAsync(
        name: consoleAndAngularClientId,
        type: OpenIddictConstants.ClientTypes.Public,
        consentType: OpenIddictConstants.ConsentTypes.Implicit,
        displayName: "Console Test / Angular Application",
        secret: null,
        grantTypes: new List<string>
        {
            OpenIddictConstants.GrantTypes.AuthorizationCode,
            OpenIddictConstants.GrantTypes.Password,
            OpenIddictConstants.GrantTypes.ClientCredentials,
            OpenIddictConstants.GrantTypes.RefreshToken,
            "LinkLogin",
            "Impersonation"
        },
        scopes: commonScopes,
        redirectUri: consoleAndAngularClientRootUrl,
        postLogoutRedirectUri: consoleAndAngularClientRootUrl,
        clientUri: consoleAndAngularClientRootUrl,
        logoUri: "/images/clients/angular.svg"
    );
}
```

### Blazor WASM

It is currently not supported.

### Microservice

#### AuthServer

1. Depends `AbpAccountPublicWebImpersonationModule(Volo.Abp.Account.Pro.Public.Web.Impersonation)` and `SaasHostApplicationContractsModule` on your `AuthServerModule`
2. Configure the `AbpAccountOptions`.

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<AbpAccountOptions>(options =>
    {
        //For impersonation in Saas module
        options.TenantAdminUserName = "admin";
        options.ImpersonationTenantPermission = SaasHostPermissions.Tenants.Impersonation;

        //For impersonation in Identity module
        options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
    });
}
```

#### Web

1. Depends `AbpAccountPublicWebImpersonationModule(Volo.Abp.Account.Pro.Public.Web.Impersonation)` on your `WebModule`
2. Chnage the base class of `AccountController` to `AbpAccountImpersonationChallengeAccountController`

```cs
public class AccountController : AbpAccountImpersonationChallengeAccountController
{

}
```

3. Add `ImpersonationViewComponent` to `\Components\Toolbar\Impersonation` folder

```cs
public class ImpersonationViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Components/Toolbar/Impersonation/Default.cshtml");
    }
}
```
```cs
@using Microsoft.AspNetCore.Mvc.Localization
@using Volo.Abp.Account.Localization
@inject IHtmlLocalizer<AccountResource> L
<form method="post" data-ajaxForm="false" action="~/Account/BackToImpersonator">
    @Html.AntiForgeryToken()
    <button type="submit" class="btn text-danger" data-bs-toggle="tooltip" data-bs-placement="left" title="@L["BackToImpersonator"]">
        <i class="fa fa-undo"></i>
    </button>
</form>
```

4. Add `ImpersonationViewComponent` to `ToolbarContributor`.

```cs
if (context.ServiceProvider.GetRequiredService<ICurrentUser>().FindImpersonatorUserId() != null)
{
    context.Toolbar.Items.Add(new ToolbarItem(typeof(ImpersonationViewComponent), order: -1));
}
```

5. Configure `AbpSaasHostWebOptions` and `AbpIdentityWebOptions`

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();

    //For impersonation in Saas module
    context.Services.Configure<AbpSaasHostWebOptions>(options =>
    {
        options.EnableTenantImpersonation = true;
    });

    //For impersonation in Identity module
    context.Services.Configure<AbpIdentityWebOptions>(options =>
    {
        options.EnableUserImpersonation = true;
    });
}
```

#### Blazor.Server

1. Depends `AbpAccountPublicWebImpersonationModule(Volo.Abp.Account.Pro.Public.Web.Impersonation)` and `AbpAccountPublicBlazorServerModule(Volo.Abp.Account.Pro.Public.Blazor.Server)` on your `BlazorModule`

2. Chnage the base class of `AccountController` to `AbpAccountImpersonationChallengeAccountController`
```cs
public class AccountController : AbpAccountImpersonationChallengeAccountController
{

}
```

3. Configure `SaasHostBlazorOptions` and `AbpAccountOptions`

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    //For impersonation in Saas module
    context.Services.Configure<SaasHostBlazorOptions>(options =>
    {
        options.EnableTenantImpersonation = true;
    });

    //For impersonation in Identity module
    context.Services.Configure<AbpIdentityProBlazorOptions>(options =>
    {
        options.EnableUserImpersonation = true;
    });
}
```

#### Blazor and PublicWeb

It is currently not supported.

## Tenant & User Impersonation permissions

![identity](../../images/identity-impersonation-permission.png)
![saas](../../images/saas-impersonation-permission.png)
