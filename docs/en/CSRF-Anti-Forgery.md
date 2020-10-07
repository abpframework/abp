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

* Server side sets a **special cookie**, named `XSRF-TOKEN` by default, that is used make the antiforgery token value available to the browser. This is **done automatically** (by the [application configuration](Application-Configuration.md) endpoint). Nothing to do in the client side.
* In the client side, it reads the token from the cookie and sends it in the **HTTP header** (named `RequestVerificationToken` by default). This is implemented for all the supported UI types.
* Server side validates the antiforgery token **only for same and cross site requests** made by the browser. It bypasses the validation for non-browser clients.

That's all. The systems works smoothly.

