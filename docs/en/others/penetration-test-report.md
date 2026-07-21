```json
//[doc-seo]
{
    "Description": "Explore the ABP Penetration Test Report detailing security findings, false positives, and actionable fixes for the ABP Commercial MVC app."
}
```

# ABP Penetration Test Report

The ABP Commercial MVC `v10.4.1` application template has been tested against security vulnerabilities by the [OWASP ZAP v2.14.0](https://www.zaproxy.org/) tool. The demo web application was scanned on a local HTTPS address. The below alerts have been reported by the pentest tool. These alerts are sorted by the risk level as high, medium, and low. The informational alerts are not mentioned in this document.

Many of these alerts are **false-positive**, meaning the vulnerability scanner detected these issues, but they are not exploitable. It's clearly explained for each false-positive alert why this alert is a false-positive. 

In the next sections, you will find the affected URLs, attack parameters (request-body), alert descriptions, false-positive explanations, and fixes for the issues. Some alerts need additional actions that can be taken by you.

## Alerts

There are high _(red flag)_, medium _(orange flag)_, low _(yellow flag)_, and informational _(blue flag)_ alerts. 

![penetration-test-10.4.0](../images/pen-test-alert-list-10.4.png)

> The informational alerts are not mentioned in this document. These alerts don't raise any risks for your application and they are optional.

### Path Traversal [Risk: High] - False Positive

- *[GET] - https://localhost:44348/api/audit-logging/audit-logs?httpMethod=audit-logs&sorting=executionTime+desc&skipCount=0&maxResultCount=10*
- *[GET] - https://localhost:44348/Account/ForgotPassword?returnUrl=%5CForgotPassword*
- *[GET] - https://localhost:44348/Account/Login?ReturnUrl=%2FAccount%2FManage*

**Description**:

The Path Traversal attack technique allows an attacker access to files, directories, and commands that potentially reside outside the web document root directory.

**Explanation**:

This is a **false-positive** alert. ABP Framework automatically validates `returnUrl` parameters and ensures they are local to the application or within a whitelist. The application does not return file contents based on these parameters.

### SQL Injection [Risk: High] - False Positive

- *[GET] - https://localhost:44348/api/audit-logging/audit-logs/entity-changes?entityTypeFullName=%27+AND+%271%27%3D%271%27+--+&sorting=changeTime+desc&skipCount=0&maxResultCount=10*
- *[GET] - https://localhost:44348/api/identity/claim-types?filter=&skipCount=0%27+AND+%271%27%3D%271%27+--+&maxResultCount=10*
- *[GET] - https://localhost:44348/api/language-management/language-texts?sorting=name+asc+AND+1%3D1+--+&skipCount=0&maxResultCount=10*

**Description**:

SQL injection may be possible.

**Explanation**:

This is a **false-positive** alert. ABP Framework uses Entity Framework Core, which inherently uses parameterized queries, preventing standard SQL injection attacks. Manual verification showed that injecting SQL syntax into parameters like `providerKey` results in the input being treated as a literal string (resulting in no match or default behavior) rather than altering the query structure.

### Content Security Policy (CSP) Header Not Set [Risk: Medium] — Application Specific Configuration

- *[GET] — https://localhost:44348/*
- *[GET] — https://localhost:44348/?page=%2FAccount%2F~%2FAccount%2FLogin*
- *[GET] — https://localhost:44348/Abp/MultiTenancy/TenantSwitchModal*
- *[GET] — https://localhost:44348/Account/ForgotPassword*
- *[GET] — https://localhost:44348/Account/Login _(other several account URLs)_*
- *[GET] — https://localhost:44348/Account/Register _(other several account URLs)_*

**Description:** 

Content Security Policy (CSP) is an added layer of security that helps to detect and mitigate certain types of attacks, including Cross Site Scripting (XSS) and data injection attacks. These attacks are used for everything from data theft to site defacement or distribution of malware. CSP provides a set of standard HTTP headers that allow website owners to declare approved sources of content that browsers should be allowed to load on a certain page — covered types are JavaScript, CSS, HTML frames, fonts, images and embeddable objects such as Java applets, ActiveX, audio, and video files.

**Solution:** 

Ensure that your web server, application server, load balancer, etc. are configured to set the `Content-Security-Policy` header, to achieve optimal browser support: "Content-Security-Policy" for Chrome 25+, Firefox 23+, and Safari 7+, "X-Content-Security-Policy" for Firefox 4.0+ and Internet Explorer 10+, and "X-WebKit-CSP" for Chrome 14+ and Safari 6+.

ABP provides CSP support through `AbpSecurityHeadersOptions`, but `UseContentSecurityPolicyHeader` is `false` by default because each application may need a different CSP depending on scripts, styles, external identity providers, CDNs, and integrations. Configure `AbpSecurityHeadersOptions` and set the `UseContentSecurityPolicyHeader` property as *true* to add the `Content-Security-Policy` header into your application:

```csharp
Configure<AbpSecurityHeadersOptions>(options => 
{
   options.UseContentSecurityPolicyHeader = true; //false by default
});
```

> See [the documentation](../framework/ui/mvc-razor-pages/security-headers.md) for more info.

### Format String Error [Risk: Medium] - False Positive

- *[GET] — https://localhost:44348/Abp/ApplicationLocalizationScript?cultureName=ZAP%25n%25s%25n%25s%0A* (with combination of different parameters)
- *[GET] — https://localhost:44348/Abp/Languages/Switch?culture=ZAP%25n%25s%25n%25s%0A&returnUrl=%2F&uiCulture=en-GB* (with combination of different parameters)
- *[GET] — https://localhost:44348/Account/Login* (with combination of different parameters)

**Description:**

A Format String error occurs when the submitted data of an input string is evaluated as a command by the application.

**Solution:**

Rewrite the background program using proper deletion of bad character strings. This will require a recompile of the background executable. 

**Explanation:**

The first affected URL is a **false-positive** alert since it's already fixed and there is not any bad character string in the responses of these endpoints anymore. (It displays an error message such as: *"The selected culture is not valid! Make sure you enter a valid culture name."*).

The second URL is also a **false-positive** alert because there is no bad character string in the response. 

> **Note**: However, it might be possible if you had any sensitive localization key-value pair in your localization entries, because this endpoint returns all localization values to be able to be used in the application. Therefore, keep that in mind while defining new localization entries. Pass the critical values in your code while using the localization entry as a parameter.

### XSLT Injection [Risk: Medium] - False Positive

- *[GET] — https://localhost:44348/api/openiddict/applications?id=%3Cxsl%3Avalue-of+select%3D%22system-property%28%27xsl%3Avendor%27%29%22%2F%3E _(same payload with different parameters...)_*
- *[GET] — https://localhost:44348/Abp/Languages/Switch?culture=%3Cxsl%3Avalue-of+select%3D%22system-property%28%27xsl%3Avendor%27%29%22%2F%3E&returnUrl=%2F&uiCulture=en-GB _(same payload with different parameters...)_*
- *[GET] — https://localhost:44348/?page=%3Cxsl%3Avalue-of+select%3D%22system-property%28%27xsl%3Avendor%27%29%22%2F%3E _(same payload with different parameters...)_*
  
**Description**: 

Injection using XSL transformations may be possible and may allow an attacker to read system information, read and write files, or execute arbitrary code.

**Explanation**: 

This is a **false-positive** alert. ABP v10.4.x uses .NET 10, and the scanned endpoints do not execute user-supplied XSLT. The local validation did not expose XSLT execution or system property output.

### Application Error Disclosure [Risk: Low] — False Positive

- *[GET] — https://localhost:44348/Account/ExternalLogins*

**Description:** 

The reported pages contain an error/warning message that may disclose sensitive information like the location of the file that produced the unhandled exception. This information can be used to launch further attacks against the web application. The alert could be a false positive if the error message is found inside a documentation page.

**Explanation:** 

This vulnerability was reported as a **positive** alert because the application ran in `Development` mode. ABP throws exceptions for developers in the `Development` environment. Production mode returned a generic error page in the local validation, without framework stack traces or database details. Therefore this alert is **false-positive** for production deployments. Further information can be found in the following issue: [github.com/abpframework/abp/issues/14177](https://github.com/abpframework/abp/issues/14177#issuecomment-1268206947).

### Cookie No `HttpOnly` Flag [Risk: Low] — Positive (No need for a fix)

* *[GET] — https://localhost:44348 (and other several URLs...)*
* *[GET] — https://localhost:44348/Abp/Languages/Switch?culture=ar&returnUrl=%2FAccount%2FForgotPassword%3FreturnUrl%3D%2522%252F%253E%253Cxsl%253Avalue-of%2520select%253D%2522system-property(%2527xsl%253Avendor%2527)%2522%252F%253E%253C!--&uiCulture=ar (and other several URLs...)*
* *[GET] — https://localhost:44348/Abp/ApplicationConfigurationScript*

**Description:** 

A cookie has been set without the `HttpOnly` flag, which means that the cookie can be accessed by JavaScript running in the browser.

**Explanation:** 

The following alert is related to the next alert. Therefore, to understand this alert, you can take a look at the next alert: _Cookie Without Secure Flag [Risk: Low]_

### Cookie Without Secure Flag [Risk: Low] — Application/Deployment Review Required

* *[GET] — https://localhost:44348 (and other several URLs...)*
* *[GET] — https://localhost:44348/Abp/Languages/Switch?culture=ar&returnUrl=%2F%3Fpage%3D% (same url with different query parameters...)*

**Description:** A cookie has been set without the `Secure` flag, which means that the cookie can be sent over unencrypted HTTP connections.

The ZAP report includes JavaScript-readable and UI preference cookies in this category. The local HTTPS validation found `XSRF-TOKEN` with `Secure=true` and `SameSite=None`, and found UI/local antiforgery cookies without `Secure` on localhost. Review the final production deployment and reverse proxy settings to ensure security-sensitive cookies are only sent over HTTPS.

**Explanation:** 

All the pages that are setting the `XSRF-TOKEN` and `.AspNetCore.Culture` cookies in the HTTP response can be reported in cookie flag alerts. This is expected for cookies that must be read by client-side code, but production deployments should still verify the `Secure` and `SameSite` attributes for each cookie.

> **Note for IDS4 users**: The `idsrv.session` cookie is being used in IDS4 and after ABP 6.x, ABP switched to OpenIddict ([github.com/abpframework/abp/issues/7221](https://github.com/abpframework/abp/issues/7221)). Therefore, this cookie is not being used in the current startup templates and you can ignore this note if you have created your application after v6.0+. However, if you are still using Identity Server 4, there is an issue related to the `idsrv.session` cookie, it cannot be set as `HttpOnly`; you can see the related thread at its own repository: [github.com/IdentityServer/IdentityServer4/issues/3873](https://github.com/IdentityServer/IdentityServer4/issues/3873)

The `.AspNetCore.Culture` and `XSRF-TOKEN` cookies are being retrieved via JavaScript in ABP Angular, MVC and Blazor WASM UIs. Therefore they cannot be set as `HttpOnly`. You can check out the following modules that retrieve these cookies via JavaScript:

* [github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.Swashbuckle/wwwroot/swagger/ui/abp.swagger.js#L28](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.Swashbuckle/wwwroot/swagger/ui/abp.swagger.js#L28)
* [github.com/abpframework/abp/blob/dev/modules/cms-kit/src/Volo.CmsKit.Admin.Web/Pages/CmsKit/Pages/update.js#L54](https://github.com/abpframework/abp/blob/dev/modules/cms-kit/src/Volo.CmsKit.Admin.Web/Pages/CmsKit/Pages/update.js#L54)
* [github.com/abpframework/abp/blob/dev/modules/cms-kit/src/Volo.CmsKit.Admin.Web/Pages/CmsKit/Pages/create.js#L84](https://github.com/abpframework/abp/blob/dev/modules/cms-kit/src/Volo.CmsKit.Admin.Web/Pages/CmsKit/Pages/create.js#L84)
* [github.com/abpframework/abp/blob/392beb897bb2d7214db8facba7a2022be7aa837c/modules/cms-kit/src/Volo.CmsKit.Admin.Web/Pages/CmsKit/BlogPosts/update.js#L91](https://github.com/abpframework/abp/blob/392beb897bb2d7214db8facba7a2022be7aa837c/modules/cms-kit/src/Volo.CmsKit.Admin.Web/Pages/CmsKit/BlogPosts/update.js#L91)
* [github.com/abpframework/abp/blob/dev/modules/cms-kit/src/Volo.CmsKit.Admin.Web/Pages/CmsKit/BlogPosts/create.js#L127](https://github.com/abpframework/abp/blob/dev/modules/cms-kit/src/Volo.CmsKit.Admin.Web/Pages/CmsKit/BlogPosts/create.js#L127)
* [github.com/abpframework/abp/blob/dev/modules/docs/app/VoloDocs.Web/wwwroot/libs/abp/jquery/abp.jquery.js#L261](https://github.com/abpframework/abp/blob/dev/modules/docs/app/VoloDocs.Web/wwwroot/libs/abp/jquery/abp.jquery.js#L261)
* [github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.AspNetCore.Components.Web/Volo/Abp/AspNetCore/Components/Web/AbpBlazorClientHttpMessageHandler.cs#L94](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.AspNetCore.Components.Web/Volo/Abp/AspNetCore/Components/Web/AbpBlazorClientHttpMessageHandler.cs#L94)

**Setting `XSRF-TOKEN` cookie as `HttpOnly`:**

If you want to set it, you can configure the `TokenCookie` property of the [AbpAntiForgeryOptions](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.AspNetCore.Mvc/Volo/Abp/AspNetCore/Mvc/AntiForgery/AbpAntiForgeryOptions.cs#L56) class.

**Setting `.AspNetCore.Culture` cookie as `HttpOnly`:**

If you want to set it, you can do it in the [AbpRequestCultureCookieHelper](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.AspNetCore/Microsoft/AspNetCore/RequestLocalization/AbpRequestCultureCookieHelper.cs#L16) class. Set the option as `HttpOnly = true`.

The related issue for this alert can be found at [github.com/abpframework/abp/issues/14214](https://github.com/abpframework/abp/issues/14214).

### Cookie with SameSite Attribute None [Risk: Low] — Positive (No need for a fix)

* *[GET] — https://localhost:44348 (and other several URLs...)*
* *[GET] — https://localhost:44348/Abp/ApplicationConfigurationScript*
* *[GET] — https://localhost:44348/Account/ForgotPassword (and there are several URLs)*
* *[GET] — https://localhost:44348/Abp/Languages/Switch?culture=ar&returnUrl=%2F%3Fpage%3D%252FAccount%252F%7E%252FAccount%252FLogin&uiCulture=a (and other several URLs...)*

**Description:** 

A cookie has been set with its `SameSite` attribute set to `none`, which means that the cookie can be sent as a result of a `cross-site` request. The `SameSite` attribute is an effective counter measure to cross-site request forgery, cross-site script inclusion, and timing attacks.

**Solution:** 

Ensure that the `SameSite` attribute is set to either `lax` or ideally `strict` for all cookies. You can see the Amazon.com `SameSite` attribute policy. We discussed setting the **SameSite** attribute to `strict` in the following issue [github.com/abpframework/abp/issues/14215](https://github.com/abpframework/abp/issues/14215) and decided to leave this change to the final developer.

![Amazon.com SameSite attribute policy](../images/pen-test-samesite-attribute.png)

### Cookie without `SameSite` Attribute [Risk: Low] — Positive (No need for a fix)

* *[GET] — https://localhost:44348/Abp/Languages/Switch?culture=ar&returnUrl=%2F&uiCulture=ar _(and other several URLs with different query parameters...)_*

**Description:** 

A cookie has been set with its `SameSite` attribute set to `none`, which means that the cookie can be sent as a result of a `cross-site` request. The `SameSite` attribute is an effective counter measure to cross-site request forgery, cross-site script inclusion, and timing attacks.

**Solution:** 

Ensure that the `SameSite` attribute is set to either `lax` or ideally `strict` for all cookies. We discussed setting the **SameSite** attribute to `strict` in the following issue [github.com/abpframework/abp/issues/14215](https://github.com/abpframework/abp/issues/14215) and decided to leave this change to the final developer.



### Strict-Transport-Security Header Not Set [Risk: Low] - Production/Deployment Setting

- *[GET] — https://localhost:44348/*
- *[GET] — https://localhost:44348/Abp/ApplicationConfigurationScript*
- *[GET] — https://localhost:44348/Abp/ApplicationLocalizationScript?cultureName=zh-Hant*
- *[DELETE] — https://localhost:44348/api/feature-management/features?providerName=E&providerKey=...*
- other URLS...

**Description**: 

HTTP Strict Transport Security (HSTS) is a web security policy mechanism whereby a web server declares that complying user agents (such as a web browser) are to interact with it using only secure HTTPS connections (i.e. HTTP layered over TLS/SSL). HSTS is an IETF standards track protocol and is specified.

**Solution**: 

Enabling HSTS on production.

**Explanation**: 

This alert is production/deployment dependent. The generated MVC template calls `UseHsts()` when the environment is not `Development`, but ASP.NET Core does not emit HSTS for localhost. Verify that HSTS is enabled and reaches the browser in the final production hosting topology.

![HSTS](../images/pen-test-hsts.png)

### Timestamp Disclosure - Unix [Risk: Low] - False Positive

- *[GET] — https://localhost:44348/libs/zxcvbn/zxcvbn.js?_v=638362269519660000*

**Description**: 

A timestamp was disclosed by the application/web server - Unix

**Solution**:

Manually confirm that the timestamp data is not sensitive, and that the data cannot be aggregated to disclose exploitable patterns.

**Explanation**: 

This vulnerability was reported as a positive alert, because ABP uses the [zxcvbn](https://github.com/dropbox/zxcvbn) library for [password complexity indicators](../framework/ui/angular/password-complexity-indicator-component.md). This library is one of the most used password strength estimator and it does not disclosure any sensitive data related to web server's timestamp and therefore it's a **false-positive** alert.

### X-Content-Type-Options Header Missing [Risk: Low] - Needs Review for Static/Deployment Paths

- *[GET] — https://localhost:44348/client-proxies/account-proxy.js?_v=639159181980000000 (and other client-proxies related URLs...)*
- *[GET] — https://localhost:44348/favicon.svg*
- *[GET] — https://localhost:44348/LeptonX/images/login-pages/login-bg-img-dark.svg*
- *[GET] — https://localhost:44348/libs/abp/aspnetcore-mvc-ui-theme-shared/authentication-state/authentication-state-listener.js?_v=639158976700000000*
- other URLs...

**Description**: 

The Anti-MIME-Sniffing header `X-Content-Type-Options` was not set to 'nosniff'. This allows older versions of Internet Explorer and Chrome to perform MIME-sniffing on the response body, potentially causing the response body to be interpreted and displayed as a content type other than the declared content type. Current (early 2014) and legacy versions of Firefox will use the declared content type (if one is set), rather than performing MIME-sniffing.

**Solution**:

Ensure that the application/web server sets the Content-Type header appropriately, and that it sets the X-Content-Type-Options header to 'nosniff' for all web pages.

If possible, ensure that the end user uses a standards-compliant and modern web browser that does not perform MIME-sniffing at all, or that can be directed by the web application/web server to not perform MIME-sniffing.

**Explanation**: 

The `X-Content-Type-Options` header allows you to avoid MIME type sniffing by saying that the MIME types are deliberately configured. This header is not strictly required, but it is highly recommended for security reasons. While modern browsers have improved security features, you can still set this header for ensuring the security of web applications.

The fresh local validation confirmed that [ABP's Security Header Middleware](../framework/ui/mvc-razor-pages/security-headers.md#security-headers-middleware) emits `X-Content-Type-Options: nosniff` for checked MVC pages and scripts. Since the ZAP report still listed missing headers for several client proxy and static asset URLs, verify static-file handling, proxy/CDN behavior, and any custom middleware ordering in the final application. This middleware also adds other pre-defined security headers, including `X-XSS-Protection`, `X-Frame-Options` and `Content-Security-Policy` (if it's enabled). Read [Security Headers](../framework/ui/mvc-razor-pages/security-headers.md) documentation for more info.
