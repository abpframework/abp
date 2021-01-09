# How to Use the Azure Active Directory Authentication for MVC / Razor Page Applications

This guide demonstrates how to  integrate AzureAD to an ABP application that enables users to sign in using OAuth 2.0 with credentials from **Azure Active Directory**. 

Adding Azure Active Directory is pretty straightforward in ABP framework. Couple of configurations needs to be done correctly. 

Two different **alternative approaches** for AzureAD integration will be demonstrated for better coverage.

1. ~~**AddAzureAD**: This approach uses Microsoft [AzureAD UI nuget package](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.AzureAD.UI/) which is very popular when users search the web about how to integrate AzureAD to their web application.~~ Now marked **Obsolete** (see https://github.com/aspnet/Announcements/issues/439). 
2. **AddOpenIdConnect**: This approach uses default [OpenIdConnect](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect/) which can be used for not only AzureAD but for all OpenId connections.
3. **AddMicrosoftIdentityWebAppAuthentication:** This approach uses newly introduced [Microsoft.Identity.Web nuget package](https://www.nuget.org/packages/Microsoft.Identity.Web/) to replace AddAzureAD.

> There is **no difference** in functionality between these approaches. AddAzureAD is an abstracted way of OpenIdConnection ([source](https://github.com/dotnet/aspnetcore/blob/c56aa320c32ee5429d60647782c91d53ac765865/src/Azure/AzureAD/Authentication.AzureAD.UI/src/AzureADAuthenticationBuilderExtensions.cs#L122)) with predefined cookie settings.
>
> However there are key differences in integration to ABP applications because of default configurated signin schemes which will be explained below.

## 1. AddAzureAD

This approach uses the most common way to integrate AzureAD by using the [Microsoft AzureAD UI nuget package](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.AzureAD.UI/). 

If you choose this approach, you will need to install `Microsoft.AspNetCore.Authentication.AzureAD.UI` package to your **.Web** project. Also, since AddAzureAD extension uses [configuration binding](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#default-configuration), you need to update your appsettings.json file located in your **.Web** project.

#### **Updating `appsettings.json`**

You need to add a new section to your `appsettings.json` which will be binded to configuration when configuring the `OpenIdConnectOptions`:

````json
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<your-tenant-id>",
    "ClientId": "<your-client-id>",
    "Domain": "domain.onmicrosoft.com",
    "CallbackPath": "/signin-azuread-oidc"	
  }
````

> Important configuration here is the CallbackPath. This value must be the same with one of your Azure AD-> app registrations-> Authentication -> RedirectUri.

Then, you need to configure the `OpenIdConnectOptions` to complete the integration.

#### Configuring OpenIdConnectOptions

In your **.Web** project, locate your **ApplicationWebModule** and modify `ConfigureAuthentication` method with the following:

````csharp
private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier);
            context.Services.AddAuthentication()
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "Acme.BookStore";
                })
            .AddAzureAD(options => configuration.Bind("AzureAd", options));

            context.Services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Authority = options.Authority + "/v2.0/";         
                options.ClientId = configuration["AzureAd:ClientId"];
                options.CallbackPath = configuration["AzureAd:CallbackPath"];
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters.ValidateIssuer = false; 
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.SignInScheme = IdentityConstants.ExternalScheme;

                options.Scope.Add("email");
            });
		}
````

> **Don't forget to:**
>
> * Add `.AddAzureAD(options => configuration.Bind("AzureAd", options))` after `.AddAuthentication()`. This binds your AzureAD appsettings and easy to miss out.
> * Add `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear()`. This will disable the default Microsoft claim type mapping.
> * Add `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier)`. Mapping this to [ClaimTypes.NameIdentifier](https://github.com/dotnet/runtime/blob/6d395de48ac718a913e567ae80961050f2a9a4fa/src/libraries/System.Security.Claims/src/System/Security/Claims/ClaimTypes.cs#L59) is important since default SignIn Manager behavior uses this claim type for external login information.
> * Add `options.SignInScheme = IdentityConstants.ExternalScheme` since [default signin scheme is `AzureADOpenID`](https://github.com/dotnet/aspnetcore/blob/c56aa320c32ee5429d60647782c91d53ac765865/src/Azure/AzureAD/Authentication.AzureAD.UI/src/AzureADOpenIdConnectOptionsConfiguration.cs#L35).
> * Add `options.Scope.Add("email")` if you are using **v2.0** endpoint of AzureAD since v2.0 endpoint doesn't return the `email` claim as default. The [Account Module](https://docs.abp.io/en/abp/latest/Modules/Account) uses `email` claim to [register external users](https://github.com/abpframework/abp/blob/be32a55449e270d2d456df3dabdc91f3ffdd4fa9/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml.cs#L215). 

You are done and integration is completed.

## 2. Alternative Approach: AddOpenIdConnect 

If you don't want to use an extra nuget package in your application, you can use the straight default [OpenIdConnect](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect/) which can be used for all OpenId connections including AzureAD external authentication.

You don't have to use `appsettings.json` configuration but it is a good practice to set AzureAD information in the `appsettings.json`. 

To get the AzureAD information from `appsettings.json`, which will be used in `OpenIdConnectOptions` configuration, simply add a new section to `appsettings.json` located in your **.Web** project:

````json
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<your-tenant-id>",
    "ClientId": "<your-client-id>",
    "Domain": "domain.onmicrosoft.com",
    "CallbackPath": "/signin-azuread-oidc"	
  }
````

Then, In your **.Web** project; you can modify the  `ConfigureAuthentication` method located in your **ApplicationWebModule** with the following:

````csharp
private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier);

            context.Services.AddAuthentication()
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "BookStore";
                })
                .AddOpenIdConnect("AzureOpenId", "Azure Active Directory OpenId", options =>
                 {
                     options.Authority = "https://login.microsoftonline.com/" + configuration["AzureAd:TenantId"] + "/v2.0/";
                     options.ClientId = configuration["AzureAd:ClientId"];
                     options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                     options.CallbackPath = configuration["AzureAd:CallbackPath"];
                     options.RequireHttpsMetadata = false;
                     options.SaveTokens = true;
                     options.GetClaimsFromUserInfoEndpoint = true;
                     
                     options.Scope.Add("email");
                 });
        }
````

And that's it, integration is completed. Keep on mind that you can connect any other external authentication providers. 

## 3. AddMicrosoftIdentityWebAppAuthentication

With .Net 5.0, AzureAd is marked [obsolete](https://github.com/dotnet/aspnetcore/issues/25807) and will not be supported in the near future. However its expanded functionality is available in [microsoft-identity-web](https://github.com/AzureAD/microsoft-identity-web/wiki) packages.

Add (or replace with) the new nuget package Microsoft.Identity.Web nuget package](https://www.nuget.org/packages/Microsoft.Identity.Web/).

In your **.Web** project; you update the  `ConfigureAuthentication` method located in your **ApplicationWebModule** with the following while having the AzureAd appsettings section as defined before:

````csharp
private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier);
            context.Services.AddAuthentication()
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "Acme.BookStore";
                });
            
    			context.Services.AddMicrosoftIdentityWebAppAuthentication(
                    configuration: configuration,
                    configSectionName: "AzureAd",
                    openIdConnectScheme:"AzureAD",
                    cookieScheme:null);
		}
````

And that's all to add new Microsoft-Identity-Web. 

> **Don't forget to:**
>
> * Pass **cookieScheme** parameter as **null** or your [*GetExternalLoginInfoAsync* method will always return null](https://github.com/AzureAD/microsoft-identity-web/issues/133#). 

Keep in mind that [Microsoft-Identity-Web](https://github.com/AzureAD/microsoft-identity-web) is relatively new and keeps getting new enhancements, features and documentation.

## The Source Code

You can find the source code of the completed example [here](https://github.com/abpframework/abp-samples/tree/master/Authentication-Customization).

# FAQ

* Help! `GetExternalLoginInfoAsync` returns `null`! (Using obsolute **AddAzureAD**)

  * There can be 2 reasons for this;

    1. You are trying to authenticate against wrong scheme. Check if you set **SignInScheme** to `IdentityConstants.ExternalScheme`:

       ````csharp
       options.SignInScheme = IdentityConstants.ExternalScheme;
       ````

    2. Your `ClaimTypes.NameIdentifier` is `null`. Check if you added claim mapping: 

       ````csharp
       JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
       JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier);
       ````


* Help! `GetExternalLoginInfoAsync` returns `null`! (Using **AddMicrosoftIdentityWebAppAuthentication**)

  
  * Pass cookieScheme parameter as **null**. (See [this issue](https://github.com/AzureAD/microsoft-identity-web/issues/133)).
  
* Help! I am getting ***System.ArgumentNullException: Value cannot be null. (Parameter 'userName')*** error! 

  
  * This occurs when you use Azure Authority **v2.0 endpoint** without requesting `email` scope. [Abp checks unique email to create user](https://github.com/abpframework/abp/blob/037ef9abe024c03c1f89ab6c933710bcfe3f5c93/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml.cs#L208). Simply add 
  
    ````csharp
    options.Scope.Add("email");
    ````
  
  to your openid configuration.
  
* Help! I keep getting ***AADSTS50011: The reply URL specified in the request does not match the reply URLs configured for the application*** error!

  * If you set your **CallbackPath** in appsettings as:

    ````csharp
      "AzureAd": {
        ...
        "CallbackPath": "/signin-azuread-oidc"	
      }
    ````

    your **Redirect URI** of your application in azure portal must be <u>with domain</u> like `https://localhost:44320/signin-azuread-oidc`, not only `/signin-azuread-oidc`. 

* Help! I keep getting ***AADSTS700051: The response_type 'token' is not enabled for the application.*** error!

  * This error occurs when you request **token** (access token) along with **id_token** without enabling Access tokens on Azure portal  app registrations. Simply tick **Access tokens** checkbox located on top of ID tokens to be able to request token aswell.

* Help! I keep getting ***AADSTS7000218: The request body must contain the following parameter: 'client_assertion' or 'client_secret*** error!

  * This error occurs when you request **code** along with **id_token**. You need to add **client secret** on azure portal app registrations, under **Certificates & secrets** menu. Afterwards, you need to add openid configuration option like:

    ````csharp
    options.ClientSecret = "Value of your secret on azure portal";
    ````

* How can I **debug/watch** which claims I get before they get mapped?

  * You can add a simple event under openid configuration to debug before mapping like: 

    ````csharp
    options.Events.OnTokenValidated = (async context =>
      {
      	var claimsFromOidcProvider = context.Principal.Claims.ToList();
      	await Task.CompletedTask;
      });
    ````
