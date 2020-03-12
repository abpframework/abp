# ASP.NET Core (MVC / Razor Pages) User Interface Customization Guide

This document explains how to override the user interface of a depended [application module](../../Modules/Index.md) for ASP.NET Core MVC / Razor Page applications.

## Overriding a Page

This section covers the [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/) development, which is the recommended approach to create server rendered user interface for ASP.NET Core. Pre-built modules typically uses the Razor Pages approach instead of the classic MVC pattern (next sections will cover the MVC pattern too).

You typically have three kind of override requirement for a page:

* Overriding **only the Page Model** (C#) side to perform additional logic without changing the page UI.
* Overring **only the Razor Page** (.chtml file) to change the UI without changing the c# behind the page.
* **Completely overriding** the page.

### Overriding a Page Model (C#)

````csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web.Pages.Identity.Users;

namespace Acme.BookStore.Web.Pages.Identity.Users
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(EditModalModel))]
    public class MyEditModalModel : EditModalModel
    {
        public MyEditModalModel(
            IIdentityUserAppService identityUserAppService, 
            IIdentityRoleAppService identityRoleAppService
            ) : base(
                identityUserAppService, 
                identityRoleAppService)
        {
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            //TODO: Additional logic
            await base.OnPostAsync();
            //TODO: Additional logic
        }
    }
}
````

* This class inherits from and replaces the `EditModalModel` for the users and overrides the `OnPostAsync` method to perform additional logic before and after the underlying code.
* It uses `ExposeServices` and `Dependency` attributes to replace the class.

### Overriding a Razor Page (.CSHTML)

Overriding a `.cshtml` file (razor page, razor view, view component... etc.) is possible through the [Virtual File System](../../Virtual-File-System.md).

Virtual File system allows us to **embed resources into assemblies**. In this way, pre-built modules define the razor pages inside their NuGet packages. When you depend a module, you can override any file added to the virtual file system by that module, including pages/views.

#### Example

This example overrides the **login page** UI defined by the [Account Module](../../Modules/Account.md).

Physical files override the embedded files defined in the same location. The account module defines a `Login.cshtml` file under the `Pages/Account` folder. So, you can override it by creating a file in the same path:

![overriding-login-cshtml](../../images/overriding-login-cshtml.png)

You typically want to copy the original `.cshtml` file of the module, then make the necessary changes. You can find the original file [here](https://github.com/abpframework/abp/blob/dev/modules/account/src/Volo.Abp.Account.Web/Pages/Account/Login.cshtml). Do not copy the `Login.cshtml.cs` file which is the code behind file for the razor page and we don't want to override it yet (see the next section).

That's all, you can change the file content however you like.

### Completely Overriding a Razor Page

You may want to completely override a page; the razor and the c# file related to the page.

In such a case;

1. Override the C# page model class just like described above, but don't replace the existing page model class.
2. Override the Razor Page just described above, but also change the @model directive to point your new page model.

#### Example

This example overrides the **login page** defined by the [Account Module](../../Modules/Account.md).

Create a page model class deriving from the ` LoginModel ` (defined in the ` Volo.Abp.Account.Web.Pages.Account ` namespace):

````csharp
public class MyLoginModel : LoginModel
{
    public MyLoginModel(
        IAuthenticationSchemeProvider schemeProvider, 
        IOptions<AbpAccountOptions> accountOptions
        ) : base(
        schemeProvider, 
        accountOptions)
    {

    }

    public override Task<IActionResult> OnPostAsync(string action)
    {
        //TODO: Add logic
        return base.OnPostAsync(action);
    }

    //TODO: Add new methods and properties...
}
````

You can override any method or add new properties/methods if needed.

> Notice that we didn't use `[Dependency(ReplaceServices = true)]` or `[ExposeServices(typeof(LoginModel))]` since we don't want to replace the existing class in the dependency injection, we define a new one.

Copy `Login.cshtml` file into your solution as just described above. Change the **@model** directive to point to the `MyLoginModel`:

````xml
@page
...
@model Acme.BookStore.Web.Pages.Account.MyLoginModel
...
````

That's all! Make any change in the view and run your application.

#### Replacing Page Model Without Inheritance

You don't have to inherit from the original page model class (like done in the previous example). Instead, you can completely re-implement the page yourself. In this case, just derive from `PageModel`, `AbpPageModel` or any suitable base class you need.