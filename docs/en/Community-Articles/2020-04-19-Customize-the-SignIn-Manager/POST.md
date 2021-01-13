# How to Customize the SignIn Manager for ABP Applications

After creating a new application using the [application startup template](https://docs.abp.io/en/abp/latest/Startup-Templates/Application), you may want extend or change the default behavior of the SignIn Manager for your authentication and registration flow needs. ABP [Account Module](https://docs.abp.io/en/abp/latest/Modules/Account) uses the [Identity Management Module](https://docs.abp.io/en/abp/latest/Modules/Identity) for SignIn Manager and the [Identity Management Module](https://docs.abp.io/en/abp/latest/Modules/Identity) uses default [Microsoft Identity SignIn Manager](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Core/src/SignInManager.cs) ([see here](https://github.com/abpframework/abp/blob/be32a55449e270d2d456df3dabdc91f3ffdd4fa9/modules/identity/src/Volo.Abp.Identity.AspNetCore/Volo/Abp/Identity/AspNetCore/AbpIdentityAspNetCoreModule.cs#L17)).

To write your Custom SignIn Manager, you need to extend [Microsoft Identity SignIn Manager](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Core/src/SignInManager.cs) class and register it to the DI container.

This document explains how to customize the SignIn Manager for your own application.

## Create a CustomSignInManager

Create a new class inheriting the [SignInMager](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Core/src/SignInManager.cs) of Microsoft Identity package.

````csharp
public class CustomSignInManager : Microsoft.AspNetCore.Identity.SignInManager<Volo.Abp.Identity.IdentityUser>
{
        public CustomSignInManager(
            Microsoft.AspNetCore.Identity.UserManager<Volo.Abp.Identity.IdentityUser> userManager,
            Microsoft.AspNetCore.Http.IHttpContextAccessor contextAccessor,
            Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<Volo.Abp.Identity.IdentityUser> claimsFactory,
            Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Identity.IdentityOptions> optionsAccessor,
            Microsoft.Extensions.Logging.ILogger<Microsoft.AspNetCore.Identity.SignInManager<Volo.Abp.Identity.IdentityUser>> logger,
            Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider schemes,
            Microsoft.AspNetCore.Identity.IUserConfirmation<Volo.Abp.Identity.IdentityUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }
}
````

> It is important to use **Volo.Abp.Identity.IdentityUser** type for SignInManager to inherit, not the AppUser of your application.

Afterwards you can override any of the SignIn Manager methods you need and add new methods and properties needed for your authentication or registration flow.

## Overriding the GetExternalLoginInfoAsync Method

In this case we'll be overriding the `GetExternalLoginInfoAsync` method which is invoked when a third party authentication is implemented.

A good way to override a method is  copying its [source code](https://github.com/dotnet/aspnetcore/blob/c56aa320c32ee5429d60647782c91d53ac765865/src/Identity/Core/src/SignInManager.cs#L638-L674). In this case, we will be using a minorly modified version of the source code which explicitly shows the namespaces of the methods and properties to help better understanding of the concept.

````csharp
public async override Task<Microsoft.AspNetCore.Identity.ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
{
    var auth = await Context.AuthenticateAsync(Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme);
    var items = auth?.Properties?.Items;
    if (auth?.Principal == null || items == null || !items.ContainsKey(LoginProviderKey))
    {
        return null;
    }

    if (expectedXsrf != null)
    {
        if (!items.ContainsKey(XsrfKey))
        {
            return null;
        }
        var userId = items[XsrfKey] as string;
        if (userId != expectedXsrf)
        {
            return null;
        }
    }

    var providerKey = auth.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
    var provider = items[LoginProviderKey] as string;
    if (providerKey == null || provider == null)
    {
        return null;
    }

    var providerDisplayName = (await GetExternalAuthenticationSchemesAsync()).FirstOrDefault(p => p.Name == provider)?.DisplayName
        ?? provider;
    return new Microsoft.AspNetCore.Identity.ExternalLoginInfo(auth.Principal, provider, providerKey, providerDisplayName)
    {
        AuthenticationTokens = auth.Properties.GetTokens(),
        AuthenticationProperties = auth.Properties
    };
}
````

To get your overridden method invoked and your customized SignIn Manager class to work, you need to register your class to the [Dependency Injection System](https://docs.abp.io/en/abp/latest/Dependency-Injection).

## Register to Dependency Injection

Registering `CustomSignInManager` should be done with adding **AddSignInManager** extension method of the [IdentityBuilderExtensions](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Core/src/IdentityBuilderExtensions.cs) of the [IdentityBuilder](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Extensions.Core/src/IdentityBuilder.cs).

Inside your `.Web` project, locate the `YourProjectNameWebModule` and add the following code under the `PreConfigureServices` method to replace the old `SignInManager` with your customized one:

````csharp
PreConfigure<IdentityBuilder>(identityBuilder =>
{
    identityBuilder.AddSignInManager<CustomSignInManager>();
});
````

## The Source Code

You can find the source code of the completed example [here](https://github.com/abpframework/abp-samples/tree/master/Authentication-Customization).
