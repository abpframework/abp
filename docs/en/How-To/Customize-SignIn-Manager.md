# Customize the SignInManager

## Introduction

ABP Framework uses Microsoft Identity underneath hence supports customization as much as Microsoft Identity does.

## Sample Code

https://github.com/abpframework/abp-samples/blob/master/aspnet-core/BookStore-AzureAD/src/Acme.BookStore.Web/CustomSignInManager.cs

## Creating CustomSignInManager

To create your own custom SignIn Manager, you need to inherit `SignInManager<Volo.Abp.Identity.IdentityUser>`.

````xml
public class CustomSignInManager : SignInManager<Volo.Abp.Identity.IdentityUser>
{
    public CustomSigninManager(
    UserManager<Volo.Abp.Identity.IdentityUser> userManager,
    IHttpContextAccessor contextAccessor,
    IUserClaimsPrincipalFactory<Volo.Abp.Identity.IdentityUser> claimsFactory,
    IOptions<IdentityOptions> optionsAccessor,
    ILogger<SignInManager<Volo.Abp.Identity.IdentityUser>> logger,
    IAuthenticationSchemeProvider schemes,
    IUserConfirmation<Volo.Abp.Identity.IdentityUser> confirmation) 
    : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }
}
````



## Overriding Methods

Afterwards you can override a method like `GetExternalLoginInfoAsync`:

````xml
public override async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
{
    var auth = await Context.AuthenticateAsync(IdentityConstants.ExternalScheme);
    var items = auth?.Properties?.Items;
    if (auth?.Principal == null || items == null || !items.ContainsKey("LoginProvider"))
    {
    	return null;
    }

    if (expectedXsrf != null)
    {
        if (!items.ContainsKey("XsrfId"))
        {
        	return null;
        }
        var userId = items[XsrfKey] as string;
        if (userId != expectedXsrf)
        {
        return null;
    }
    var providerKey = auth.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
    var provider = items[LoginProviderKey] as string;
    if (providerKey == null || provider == null)
    {
    	return null;
    }

    var providerDisplayName = (await GetExternalAuthenticationSchemesAsync()).FirstOrDefault(p => p.Name == provider)?.DisplayName ?? provider;
    return new ExternalLoginInfo(auth.Principal, provider, providerKey, providerDisplayName)
    {
    	AuthenticationTokens = auth.Properties.GetTokens()
    };
}
````



## Registering to DI

You need to register your Custom SignIn Manager to DI to activate it. Inside the `.Web` project, locate the `ApplicationNameWebModule` and add the following under `ConfigureServices` method:

````xml
context.Services
.GetObject<IdentityBuilder>()
    .AddSignInManager<CustomSigninManager>();
````