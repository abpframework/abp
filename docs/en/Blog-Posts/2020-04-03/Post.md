# Custom Login PageModel in **ABP** ASP.NET Core MVC application

ABP Framework uses Microsoft Identity underneath hence supports customization as much as Microsoft Identity does.

### Creating Login PageModel

To create your own custom Login PageModel, you need to inherit [Abp LoginModel](https://github.com/abpframework/abp/blob/037ef9abe024c03c1f89ab6c933710bcfe3f5c93/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml.cs).

````xml
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

### Overriding Methods

Afterwards you can override a method like `CreateExternalUserAsync`:

````xml
protected override async Task<Volo.Abp.Identity.IdentityUser> CreateExternalUserAsync(ExternalLoginInfo info)
{
    var emailAddress = info.Principal.FindFirstValue(AbpClaimTypes.Email);

    var user = new IdentityUser(GuidGenerator.Create(), emailAddress, emailAddress, CurrentTenant.Id);

    CheckIdentityErrors(await UserManager.CreateAsync(user));
    CheckIdentityErrors(await UserManager.SetEmailAsync(user, emailAddress));
    CheckIdentityErrors(await UserManager.AddLoginAsync(user, info));

    return user;
}
````

### Overriding Login Page UI

Overriding `.cshtml` files can be easily done via [Virtual File System](https://docs.abp.io/en/abp/latest/Virtual-File-System). Create folder named **Account** under **Pages** directory. Create **Login.cshtml** under Pages/Account directory. 

Set the model with your newly created Login Page Model and customize to your preferences like:

````xml
@page
@using Volo.Abp.Account.Settings
@using Volo.Abp.Settings
@model Acme.BookStore.Web.CustomLoginModel
@inherits Volo.Abp.Account.Web.Pages.Account.AccountPage
@inject Volo.Abp.Settings.ISettingProvider SettingProvider

<div class="jumbotron">
    <p>My Customized Login Page! </p>
</div>
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

Further readings, [ASP.NET Core (MVC / Razor Pages) User Interface Customization Guide](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Customization-User-Interface#asp-net-core-mvc-razor-pages-user-interface-customization-guide).