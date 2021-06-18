# 账户模块

该模块提供必要的UI页面与组件使用户登录和注册到应用程序.

> 文档正在更新

## 社交/外部登录

### 示例: Facebook 认证

按照[ASP.NET Core Facebook集成文档](https://docs.microsoft.com/zh-cn/aspnet/core/security/authentication/social/facebook-logins)向你应用程序添加Facebook登录.

#### 添加NuGet包

添加[Microsoft.AspNetCore.Authentication.Facebook]包到你的项目. 基于你的架构,可能是 `.Web`,`.IdentityServer`(对于分层启动)或 `.Host` 项目.

#### 配置提供程序

在你模块的 `ConfigureServices` 方法中使用 `.AddFacebook(...)` 扩展方法来配置客户端:

````csharp
context.Services.AddAuthentication()
    .AddFacebook(facebook =>
    {
        facebook.AppId = "...";
        facebook.AppSecret = "...";
        facebook.Scope.Add("email");
        facebook.Scope.Add("public_profile");
    });
````

> 最佳实践是使用 `appsettings.json` 或ASP.NET Core用户机密系统来存储你的凭据,而不是像这样硬编码值. 请参阅[微软](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins)文档了解如何使用用户机密.

### Angular UI

从v3.1开始,Angular UI使用授权码流程(作为最佳实践)通过重定向到MVC UI登录页面来对用户进行身份验证. 因此,即使你使用的是Angular UI,社交/外部登录集成也与上面说明的相同.并且可以开箱即用.