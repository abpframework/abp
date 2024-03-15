# ABP Studio CLI

ABP Studio CLI is a command line tool that extends [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) by adding more commands to perform operations of ABP Studio features.

## Installation

ABP Studio CLI is installed automatically when you install ABP Studio.

## Commands

As ABP Studio CLI extends [ABP CLI](https://docs.abp.io/en/abp/latest/CLI), all commands provided by [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) is also valid for ABP Studio CLI. Here, is the list of additional commands before explaining their details:

* new-solution: Generates a new solution based on the ABP Studio [startup templates](solution-templates/index.md).
* new-module: Generates a new module based on the given template.
* new-package: Generates a new package based on the given template.
* add-package-ref: Adds package to given project.
* add-source: Downloads the source code and replaces package references with project references.
* init-solution: Creates ABP Studio configuration files for a given solution.
* install-local-module: Installs a local module to given module.
* install-module:  Installs a module to given module via NuGet packages.
* kube-connect: Connects to kubernetes environment.
* kube-intercept: Intercepts a service running in Kubernetes environment.

### new-solution

Generates a new solution based on the ABP Studio [startup templates](solution-templates/index.md).

````bash
abpc new-solution <solution-name> [options]
````

Example:

````bash
abpc new-solution Acme.BookStore
````

* `Acme.BookStore` is the solution name here.
* Common convention is to name a solution is like *YourCompany.YourProject*. However, you can use different naming like *YourProject* (single level namespacing) or *YourCompany.YourProduct.YourModule* (three levels namespacing).

#### options

* `--template` or `-t`: Specifies the template name. Default template name is `empty`, which generates a empty solution. Available templates:
  * **`empty`**: Empty solution template.
    * **`app-pro`**: Application template. Additional options:
      * `--ui` or `-u`: Specifies the UI framework. Default framework is `mvc`. Available frameworks:
        * `mvc`: ASP.NET Core MVC. There are some additional options for this template:
          * `--tiered`: Creates a tiered solution where Web and Http API layers are physically separated. If not specified, it creates a layered solution which is less complex and suitable for most scenarios.
        * `angular`: Angular UI. There are some additional options for this template:
          * `--separate-auth-server`: The Auth Server project comes as a separate project and runs at a different endpoint. It separates the Auth Server from the API Host application. If not specified, you will have a single endpoint in the server side.
          * `--pwa`: Specifies the project as Progressive Web Application.
        * `blazor`: Blazor UI. There are some additional options for this template:
          * `--separate-auth-server`The Auth Server project comes as a separate project and runs at a different endpoint. It separates the Auth Server from the API Host application. If not specified, you will have a single endpoint in the server side.
          * `--pwa`: Specifies the project as Progressive Web Application.
        * `blazor-server`: Blazor Server UI. There are some additional options for this template:
          * `--tiered`: The Auth Server and the API Host project comes as separate projects and run at different endpoints. It has 3 startup projects: *HttpApi.Host*, *AuthServer* and *Blazor* and and each runs on different endpoints. If not specified, you will have a single endpoint for your web project.
        * `maui-blazor`: Blazor Maui UI. There are some additional options for this template:
          * `--tiered`: The Auth Server and the API Host project comes as separate projects and run at different endpoints. It has 3 startup projects: *HttpApi.Host*, *AuthServer* and *Blazor* and and each runs on different endpoints. If not specified, you will have a single endpoint for your web project.
        * `no-ui`: Without UI. No front-end layer will be created. There are some additional options for this template:
          * `--separate-auth-server`: The Auth Server project comes as a separate project and runs at a different endpoint. It separates the Auth Server from the API Host application. If not specified, you will have a single endpoint in the server side.
      * `--mobile` or `-m`: Specifies the mobile application framework. Default value is `react-native`. Available frameworks:
        * `none`: Without any mobile application.
        * `react-native`: React Native.
        * `maui`: MAUI.
      * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
          * `ef`: Entity Framework Core.
          * `mongodb`: MongoDB.
      * `--connection-string` or `-cs`:  Overwrites the default connection strings in all `appsettings.json` files. The default connection string is `Server=localhost;Database=MyProjectName;Trusted_Connection=True` for EF Core and it is configured to use the SQL Server. If you want to use the EF Core, but need to change the DBMS, you can change it as [described here](Entity-Framework-Core-Other-DBMS.md) (after creating the solution).
      * `--theme`: Specifes the theme. Default theme is `leptonx`. Available themes:
          * `leptonx`: LeptonX Theme.
          * `basic`: Basic Theme.
    
  * **`app-nolayers-pro`**: Single-layer application template. Additional options:
    * `--ui` or `-u`: Specifies the UI framework. Default framework is `mvc`. Available frameworks:
      * `mvc`: ASP.NET Core MVC. There are some additional options for this template:
      * `angular`: Angular UI. There are some additional options for this template:
      * `blazor`: Blazor UI. There are some additional options for this template:
      * `blazor-server`: Blazor Server UI. There are some additional options for this template:
      * `no-ui`: Without UI. No front-end layer will be created. There are some additional options for this template:
    * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
    * `--connection-string` or `-cs`:  Overwrites the default connection strings in all `appsettings.json` files. The default connection string is `Server=localhost;Database=MyProjectName;Trusted_Connection=True` for EF Core and it is configured to use the SQL Server. If you want to use the EF Core, but need to change the DBMS, you can change it as [described here](Entity-Framework-Core-Other-DBMS.md) (after creating the solution).
    * `--theme`: Specifes the theme. Default theme is `leptonx`. Available themes:
      * `leptonx`: LeptonX Theme.
      * `basic`: Basic Theme.
  
  * **`microservice-pro`**: Microservice solution template. Additional options:
    * `--ui` or `-u`: Specifies the UI framework. Default framework is `mvc`. Available frameworks:
      * `mvc`: ASP.NET Core MVC. There are some additional options for this template:
      * `angular`: Angular UI. There are some additional options for this template:
      * `blazor`: Blazor UI. There are some additional options for this template:
      * `blazor-server`: Blazor Server UI. There are some additional options for this template:
      * `maui-blazor`: Blazor Maui UI. There are some additional options for this template:
      * `no-ui`: Without UI. No front-end layer will be created. There are some additional options for this template:
    * `--mobile` or `-m`: Specifies the mobile application framework. Default value is `react-native`. Available frameworks:
      * `none`: Without any mobile application.
      * `react-native`: React Native.
      * `maui`: MAUI.
    * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
    * `--theme`: Specifes the theme. Default theme is `leptonx`. Available themes:
      * `leptonx`: LeptonX Theme.
      * `basic`: Basic Theme.
* `--output-folder` or `-o`: Specifies the output folder. Default value is the current directory.
* `--database-management-system` or `-dbms`: Sets the database management system. Default is **SQL Server**. Supported DBMS's:
  * `SqlServer`
  * `MySQL`
  * `PostgreSQL`
  * `SQLite`  (`app-pro` & `app-nolayers-pro`)
  * `Oracle` (`app-pro` & `app-nolayers-pro`)
  * `Oracle-Devart`  (`app-pro` & `app-nolayers-pro`)
* `--dont-run-install-libs`: Skip installing client side packages.
* `--dont-run-bundling`: Skip bundling for Blazor packages.
* `--no-kubernetes-configuration` or `-nkc`: Skips the Kubernetes configuration files.
* *Module Options*: You can skip some modules if you don't want to add them to your solution. Available commands:
  * `-no-saas`: Skips the Saas module.
  * `-no-gdpr`: Skips the GDPR module.
  * `-no-openiddict-admin-ui`: Skips the OpenIddict Admin UI module.
  * `-no-audit-logging`: Skips the Audit Logging module.
  * `-no-file-management`: Skips the File Management module.
  * `-no-language-management`: Skips the Language Management module.
  * `-no-text-template-management`: Skips the Text Template Management module.
  * `-no-chat`: Skips the Chat module.
### new-module

Generates a new module.

````bash
abpc new-module <module-name> [options]
````

Example:

````bash
abpc new-module Acme.BookStore -t module:ddd
````

#### options

* `--template` or `-t`: Specifies the template name. Default template name is `empty`, which generates a empty module. Module templates are provided by the main template, see their own startup template documentation for available modules. `empty` and `module:ddd` template is available for all solution structure.
* `--output-folder` or `-o`: Specifies the output folder. Default value is the current directory.
* `--target-solution` or `-ts`: If set, the new module will be added to the given solution. Otherwise the new module will added to the closest solution in the file system. If no solution found, it will throw an error.
* `--solution-folder` or `-sf`: Specifies the target folder in the [Solution Explorer](./solution-explorer.md#folder)  virtual folder system.
* `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. This option is only available if the module template supports it. You can add multiple values separated by commas, such as `ef, mongodb` if the module template supports it. Available providers:
  * `ef`: Entity Framework Core.
  * `mongodb`: MongoDB.
* `--ui-framework` or `-u`: Specifies the UI framework. Default framework is `mvc`. This option is only available if the module template supports it. You can add multiple values separated by commas, such as `mvc,angular` if the module template supports it. Available frameworks:
  * `mvc`: ASP.NET Core MVC.
  * `angular`: Angular UI.
  * `blazor`: Blazor UI.
  * `blazor-server`: Blazor Server UI.

### new-package

Generates a new package.

````bash
abpc new-package [options]
````

Example:

````bash
abpc new-package --name Acme.BookStore.Domain --template lib.domain
````

#### options

* `--template` or `-t`: Specifies the template name. This parameter doesn't have a default value and must be set. Available templates and their sub-options:
	* `lib.class-library`
	* `lib.domain-shared`
	* `lib.domain`
	* `lib.application-contracts`
	* `lib.application`
		* `--with-automapper`:  Adds automapper configuration. 
	* `lib.ef`
		* `--include-migrations`: Allows migration operations on this package.
		* `--connection-string-name`: Default value is the last part of the package's namespace (or package name simply).
		* `--connection-string`: Connection string value. Defaut value is null. You can set it alter.
	* `lib.mongodb`
	* `lib.http-api`
	* `lib.http-api-client`
	* `lib.mvc`
	* `lib.blazor`
	* `lib.blazor-wasm`
	* `lib.blazor-server`
	* `host.http-api`
		* `--with-serilog`: Includes Serilog configuration.
		* `--with-swagger`: Includes Swagger configuration.
	* `host.mvc`
		* `--with-serilog`: Includes Serilog configuration.
		* `--with-swagger`: Includes Swagger configuration.
	* `host.blazor-wasm`
		* `--backend`: Name of the backend project in the module (not path).
	* `host.blazor-server`
	* `csharp.console`
	* `csharp.library`
* `--module-file` or `-m`: If set, the new package will be added to the given module. Otherwise the new package will added to the closest module in the file system. If no module found, it will throw an error.
* `--name` or `-n`: Specifies the name of the package. If not set, a name based on the template type and module name will be generated.
* `--folder` or `-f`: Specifies the target folder in the target module's virtual folder system.


### add-package-ref

Adds one or more package reference to target project, also adds ABP module dependency. Both reference and target  projects must belong to same module.

````bash
abpc add-package-ref <package-names> [options]
````

Example:

````bash
abpc add-package-ref Acme.BookStore.Domain
abpc add-package-ref "Acme.BookStore.Domain Acme.BookStore.Domain.Shared" -t Acme.BookStore.Web
````

#### options

* `--target-project` or `-t`: Name of the project that reference will be added. If not set, project in the current directory will be used.

### add-source

Downloads the source code of a module and replaces package references with project references. This command only works if your ABP Commercial License has source-code access, or if source-code of the target module is free to all type of ABP Commercial Licenses.

````bash
abpc add-source <module-name> [options]
````

Example:

````bash
abpc add-source Volo.Chat --add-to-solution-file
````

#### options

* `--target-module` or `-t`: The module that will refer the downloaded source code. If not set, the module in the current directory will be used.
* `--add-to-solution-file`: Adds the downloaded source code to C# solution file and ABP Studio solution file.

### init-solution

Creates necessary files for a solution to be readable by ABP Studio. If the solution is generated via ABP Studio, you don't need this command. But it is not generated by ABP Studio, you need this command to make it work with ABP Studio.

````bash
abpc init-solution [options]
````

Example:

````bash
abpc init-solution --name Acme.BookStore
````

#### options

* `--name` or `-n`: Name for the solution. If not set,  it will be the same as the name of closest c# solution in the file system.

### install-local-module

Installs one module to another. Project relations are created according the types of the projects. For example: a `lib.domain-shared` project is added to `lib.domain-shared` project

````bash
abpc install-local-module <module-path> [options]
````

Example:

````bash
abpc install-local-module Acme.OrderManagement

abpc install-local-module Acme.OrderManagement -t "modules/crm/Acme.Crm.abpmdl"
````

#### options

* `--target-module` or `-t`: Path (or folder path) of the target module that the other module will be installed to. If not set, the closest module to the current directory will be used.

### install-module

Installs a module, that is published as nuget packages, to a local module. Project relations are created according the types of the projects. For example: a `lib.domain-shared` project is added to `lib.domain-shared` project

````bash
abpc install-module <module-name> [options]
````

Example:

````bash
abpc install-module Volo.Blogging

abpc install-module Volo.Blogging -t "modules/crm/Acme.Crm.abpmdl"
````

#### options

* `--target-module` or `-t`: Path (or folder path) of the target module that the other module will be installed to. If not set, the closest module to the current directory will be used.
* `--version` or `-v`: Nuget version of the module to be installed.

### kube-connect

Connects to Kubernetes cluster. Press `ctrl+c` to disconnect.

````bash
abpc kube-connect [options]
````

Example:

````bash
abpc kube-connect

abpc kube-connect -p Default.abpk8s.json

abpc kube-connect -c docker-desktop -ns mycrm-local
````

#### options

* `--profile` or `-p`: Kubernetes Profile path or name to be used. Path can be relative (to current directory) or full path, or you can simply give the name of profile if you run this command in same directory with the solution or profile. This parameter is not needed if you use `--namespace` and `--context` parameters.
* `--namespace` or `-ns`: The namespace that services running on.
* `--context` or `-c`: The context that services running in.
* `--wireguard-password` or `-wp`: Wireguard password for the profile. This is not needed if you already set it on the ABP Studio user interface.
* `--solution-id` or `-si`: Id of the solution. If not set, the closest solution in file system will be used.

### kube-intercept

Intercepts a service running in Kubernetes environment. Press `ctrl+c` to stop interception.

````bash
abpc kube-intercept <service-name> [options]
````

Example:

````bash
abpc kube-intercept mycrm-product-service -ns mycrm-local

abpc kube-intercept mycrm-product-service -ns mycrm-local -a MyCrm.ProductService.HttpApi.Host.csproj

abpc kube-intercept mycrm-product-service -ns mycrm-local -a MyCrm.ProductService.HttpApi.Host.csproj -pm 8080:80,8081:443
````

#### options

* `--application` or `-a`: Relative or full path of the project that will intercept the service. If not set, the project in the current directory will be used.
* `--namespace` or `-ns`: The namespace that service running on.
* `--context` or `-sc`: The context that service running in. Default value is `docker-desktop`.
* `--port-mappings` or `-pm`: Port mappings for the service.

## See Also

* [ABP CLI](https://docs.abp.io/en/abp/latest/CLI)
