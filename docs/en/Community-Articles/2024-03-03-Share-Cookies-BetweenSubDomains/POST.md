# How to share the cookies between subdomains

## Introduction

Sharing cookies between subdomains is a common requirement in web development. For example, you have a website with multiple subdomains, and you want to share the login status between these subdomains. Once a user logs in to one subdomain, the user should be logged in to all subdomains. 

This article will show you how to achieve this in an ASP.NET Core application.

## Implementation principle

The `cookie` has a `Domain` attribute which specifies which server can receive a cookie.
If specified, then cookies are available on the server and its subdomains. For example, if you set `Domain=.abp.io`, cookies are available on `abp.io` and its subdomains like `community.abp.io`.

If the server does not specify a **Domain**, the cookies are available on the server but not on its subdomains. Therefore, specifying the **Domain** is less restrictive than omitting it. However, it can be helpful when subdomains need to share information about a user.

## Change the domain of the cookie in ASP.NET Core

There is a `CookiePolicyMiddleware` in ASP.NET Core, you can add some policies to the `CookiePolicyOptions` during cookies are appended or deleted.

We will add a policy to the `CookiePolicyOptions` to change the `domain` of the cookie:

```csharp
services.Configure<CookiePolicyOptions>(options =>
{
    options.OnAppendCookie = cookieContext =>
    {
        ChangeCookieDomain(cookieContext, null);
    };

    options.OnDeleteCookie = cookieContext =>
    {
        ChangeCookieDomain(null, cookieContext);
    };
});

private static void ChangeCookieDomain(AppendCookieContext appendCookieContext, DeleteCookieContext deleteCookieContext)
{
    if (appendCookieContext != null)
    {
        // Change the domain of all cookies
        //appendCookieContext.CookieOptions.Domain = ".abp.io";

        // Change the domain of the specific cookie
        if (appendCookieContext.CookieName == ".AspNetCore.Culture")
        {
            appendCookieContext.CookieOptions.Domain = ".abp.io";
        }
    }

    if (deleteCookieContext != null)
    { 
        // Change the domain of all cookies
        //appendCookieContext.CookieOptions.Domain = ".abp.io";

        // Change the domain of the specific cookie
        if (deleteCookieContext.CookieName == ".AspNetCore.Culture")
        {
            deleteCookieContext.CookieOptions.Domain = ".abp.io";
        }
    }
}
```

Add the `app.UseCookiePolicy()` in the ASP.NET Core pipeline:

```csharp
//...
app.UseStaticFiles();
app.UseCookiePolicy();
//...
```

If you check the HTTP response headers, you will see the `Set-Cookie` header with the `domain` attribute as follows:

```http
Set-Cookie: .AspNetCore.Culture=c%3Den%7Cuic%3Den; expires=Mon, 09 Mar 2026 02:00:00 GMT; domain=.abp.io; path=/
```

The subdomains can share the `.AspNetCore.Culture` cookie now.

In another community article, we use the same middleware to [fix the Chrome login issue for the IdentityServer4](https://community.abp.io/posts/patch-for-chrome-login-issue-identityserver4-samesite-cookie-problem-weypwp3n)

## Summary

The `CookiePolicy` middleware provides a way to control cookies in an ASP.NET Core,  It is very useful if you have more complex requirements for Cookies. 
