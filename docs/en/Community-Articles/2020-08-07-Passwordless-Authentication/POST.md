# Implementing Passwordless Authentication in ASP.NET Core Identity

## Introduction

In this tutorial, we will show you how to add a custom token provider to authenticate a user with a link, instead of entering the password.

This can be useful especially if you want to make someone login to the application with your user, without sharing your secret password. The generated link will be for a single use.

### Source Code

The completed sample is available on [GitHub repository](https://github.com/abpframework/abp-samples/tree/master/PasswordlessAuthentication).

## Creating the Solution

Before starting the development, create a new solution named `PasswordlessAuthentication` and run it by following the [getting started tutorial](https://docs.abp.io/en/abp/latest/Getting-Started?UI=MVC&DB=EF&Tiered=No).

## Step-1

Create a class named **PasswordlessLoginProvider** in your ***.Web** project:

```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PasswordlessAuthentication.Web
{
    public class PasswordlessLoginProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser>
        where TUser : class
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }

        //We need to override this method as well.
        public async override Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var userId = await manager.GetUserIdAsync(user);

            return "PasswordlessLogin:" + purpose + ":" + userId;
        }
    }
}
```
## Step-2

Create **IdentityBuilderExtensions.cs** in your ***.Web** project. We will use this extension method in the `ConfigureServices`.

```csharp
using Microsoft.AspNetCore.Identity;

namespace PasswordlessAuthentication.Web
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddPasswordlessLoginProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var totpProvider = typeof(PasswordlessLoginProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider("PasswordlessLoginProvider", totpProvider);
        }
    }
}
```
## Step-3

Add the token provider to the `Identity` middleware. To do this, find the module class (eg: `PasswordlessAuthenticationWebModule.cs` in here) in your ***.Web** project and add the below into the `ConfigureServices()` method.

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
	//...
	context.Services
		.GetObject<IdentityBuilder>()
		.AddDefaultTokenProviders()
		.AddPasswordlessLoginProvider();
}
```

## Step-4

We need to create a user interface to be able to generate the magic login link. To do this quickly, open your existing **Index.cshtml.cs** in your ***.Web** project. It's under `Pages` folder. And copy-paste the below content.

**Index.cshtml.cs**

```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace PasswordlessAuthentication.Web.Pages
{
    public class IndexModel : PasswordlessAuthenticationPageModel
    {
        protected IdentityUserManager UserManager { get; }

        private readonly IIdentityUserRepository _userRepository;

        public string PasswordlessLoginUrl { get; set; }

        public string Email { get; set; }

        public IndexModel(IdentityUserManager userManager, IIdentityUserRepository userRepository)
        {
            UserManager = userManager;
            _userRepository = userRepository;
        }

        public ActionResult OnGet()
        {
            if (!CurrentUser.IsAuthenticated)
            {
                return Redirect("/Account/Login");
            }

            return Page();
        }

        //added for passwordless authentication
        public async Task<IActionResult> OnPostGeneratePasswordlessTokenAsync()
        {
            var adminUser = await _userRepository.FindByNormalizedUserNameAsync("admin");

            var token = await UserManager.GenerateUserTokenAsync(adminUser, "PasswordlessLoginProvider",
                "passwordless-auth");

            PasswordlessLoginUrl = Url.Action("Login", "Passwordless",
                new {token = token, userId = adminUser.Id.ToString()}, Request.Scheme);

            return Page();
        }
    }
}
```

We added `OnPostGeneratePasswordlessTokenAsync()` action to generate the link. We will generate a link for the **admin** user. Therefore, we injected `IIdentityUserRepository` to get admin user Id. Using the `UserManager.GenerateUserTokenAsync()` method, we generated a token. After that, we created the URL with the admin user Id and the token. Now we will show the `PasswordlessLoginUrl` on the page.

## Step-5

Create a class named **PasswordlessAuthenticationMenus** under `Menus` folder in your ***.Web** project. And set the content as below.

```csharp
namespace PasswordlessAuthentication.Web.Menus
{
    public class PasswordlessAuthenticationMenus
    {
        public const string GroupName = "PasswordlessAuthentication";

        public const string Home = GroupName + ".Home";
    }
}
```

## Step-6

Open your **Index.cshtml** and set the content as below. We added a form that posts to `GeneratePasswordlessToken` action in the razor page. And it will set the `PasswordlessLoginUrl` field.

```html
@page
@using MyBookStore.Web.Menus
@using Volo.Abp.AspNetCore.Mvc.UI.Layout
@model MyBookStore.Web.Pages.IndexModel
@using Microsoft.AspNetCore.Mvc.Localization
@using MyBookStore.Localization
@inject IHtmlLocalizer<MyBookStoreResource> L
@{
    ViewBag.PageTitle = "Home";
}
@inject IPageLayout PageLayout
@{
    PageLayout.Content.Title = L["Home"].Value;
    PageLayout.Content.BreadCrumb.Add(L["Menu:Home"].Value);
    PageLayout.Content.MenuItemName = MyBookStoreMenus.Home;
}
<abp-card>
    <abp-card-body>
        <form asp-page-handler="GeneratePasswordlessToken" method="post">

            <abp-button button-type="Dark" type="submit">Generate passwordless token link</abp-button>

            @if (Model.PasswordlessLoginUrl != null)
            {
                <abp-card class="mt-3 p-3">
                    <a href="@Model.PasswordlessLoginUrl">@Model.PasswordlessLoginUrl</a>
                </abp-card>
            }
        </form>
    </abp-card-body>
</abp-card>
```
## Step-7

We implemented token generation infrastructure, now it's time validate the token and let the user in. To do this create a folder named `Controllers` in your ***.Web** project and create a controller, named **PasswordlessController** inside it:

```csharp
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace PasswordlessAuthentication.Web.Controllers
{
    public class PasswordlessController : AbpController
    {
        protected IdentityUserManager UserManager { get; }

        public PasswordlessController(IdentityUserManager userManager)
        {
            UserManager = userManager;
        }

        public virtual async Task<IActionResult> Login(string token, string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);

            var isValid = await UserManager.VerifyUserTokenAsync(user, "PasswordlessLoginProvider", "passwordless-auth", token);
            if (!isValid)
            {
                throw new UnauthorizedAccessException("The token " + token + " is not valid for the user " + userId);
            }

            await UserManager.UpdateSecurityStampAsync(user);

            var roles = await UserManager.GetRolesAsync(user);

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(CreateClaims(user, roles), IdentityConstants.ApplicationScheme)
            );

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

            return Redirect("/");
        }

        private static IEnumerable<Claim> CreateClaims(IUser user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim(AbpClaimTypes.UserId, user.Id.ToString()),
                new Claim(AbpClaimTypes.Email, user.Email),
                new Claim(AbpClaimTypes.UserName, user.UserName),
                new Claim(AbpClaimTypes.EmailVerified, user.EmailConfirmed.ToString().ToLower()),
            };

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                claims.Add(new Claim(AbpClaimTypes.PhoneNumber, user.PhoneNumber));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(AbpClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
```

We created an endpoint for `/Passwordless/Login` that gets the token and the user Id.  In this action, we find the user via repository and validate the token via `UserManager.VerifyUserTokenAsync()` method. If it's valid, we create claims of the user then call `HttpContext.SignInAsync` to be able to create an encrypted cookie and add it to the current response.  Finally we redirect the page to the root URL.

That's all! We created a passwordless login with 7 steps.

## Source Code

The completed sample is available on [GitHub repository](https://github.com/abpframework/abp-samples/tree/master/PasswordlessAuthentication).
