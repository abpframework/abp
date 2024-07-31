# How to Customize the Login Page for MVC / Razor Page Applications

When you create a new application using the [application startup template](https://docs.abp.io/en/abp/latest/Startup-Templates/Application), source code of the login page will not be inside your solution, so you can not directly change it. The login page comes from the [Account Module](https://docs.abp.io/en/abp/latest/Modules/Account) that is used a [NuGet package](https://www.nuget.org/packages/Volo.Abp.Account.Web) reference.

This document explains how to customize the login page for your own application.

## Create a Login PageModel

Create a new class inheriting from the [LoginModel](https://github.com/abpframework/abp/blob/037ef9abe024c03c1f89ab6c933710bcfe3f5c93/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml.cs) of the Account module.

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

> Naming convention is important here. If your class name doesn't end with `LoginModel`, you need to manually replace the `LoginModel` using the [dependency injection](https://docs.abp.io/en/abp/latest/Dependency-Injection) system.

Then you can override any method you need and add new methods and properties needed by the UI.

## Overriding the Login Page UI

Create folder named **Account** under **Pages** directory and create a **Login.cshtml** under this folder. It will automatically override the `Login.cshtml` file defined in the Account Module.

A good way to customize a page is to copy its source code. [Click here](https://github.com/abpframework/abp/blob/dev/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml) for the source code of the login page. At the time this document has been written, the source code was like below:

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

Just changed the `@model` to `Acme.BookStore.Web.Pages.Account.CustomLoginModel`  to use the customized `PageModel` class. You can change it however your application needs.

## The Source Code

You can find the source code of the completed example [here](https://github.com/abpframework/abp-samples/tree/master/Authentication-Customization).