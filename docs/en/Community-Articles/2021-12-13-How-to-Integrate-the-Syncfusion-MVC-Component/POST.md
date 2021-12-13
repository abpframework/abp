# How to Integrate the Syncfusion MVC Components to the ABP MVC UI?

## Introduction

Hi, in this step by step article we will see how we can integrate the Syncfusion MVC Components to our ABP MVC UI.

## Source Code

You can find the source code of the application at https://github.com/EngincanV/ABP-Syncfusion-Components-Demo.

## Prerequisites

* [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

    * In this article, I will create a new startup template in v5.0.0-rc.2 and if you follow this article from top to end and create a new startup template with me, you need to install the [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) before starting.


Also update your ABP CLI to the v5.0.0-rc.2, you can use the below command to update your CLI version:

```bash
dotnet tool update Volo.Abp.Cli -g --version 5.0.0-rc.2
```

or install if you haven't installed before:

```bash
dotnet tool install Volo.Abp.Cli -g --version 5.0.0-rc.2
```

## Creating the Solution

In this article, I will create a new startup template with EF Core as a database provider and MVC for UI framework. But if you already have a project with MVC UI, you don't need to create a new startup template, you can directly implement the following steps to your existing project.

> If you already have a project, you can skip this section.

We can create a new startup template by using the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI):

```bash
abp new SyncfusionComponentsDemo -t app --preview
```

Our project boilerplate will be ready after the download is finished. Then, we can open the solution and start developing.

## Starting the Development

### Pre-requisite

> If you've already had a license from Syncfusion, you can skip this section and starts with Step 1.

* First thing we need to do is creating an account to be able to get license from Syncfusion. Let's navigate to https://www.syncfusion.com/aspnet-core-ui-controls and click the "Download Free Trial" button. 

* Then fill the form and starts your 30-day free trial.

* After that, navigates to https://www.syncfusion.com/account/manage-trials/downloads to get our license key that will be used in our application. 

![](./manage-trial-1.png)

click the "Get License Key" link for "ASP.NET Core (Essential JS 2)".

![](./manage-trial-2.png)

Then a modal will be open like above, select the version and click the "Get License Key" button.

![](./copy-license-key.png)

* Lastly, copy the generated license key value. This license key will be used in our application, to let Syncfusion check do our license is not expired and valid.


### Step 1 (Configurations)

After providing a license key from Syncfusion, we can start with configuration thats need to be done in our application.

* We need to install the `Syncfusion.EJ2.AspNet.Core` Nuget package to our Web project (*.Web).

![](./syncfusion-package.png)

* After installing the package, we need to register our license key to be able to use the Syncfusion Components. 

* To register the license key, open your web module class and update the `ConfigureServices` methods as follows:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var hostingEnvironment = context.Services.GetHostingEnvironment();
    var configuration = context.Services.GetConfiguration();

    //Register Syncfusion license
    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey: configuration["Syncfusion:LicenseKey"]);

    ConfigureUrls(configuration);
    ConfigureBundles();
    ConfigureAuthentication(context, configuration);
    ConfigureAutoMapper();
    ConfigureVirtualFileSystem(hostingEnvironment);
    ConfigureLocalizationServices();
    ConfigureNavigationServices();
    ConfigureAutoApiControllers();
    ConfigureSwaggerServices(context.Services);
}
```

Instead of writing the license key in here we can define it in **appsettings.json** file and use it in here by using the Configuration system of .NET.

* Open your **appsettings.json** file and add a new section named "Syncfusion":

```json
{  
  //...
  
  "Syncfusion": {
    "LicenseKey": "<your-license-key>"
  }
}
```

> Replace the `<your-license-key> part with your license key that we've obtained in the previous section.`

* To be able to use the Syncfusion Components we need to define it as tag helper in our **_ViewImports.cshtml** file. By doing that we can use the Syncfusion components everywhere in our application.

* So open your **/Pages/_ViewImports.cshtml** file and add a new tag helper:

```cshtml
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI.Bootstrap
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI.Bundling
@addTagHelper *, Syncfusion.EJ2 //use Syncfusion components
```

### Step 2 - (Add styles and script to our application)

* Last thing we need to do is, add some style and script files that provided by Syncfusion, between to our head-body tags. 

* We can do this by creating two view component (one for Styles and other for Scripts). Let's do that.

First, create folder structure like below under the **Components** folder.

![](./component-folder-structure.png)

Then open the related files and add the following codes to each of these files.

* **Default.cshtml** (/Components/Syncfusion/Script/Default.cshtml)

```cshtml
@addTagHelper *, Syncfusion.EJ2 //add this line

<!-- Syncfusion Essential JS 2 Scripts -->
<script src="https://cdn.syncfusion.com/ej2/dist/ej2.min.js"></script>

<!-- Syncfusion Essential JS 2 ScriptManager -->
<ejs-scripts></ejs-scripts>
```

* **SyncfusionScriptComponent.cs**

```csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace SyncfusionComponentsDemo.Web.Components.Syncfusion.Script
{
    public class SyncfusionScriptComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Components/Syncfusion/Script/Default.cshtml");
        }
    }
}
```

* **Default.cshtml** (/Components/Syncfusion/Style/Default.cshtml)

```cshtml
<!-- Syncfusion Essential JS 2 Styles -->
<link rel="stylesheet" href="https://cdn.syncfusion.com/ej2/material.css" />
```

* SyncfusionStyleComponent.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace SyncfusionComponentsDemo.Web.Components.Syncfusion.Style
{
    public class SyncfusionStyleComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Components/Syncfusion/Style/Default.cshtml");
        }
    }
}
```

After creating these two components, we can use the [**Layout Hooks**](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Layout-Hooks) system of ABP to inject this two components between head and script tags.

To do this, open your web module class and update the `ConfigureServices` method as below:


```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var hostingEnvironment = context.Services.GetHostingEnvironment();
    var configuration = context.Services.GetConfiguration();

    //Register Syncfusion license
    var licenseKey = configuration["Syncfusion:LicenseKey"].ToString();
    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey: licenseKey);

    Configure<AbpLayoutHookOptions>(options =>
    {
        //Now, the SyncfusionStyleComponent code will be inserted in the head of the page as the last item.
        options.Add(LayoutHooks.Head.Last, typeof(SyncfusionStyleComponent));
        
        //the SyncfusionScriptComponent will be inserted in the body of the page as the last item.
        options.Add(LayoutHooks.Body.Last, typeof(SyncfusionScriptComponent));
    });

    ConfigureUrls(configuration);
    ConfigureBundles();
    ConfigureAuthentication(context, configuration);
    ConfigureAutoMapper();
    ConfigureVirtualFileSystem(hostingEnvironment);
    ConfigureLocalizationServices();
    ConfigureNavigationServices();
    ConfigureAutoApiControllers();
    ConfigureSwaggerServices(context.Services);
}
```

After injecting the Syncfusion style and script into our application our configurations have been completed. We can try with a simple component to see if it works as we expected.

Let's try with the [Calendar](https://www.syncfusion.com/aspnet-core-ui-controls/calendar) component. Open your **Index.cshtml** file and update with the below content:

```cshtml
@page
@using Microsoft.AspNetCore.Mvc.Localization
@using SyncfusionComponentsDemo.Localization
@using Volo.Abp.Users
@model SyncfusionComponentsDemo.Web.Pages.IndexModel

@section styles {
	<abp-style src="/Pages/Index.css" />
}

@section scripts {
	<abp-script src="/Pages/Index.js" />
}

<div class="container">
	<h2>Syncfusion - Calendar Component</h2>
	<ejs-calendar id="calendar"></ejs-calendar>
</div>
```

* Then when we run the application, we need to see the relevant calendar component as below.

![](./calendar-component.png)

--- 

## See the Syncfusion Components in Action

//TODO: create a todo application and show some components.