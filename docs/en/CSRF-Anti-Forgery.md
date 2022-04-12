# CSRF/XSRF & Anti Forgery System

"*Cross-Site Request Forgery (CSRF) is a type of attack that occurs when a malicious web site, email, blog, instant message, or program causes a user’s web browser to perform an unwanted action on a trusted site for which the user is currently authenticated*" ([OWASP](https://www.owasp.org/index.php/Cross-Site_Request_Forgery_(CSRF)_Prevention_Cheat_Sheet)).

**ABP Framework completely automates CSRF preventing** and works out of the box without any configuration. Read this documentation only if you want to understand it better or need to customize.

## The Problem

ASP.NET Core [provides infrastructure](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery) to prevent CSRF attacks by providing a system to **generate** and **validate antiforgery tokens**. However, the standard implementation has a few drawbacks;

Antiforgery token validation is only **enabled for razor pages by default** and not enabled for **HTTP APIs**. You need to enable it yourself for the Controllers. You can use the `[ValidateAntiForgeryToken]` attribute for a specific API Controller/Action or the `[AutoValidateAntiforgeryToken]` attribute to prevent attacks globally.

Once you enable it;

* You need to manually add an HTTP header, named `RequestVerificationToken` to every **AJAX request** made in your application. You should care about obtaining the token, saving in the client side and adding to the HTTP header on every HTTP request.
* All your clients, including **non-browser clients**, should care about obtaining and sending the antiforgery token in every request. In fact, non-browser clients has no CSRF risk and should not care about this.

Especially, the second point is a pain for your clients and unnecessarily consumes your server resources.

> You can read more about the ASP.NET Core antiforgery system in its own [documentation](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery). 

## The Solution

ABP Framework provides `[AbpValidateAntiForgeryToken]` and `[AbpAutoValidateAntiforgeryToken]` attributes, just like the attributes explained above. `[AbpAutoValidateAntiforgeryToken]` is already added to the global filters, so you should do nothing to enable it for your application.

ABP Framework also automates the following infrastructure;

* Server side sets a **special cookie**, named `XSRF-TOKEN` by default, that is used make the antiforgery token value available to the browser. This is **done automatically** (by the [application configuration](API/Application-Configuration.md) endpoint). Nothing to do in the client side.
* In the client side, it reads the token from the cookie and sends it in the **HTTP header** (named `RequestVerificationToken` by default). This is implemented for all the supported UI types.
* Server side validates the antiforgery token **only for same and cross site requests** made by the browser. It bypasses the validation for non-browser clients.

That's all. The systems works smoothly.

## Configuration / Customization

### AbpAntiForgeryOptions

`AbpAntiForgeryOptions` is the main [options class](Options.md) to configure the ABP Antiforgery system. It has the following properties;

* `TokenCookie`:  Can be used to configure the cookie details. This cookie is used to store the antiforgery token value in the client side, so clients can read it and sends the value as the HTTP header. Default cookie name is `XSRF-TOKEN`, expiration time is 10 years (yes, ten years! It should be a value longer than the authentication cookie max life time, for the security).
* `AuthCookieSchemaName`: The name of the authentication cookie used by your application. Default value is `Identity.Application` (which becomes `AspNetCore.Identity.Application` on runtime). The default value properly works with the ABP startup templates. **If you change the authentication cookie name, you also must change this.**
* `AutoValidate`: The single point to enable/disable the ABP automatic antiforgery validation system. Default value is `true`.
* `ShouldValidatePredicates`: A list of predicates that gets `AuthorizationFilterContext` and returns a boolean. If any of these conditions return `false`, the request is excluded from the automatic antiforgery token validation.
* `AutoValidateIgnoredHttpMethods`: A list of HTTP Methods to ignore on automatic antiforgery validation. Default value: "GET", "HEAD", "TRACE", "OPTIONS". These HTTP Methods are safe to skip antiforgery validation since they don't change the application state.

If you need to change these options, do it in the `ConfigureServices` method of your [module](Module-Development-Basics.md).

**Example: Configuring the AbpAntiForgeryOptions**

```csharp
Configure<AbpAntiForgeryOptions>(options =>
{
    options.TokenCookie.Expiration = TimeSpan.FromDays(365);
    options.AutoValidateIgnoredHttpMethods.Remove("GET");
    options.ShouldValidatePredicates.Add(filterContext =>
    {
        var controllerActionDescriptor = filterContext.ActionDescriptor.AsControllerActionDescriptor();

        if (controllerActionDescriptor.ControllerTypeInfo.AsType() == typeof(IgnoreController) &&
            controllerActionDescriptor.ActionName == nameof(IgnoreController.IgnoreMethod))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    });
});
```

This configuration;

* Sets the antiforgery token expiration time to ~1 year.
* Enables antiforgery token validation for GET requests too.
* Ignores the controller types in the specified namespace.

### AntiforgeryOptions

`AntiforgeryOptions` is the standard [options class](Options.md) of the ASP.NET Core. **You can find all the information about this class in its [own documentation](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery)**.

`HeaderName` option is especially important for the ABP Framework point of view. Default value of this value is `RequestVerificationToken` and the clients uses this name while sending the token value in the header. So, if you change this option, you should also arrange your clients to align the change. If you don't have a good reason, leave it as default.

### AbpValidateAntiForgeryToken Attribute

If you disable the automatic validation or want to perform the validation for an endpoint that is not validated by default (for example, an endpoint with HTTP GET Method), you can use the `[AbpValidateAntiForgeryToken]` attribute for a **controller type or method** (action).

**Example: Add `[AbpValidateAntiForgeryToken]` to a HTTP GET method**

```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;

namespace MyCompanyName.MyProjectName.Controllers
{
    [Route("api/products")]
    public class ProductController : AbpController
    {
        [HttpGet]
        [AbpValidateAntiForgeryToken]
        public async Task GetAsync()
        {
            //TODO: ...
        }
    }
}
```

### Angular UI

Angular supports CSRF Token out of box, but the token header name is `X-XSRF-TOKEN`. Since ABP Framework follows the ASP.NET Core conventions, it changes this value to `RequestVerificationToken` in the core package. 

You don't need to make anything unless you need to change the `AntiforgeryOptions.HeaderName` as explained before. If you change it, remember to change the header name for the Angular application too. To do that, add an import declaration for the `HttpClientXsrfModule` into your root module.

**Example: Change the header name to *MyCustomHeaderName***

```typescript
@NgModule({
  // ...
  imports: [
    //...
    HttpClientXsrfModule.withOptions({
      cookieName: 'XSRF-TOKEN',
      headerName: 'MyCustomHeaderName'
    })
  ],
})
export class AppModule {}
```

**Note:** XSRF-TOKEN is only valid if both frontend application and APIs run on the same domain. Therefore, when you make a request, you should use a relative path. 

For example, let's say your APIs is hosted at `https://testdomain.com/ws`
and your angular application is hosted at `https://testdomain.com/admin`

So if your API request should look like this `https://testdomain.com/ws/api/identity/users`

your `environment.prod.ts` has to be as follows:

```typescript
export const environment = {
  production: true,
  // ....
  apis: {
    default: {
      url: '/ws', // <- just use the context root here
     // ...
    },
  },
} as Config.Environment;
```

Let's talk about why.

First, take a look at [Angular's code](https://github.com/angular/angular/blob/master/packages/common/http/src/xsrf.ts#L81)

It does not intercept any request that starts with `http://` or `https://`. There is a good reason for that. Any cross-site request does not need this token for security. This verification is only valid if the request is made to the same domain from which the web page is served. So, simply put, if you serve everything from a single domain, you just use a relative path.

If you serve your APIs from the root, i.e. no context root (https://testdomain.com/api/identity/users), leave `url` empty as follows: 

```typescript
export const environment = {
  production: true,
  // ....
  apis: {
    default: {
      url: '', // <- should be empty string, not '/'
     // ...
    },
  },
} as Config.Environment;
```
