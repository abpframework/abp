# How to fix the Chrome login issue for the  IdentityServer4 

## Introduction

When you use HTTP on your Identity Server 4 enabled website, users may not login because of the changes made by Chrome in the version 8x. This occurs when you use HTTP schema in your website. The issue is explained here https://docs.microsoft.com/en-gb/dotnet/core/compatibility/3.0-3.1#http-browser-samesite-changes-impact-authentication

## How to solve it?

### Step-1

Create the below extension in your  ***.Web** project.

```csharp
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SameSiteCookiesServiceCollectionExtensions
    {
        /// <summary>
        /// -1 defines the unspecified value, which tells ASPNET Core to NOT
        /// send the SameSite attribute. With ASPNET Core 3.1 the
        /// <seealso cref="SameSiteMode" /> enum will have a definition for
        /// Unspecified.
        /// </summary>
        private const SameSiteMode Unspecified = (SameSiteMode)(-1);

        /// <summary>
        /// Configures a cookie policy to properly set the SameSite attribute
        /// for Browsers that handle unknown values as Strict. Ensure that you
        /// add the <seealso cref="Microsoft.AspNetCore.CookiePolicy.CookiePolicyMiddleware" />
        /// into the pipeline before sending any cookies!
        /// </summary>
        /// <remarks>
        /// Minimum ASPNET Core Version required for this code:
        ///   - 2.1.14
        ///   - 2.2.8
        ///   - 3.0.1
        ///   - 3.1.0-preview1
        /// Starting with version 80 of Chrome (to be released in February 2020)
        /// cookies with NO SameSite attribute are treated as SameSite=Lax.
        /// In order to always get the cookies send they need to be set to
        /// SameSite=None. But since the current standard only defines Lax and
        /// Strict as valid values there are some browsers that treat invalid
        /// values as SameSite=Strict. We therefore need to check the browser
        /// and either send SameSite=None or prevent the sending of SameSite=None.
        /// Relevant links:
        /// - https://tools.ietf.org/html/draft-west-first-party-cookies-07#section-4.1
        /// - https://tools.ietf.org/html/draft-west-cookie-incrementalism-00
        /// - https://www.chromium.org/updates/same-site
        /// - https://devblogs.microsoft.com/aspnet/upcoming-samesite-cookie-changes-in-asp-net-and-asp-net-core/
        /// - https://bugs.webkit.org/show_bug.cgi?id=198181
        /// </remarks>
        /// <param name="services">The service collection to register <see cref="CookiePolicyOptions" /> into.</param>
        /// <returns>The modified <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureNonBreakingSameSiteCookies(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = Unspecified;
                options.OnAppendCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            return services;
        }

        private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

                if (DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = Unspecified;
                }
            }
        }

        /// <summary>
        /// Checks if the UserAgent is known to interpret an unknown value as Strict.
        /// For those the <see cref="CookieOptions.SameSite" /> property should be
        /// set to <see cref="Unspecified" />.
        /// </summary>
        /// <remarks>
        /// This code is taken from Microsoft:
        /// https://devblogs.microsoft.com/aspnet/upcoming-samesite-cookie-changes-in-asp-net-and-asp-net-core/
        /// </remarks>
        /// <param name="userAgent">The user agent string to check.</param>
        /// <returns>Whether the specified user agent (browser) accepts SameSite=None or not.</returns>
        private static bool DisallowsSameSiteNone(string userAgent)
        {
            // Cover all iOS based browsers here. This includes:
            //   - Safari on iOS 12 for iPhone, iPod Touch, iPad
            //   - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            //   - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the
            // iOS networking stack.
            // Notes from Thinktecture:
            // Regarding https://caniuse.com/#search=samesite iOS versions lower
            // than 12 are not supporting SameSite at all. Starting with version 13
            // unknown values are NOT treated as strict anymore. Therefore we only
            // need to check version 12.
            if (userAgent.Contains("CPU iPhone OS 12")
               || userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack.
            // This includes:
            //   - Safari on Mac OS X.
            // This does not include:
            //   - Chrome on Mac OS X
            // because they do not use the Mac OS networking stack.
            // Notes from Thinktecture: 
            // Regarding https://caniuse.com/#search=samesite MacOS X versions lower
            // than 10.14 are not supporting SameSite at all. Starting with version
            // 10.15 unknown values are NOT treated as strict anymore. Therefore we
            // only need to check version 10.14.
            if (userAgent.Contains("Safari")
               && userAgent.Contains("Macintosh; Intel Mac OS X 10_14")
               && userAgent.Contains("Version/"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions,
            // but pre-Chromium Edge does not require SameSite=None.
            // Notes from Thinktecture:
            // We can not validate this assumption, but we trust Microsofts
            // evaluation. And overall not sending a SameSite value equals to the same
            // behavior as SameSite=None for these old versions anyways.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }
            
            if (GetChromeVersion(userAgent) >= 80)
            {
                return true;
            }

            return false;
        }

        private static int GetChromeVersion(string userAgent)
        {
            try
            {
                return Convert.ToInt32(userAgent.Split("Chrome/")[1].Split('.')[0]);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
```

### Step-2

Assume that your project name is *Acme.BookStore*. Then open `AcmeBookStoreWebModule.cs` class.

Add the following line to `ConfigureServices()` method.

```csharp
 context.Services.ConfigureNonBreakingSameSiteCookies();
```
### Step-3

Go to`OnApplicationInitialization()` method in `AcmeBookStoreWebModule.cs` add `app.UseCookiePolicy();`  

```csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
                app.UseDeveloperExceptionPage();
        }
        else
        {
                app.UseErrorPage();
                app.UseHsts();
        }

        app.UseCookiePolicy(); //<--- added this --->
	
	//....
}
```



It's all! You are ready to go!



---

Referenced from https://www.thinktecture.com/en/identity/samesite/prepare-your-identityserver/
