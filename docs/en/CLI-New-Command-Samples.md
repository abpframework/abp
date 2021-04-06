# ABP CLI Create Solution Sample Commands 

The `abp new` command creates an ABP solution or other artifacts based on an ABP template. ABP CLI has several parameters to create a new ABP solution. In this document we will show you some sample commands to create a new solution. All the project names are `Acme.BookStore`. Currently, the only available mobile project is a `React Native` mobile app. Available database providers are `Entity Framework Core` and `MongoDB`. All the commands starts with `abp new`.

## Angular

The following commands are for creating Angular UI projects:

* **Entity Framework Core**,  no mobile app, creates the project in a new folder:

  ````bash
  abp new Acme.BookStore -u angular --mobile none --database-provider ef -csf
  ````
  
* **Entity Framework Core**, default app template, **separate Identity Server**, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -t app -u angular -m none --separate-identity-server --database-provider ef -csf
  ```

* **Entity Framework Core**,  **custom connection string**, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -u angular -csf --connection-string Server=localhost;Database=MyDatabase;Trusted_Connection=True
  ```

* **MongoDB**, default app template, mobile project included, creates solution in `C:\MyProjects\Acme.BookStore`

  ```bash
  abp new Acme.BookStore -u angular --database-provider mongodb --output-folder C:\MyProjects\Acme.BookStore
  ```

* **MongoDB**, default app template, no mobile app, **separate Identity Server**, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -t app -u angular -m none --separate-identity-server --database-provider mongodb -csf
  ```

## MVC

The following commands are for creating MVC UI projects:

* **Entity Framework Core**, no mobile app, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -t app -u mvc --mobile none --database-provider ef -csf
  ```

* **Entity Framework Core**, **tier architecture** (*Web and HTTP API are separated*), no mobile app, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -u mvc --mobile none --tiered --database-provider ef -csf
  ```

* **MongoDB**, no mobile app, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -t app -u mvc --mobile none --database-provider mongodb -csf
  ```
  
* **MongoDB**, **tier architecture**, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -u mvc --tiered --database-provider mongodb -csf
  ```


## Blazor

The following commands are for creating Blazor projects:

* **Entity Framework Core**, no mobile app:

  ```bash
  abp new Acme.BookStore -t app -u blazor --mobile none
  ```

* **Entity Framework Core**, **separate Identity Server**, mobile app included:
  
  ```bash
  abp new Acme.BookStore -u blazor --separate-identity-server
  ```

* **MongoDB**, no mobile app, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -u blazor --database-provider mongodb --mobile none -csf
  ```

## No UI 

In the default app template, there is always a frontend project. In this option there is no frontend project. It has a `HttpApi.Host` project to serve your HTTP WebAPIs. It's appropriate if you want to create a WebAPI service.

* **Entity Framework Core**, separate Identity Server, creates the project in a new folder:

    ```bash
    abp new Acme.BookStore -u none --separate-identity-server -csf
    ```
* **MongoDB**, no mobile app:

    ```bash
    abp new Acme.BookStore -u none --mobile none --database-provider mongodb
    ```

## Console application

It's a template of a basic .NET console application with ABP module architecture integrated. To create a console application use the following command:

* This project consists of the following files: `Acme.BookStore.csproj`, `appsettings.json`, `BookStoreHostedService.cs`, `BookStoreModule.cs`, `HelloWorldService.cs` and `Program.cs`.

  ```bash
  abp new Acme.BookStore -t console -csf
  ```

## Module

Module are reusable sub applications used by your main project. Using ABP Module is a best practice if you are building a microservice solution. As modules are not final applications, each module has all the frontend UI projects and database providers. The module template comes with an MVC UI to be able to develop without the final solution. But if you will develop your module under a final solution, you add `--no-ui` parameter to exclude MVC UI project.

* Included frontends: `MVC`, `Angular`, `Blazor`. Included database providers: `Entity Framework Core`, `MongoDB`. Includes MVC startup project.

  ```bash
  abp new Acme.IssueManagement -t module
  ```
* The same with the upper but doesn't include MVC startup project.

  ```bash
  abp new Acme.IssueManagement -t module --no-ui
  ```

## Create a solution from a specific version

When you create a solution, it always creates with the latest version. To create a project from an older version, you can pass the `--version` parameter.

* Create a solution from v3.3.0, with Angular UI and Entity Framework Core.

  ```bash
  abp new Acme.BookStore -t app -u angular -m none --database-provider ef -csf --version 3.3.0
  ```

To get the ABP version list, checkout following link: https://www.nuget.org/packages/Volo.Abp.Core/

## Create from a custom template

ABP CLI uses the default [app template](https://github.com/abpframework/abp/tree/dev/templates/app) to create your project. If you want to create a new solution from your customized template, you can use the parameter `--template-source`. 

* MVC UI, Entity Framework Core, no mobile app, using the template in `c:\MyProjects\templates\app` directory. 

  ```bash
  abp new Acme.BookStore -t app -u mvc --mobile none --database-provider ef --template-source "c:\MyProjects\templates\app"
  ```

* Same with the previous one except this command retrieves the template from the URL `https://myabp.com/app-template.zip`.

  ```bash
  abp new Acme.BookStore -t app -u mvc --mobile none --database-provider ef --template-source https://myabp.com/app-template.zip
  ```

## Create a preview version

ABP CLI always uses the latest version. In order to create a solution from a preview (RC) version add the `--preview` parameter.

* Blazor UI, Entity Framework Core, no mobile, **preview version**, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -t app -u blazor --mobile none -csf --preview
  ```

## Choose database management system

The default database management system (DBMS) is `Entity Framework Core` / ` SQL Server`. You can choose a DBMS by passing `--database-management-system` parameter. Accepted values are `SqlServer`, `MySQL`, `SQLite`, `Oracle-Devart`, `PostgreSQL`. The default value is `SqlServer`.

* Angular UI, **PostgreSQL** database, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -u angular --database-management-system PostgreSQL -csf
  ```



## Use static HTTP ports

ABP CLI always assigns random ports to the hostable projects. If you need to keep the default ports and create a solution always with the same HTTP ports, add the parameter `--no-random-port`.

* MVC UI,  Entity Framework Core, **static ports**, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore --no-random-port -csf
  ```

## Use local ABP framework references

ABP libraries are referenced from NuGet by default in the ABP solutions. Sometimes you need to reference ABP libraries locally to your solution. This is useful to debug the framework itself. Your local ABP Framework 's root directory must have the `Volo.Abp.sln` file. You can copy the content of the following directory to your file system https://github.com/abpframework/abp/tree/dev/framework

* MVC UI,  Entity Framework Core, **ABP libraries are local project references**:

The local path must be the root directory of ABP repository. 
If `C:\source\abp\framework\Volo.Abp.sln` is your framework solution path, then you must write `C:\source\abp` to the `--abp-path` paramter.

  ```bash
  abp new Acme.BookStore --local-framework-ref --abp-path C:\source\abp
  ```

**Output**:

As seen below, ABP Framework libraries are local project references.

```xml
<ItemGroup>
	<ProjectReference Include="C:\source\abp\framework\src\Volo.Abp.Autofac\Volo.Abp.Autofac.csproj" />
	<ProjectReference Include="C:\source\abp\framework\src\Volo.Abp.AspNetCore.Serilog\Volo.Abp.AspNetCore.Serilog.csproj" />
	<ProjectReference Include="C:\source\abp\framework\src\Volo.Abp.AspNetCore.Authentication.JwtBearer\Volo.Abp.AspNetCore.Authentication.JwtBearer.csproj" />
	<ProjectReference Include="..\Acme.BookStore.Application\Acme.BookStore.Application.csproj" />
	<ProjectReference Include="..\Acme.BookStore.HttpApi\Acme.BookStore.HttpApi.csproj" />
	<ProjectReference Include="..\Acme.BookStore.EntityFrameworkCore.DbMigrations\Acme.BookStore.EntityFrameworkCore.DbMigrations.csproj" />
</ItemGroup>    
```
