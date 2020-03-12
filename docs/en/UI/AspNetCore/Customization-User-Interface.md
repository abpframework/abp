# ASP.NET Core (MVC / Razor Pages) User Interface Customization Guide

This document explains how to override the user interface of a depended [application module](../../Modules/Index.md) for ASP.NET Core MVC / Razor Page applications.

## Overriding a Page Model

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

This class replaces `EditModalModel` for the users and overrides the `OnPostAsync` method.

