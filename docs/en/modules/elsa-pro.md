# Elsa Module (Pro)

> You must have an ABP Team or a higher license to use this module.

This module integrates [Elsa Workflows](https://docs.elsaworkflows.io/) into ABP Framework applications and is designed to make it easy for developers to use Elsa's capabilities within their ABP-based projects. For creating, managing, and customizing workflows themselves, please refer to [the official Elsa documentation](https://docs.elsaworkflows.io/).

## How to install

The Elsa module is not installed in [the startup templates](../solution-templates/layered-web-application) by default and must be installed manually. There are two ways of installing a module into your application and each one of these approaches is explained in the next sections.

### Using ABP CLI

ABP CLI allows adding a module to a solution using the ```add-module``` command. You can check its [documentation](../cli#add-module) for more information. So, the Elsa module can be added using the following command:

```bash
abp add-module Volo.Elsa
```

### Manual Installation

If you modified your solution structure, adding the module using ABP CLI might not work for you. In such cases, you can add the Elsa module into your solution manually.

In order to do that, add packages listed below to the matching project in your solution. For example, `Volo.Abp.Elsa.Application` package to your **{ProjectName}.Application.csproj** as shown below:

```xml
<PackageReference Include="Volo.Abp.Elsa.Application" Version="x.x.x" />
```

After adding the package references, open the module class of the project (e.g.: `{ProjectName}ApplicationModule`) and add the code below to the `DependsOn` attribute:

```csharp
[DependsOn(
  //...
  typeof(AbpElsaApplicationModule)
)]
```

> If you are using Blazor Web App, you need to add the `Volo.Elsa.Admin.Blazor.WebAssembly` package to the **{ProjectName}.Blazor.Client.csproj** project and add the `Volo.Elsa.Admin.Blazor.Server` package to the **{ProjectName}.Blazor.csproj** project.

## The Elsa Module

The Elsa Workflows has its own database provider, and also has a Tenant/Role/User system. They are under active development, so the ABP Elsa module is not yet fully integrated. Below is the current status of each module in the ABP's Elsa Module:

- `AbpElsaAspNetCoreModule(Volo.Elsa.Abp.AspNetCore)` module is used to integrate Elsa authentication.
- `AbpElsaIdentityModule(Volo.Elsa.Abp.Identity)` module is used to integrate ABP Identity authentication.
- `AbpElsaApplicationModule(Volo.Elsa.Abp.Application)` and `AbpElsaApplicationContractsModule(Volo.Elsa.Abp.Application.Contracts)` modules are used to define the Elsa permissions.

The rest of the projects/modules are basically empty and will be implemented in the future based on the Elsa features:

- `AbpElsaDomainModule(Volo.Elsa.Abp.Domain)`
- `AbpElsaEntityFrameworkCoreModule(Volo.Elsa.Abp.EntityFrameworkCore)`
- `AbpElsaHttpApiModule(Volo.Elsa.Abp.HttpApi)`
- `AbpElsaHttpApiClientModule(Volo.Elsa.Abp.HttpApi.Client)`
- `AbpElsaBlazorModule(Volo.Elsa.Abp.Blazor)`
- `AbpElsaBlazorServerModule(Volo.Elsa.Abp.Blazor.Server)`
- `AbpElsaBlazorWebAssemblyModule(Volo.Elsa.Abp.Blazor.WebAssembly)`
- `AbpElsaWebModule(Volo.Elsa.Abp.Web)`

### Elsa Module Permissions

The Elsa Workflow API endpoints check permissions. Also, it has a `*` wildcard permission to allow all permissions.

The ABP Elsa module defines all permissions that are used in the Elsa workflow. You can use ABP Permission Management module to manage the permissions.

`AbpElsaAspNetCoreModule(Volo.Elsa.Abp.AspNetCore)` module will check and add these permissions to the current user's claims:

![Elsa Permissions](../images/elsa-permissions.png)

You can also grant parts of the permissions to a role or user. It will add the `permissions` claims to the current user's `Cookies` or `Token`. Elsa Server will read the claims and allow or deny access:

![Elsa Part Permissions](../images/elsa-part-permissions.png)

### Elsa Studio

Elsa Studio is an **independent** web application that allows you to design, manage, and execute workflows. It is built using **Blazor Server/WebAssembly**.

Elsa Studio requires authentication and there are two ways to authenticate Elsa Studio:

* Password Flow Authentication
* Code Flow Authentication

#### Elsa Studio - Password Flow Authentication

The `AbpElsaIdentityModule(Volo.Elsa.Abp.Identity)` module is used to integrate with [ABP Identity module](./identity-pro.md) to check Elsa Studio *username* and *password* against ABP Identity. 

You need to replace `UseIdentity` with `UseAbpIdentity` when configuring Elsa in your Elsa server project as follows:

```csharp
context.Services
    .AddElsa(elsa => elsa
        .UseAbpIdentity(identity =>
        {
            identity.TokenOptions = options => options.SigningKey = "large-signing-key-for-signing-JWT-tokens";
        });
    );
```

After that, you can add the below code to use `Identity` as the login method in your Elsa Studio client project:

```csharp
builder.Services.AddLoginModule().UseElsaIdentity();
```

Then, you can log in to the Elsa Studio application with the default credentials (`admin` as the username, and `1q2w3E*` as the password):

![elsa-login](../images/elsa-password-login.png)

Once, you logged in to the application, you can start defining workflows, manage them and see their execution instances and more:

![elsa-main](../images/elsa-main-page.png)

#### Elsa Studio - Code Flow Authentication

ABP applications use [OpenIddict](./openiddict-pro.md) for authentication. So, you can use the [Authorization Code Flow](https://oauth.net/2/grant-types/authorization-code/) to authenticate Elsa Studio.

To do that, you can add the code block below to your Elsa Studio client project:

```csharp
builder.Services.AddLoginModule().UseOpenIdConnect(connectConfiguration =>
{
    var authority = configuration["AuthServer:Authority"]!.TrimEnd('/'); // Your Server URL
    connectConfiguration.AuthEndpoint = $"{authority}/connect/authorize";
    connectConfiguration.TokenEndpoint = $"{authority}/connect/token";
    connectConfiguration.EndSessionEndpoint = $"{authority}/connect/endsession";
    connectConfiguration.ClientId = configuration["AuthServer:ClientId"]!;
    connectConfiguration.Scopes = ["openid", "profile", "email", "phone", "roles", "offline_access", "ElsaDemoAppServer"];
});
```

After that, Elsa Studio will redirect to your ABP application's login page, then redirect back to Elsa Studio after the successful login.

### Elsa Workflows - Sample Workflow Demo

ABP provides a complete demo application that shows how to use the Elsa module in your ABP application. You can download the demo application and see the integration points, if you stuck at any point. Please see the [Elsa Workflows - Sample Workflow Demo](../samples/elsa-workflows-demo.md) page for more information.
