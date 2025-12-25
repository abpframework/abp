```json
//[doc-seo]
{
    "Description": "Explore sample commands to create ABP solutions using the ABP CLI, including Angular projects and database options for developers."
}
```

# ABP CLI - New Solution Sample Commands 

The `abp new` command creates an ABP solution or other artifacts based on an ABP template. [ABP CLI](../cli/index.md) has several parameters to create a new ABP solution. In this document we will show you some sample commands to create a new solution. All the project names are `Acme.BookStore`. Currently, the available mobile projects are `React Native` and `MAUI` mobile app (they are *available for* ***Team*** *or higher licenses*). Available database providers are `Entity Framework Core` and `MongoDB`. All the commands starts with `abp new`.

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
  abp new Acme.BookStore -u angular -csf --connection-string "Server=localhost;Database=MyDatabase;Trusted_Connection=True"
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
  abp new Acme.BookStore -t console -csf --old
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

## Using Existing Configuration

If you want to programmaticaly create solutions, you can use an existing configuration instead of passing parameters to CLI one by one. ABP Studio keeps the solution creation history locally in `(UserProfile)\.abp\studio\solution-creation-history.json` file, there you can find the configurations of the solutions created in your machine.

### Using a Solution Id

In `*.abpsln` file of the solutions, there is an ID field that you can use to recreate a solution. To do this, pass the id to cli using `-shi or --solution-history-id` parameters.

```bash
abp new -shi dbb1afa9-190e-419a-842d-2780bb1bad1f
abp new -shi dbb1afa9-190e-419a-842d-2780bb1bad1f -o D:\test\Acme.BookStore
```

### Using a JSON Configuration File

You can also use a configuration file to create solutions. You need to use `-rcp or ready-config-path` parameters to do that.

```bash
abp new -rcp MyTests\config.json
abp new -rcp D:\MyTests\config.json
abp new -rcp D:\MyTests\config.json -o D:\test\Acme.BookStore
```

To prepare a config file, you can check the records in `solution-creation-history.json` file mentioned above. An example config file would look like that:

```json
{
        "solutionName": {
          "fullName": "Acme.BookStore",
          "companyName": "Acme",
          "projectName": "BookStore"
        },
        "pro": true,
        "useOpenSourceTemplate": false,
        "booksSample": false,
        "databaseProvider": "ef",
        "createInitialMigration": true,
        "runDbMigrator": true,
        "uiFramework": "angular",
        "theme": "leptonx",
        "themeStyle": "system",
        "mobileFramework": "none",
        "databaseManagementSystem": "sqlserver",
        "databaseManagementSystemBuilderExtensionMethod": "UseSqlServer",
        "connectionString": "Server=(LocalDb)\\\\MSSQLLocalDB;Database=BookStore;Trusted_Connection=True;TrustServerCertificate=true",
        "mauiBlazorApplicationIdGuid": "d3499a09-f3d4-4bb7-9d58-4c7b1caee331",
        "tiered": false,
        "publicWebsite": false,
        "cmskit": false,
        "openIddictAdmin": true,
        "languageManagement": true,
        "textTemplateManagement": true,
        "multiTenancy": true,
        "auditLogging": false,
        "gdpr": true,
        "chat": false,
        "fileManagement": false,
        "socialLogins": true,
        "includeTests": true,
        "distributedEventBus": "none",
        "publicRedis": false,
        "separateTenantSchema": false,
        "progressiveWebApp": false,
        "runProgressiveWebAppSupport": false,
        "runInstallLibs": false,
        "runBundling": false,
        "kubernetesConfiguration": true,
        "templateName": "app"
      }
```



## See Also

* [ABP CLI documentation](../cli/index.md)
