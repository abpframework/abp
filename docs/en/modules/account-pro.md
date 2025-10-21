```json
//[doc-seo]
{
    "Description": "Explore the Account Module (Pro) for ABP Framework, featuring user authentication, two-factor setup, and tenant switching functionalities."
}
```

# Account Module (Pro)

> You must have an ABP Team or a higher license to use this module.

This module implements the Login, Register, Forgot Password, Email Confirmation, Password Reset, sending and confirming Two-Factor Authentication, user lockout, switch between tenants functionalities of an application;

* Built on the [Microsoft's ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity) library.
* OpenIddict Grant and Consent pages.
* Setting page to manage **self registration** and two-factor authentication.

See [the module description page](https://abp.io/modules/Volo.Account.Pro) for an overview of the module features.

## How to Install

The account is pre-installed in [the startup templates](../solution-templates/layered-web-application/index.md). So, no need to manually install it.

## Packages

This module follows the [module development best practices guide](../framework/architecture/best-practices) and consists of several NuGet and NPM packages. See the guide if you want to understand the packages and the relations between them.

You can visit the [Account module package list page](https://abp.io/packages?moduleName=Volo.Account.Pro) to see a list of packages related to this module.

## User Interface

### Menu Items

This module doesn't define any menu items.

### Pages

#### Login Page

The login page is used to log in to the system.

![account-pro-module-login-page](../images/account-pro-module-login-page.png)

#### Register Page

The register page allows new users to register to your system.

![identity-users-page](../images/account-pro-module-register-page.png)

#### Two-Factor Authentication

The identity module allows two-factor authentication pages.

##### Send Security Code

Send security code page allows selecting a two-factor authentication provider (Email, Phone etc...) and sends a security code to the user via the selected provider.

![account-pro-module-two-factor-send-page](../images/account-pro-module-two-factor-send-page.png)

##### Verify Security Code

Verify security code page verifies the security code sent to the user and if the code is verified, the user logs in to the system.

![account-pro-module-two-factor-verify-page](../images/account-pro-module-two-factor-verify-page.png)

## Data Seed

This module doesn't seed any data.

## Options

### AbpAccountOptions

`AbpAccountOptions` can be configured in the UI layer in the `ConfigureServices` method of your [module](../framework/architecture/modularity/basics.md). Example:

````csharp
Configure<AbpAccountOptions>(options =>
{
    //Set options here...
});
````

`AbpAccountOptions` properties:

* `WindowsAuthenticationSchemeName` (default: Windows): Name of the Windows authentication scheme.
* `TenantAdminUserName` (default: admin): The tenant admin user name.
* `ImpersonationTenantPermission`: The permission name for tenant impersonation.
* `ImpersonationUserPermission`: The permission name for user impersonation.
* `ExternalProviderIconMap`: A dictionary of external provider names and their corresponding font-awesome icon classes. You can add new mapping to this dictionary to change the icon of an external provider.(Popular external provider icons are already defined, such as `Facebook`, `Google`, `Microsoft`, `Twitter`, etc.)

### AbpProfilePictureOptions

`AbpProfilePictureOptions` can be configured in the UI layer in the `ConfigureServices` method of your [module](../framework/architecture/modularity/basics.md). Example:

````csharp
Configure<AbpProfilePictureOptions>(options =>
{
    //Set options here...
});
````

`AbpProfilePictureOptions` properties:

* `EnableImageCompression` (default: false): Enables the image compression for the profile picture. When enabled, the selected compression library will compress the profile picture to decrease the image size. For more information see [image manipulation](../framework/infrastructure/image-manipulation.md)

## Local login

The user can't log in through the local account and use the local account-related features such as `register` and `find password` if this setting is disabled.

If you use `Social / External Logins`, It is automatically called for authentication when logging in.

![account-pro-module-local-login-setting](../images/account-pro-module-local-login-setting.png)

### Switching users during OAuth login

If you have an OAuth/Auth Server application using the Account Pro module, you can pass the `prompt=select_account` parameter to force the user to select an account.

Example to pass `prompt=select_account` parameter in OpenIdConnect:

```csharp
.AddAbpOpenIdConnect("oidc", options =>
{
    // ...
    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProvider = redirectContext =>
        {
            redirectContext.ProtocolMessage.Prompt = "select_account";
            return Task.CompletedTask;
        }
    };
    // ...
});
```

![account-pro-secect-account](../images/account-pro-select-account-parameter.png)

You have three options:

- Continue: The login process will continue with the current account.
- Switch to another account: Will be redirected to the login page to log in with another account.
- Create a new account: Will be redirected to the register page to create a new account.

> The OAuth login process will continue after the user selects one of the options.

![account-pro-secect-account](../images/account-pro-select-account.png)

All available prompt parameters:

| **Parameter**    | **Description**                                                                                             |
|------------------|-------------------------------------------------------------------------------------------------------------|
| `login`          | Forces the user to re-authenticate, even if they are already logged in.                                      |
| `consent`        | Forces the user to re-consent to the requested permissions, even if they have consented before.             |
| `select_account` | Forces the user to select an account, even if they are already logged in (especially relevant if multiple accounts are available). |
| `none`           | Does not trigger any prompt. If the user is not logged in, or their consent is not granted, it will return an error or redirect accordingly. |

## Social / External Logins

The account module implements a social/external login system. All you need to do is to install & configure the provider you want to use.

The application startup template comes with **Twitter**, **Google** and **Microsoft** logins pre-installed. You can configure the client id and secrets on the Settings page:

![account-pro-external-login-settings](../images/account-pro-external-login-settings.png)

The social/External login system is compatible with the multi-tenancy. Each tenant can enable or disable the external login provider and configure their own provider settings if your application is multi-tenant.

### Install a new External Login

Follow the steps below to install a new external/social login. We will show Facebook authentication as an example.

> When you follow the steps below, the provider settings (e.g., ClientId and ClientSecret) will be managed on the settings page on the UI and will support multi-tenancy as explained above. If you don't want these features, follow [the standard way](../social-external-logins.md) to install and configure the provider.

#### Add the NuGet Package

Add the [Microsoft.AspNetCore.Authentication.Facebook](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.Facebook) package to your project. Based on your architecture, this can be `.Web`, `.AuthServer` (for tiered setup) or `.Host` project.

#### Configure the Provider

Use the `.AddFacebook(...)` and `WithDynamicOptions()` extension methods in the `ConfigureServices` method of your module:

````csharp
context.Services.AddAuthentication()
    .AddFacebook(facebook =>
    {
        facebook.Scope.Add("email");
        facebook.Scope.Add("public_profile");
    })
    .WithDynamicOptions<FacebookOptions>(
        FacebookDefaults.AuthenticationScheme, // Facebook
        options =>
        {
            options.WithProperty(x => x.AppId);
            options.WithProperty(x => x.AppSecret, isSecret: true);
        }
    );
````

* `AddFacebook()` is the standard method that you can set hard-coded configuration.
* `WithDynamicOptions<FacebookOptions>` is provided by the Account Module which makes possible to configure the provided properties on the UI.

#### Localize Provider Properties

You can add following translation to localize the properties of the external login providers:

`en.json`:

````json
"ExternalProvider:Facebook": "Facebook",
"ExternalProvider:Facebook:AppId": "App ID",
"ExternalProvider:Facebook:AppSecret": "App Secret",
````

### IPostConfigureAccountExternalProviderOptions

Some external logins may be initialized based on dynamic properties. You can implement an `IPostConfigureAccountExternalProviderOptions` to initialize again after dynamic properties are initialized.

Example `OpenIdConnect`:

````csharp
public class OpenIdConnectPostConfigureAccountExternalProviderOptions : IPostConfigureAccountExternalProviderOptions<OpenIdConnectOptions>
{
    private readonly IEnumerable<IPostConfigureOptions<OpenIdConnectOptions>> _postConfigureOptions;
    public OpenIdConnectPostConfigureAccountExternalProviderOptions(IEnumerable<IPostConfigureOptions<OpenIdConnectOptions>> postConfigureOptions)
    {
        _postConfigureOptions = postConfigureOptions;
    }
    public Task PostConfigureAsync(string name, OpenIdConnectOptions options)
    {
        foreach (var configureOption in _postConfigureOptions)
        {
            configureOption.PostConfigure(name, options);
        }
        return Task.CompletedTask;
    }
}
````

````csharp
context.Services.AddAuthentication()
	.AddOpenIdConnect("AzureOpenId", "Azure AD", options =>
	{
		options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
		options.RequireHttpsMetadata = false;
		options.SaveTokens = true;
		options.GetClaimsFromUserInfoEndpoint = true;
		options.Scope.Add("email");
		options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
		options.CallbackPath = configuration["AzureAd:CallbackPath"];
	})
	.WithDynamicOptions<OpenIdConnectOptions, OpenIdConnectHandler>(
		"AzureOpenId",
		options =>
		{
			options.WithProperty(x => x.Authority);
			options.WithProperty(x => x.ClientId);
			options.WithProperty(x => x.ClientSecret, isSecret: true);
		}
	);
context.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureAccountExternalProviderOptions<OpenIdConnectOptions>, OpenIdConnectPostConfigureAccountExternalProviderOptions>());
````

#### For Tiered / Separate AuthServer Solutions

If your `.AuthServer` is separated from the `.Host` project, then the `.Host` project should also be configured.

* Add the [Microsoft.AspNetCore.Authentication.Facebook](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.Facebook) package to your `.Host` project.
* Add `WithDynamicOptions<FacebookOptions>()` configuration into the `ConfigureServices` method of your module (just copy the all code above and remove the `.AddFacebook(...)` part since it is only needed in the AuthServer side).

## Manage External Logins

You can link an external login in `Account/ExternalLogins` page of `Account` module.

![my-externa-logins](../images/my-externa-logins.png)

![new-external-login](../images/new-external-login.png)

### Social Account Security Setting

Users who register via both local registration and external/social login using the same email address will be required to enter their local password on the first external/social login.

![require-local-password-on-social-account-linking](../images/require-local-password-on-social-account-linking.png)

### Time Zone Setting

Users can to set their own time zone in the account settings page if application is [supports multiple timezones](../framework/infrastructure/timing.md#clock-options).

![account-pro-time-zone-setting](../images/account-pro-time-zone-setting.png)

## Internals

### Settings

See the `IAccountSettingNames` class members for all settings defined for this module.

### Application Layer

#### Application Services

* `AccountAppService` (implements `IAccountAppService`): Implements the use cases of the register and password reset UIs.
* `AccountSettingsAppService` (implements `IAccountSettingsAppService`):  Implements the use case of the account settings UI.

### Permissions

See the `AccountPermissions` class members for all permissions defined for this module.


### Angular UI

#### Installation

In order to configure the application to use the public account module and the admin account module, you first need to import `provideAccountPublicConfig` from `@volo/abp.ng.account/public/config` and `provideAccountAdminConfig` from `@volo/abp.ng.account/admin/config`. Then, you will need to append them to the `appConfig` array.

```js
// app.config.ts
import { provideAccountPublicConfig } from '@volo/abp.ng.account/public/config';
import { provideAccountAdminConfig } from '@volo/abp.ng.account/admin/config';

export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    provideAccountAdminConfig(),
    provideAccountPublicConfig(),
  ],
};
```

The account public package should be imported and lazy-loaded in your routing array. It has a static `createRoutes` method for configuration. Available options are listed below. It is available for import from `@volo/abp.ng.account/public`.

```js
// app.routes.ts
export const APP_ROUTES: Routes = [
  // ...
  {
    path: 'account',
    loadChildren: () => import('@volo/abp.ng.account/public').then(c => c.createRoutes(/* options here */)),
  },
];

```

> If you have generated your project via the startup template, you do not have to do anything, because it already has the necessary configurations.

<h4 id="h-account-module-options">Options</h4>

You can modify the look and behavior of the module pages by passing the following options to `createRoutes` static method:

- **redirectUrl**: Default redirect URL after logging in.
- **entityActionContributors:** Changes grid actions. Please check [Entity Action Extensions for Angular](../framework/ui/angular/entity-action-extensions.md) for details.
- **toolbarActionContributors:** Changes page toolbar. Please check [Page Toolbar Extensions for Angular](../framework/ui/angular/page-toolbar-extensions.md) for details.
- **entityPropContributors:** Changes table columns. Please check [Data Table Column Extensions for Angular](../framework/ui/angular/data-table-column-extensions.md) for details.

#### Services / Models

Account module services and models are generated via `generate-proxy` command of the [ABP CLI](../cli). If you need the module's proxies, you can run the following commands in the Angular project directory.

The command below generates `AccountPublicModule` proxies:

```bash
abp generate-proxy --module account
```

The command below generates `AccountAdminModule` proxies:
```bash
abp generate-proxy --module accountAdmin
```

#### Replaceable Components

`eAccountComponents` enum provides all replaceable component keys. It is available for import from `@volo/abp.ng.account/public`.

Please check [Component Replacement document](../framework/ui/angular/component-replacement.md) for details.


#### Remote Endpoint URL

The Account module remote endpoint URLs can be configured in the environment files.

```js
export const environment = {
  // other configurations
  apis: {
    default: {
      url: 'default url here',
    },
    AbpAccountPublic: {
      url: 'AbpAccountPublic remote url here'
    },
    AbpAccountAdmin: {
      url: 'AbpAccountAdmin remote url here'
    },
    // other api configurations
  },
};
```

The Account module remote URL configurations shown above are optional. If you don't set any URLs, the `default.url` will be used as fallback.

### MauiBlazor UI

#### Resource Owner Password Flow

OAuth is preconfigured as authorization code flow in MauiBlazor application templates by default. If you added the account module to your project, you can switch the flow to resource owner password flow by changing the OAuth configuration in the `appsettings.json` files as shown below:

```json
"OAuthConfig": {
    "Authority": "https://localhost:44305", // AuthServer url
    "RequireHttpsMetadata": "true",
    "ClientId": "MyProjectName_MauiBlazor",
    "RedirectUri": "myprojectnamemauiblazor://",
    "PostLogoutRedirectUri": "myprojectnamemauiblazor://",
    "Scope": "offline_access MyProjectName",
    "GrantType": "password"
}
```

## Distributed Events

This module doesn't define any additional distributed event. See the [standard distributed events](../framework/infrastructure/event-bus).

## See Also

* [Authority Delegation](./account/authority-delegation.md)
* [Impersonation](./account/impersonation.md)
* [Linked Accounts](./account/linkedaccounts.md)
* [Session Management](./account/session-management.md)
* [Idle Session Timeout](./account/idle-session-timeout.md)
