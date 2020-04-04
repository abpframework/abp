# How to Use the Azure Active Directory Authentication for MVC / Razor Page Applications

## Introduction

This post demonstrates how to  integrate AzureAD to an ABP application that enables users to sign in using OAuth 2.0 with credentials from Azure Active Directory. 

Adding Azure Active Directory is pretty straightforward in ABP framework. Couple of configurations needs to be done correctly. 

There will be two samples of connections for better coverage;

- **AddAzureAD** (Microsoft.AspNetCore.Authentication.AzureAD.UI package)
- **AddOpenIdConnect** (Default Microsoft.AspNetCore.Authentication.OpenIdConnect package)



## Sample Code

https://github.com/abpframework/abp-samples/tree/master/aspnet-core/BookStore-AzureAD



## Setup

Update your `appsettings.json` in your **.Web** application and add the following section filled with your AzureAD application settings.

````xml
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<your-tenant-id",
    "ClientId": "<your-client-id>",
    "Domain": "domain.onmicrosoft.com",
    "CallbackPath": "/signin-azuread-oidc"	
  }
````



## AddAzureAD

#### **Update your `appsettings.json`**

Install `Microsoft.AspNetCore.Authentication.AzureAD.UI` package to your **.Web** application.

In your **.Web** application, add the following section filled with your AzureAD application settings. Modify `ConfigureAuthentication` method of your **BookStoreWebModule** with the following:

````xml
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
                options.ResponseType = OpenIdConnectResponseType.IdToken;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters.ValidateIssuer = false; 
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.SignInScheme = IdentityConstants.ExternalScheme;

                options.Scope.Add("email");
            });
		}
````



## AddOpenIdConnect 

Modify `ConfigureAuthentication` method of your **BookStoreWebModule** with the following:

````xml
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
                .AddOpenIdConnect("AzureOpenId", "AzureAD", options =>
                 {
                     options.Authority = "https://login.microsoftonline.com/" + configuration["AzureAd:TenantId"];
                     options.ClientId = configuration["AzureAd:ClientId"];
                     options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                     options.CallbackPath = configuration["AzureAd:CallbackPath"];
                     options.RequireHttpsMetadata = false;
                     options.SaveTokens = true;
                     options.GetClaimsFromUserInfoEndpoint = true;
                 });
        }
````



# FAQ

* Help! `GetExternalLoginInfoAsync` returns `null`!

  * There can be 2 reasons for this;

    1. You are trying to authenticate against wrong scheme. Check if you set **SignInScheme** to `IdentityConstants.ExternalScheme`:

       ````xml
       options.SignInScheme = IdentityConstants.ExternalScheme;
       ````

    2. Your `ClaimTypes.NameIdentifier` is `null`. Check if you added claim mapping: 

       ````xml
       JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
       JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier);
       ````


* Help! I keep getting ***AADSTS50011: The reply URL specified in the request does not match the reply URLs configured for the application*** error!

  * If you set your **CallbackPath** in appsettings as:

    ````xml
      "AzureAd": {
        ...
        "CallbackPath": "/signin-azuread-oidc"	
      }
    ````

    your **Redirect URI** of your application in azure portal must be with <u>domain</u> like `https://localhost:44320/signin-azuread-oidc`, not only `/signin-azuread-oidc`. 

* Help! I am getting ***System.ArgumentNullException: Value cannot be null. (Parameter 'userName')*** error!


  * This occurs when you use Azure Authority **v2.0 endpoint** without requesting `email` scope. [Abp checks unique email to create user](https://github.com/abpframework/abp/blob/037ef9abe024c03c1f89ab6c933710bcfe3f5c93/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml.cs#L208). Simply add 

    ````xml
    options.Scope.Add("email");
    ````

    to your openid configuration.

* How can I **debug/watch** which claims I get before they get mapped?

  * You can add a simple event under openid configuration to debug before mapping like: 

    ````xml
    options.Events.OnTokenValidated = (async context =>
    {
    	var claimsFromOidcProvider = context.Principal.Claims.ToList();
    	await Task.CompletedTask;
    });
    ````

* I want to debug further, how can I implement my custom **SignInManager**?

  * You can check [Customizing SignInManager in Abp Framework](link here) post.

* I want to add extra properties to user while they are being created. How can I do that?

  * You can check [Customizing Login Page in Abp Framework]() post.

* Why can't I see **External Register Page** after I sign in from external provider for the first time?

  * ABP framework automatically registers your user with supported email claim from your external authentication provider. You can change this behavior by [Customizing Login Page in Abp Framework](will be link here).
