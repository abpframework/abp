# Migration Guide for the Blazor UI from the v3.2 to the v3.3

## Startup Template Changes

All these changes should be done for the `.Blazor` project in your solution;

* Update the`AddOidcAuthentication` options in your *YourProjectBlazorModule* class as described in the issue [#5913](https://github.com/abpframework/abp/issues/5913).
* Add a `Components/Layout/MainFooterComponent.razor ` file with the following content:

````html
<span class="copyright-text">@DateTime.Now.Year © MyProjectName</span> 
````

Change `MyProjectName` with your own project name or completely modify the footer based on your preference.

* For the `.Blazor.csproj` file, remove the `Volo.Abp.Account.Pro.Public.Blazor` package and add these packages: `Volo.Abp.SettingManagement.Blazor`, `Volo.Saas.Host.Blazor`, `Volo.Abp.LeptonTheme.Management.Blazor`, `Volo.Abp.Account.Pro.Admin.Blazor`, `Volo.Abp.TextTemplateManagement.Blazor`, `Volo.Abp.AuditLogging.Blazor`, `Volo.Abp.LanguageManagement.Blazor`.
* Add the following module dependencies for *YourProjectBlazorModule* class:

````csharp
typeof(SaasHostBlazorModule),
typeof(AbpSettingManagementBlazorModule),
typeof(LeptonThemeManagementBlazorModule),
typeof(AbpAccountAdminBlazorModule),
typeof(AbpAuditLoggingBlazorModule),
typeof(TextTemplateManagementBlazorModule),
typeof(LanguageManagementBlazorModule)
````

* Add the following code into the `ConfigureServices` method of *YourProjectBlazorModule* class:

````csharp
Configure<LeptonThemeOptions>(options =>
{
    options.FooterComponent = typeof(MainFooterComponent);
});
````

This sets the footer component you've created before.

* You may want to add (or change the existing) menu contributor to re-order the menu items added by the depended modules:

````csharp
using System.Threading.Tasks;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AuditLogging.Blazor.Menus;
using Volo.Abp.Identity.Pro.Blazor.Navigation;
using Volo.Abp.LanguageManagement.Blazor.Menus;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TextTemplateManagement.Blazor.Menus;
using Volo.Abp.UI.Navigation;
using Volo.Saas.Host.Blazor.Navigation;

namespace MyCompanyName.MyProjectName.Blazor.Navigation
{
    public class MyProjectNameMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.DisplayName != StandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            var l = context.GetLocalizer<MyProjectNameResource>();

            context.Menu.AddItem(new ApplicationMenuItem(
                MyProjectNameMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home",
                order: 1
            ));

            //Administration
            var administration = context.Menu.GetAdministration();
            administration.Order = 2;

            //Administration->Saas
            administration.SetSubItemOrder(SaasHostMenus.GroupName, 1);

            //Administration->Identity
            administration.SetSubItemOrder(IdentityProMenus.GroupName, 2);

            //Administration->Language Management
            administration.SetSubItemOrder(LanguageManagementMenus.GroupName, 3);

            //Administration->Text Template Management
            administration.SetSubItemOrder(TextTemplateManagementMenus.GroupName, 4);

            //Administration->Audit Logs
            administration.SetSubItemOrder(AbpAuditLoggingMenus.GroupName, 5);

            //Administration->Settings
            administration.SetSubItemOrder(SettingManagementMenus.GroupName, 6);

            return Task.CompletedTask;
        }
    }
}
````

* You may need to add or update the `MyProjectNameMenus.cs` for your project:

````csharp
namespace MyCompanyName.MyProjectName.Blazor.Navigation
{
    public class MyProjectNameMenus
    {
        private const string Prefix = "MyProjectName";

        public const string Home = Prefix + ".Home";
    }
}
````

* Add the missing files into the `wwwroot` folder. The best way to do that is to create a new Blazor UI solution, open the `wwwroot` folder and copy the missing files. In the next versions, we will work to build a 3rd party library management system for automatic upgrades. However, please consider the current Blazor UI is not so complete.
* Open the `wwwroot/index.html` file and make the following changes;

Remove the following line:

````html
<link href="_content/Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme/theme.css" rel="stylesheet" />
````

Add the following lines in the `head` section, before the `main.css`:

````html
<link href="_content/Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme/themes/lepton/styles/lepton1.css" rel="stylesheet" id="LeptonStyle" />
<link href="flag-icon-css/css/flag-icon.min.css" rel="stylesheet"/>
````

Remove the following code (since the ABP now provides a better error handling system):

````html
<div id="blazor-error-ui">
    An unhandled error has occurred.
    <a href="" class="reload">Reload</a>
    <a class="dismiss">🗙</a>
</div>
````

Remove the following line:

````html
<script src="_content/Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme/theme.js"></script>
````

Add the following lines instead:

````html
<script src="_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/abp_theming.js"></script>
<script src="_content/Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme/themes/lepton/scripts/lepton.js"></script>
````

* Upgrade Microsoft.Extensions.* packages to 3.1.8+.

If you have trouble, it is best to download a new solution and compare the files with yours. There are not so many files in the startup template.

## BlazoriseCrudPageBase to AbpCrudPageBase

Renamed `BlazoriseCrudPageBase` to `AbpCrudPageBase`. Just update the usages. It also has some changes, you may need to update method calls/usages manually.