# 如何为MVC / Razor页面应用程序自定义登录页面

当你使用[应用程序启动模板](../Startup-Templates/Application.md)创建了一个新的应用程序, 登录页面的源代码并不在你的解决方案中,所以你不能直接更改. 它来自[账户模块](../Modules/Account.md),使用[NuGet包](https://www.nuget.org/packages/Volo.Abp.Account.Web)引用.

本文介绍了如何为自己的应用程序自定义登录页面.

## 创建登录 PageModel

创建一个新的类继承账户模块的[LoginModel](https://github.com/abpframework/abp/blob/037ef9abe024c03c1f89ab6c933710bcfe3f5c93/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml.cs).

````csharp
public class CustomLoginModel : LoginModel
{
    public CustomLoginModel(
    Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider schemeProvider,
    Microsoft.Extensions.Options.IOptions<Volo.Abp.Account.Web.AbpAccountOptions> accountOptions)
        : base(schemeProvider, accountOptions)
        {
        }
}
````

> 在这里命令约定很重要. 如果你的类名不是以 `LoginModel` 结束,你需要手动在[依赖注入](../Dependency-Injection.md)系统替换 `LoginModel`.

然后你可以覆盖任何方法并添加用户界面需要的新方法和属性.

## 重写登录页面UI

在 **Pages** 目录下创建名为 **Account** 的文件夹,并在这个文件夹中创建 `Login.cshtml` , 它会自动覆盖账户模块的页面文件.

自定义页面一个很好的开始是复制它的源代码. [点击这里](https://github.com/abpframework/abp/blob/dev/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml)找到登录页面的源码. 在编写本文档时,源代码如下:

````xml
@page
@using Volo.Abp.Account.Settings
@using Volo.Abp.Settings
@model Acme.BookStore.Web.Pages.Account.CustomLoginModel
@inject Volo.Abp.Settings.ISettingProvider SettingProvider
@if (Model.EnableLocalLogin)
{
    <div class="card mt-3 shadow-sm rounded">
        <div class="card-body p-5">
            <h4>@L["Login"]</h4>
            @if (await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled))
            {
                <strong>
                    @L["AreYouANewUser"]
                    <a href="@Url.Page("./Register", new {returnUrl = Model.ReturnUrl, returnUrlHash = Model.ReturnUrlHash})" class="text-decoration-none">@L["Register"]</a>
                </strong>
            }
            <form method="post" class="mt-4">
                <input asp-for="ReturnUrl" />
                <input asp-for="ReturnUrlHash" />
                <div class="form-group">
                    <label asp-for="LoginInput.UserNameOrEmailAddress"></label>
                    <input asp-for="LoginInput.UserNameOrEmailAddress" class="form-control" />
                    <span asp-validation-for="LoginInput.UserNameOrEmailAddress" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label asp-for="LoginInput.Password"></label>
                    <input asp-for="LoginInput.Password" class="form-control" />
                    <span asp-validation-for="LoginInput.Password" class="text-danger"></span>
                </div>
                <div class="form-check">
                    <label asp-for="LoginInput.RememberMe" class="form-check-label">
                        <input asp-for="LoginInput.RememberMe" class="form-check-input" />
                        @Html.DisplayNameFor(m => m.LoginInput.RememberMe)
                    </label>
                </div>
                <abp-button type="submit" button-type="Primary" name="Action" value="Login" class="btn-block btn-lg mt-3">@L["Login"]</abp-button>
            </form>
        </div>

        <div class="card-footer text-center border-0">
            <abp-button type="button" button-type="Link" name="Action" value="Cancel" class="px-2 py-0">@L["Cancel"]</abp-button> @* TODO: Only show if identity server is used *@
        </div>
    </div>
}

@if (Model.VisibleExternalProviders.Any())
{
    <div class="col-md-6">
        <h4>@L["UseAnotherServiceToLogIn"]</h4>
        <form asp-page="./Login" asp-page-handler="ExternalLogin" asp-route-returnUrl="@Model.ReturnUrl" asp-route-returnUrlHash="@Model.ReturnUrlHash" method="post">
            <input asp-for="ReturnUrl" />
            <input asp-for="ReturnUrlHash" />
            @foreach (var provider in Model.VisibleExternalProviders)
            {
                <button type="submit" class="btn btn-primary" name="provider" value="@provider.AuthenticationScheme" title="@L["GivenTenantIsNotAvailable", provider.DisplayName]">@provider.DisplayName</button>
            }
        </form>
    </div>
}

@if (!Model.EnableLocalLogin && !Model.VisibleExternalProviders.Any())
{
    <div class="alert alert-warning">
        <strong>@L["InvalidLoginRequest"]</strong>
        @L["ThereAreNoLoginSchemesConfiguredForThisClient"]
    </div>
}
````

只需更改 `@model` 为 `Acme.BookStore.Web.Pages.Account.CustomLoginModel` 使用自定义的 `PageModel` 类. 你可以做任何应用程序需要的更改.

## 本文的源代码

你可以在[这里](https://github.com/abpframework/abp-samples/tree/master/Authentication-Customization)找到已完成的示例源码.

## 另请参阅

* [ASP.NET Core (MVC / Razor Pages) 用户界面自定义指南](../UI/AspNetCore/Customization-User-Interface.md).
