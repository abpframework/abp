# MyCompanyName.MyProjectName

## About This Solution

This is a layered startup solution based on [Domain Driven Design (DDD)](https://docs.abp.io/en/abp/latest/Domain-Driven-Design) practises. All the fundamental ABP modules are already installed. 

### Pre-requirements

* [.NET 7.0+ SDK](https://dotnet.microsoft.com/download/dotnet)
* [Node v18 or 20](https://nodejs.org/en)
<TEMPLATE-REMOVE IF-NOT='TIERED'>
* [Redis](https://redis.io/)
</TEMPLATE-REMOVE>

### Configurations

The solution comes with a default configuration that works out of the box. However, you may consider to change the following configuration before running your solution:

<TEMPLATE-REMOVE IF-NOT='TIERED'>
* Check the `ConnectionStrings` in `appsettings.json` files under the `MyCompanyName.MyProjectName.AuthServer`, `MyCompanyName.MyProjectName.HttpApi.Host` and `MyCompanyName.MyProjectName.DbMigrator` projects and change it if you need.
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF='TIERED'>
<TEMPLATE-REMOVE IF-NOT='ui:mvc'>
* Check the `ConnectionStrings` in `appsettings.json` files under the `MyCompanyName.MyProjectName.Web` and `MyCompanyName.MyProjectName.DbMigrator` projects and change it if you need.
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='ui:blazor-server'>
* Check the `ConnectionStrings` in `appsettings.json` files under the `MyCompanyName.MyProjectName.Blazor` and `MyCompanyName.MyProjectName.DbMigrator` projects and change it if you need.
</TEMPLATE-REMOVE> 
<TEMPLATE-REMOVE IF-NOT='ui:blazor'>
* Check the `ConnectionStrings` in `appsettings.json` files under the `MyCompanyName.MyProjectName.HttpApi.Host` and `MyCompanyName.MyProjectName.DbMigrator` projects and change it if you need.
</TEMPLATE-REMOVE> 
<TEMPLATE-REMOVE IF-NOT='ui:angular'>
* Check the `ConnectionStrings` in `appsettings.json` files under the `MyCompanyName.MyProjectName.HttpApi.Host` and `MyCompanyName.MyProjectName.DbMigrator` projects and change it if you need.
</TEMPLATE-REMOVE>
</TEMPLATE-REMOVE>

### Preparings for the first run

* Run `abp install-libs` command on your solution folder to install client-side package dependencies. This step is automatically done when you create a new solution with ABP CLI. However, you should run it yourself if you have first cloned this solution from your source control, or added a new client-side package dependency to your solution.
* Run `MyCompanyName.MyProjectName.DbMigrator` to create the initial database. This should be done in the first run. It is also needed if a new database migration is added to the solution later.

### Solution structure

This is a layered monolith application that consists of the following applications:

* `MyCompanyName.MyProjectName.DbMigrator`: A console application which applies the migrations and also seeds the initial data. It is useful on development as well as on production environment.
<TEMPLATE-REMOVE IF-NOT='TIERED'>
* `MyCompanyName.MyProjectName.AuthServer`: ASP.NET Core MVC / Razor Pages application that is integrated OAuth 2.0(`OpenIddict`) and account modules. It is used to authenticate users and issue tokens.
* `MyCompanyName.MyProjectName.HttpApi.Host`: ASP.NET Core API application that is used to expose the APIs to the clients.
<TEMPLATE-REMOVE IF-NOT='ui:mvc'>
* `MyCompanyName.MyProjectName.Web`: ASP.NET Core MVC / Razor Pages application that is the essential web application of the solution.
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='ui:blazor-server'>
* `MyCompanyName.MyProjectName.Blazor`: ASP.NET Core Blazor Server application that is the essential web application of the solution.
</TEMPLATE-REMOVE> 
<TEMPLATE-REMOVE IF-NOT='ui:blazor'>
* `MyCompanyName.MyProjectName.Blazor`: ASP.NET Core Blazor WASM application that is a single page application that runs on the browser.
</TEMPLATE-REMOVE> 
<TEMPLATE-REMOVE IF-NOT='ui:angular'>
* `angular`: Angular application.
</TEMPLATE-REMOVE>
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF='TIERED'>
<TEMPLATE-REMOVE IF-NOT='ui:mvc'>
* `MyCompanyName.MyProjectName.Web`: ASP.NET Core MVC / Razor Pages application that is the essential web application of the solution.
</TEMPLATE-REMOVE>
<TEMPLATE-REMOVE IF-NOT='ui:blazor-server'>
* `MyCompanyName.MyProjectName.Blazor`: ASP.NET Core Blazor Server application that is the essential web application of the solution.
</TEMPLATE-REMOVE> 
<TEMPLATE-REMOVE IF-NOT='ui:blazor'>
* `MyCompanyName.MyProjectName.HttpApi.Host`: ASP.NET Core API application that is used to expose the APIs to the clients.
* `MyCompanyName.MyProjectName.Blazor`: ASP.NET Core Blazor Server application that is the essential web application of the solution.
</TEMPLATE-REMOVE> 
<TEMPLATE-REMOVE IF-NOT='ui:angular'>
* `MyCompanyName.MyProjectName.HttpApi.Host`: ASP.NET Core API application that is used to expose the APIs to the clients.
* `angular`: Angular application.
</TEMPLATE-REMOVE>
</TEMPLATE-REMOVE>

## Deploying the Application

Deploying an ABP application is not different than deploying any .NET or ASP.NET Core application. However, there are some topics that you should care about when you are deploying your applications. You can check ABP's [Deployment documentation](https://docs.abp.io/en/abp/latest/Deployment/Index) before deploying your application.

### Additional resources

You can see the following resources to learn more about your solution and the ABP Framework:

* [Web Application Development Tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1)
* [Application Startup Template Structure](https://docs.abp.io/en/abp/latest/Startup-Templates/Application)
