# 如何对MVC / Razor页面应用程序使用Azure Active Directory身份验证

本文介绍了如何将AzureAD集成到ABP应用程序中,用 **Azure Active Directory** 凭据使用 OAuth 2.0 登录.

添加Azure Active Directory到ABP框架非常简单,只需要正确的完成几个配置.

为了覆盖更多范围,我们演示两种不同的集成AzureAD的**方法**.

1. **AddAzureAD**: 该方法使用微软[AzureAD UI nuget 包](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.AzureAD.UI/),在网络上搜索如何将AzureAD集成到应用程序时,这个包是最流行的.

2. **AddOpenIdConnect**: 该方法使用默认的[OpenIdConnect](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect/). 它不仅可用于AzureAD,还可用于所有OpenId连接.

> 这些方法之间的功能**没有区别**,AddAzureAD是具有预定义Cookie设置的OpenIdConnection([源](https://github.com/dotnet/aspnetcore/blob/c56aa320c32ee5429d60647782c91d53ac765865/src/Azure/AzureAD/Authentication.AzureAD.UI/src/AzureADAuthenticationBuilderExtensions.cs#L122))的抽象方法.
>
> 但是默认配置的登录方案在与ABP应用程序集成方面存在关键差异,下面将对此进行说明.

## 1. AddAzureAD

这个方法使用 [Microsoft AzureAD UI nuget 包](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.AzureAD.UI/),它是最常用的集成AzureAD方法.

如果选择这种方法,需要将 `Microsoft.AspNetCore.Authentication.AzureAD.UI` 软件包安装到 **.Web** 项目中. 由于AddAzureAD扩展使用[配置绑定](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#default-configuration),你需要更改 **.Web** 项目中的appsettings.json文件.

#### **更改 `appsettings.json`**

你添加向 `appsettings.json` 添加新的配置节,在配置 `OpenIdConnectOptions` 时绑定配置:

````json
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<your-tenant-id>",
    "ClientId": "<your-client-id>",
    "Domain": "domain.onmicrosoft.com",
    "CallbackPath": "/signin-azuread-oidc"
  }
````

> 这里重要的配置是CallbackPath. 值必须与你的 Azure AD-> app registrations-> Authentication -> RedirectUri 之一相同.

然后你需要配置 `OpenIdConnectOptions` 完成集成.

#### 配置 OpenIdConnectOptions

在你的 **.Web** 项目找到 **ApplicationWebModule** 使用以下代码修改 `ConfigureAuthentication` 方法:

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

> **不要忘记:**
>
> * 在 `AddAuthentication()` 之后添加 `.AddAzureAD(options => configuration.Bind("AzureAd", options))` . 它绑定了你的 AzureAD 配置并且容易忘记.
> * 添加 `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear()`. 它会禁用默认的 Microsoft claim type 映射.
> * 添加 `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier)`. 映射 [ClaimTypes.NameIdentifier](https://github.com/dotnet/runtime/blob/6d395de48ac718a913e567ae80961050f2a9a4fa/src/libraries/System.Security.Claims/src/System/Security/Claims/ClaimTypes.cs#L59) 很重要,因为默认SignIn Manager和行为使用这个claim type用于外部登录信息.
> * 添加 `options.SignInScheme = IdentityConstants.ExternalScheme` 因为 [默认登录方法为 `AzureADOpenID`](https://github.com/dotnet/aspnetcore/blob/c56aa320c32ee5429d60647782c91d53ac765865/src/Azure/AzureAD/Authentication.AzureAD.UI/src/AzureADOpenIdConnectOptionsConfiguration.cs#L35).
> * 如果你使用的是 **v2.0** 端点,应添加 `options.Scope.Add("email")` 因为 v2.0 端点不会将 `email` 做为默认值返回. [账户模块](../Modules/Account.md) 使用 `email` claim 来 [注册外部账户](https://github.com/abpframework/abp/blob/be32a55449e270d2d456df3dabdc91f3ffdd4fa9/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml.cs#L215).

你已经完成了集成.

## 2. 替代方法: AddOpenIdConnect

如果你不想在应用程序安装一个额外的NuGet包,你可以使用默认的[OpenIdConnect](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect/),它适用于所有的OpenId连接,包括AzureAD外部认证.

你不必使用 `appsettings.json` 配置, 但将AzureAD信息放在 `appsettings.json` 是一个很好的做法.

为了从 `appsettings.json` 获取AzureAD信息在 `OpenIdConnectOptions` 配置使用,只需要在你的 **.Web** 项目中的 `appsettings.json` 添加一个新的配置节:

````json
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<your-tenant-id>",
    "ClientId": "<your-client-id>",
    "Domain": "domain.onmicrosoft.com",
    "CallbackPath": "/signin-azuread-oidc"	
  }
````

然后在你的 **.Web** 项目的 **ApplicationWebModule** 用以下代码修改 `ConfigureAuthentication` 方法:

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

集成结束. 请记住你可以连接任何其他外部认证供应商.

## 本文的源代码

你可以在[这里](https://github.com/abpframework/abp-samples/tree/master/Authentication-Customization)找到已完成的示例源码.

# FAQ

* Help! `GetExternalLoginInfoAsync` 返回 `null`!

  * 有两方面的原因;

    1. 你在尝试验证错误的方案. 检查是否设置 **SignInScheme** 为 `IdentityConstants.ExternalScheme`:

       ````csharp
       options.SignInScheme = IdentityConstants.ExternalScheme;
       ````

    2. 你的 `ClaimTypes.NameIdentifier` 为 `null`. 检查是否添加 claim 映射:

       ````csharp
       JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
       JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier);
       ````

* Help! 我一直得到 ***System.ArgumentNullException: Value cannot be null. (Parameter 'userName')*** 错误!

    * 当你使用 Azure Authority **v2.0 端点** 而不请求 `email` 域, 会发生这些情况. [Abp 创建用户检查了唯一的邮箱](https://github.com/abpframework/abp/blob/037ef9abe024c03c1f89ab6c933710bcfe3f5c93/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml.cs#L208). 只需添加

      ````csharp
      options.Scope.Add("email");
      ````

      到你的 openid 配置.

* Help! 我一直得到 ***AADSTS50011: The reply URL specified in the request does not match the reply URLs configured for the application*** 错误!

  * 如果你在appsettings设置 **CallbackPath** 为:

    ````csharp
      "AzureAd": {
        ...
        "CallbackPath": "/signin-azuread-oidc"
      }
    ````

   你在azure门户的应用程序**重定向URI**必须具有之类 `https://localhost:44320/signin-azuread-oidc` 的<u>域</u>, 而不仅是 `/signin-azuread-oidc`.

* Help! 我一直得到 ***AADSTS700051: The response_type 'token' is not enabled for the application.*** 错误!

  * 当你请求**token**(访问令牌)和**id_token**时没有在Azure门户应用程序启用访问令牌时会发生这个错误,只需勾选ID令牌顶部的**访问令牌**复选框即可同时请求令牌.

* Help! 我一直得到 ***AADSTS7000218: The request body must contain the following parameter: 'client_assertion' or 'client_secret*** 错误!

  * 当你与 **id_token** 一起请求 **code**时,你需要在Azure门户的**证书和机密**菜单下添加**证书和机密**. 然后你需要添加openid配置选项:

    ````csharp
    options.ClientSecret = "Value of your secret on azure portal";
    ````

* 如何**调试/监视**在映射之前获得的声明?

  * 你可以在 openid 配置下加一个简单的事件在映射之前进行调试,例如:

    ````csharp
    options.Events.OnTokenValidated = (async context =>
    {
    	var claimsFromOidcProvider = context.Principal.Claims.ToList();
    	await Task.CompletedTask;
    });
    ````

## 另请参阅

* [如何为MVC / Razor页面应用程序自定义登录页面](Customize-Login-Page-MVC.md).
* [如何为ABP应用程序定制SignIn Manager](Customize-SignIn-Manager.md).