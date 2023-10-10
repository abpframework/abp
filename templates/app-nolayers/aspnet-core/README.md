# MyCompanyName.MyProjectName

## About this solution

This is a minimalist, non-layered startup solution with the ABP Framework. All the fundamental ABP modules are already installed. 

### Pre-requirements

* [.NET 7.0+ SDK](https://dotnet.microsoft.com/download/dotnet)
* [Node v18 or 20](https://nodejs.org/en)

### Configurations

The solution comes with a default configuration that works out of the box. However, you may consider to change the following configuration before running your solution:

* Check the `ConnectionStrings` in `appsettings.json` files under the `MyCompanyName.MyProjectName` project and change it if you need.

### Before running the application

* Run `abp install-libs` command on your solution folder to install client-side package dependencies. This step is automatically done when you create a new solution with ABP CLI. However, you should run it yourself if you have first cloned this solution from your source control, or added a new client-side package dependency to your solution.
* The application needs to connect to a database. Run the following command in the `MyCompanyName.MyProjectName` directory:

````bash
dotnet run --migrate-database
````

This will create and seed the initial database. Then you can run the application with any IDE that supports .NET.

## Deploying the application

Deploying an ABP application is not different than deploying any .NET or ASP.NET Core application. However, there are some topics that you should care about when you are deploying your applications. You can check ABP's [Deployment documentation](https://docs.abp.io/en/abp/latest/Deployment/Index) before deploying your application.

### Additional resources

You can see the following resources to learn more about your solution and the ABP Framework:

* [Application (Single Layer) Startup Template](https://docs.abp.io/en/abp/latest/Startup-Templates/Application-Single-Layer)
<TEMPLATE-REMOVE IF-NOT='ui:mvc'>
<TEMPLATE-REMOVE IF-NOT='BASIC'>
* [ASP.NET Core MVC / Razor Pages: The Basic Theme](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Basic-Theme)
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='LEPTONXLITE'>
* [LeptonX Lite MVC UI](https://docs.abp.io/en/abp/latest/Themes/LeptonXLite/AspNetCore)
</TEMPLATE-REMOVE>
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='ui:blazor-server'>
<TEMPLATE-REMOVE IF-NOT='BASIC'>
* [Blazor UI: Basic Theme](https://docs.abp.io/en/abp/latest/UI/Blazor/Basic-Theme?UI=BlazorServer)
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='LEPTONXLITE'>
* [LeptonX Lite Blazor UI](https://docs.abp.io/en/abp/latest/Themes/LeptonXLite/Blazor?UI=BlazorServer)
</TEMPLATE-REMOVE>
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='ui:blazor'>
<TEMPLATE-REMOVE IF-NOT='BASIC'>
* [Blazor UI: Basic Theme](https://docs.abp.io/en/abp/latest/UI/Blazor/Basic-Theme?UI=Blazor)
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='LEPTONXLITE'>
* [LeptonX Lite Blazor UI](https://docs.abp.io/en/abp/latest/Themes/LeptonXLite/Blazor?UI=Blazor)
</TEMPLATE-REMOVE>
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='ui:angular'>
<TEMPLATE-REMOVE IF-NOT='BASIC'>
* [Angular UI: Basic Theme](https://docs.abp.io/en/abp/latest/UI/Angular/Basic-Theme)
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='LEPTONXLITE'>
* [LeptonX Lite Angular UI](https://docs.abp.io/en/abp/latest/Themes/LeptonXLite/Angular)
</TEMPLATE-REMOVE>
</TEMPLATE-REMOVE>
