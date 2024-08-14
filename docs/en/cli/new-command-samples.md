# ABP CLI - New Solution Sample Commands 

The `abp new` command creates an ABP solution or other artifacts based on an ABP template. [ABP CLI](../cli/index.md) has several parameters to create a new ABP solution. In this document we will show you some sample commands to create a new solution. All the project names are `Acme.BookStore`. Currently, the available mobile projects are `React Native` and `MAUI` mobile app. Available database providers are `Entity Framework Core` and `MongoDB`. All the commands starts with `abp new`.

## Angular

The following commands are for creating Angular UI projects:

* **Entity Framework Core**,  no mobile app, creates the project in a new folder:

  ````bash
  abp new Acme.BookStore -u angular --mobile none --database-provider ef -csf
  ````
  
* **Entity Framework Core**, default app template, **separate Auth Server**, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -t app -u angular -m none --separate-auth-server --database-provider ef -csf
  ```

* **Entity Framework Core**,  **custom connection string**, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -u angular -csf --connection-string Server=localhost;Database=MyDatabase;Trusted_Connection=True
  ```

* **MongoDB**, default app template, mobile project included, creates solution in `C:\MyProjects\Acme.BookStore`

  ```bash
  abp new Acme.BookStore -u angular --database-provider mongodb --output-folder C:\MyProjects\Acme.BookStore
  ```

* **MongoDB**, default app template, no mobile app, **separate Auth Server**, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -t app -u angular -m none --separate-auth-server --database-provider mongodb -csf
  ```

* **No DB migration!**, the DB migration will not be generated

  ```bash
  abp new Acme.BookStore -u angular --skip-migrations --skip-migrator
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

* **Public Website**, Entity Framework Core, no mobile app, creates the project in a new folder:
  
  ```bash
  abp new Acme.BookStore -t app -u mvc --mobile none --database-provider ef -csf --public-website
  ```
  
  _Note that Public Website is only included in PRO templates._
  
* **No initial configuration!**, the DB migration will not be generated, client-side scripts will not be installed and bundling will not run

  ```bash
  abp new Acme.BookStore -u mvc --skip-migrations --skip-migrator --dont-run-install-libs --dont-run-bundling
  ```
  
## Blazor WebAssembly

The following commands are for creating Blazor WASM projects:

* **Entity Framework Core**, no mobile app:

  ```bash
  abp new Acme.BookStore -t app -u blazor --mobile none
  ```

* **Entity Framework Core**, **separate Auth Server**, mobile app included:
  
  ```bash
  abp new Acme.BookStore -u blazor --separate-auth-server
  ```

* **MongoDB**, no mobile app, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -u blazor --database-provider mongodb --mobile none -csf
  ```
  
* **Client-side libraries are not loaded automatically!**, the libs folder will not be installed from ([NPM](https://www.npmjs.com/))

  ```bash
  abp new Acme.BookStore -u blazor --dont-run-install-libs
  ```

## Blazor Server

The following commands are for creating Blazor projects:

* **Entity Framework Core**, no mobile app:

  ```bash
  abp new Acme.BookStore -t app -u blazor-server --mobile none
  ```

* **Entity Framework Core**, **separate Auth Server**, **separate API Host**, mobile app included:
  
  ```bash
  abp new Acme.BookStore -u blazor-server --tiered
  ```

* **MongoDB**, no mobile app, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -u blazor --database-provider mongodb --mobile none -csf
  ```

* **Skip bundling for the packages**, 

  ```bash
  abp new Acme.BookStore -u blazor --dont-run-bundling
  ```
  
## No UI 

In the default app template, there is always a frontend project. In this option there is no frontend project. It has a `HttpApi.Host` project to serve your HTTP WebAPIs. It's appropriate if you want to create a WebAPI service.

* **Entity Framework Core**, separate Auth Server, creates the project in a new folder:

    ```bash
    abp new Acme.BookStore -u none --separate-auth-server -csf
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

Module are reusable sub applications used by your main project. Using ABP Module is a best practice if you are building a microservice solution. As modules are not final applications, each module could contains different frontend UI projects and database providers.

* Available frontends: `MVC`, `Angular`, `Blazor`. Available database providers: `Entity Framework Core`, `MongoDB`. 

  ```bash
  abp new-module Acme.IssueManagement
  ```

* The same with the upper but includes MVC and angular projects.

  ```bash
  abp new-module Acme.IssueManagement -u mvc,angular
  ```

## Choose database management system

The default database management system (DBMS) is `Entity Framework Core` / ` SQL Server`. You can choose a DBMS by passing `--database-management-system` parameter. Accepted values are `SqlServer`, `MySQL`, `SQLite`, `Oracle`, `Oracle-Devart`, `PostgreSQL`. The default value is `SqlServer`.

* Angular UI, **PostgreSQL** database, creates the project in a new folder:

  ```bash
  abp new Acme.BookStore -u angular --database-management-system PostgreSQL -csf
  ```

## Use local ABP references

ABP libraries are referenced from NuGet by default in the ABP solutions. Sometimes you need to reference ABP libraries locally to your solution. This is useful to debug the framework itself. Your local ABP 's root directory must have the `Volo.Abp.sln` file. You can copy the content of the following directory to your file system https://github.com/abpframework/abp/tree/dev/framework

* MVC UI,  Entity Framework Core, **ABP libraries are local project references**:

The local path must be the root directory of ABP repository. 
If `C:\source\abp\framework\Volo.Abp.sln` is your framework solution path, then you must write `C:\source\abp` to the `--abp-path` paramter.

  ```bash
  abp new Acme.BookStore --local-framework-ref --abp-path C:\source\abp
  ```

**Output**:

As seen below, ABP libraries are local project references.

```xml
<ItemGroup>
	<ProjectReference Include="C:\source\abp\framework\src\Volo.Abp.Autofac\Volo.Abp.Autofac.csproj" />
	<ProjectReference Include="C:\source\abp\framework\src\Volo.Abp.AspNetCore.Serilog\Volo.Abp.AspNetCore.Serilog.csproj" />
	<ProjectReference Include="C:\source\abp\framework\src\Volo.Abp.AspNetCore.Authentication.JwtBearer\Volo.Abp.AspNetCore.Authentication.JwtBearer.csproj" />
	<ProjectReference Include="..\Acme.BookStore.Application\Acme.BookStore.Application.csproj" />
	<ProjectReference Include="..\Acme.BookStore.HttpApi\Acme.BookStore.HttpApi.csproj" />
	<ProjectReference Include="..\Acme.BookStore.EntityFrameworkCore\Acme.BookStore.EntityFrameworkCore.csproj" />
</ItemGroup>    
```

## See Also

* [ABP CLI documentation](../cli/index.md)
