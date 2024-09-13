# ABP CLI

ABP CLI (Command Line Interface) is a command line tool to perform some common operations for ABP based solutions or ABP Studio features.

> 🛈 With **v8.2+**, the old/legacy ABP CLI has been replaced with a new CLI system to align with the new templating system and [ABP Studio](../studio/index.md). The new ABP CLI commands are explained in this documentation. However, if you want to learn more about the differences between the old and new CLIs, want to learn the reason for the change, or need guidance to use the old ABP CLI, please refer to the [Old vs New CLI](differences-between-old-and-new-cli.md) documentation.
>
> You may need to remove the Old CLI before installing the New CLI, by running the following command: `dotnet tool uninstall -g Volo.Abp.Cli`

## Installation

ABP CLI is a [dotnet global tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools). Install it using a command line window:

````bash
dotnet tool install -g Volo.Abp.Studio.Cli
````

To update an existing installation:

````bash
dotnet tool update -g Volo.Abp.Studio.Cli
````

## Global Options

While each command may have a set of options, there are some global options that can be used with any command;

* `--skip-cli-version-check` or `-scvc`: Skips to check the latest version of the ABP CLI. If you don't specify, it will check the latest version and shows a warning message if there is a newer version of the ABP CLI.
- `--skip-extension-version-check` or `-sevc`: Skips to check the latest version of the ABP CLI extensions. If you don't specify, it will check the latest version and download the latest version if there is a newer version of the ABP CLI extensions.
* `--old`: ABP CLI has two variations: `Volo.Abp.Studio.Cli` and `Volo.Abp.Cli`. New features/templates are added to the `Volo.Abp.Studio.Cli`. But if you want to use the old version, you can use this option **at the end of your commands**. For example, `abp new Acme.BookStore --old`.
* `--help` or `-h`: Shows help for the specified command.

## Commands

Here, is the list of all available commands before explaining their details:

* **`help`**: Shows help on the usage of the ABP CLI.
* **`cli`**: Update or remove ABP CLI.
* **`new`**: Generates a new solution based on the ABP [startup templates](../solution-templates/index.md).
* **`new-module`**: Generates a new module based on the given template.
* **`new-package`**: Generates a new package based on the given template.
* **`update`**: Automatically updates all ABP related NuGet and NPM packages in a solution.
* **`clean`**: Deletes all `BIN` and `OBJ` folders in the current folder.
* **`add-package`**: Adds an ABP package to a project.
* **`add-package-ref`**: Adds package to given project.
* **`install-module`**: Adds a [multi-package application module](../modules/index.md) to a given module.
* **`install-local-module`**: Installs a local module to given module.
* **`list-modules`**: Lists names of open-source application modules.
* **`list-templates`**: Lists the names of available templates to create a solution.
* **`get-source`**: Downloads the source code of a module.
* **`add-source-code`**: Downloads the source code and replaces package references with project references.
* **`init-solution`**: Creates ABP Studio configuration files for a given solution.
* **`kube-connect`**: Connects to kubernetes environment. (*Available for* ***Business*** *or higher licenses*)
* **`kube-intercept`**: Intercepts a service running in Kubernetes environment. (*Available for* ***Business*** *or higher licenses*)
* **`list-module-sources`**: Lists the remote module sources.
* **`add-module-source`**: Adds a remote module source.
* **`delete-module-source`**: Deletes a remote module source.
* **`generate-proxy`**: Generates client side proxies to use HTTP API endpoints.
* **`remove-proxy`**: Removes previously generated client side proxies.
* **`switch-to-preview`**: Switches to the latest preview version of the ABP.
* **`switch-to-nightly`**: Switches to the latest [nightly builds](../release-info/nightly-builds.md) of the ABP related packages on a solution.
* **`switch-to-stable`**: Switches to the latest stable versions of the ABP related packages on a solution.
* **`switch-to-local`**: Changes NuGet package references on a solution to local project references.
* **`upgrade`**: It converts the application to use pro modules.
* **`translate`**: Simplifies to translate localization files when you have multiple JSON [localization](../framework/fundamentals/localization.md) files in a source control repository.
* **`login`**: Authenticates on your computer with your [abp.io](https://abp.io/) username and password.
* **`login-info`**: Shows the current user's login information.
* **`logout`**: Logouts from your computer if you've authenticated before.
* **`bundle`**: Generates script and style references for ABP Blazor and MAUI Blazor project. 
* **`install-libs`**: Install NPM Packages for MVC / Razor Pages and Blazor Server UI types.
* **`clear-download-cache`**: Clears the templates download cache.
* **`check-extensions`**: Checks the latest version of the ABP CLI extensions.
* **`install-old-cli`**: Installs old ABP CLI.

### help

Shows basic usages of the ABP CLI.

Usage:

````bash
abp help [command-name]
````

Examples:

````bash
abp help        # Shows a general help.
abp help new    # Shows help about the "new" command.
````

### cli

Update or remove ABP CLI.

Usage:

````bash
abp cli [command-name]
````

Examples:

````bash
abp cli update
abp cli update --preview
abp cli update --version 1.0.0
abp cli remove
abp cli check-version
abp cli clear-cache
````

### new

Generates a new solution based on the ABP [startup templates](../solution-templates).

Usage:

````bash
abp new <solution-name> [options]
````

Examples:

````bash
abp new Acme.BookStore
````

* `Acme.BookStore` is the solution name here.
* Common convention is to name a solution is like *YourCompany.YourProject*. However, you can use different naming like *YourProject* (single level namespacing) or *YourCompany.YourProduct.YourModule* (three levels namespacing).

For more samples, go to [ABP CLI Create Solution Samples](new-command-samples.md)

#### Options

* `--template` or `-t`: Specifies the template name. Default template name is `app`, which generates a application solution. Available templates:
  * **`empty`**: Empty solution template.
  * **`app`**: Application template. Additional options:
    * `--ui-framework` or `-u`: Specifies the UI framework. Default framework is `mvc`. Available frameworks:
      * `mvc`: ASP.NET Core MVC. There are some additional options for this template:
        * `--tiered`: Creates a tiered solution where Web and Http API layers are physically separated. If not specified, it creates a layered solution which is less complex and suitable for most scenarios. (*Available for* ***Team*** *or higher licenses*)
      * `angular`: Angular UI. There are some additional options for this template:
        * `--tiered`: The Auth Server project comes as a separate project and runs at a different endpoint. It separates the Auth Server from the API Host application. If not specified, you will have a single endpoint in the server side. (*Available for* ***Team*** *or higher licenses*)
        * `--progressive-web-app` or `-pwa`: Specifies the project as Progressive Web Application.
      * `blazor`: Blazor UI. There are some additional options for this template:
        * `--tiered`The Auth Server project comes as a separate project and runs at a different endpoint. It separates the Auth Server from the API Host application. If not specified, you will have a single endpoint in the server side. (*Available for* ***Team*** *or higher licenses*)
        * `--progressive-web-app` or `-pwa`: Specifies the project as Progressive Web Application.
      * `blazor-server`: Blazor Server UI. There are some additional options for this template:
        * `--tiered`: The Auth Server and the API Host project comes as separate projects and run at different endpoints. It has 3 startup projects: *HttpApi.Host*, *AuthServer* and *Blazor* and and each runs on different endpoints. If not specified, you will have a single endpoint for your web project. (*Available for* ***Team*** *or higher licenses*)
      * `maui-blazor`: Blazor Maui UI (*Available for* ***Team*** *or higher licenses*). There are some additional options for this template:
        * `--tiered`: The Auth Server and the API Host project comes as separate projects and run at different endpoints. It has 3 startup projects: *HttpApi.Host*, *AuthServer* and *Blazor* and and each runs on different endpoints. If not specified, you will have a single endpoint for your web project.
      * `no-ui`: Without UI. No front-end layer will be created. There are some additional options for this template:
        * `--tiered`: The Auth Server project comes as a separate project and runs at a different endpoint. It separates the Auth Server from the API Host application. If not specified, you will have a single endpoint in the server side. (*Available for* ***Team*** *or higher licenses*)
    * `--mobile` or `-m`: Specifies the mobile application framework. Default value is `none`. Available frameworks:
      * `none`: Without any mobile application.
      * `react-native`: React Native.
      * `maui`: MAUI. This mobile option is only available for ABP. (*Available for* ***Team*** *or higher licenses*)
    * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
        * `ef`: Entity Framework Core.
        * `mongodb`: MongoDB.
    * `--connection-string` or `-cs`:  Overwrites the default connection strings in all `appsettings.json` files. The default connection string is `Server=localhost;Database=MyProjectName;Trusted_Connection=True` for EF Core and it is configured to use the SQL Server. If you want to use the EF Core, but need to change the DBMS, you can change it as [described here](../framework/data/entity-framework-core/other-dbms.md) (after creating the solution).
    * `--skip-migrations` or `-sm`: Skips the creating initial database migration step.
    * `--skip-migrator` or `-smr`: Skips the run database migrator step.
    * `--public-website`: Public Website is a front-facing website for describing your project, listing your products and doing SEO for marketing purposes. Users can login and register on your website with this website. This option is only included in PRO templates.
        * `--without-cms-kit`: When you add a public website to your solution, it automatically includes the [CmsKit](./../modules/cms-kit-pro/index.md) module. If you don't want to include *CmsKit*, you can use this parameter.
    * `--separate-tenant-schema`: Creates a different DbContext for tenant schema. If not specified, the tenant schema is shared with the host schema. This option is only included in PRO templates.
    * `--sample-crud-page` or `-scp`: It adds the [BookStore](./../tutorials/book-store/index.md) sample to your solution.
    * `--theme` or `-th`: Specifes the theme. Default theme is `leptonx`. Available themes:
        * `leptonx`: LeptonX Theme. (*Available for* ***Team*** *or higher licenses*)
        * `leptonx-lite`: LeptonX-Lite Theme.
        * `basic`: Basic Theme.
    * `--use-open-source-template`or `-uost`: Uses the open-source template. (*Available for* ***Team*** *or higher licenses*)
  * **`app-nolayers`**: Single-layer application template. Additional options:
    * `--ui-framework` or `-u`: Specifies the UI framework. Default framework is `mvc`. Available frameworks:
      * `mvc`: ASP.NET Core MVC. There are some additional options for this template:
      * `angular`: Angular UI. There are some additional options for this template:
      * `blazor`: Blazor UI. There are some additional options for this template:
      * `blazor-server`: Blazor Server UI. There are some additional options for this template:
      * `no-ui`: Without UI. No front-end layer will be created. There are some additional options for this template:
    * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
    * `--connection-string` or `-cs`:  Overwrites the default connection strings in all `appsettings.json` files. The default connection string is `Server=localhost;Database=MyProjectName;Trusted_Connection=True` for EF Core and it is configured to use the SQL Server. If you want to use the EF Core, but need to change the DBMS, you can change it as [described here](../framework/data/entity-framework-core/other-dbms.md) (after creating the solution).
    * `--skip-migrations` or `-sm`: Skips the creating initial database migration step.
    * `--skip-migrator` or `-smr`: Skips the run database migrator step.
    * `--sample-crud-page` or `-scp`: It adds the [BookStore](./../tutorials/book-store/index.md) sample to your solution.
    * `--theme`: Specifes the theme. Default theme is `leptonx`. Available themes:
      * `leptonx`: LeptonX Theme. (*Available for* ***Team*** *or higher licenses*)
      * `leptonx-lite`: LeptonX-Lite Theme.
      * `basic`: Basic Theme.
    * `--use-open-source-template`or `-uost`: Uses the open-source template. (*Available for* ***Team*** *or higher licenses*)
  * **`microservice`**: Microservice solution template (*Available for* ***Business*** *or higher licenses*).  Additional options:
    * `--ui-framework` or `-u`: Specifies the UI framework. Default framework is `mvc`. Available frameworks:
      * `mvc`: ASP.NET Core MVC. There are some additional options for this template:
      * `angular`: Angular UI. There are some additional options for this template:
      * `blazor`: Blazor UI. There are some additional options for this template:
      * `blazor-server`: Blazor Server UI. There are some additional options for this template:
      * `maui-blazor`: Blazor Maui UI. There are some additional options for this template:
      * `no-ui`: Without UI. No front-end layer will be created. There are some additional options for this template:
    * `--mobile` or `-m`: Specifies the mobile application framework. Default value is `none`. Available frameworks:
      * `none`: Without any mobile application.
      * `react-native`: React Native.
      * `maui`: MAUI.
    * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
    * `--theme`: Specifes the theme. Default theme is `leptonx`. Available themes:
      * `leptonx`: LeptonX Theme.
      * `basic`: Basic Theme.
    * `--public-website`: Public Website is a front-facing website for describing your project, listing your products and doing SEO for marketing purposes. Users can login and register on your website with this website. This option is only included in PRO templates.
* `--output-folder` or `-o`: Specifies the output folder. Default value is the current directory.
* `--local-framework-ref` or `-lfr`: Uses local projects references to the ABP framework instead of using the NuGet packages. It tries to find the paths from `ide-state.json`. The file is located at `%UserProfile%\.abp\studio\ui\ide-state.json` (for Windows) and `~/.abp/studio/ui/ide-state.json` (for MAC).
* `--create-solution-folder` or `-csf`: Specifies if the project will be in a new folder in the output folder or directly the output folder.
* `--database-management-system` or `-dbms`: Sets the database management system. Default is **SQL Server**. Supported DBMS's:
  * `SqlServer`
  * `MySQL`
  * `PostgreSQL`
  * `SQLite`  (`app` & `app-nolayers`)
  * `Oracle` (`app` & `app-nolayers`)
  * `Oracle-Devart`  (`app` & `app-nolayers`)
* `--dont-run-install-libs`: Skip installing client side packages.
* `--dont-run-bundling`: Skip bundling for Blazor packages.
* `--no-kubernetes-configuration` or `-nkc`: Skips the Kubernetes configuration files.
* `--no-social-logins` or `-nsl`: Skipts the social login configuration.
* *Module Options*: You can skip some modules if you don't want to add them to your solution (*Available for* ***Team*** *or higher licenses*). Available commands:
  * `-no-saas`: Skips the Saas module.
  * `-no-gdpr`: Skips the GDPR module.
  * `-no-openiddict-admin-ui`: Skips the OpenIddict Admin UI module.
  * `-no-audit-logging`: Skips the Audit Logging module.
  * `-no-file-management`: Skips the File Management module.
  * `-no-language-management`: Skips the Language Management module.
  * `-no-text-template-management`: Skips the Text Template Management module.
  * `-no-chat`: Skips the Chat module.
* `--legacy`: Generates a legacy solution.
  * `trust-version`: Trusts the user's version and does not check if the version exists or not. If the template with the given version is found in the cache, it will be used, otherwise throws an exception.


### new-module

Generates a new module.

````bash
abp new-module <module-name> [options]
````

Examples:

````bash
abp new-module Acme.BookStore -t module:ddd
````

#### options

* `--template` or `-t`: Specifies the template name. Default template name is `module:ddd`, which generates a DDD module. Module templates are provided by the main template, see their own startup template documentation for available modules. `empty:empty` and `module:ddd` template is available for all solution structure.
* `--output-folder` or `-o`: Specifies the output folder. Default value is the current directory.
* `--target-solution` or `-ts`: If set, the new module will be added to the given solution. Otherwise the new module will added to the closest solution in the file system. If no solution found, it will throw an error.
* `--solution-folder` or `-sf`: Specifies the target folder in the [Solution Explorer](../studio/solution-explorer.md#folder)  virtual folder system.
* `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. This option is only available if the module template supports it. You can add multiple values separated by commas, such as `ef,mongodb` if the module template supports it. Available providers:
  * `ef`: Entity Framework Core.
  * `mongodb`: MongoDB.
* `--ui-framework` or `-u`: Specifies the UI framework. This option is only available if the module template supports it. You can add multiple values separated by commas, such as `mvc,angular` if the module template supports it. Available frameworks:
  * `mvc`: ASP.NET Core MVC.
  * `angular`: Angular UI.
  * `blazor`: Blazor UI.
  * `blazor-server`: Blazor Server UI.

### new-package

Generates a new package.

````bash
abp new-package [options]
````

Examples:

````bash
abp new-package --name Acme.BookStore.Domain --template lib.domain
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

### update

Updating all ABP related packages can be tedious since there are many packages of the framework and modules. This command automatically updates all ABP related NuGet and NPM packages in a solution or project to the latest versions. You can run it in the root folder of your solutions.

Usage:

````bash
abp update [options]
````

* If you run in a directory with a .csproj file, it updates all ABP related packages of the project to the latest versions.
* If you run in a directory with a .sln file, it updates all ABP related packages of the all projects of the solution to the latest versions.
* If you run in a directory that contains multiple solutions in sub-folders, it can update all the solutions, including Angular projects.

Note that this command can upgrade your solution from a previous version, and also can upgrade it from a preview release to the stable release of the same version.

#### Options

* `--npm`: Only updates NPM packages.
* `--nuget`: Only updates NuGet packages.
* `--solution-path` or `-sp`: Specify the solution path. Use the current directory by default
* `--solution-name` or `-sn`: Specify the solution name. Search `*.sln` files in the directory by default.
* `--check-all`: Check the new version of each package separately. Default is `false`.
* `--version` or `-v`: Specifies the version to use for update. If not specified, latest version is used.

### clean

Deletes all `BIN` and `OBJ` folders in the current folder.

Usage:

````bash
abp clean
````


### add-package

Adds an ABP package to a project by,

* Adding related nuget package as a dependency to the project.
* Adding `[DependsOn(...)]` attribute to the module class in the project (see the [module development document](../framework/architecture/modularity/basics.md)).

> Notice that the added module may require additional configuration which is generally indicated in the documentation of the related package.

Basic usage:

````bash
abp add-package <package-name> [options]
````

Examples:

````bash
abp add-package Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic
````

* This example adds the `Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic` package to the project.

#### Options

* `--project` or `-p`: Specifies the project (.csproj) file path. If not specified, CLI tries to find a .csproj file in the current directory.
* `--with-source-code`: Downloads the source code of the package to your solution folder and uses local project references instead of NuGet/NPM packages.
* `--add-to-solution-file`: Adds the downloaded package to your solution file, so you will also see the package when you open the solution on a IDE. (only available when `--with-source-code` is True)

> Currently only the source code of the basic theme packages([MVC](../framework/ui/mvc-razor-pages/basic-theme.md) and [Blazor](../framework/ui/blazor/basic-theme.md)) can be downloaded.
> - Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic
> - Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
> - Volo.Abp.AspNetCore.Components.Web.BasicTheme
> - Volo.Abp.AspNetCore.Components.Server.BasicTheme

### add-package-ref

Adds one or more package reference to target project, also adds ABP module dependency. Both reference and target projects must belong to same module.

````bash
abp add-package-ref <package-names> [options]
````

Examples:

````bash
abp add-package-ref Acme.BookStore.Domain
abp add-package-ref "Acme.BookStore.Domain Acme.BookStore.Domain.Shared" -t Acme.BookStore.Web
````

#### Options

* `--target-project` or `-t`: Name of the project that reference will be added. If not set, project in the current directory will be used.

### install-module

Installs a module, that is published as nuget packages, to a local module. Project relations are created according the types of the projects. For Examples: a `lib.domain-shared` project is added to `lib.domain-shared` project

````bash
abp install-module <module-name> [options]
````

Examples:

````bash
abp install-module Volo.Blogging

abp install-module Volo.Blogging -t "modules/crm/Acme.Crm.abpmdl"
````

#### Options

* `--target-module` or `-t`: Path (or folder path) of the target module that the other module will be installed to. If not set, the closest module to the current directory will be used.
* `--version` or `-v`: Nuget version of the module to be installed.

### install-local-module

Installs one module to another. Project relations are created according the types of the projects. For Examples: a `lib.domain-shared` project is added to `lib.domain-shared` project

````bash
abp install-local-module <module-path> [options]
````

Examples:

````bash
abp install-local-module Acme.OrderManagement

abp install-local-module Acme.OrderManagement -t "modules/crm/Acme.Crm.abpmdl"
````

#### Options

* `--target-module` or `-t`: Path (or folder path) of the target module that the other module will be installed to. If not set, the closest module to the current directory will be used.

### list-modules

Lists names of open-source application modules.

Usage:

````bash
abp list-modules [options]
````

Examples:

```bash
abp list-modules
```

### list-templates

Lists all available templates to create a solution.

Usage:

```bash
abp list-templates
```

### get-source

Downloads the source code of a module to your computer.

Usage:

````bash
abp get-source <module-name> [options]
````

Examples:

```bash
abp get-source Volo.Blogging

abp get-source Volo.Blogging --local-framework-ref --abp-path D:\GitHub\abp
```

#### Options

* `--output-folder` or `-o`: Specifies the directory that source code will be downloaded in. If not specified, current directory is used.
* `--version` or `-v`: Specifies the version of the  source code that will be downloaded. If not specified, latest version is used.
* `--preview`: If no version option is specified, this option specifies if latest [preview version](../release-info/previews.md) will be used instead of latest stable version.
* `--local-framework-ref --abp-path`: Path of [ABP GitHub repository](https://github.com/abpframework/abp) in your computer. This will be used for converting project references to your local system. If this is not specified, project references will be converted to NuGet references.

### add-source-code

Downloads the source code of a module and replaces package references with project references. This command only works if your ABP Commercial License has source-code access, or if source-code of the target module is free to all type of ABP Commercial Licenses.

````bash
abp add-source-code <module-name> [options]
````

Examples:

````bash
abp add-source-code Volo.Chat --add-to-solution-file
````

#### Options

* `--target-module` or `-t`: The module that will refer the downloaded source code. If not set, the module in the current directory will be used.
* `--add-to-solution-file`: Adds the downloaded source code to C# solution file and ABP Studio solution file.

### init-solution

Creates necessary files for a solution to be readable by ABP Studio. If the solution is generated via ABP Studio, you don't need this command. But it is not generated by ABP Studio, you need this command to make it work with ABP Studio.

````bash
abp init-solution [options]
````

Examples:

````bash
abp init-solution --name Acme.BookStore
````

#### Options

* `--name` or `-n`: Name for the solution. If not set,  it will be the same as the name of closest c# solution in the file system.

### kube-connect

Connects to Kubernetes cluster (*Available for* ***Business*** *or higher licenses*). Press `ctrl+c` to disconnect.

````bash
abp kube-connect [options]
````

Examples:

````bash
abp kube-connect

abp kube-connect -p Default.abpk8s.json

abp kube-connect -c docker-desktop -ns mycrm-local
````

#### Options

* `--profile` or `-p`: Kubernetes Profile path or name to be used. Path can be relative (to current directory) or full path, or you can simply give the name of profile if you run this command in same directory with the solution or profile. This parameter is not needed if you use `--namespace` and `--context` parameters.
* `--namespace` or `-ns`: The namespace that services running on.
* `--context` or `-c`: The context that services running in.
* `--wireguard-password` or `-wp`: Wireguard password for the profile. This is not needed if you already set it on the ABP Studio user interface.
* `--solution-path` or `-sp`: Path of the solution. If not set, the closest solution in file system will be used.

### kube-intercept

Intercepts a service running in Kubernetes environment (*Available for* ***Business*** *or higher licenses*). Press `ctrl+c` to stop interception.

````bash
abp kube-intercept <service-name> [options]
````

Examples:

````bash
abp kube-intercept mycrm-product-service -ns mycrm-local

abp kube-intercept mycrm-product-service -ns mycrm-local -a MyCrm.ProductService.HttpApi.Host.csproj

abp kube-intercept mycrm-product-service -ns mycrm-local -a MyCrm.ProductService.HttpApi.Host.csproj -pm 8080:80,8081:443
````

#### Options

* `--application` or `-a`: Relative or full path of the project that will intercept the service. If not set, the project in the current directory will be used.
* `--namespace` or `-ns`: The namespace that service running on.
* `--context` or `-sc`: The context that service running in. Default value is `docker-desktop`.
* `--port-mappings` or `-pm`: Port mappings for the service.

### list-module-sources

With this command, you can see the list of remote module sources that you can use to install modules. It is similar to the NuGet feed list in Visual Studio. 

````bash
abp list-module-sources
````

### add-module-source

Adds a remote module source to the list of sources that you can use to install modules. 

````bash
abp add-module-source [options]
````

You can create your own module source and add it to the list. It accepts a name and a url or a path as parameter. If you provide a path, it should be a local path that contains the modules json file. If you provide a url, it should be a url that contains the modules json file. The json file should be in the following format:

````json
{
	"name": "ABP Open Source Modules",
	"modules" : {
		"Volo.Abp.Account": {},
		"Volo.Abp.AuditLogging": {},
		"Volo.Abp.Identity": {},
    ...
	}
}
````

When you add a module source, you can install modules from that source using the `install-module` command. It attempts to find the package from NuGet, such as `Volo.Abp.Account.Installer`. You can configure a private NuGet feed and publish your modules to that feed. Each module has an installer package that is utilized to install the module into a solution. When you publish your module to a private feed, you should also publish the installer package to the same feed.

Examples:

````bash
abp add-module-source -n "Custom Source" -p "D:\packages\abp\modules.json"

abp add-module-source -n "Custom Http Source" -p "https://raw.githubusercontent.com/x/abp-module-store/main/abp-module-store.json"
````

#### Options

* `--name` or `-n`: The name of the module source.
* `--path` or `-p`: The path of the module source. It can be a local path or a url.

### delete-module-source

Deletes a remote module source from the list of sources that you can use to install modules. 

````bash
abp delete-module-source [options]
````

Examples:

````bash
abp delete-module-source -n "Custom Source"
````

#### Options

* `--name` or `-n`: The name of the module source.

### generate-proxy

Generates Angular, C# or JavaScript service proxies for your HTTP APIs to make easy to consume your services from the client side. Your host (server) application must be up and running before running this command.

Usage:

````bash
abp generate-proxy -t <client-type> [options]
````

Examples:

````bash
abp generate-proxy -t ng -url https://localhost:44302/
abp generate-proxy -t js -url https://localhost:44302/
abp generate-proxy -t csharp -url https://localhost:44302/
````

#### Options

* `--type` or `-t`: The name of client type. Available clients:
  * `csharp`: C#, work in the `*.HttpApi.Client` project directory. There are some additional options for this client:
    * `--without-contracts`: Avoid generating the application service interface, class, enum and dto types.
    * `--folder`: Folder name to place generated CSharp code in. Default value: `ClientProxies`.
  * `ng`: Angular. There are some additional options for this client:
    * `--api-name` or `-a`: The name of the API endpoint defined in the `/src/environments/environment.ts`. Default value: `default`.
    * `--source` or `-s`: Specifies the Angular project name to resolve the root namespace & API definition URL from. Default value: `defaultProject`.
    * `--target`: Specifies the Angular project name to place generated code in. Default value: `defaultProject`.
    * `--module`:  Backend module name. Default value: `app`.
    * `--entry-point`: Targets the Angular project to place the generated code.
    * `--url`: Specifies api definition url. Default value is API Name's url in environment file.
    * `--prompt` or `-p`: Asks the options from the command line prompt (for the unspecified options).

  * `js`: JavaScript. work in the `*.Web` project directory. There are some additional options for this client:
    * `--output` or `-o`: JavaScript file path or folder to place generated code in.
* `--module` or `-m`: Specifies the name of the backend module you wish to generate proxies for. Default value: `app`.
* `--working-directory` or `-wd`: Execution directory. For `csharp` and `js` client types.
* `--url` or `-u`: API definition URL from.
* `--service-type` or `-st`: Specifies the service type to generate. `application`, `integration` and `all`, Default value: `all` for C#, `application` for JavaScript / Angular.

> See the [Angular Service Proxies document](../framework/ui/angular/service-proxies.md) for more.

### remove-proxy

Removes previously generated proxy code from the Angular, CSharp or JavaScript application. Your host (server) application must be up and running before running this command.

This can be especially useful when you generate proxies for multiple modules before and need to remove one of them later.

Usage:

````bash
abp remove-proxy -t <client-type> [options]
````

Examples:

````bash
abp remove-proxy -t ng
abp remove-proxy -t js -m identity -o Pages/Identity/client-proxies.js
abp remove-proxy -t csharp --folder MyProxies/InnerFolder
````

#### Options

* `--type` or `-t`: The name of client type. Available clients:
  * `csharp`: C#, work in the `*.HttpApi.Client` project directory. There are some additional options for this client:
    * `--folder`: Folder name to place generated CSharp code in. Default value: `ClientProxies`.
  * `ng`: Angular. There are some additional options for this client:
    * `--api-name` or `-a`: The name of the API endpoint defined in the `/src/environments/environment.ts`. Default value: `default`.
    * `--source` or `-s`: Specifies the Angular project name to resolve the root namespace & API definition URL from. Default value: `defaultProject`.
    * `--target`: Specifies the Angular project name to place generated code in. Default value: `defaultProject`.
    * `--url`: Specifies api definition url. Default value is API Name's url in environment file.
    * `--prompt` or `-p`: Asks the options from the command line prompt (for the unspecified options).
  * `js`: JavaScript. work in the `*.Web` project directory. There are some additional options for this client:
    * `--output` or `-o`: JavaScript file path or folder to place generated code in.
* `--module` or `-m`: Specifies the name of the backend module you wish to generate proxies for. Default value: `app`.
* `--working-directory` or `-wd`: Execution directory. For `csharp` and `js` client types.
* `--url` or `-u`: API definition URL from.

> See the [Angular Service Proxies document](../framework/ui/angular/service-proxies.md) for more.

### switch-to-preview

You can use this command to switch your solution or project to latest preview version of the ABP.

Usage:

````bash
abp switch-to-preview [options]
````

#### Options

* `--directory` or `-d`: Specifies the directory. The solution or project should be in that directory or in any of its sub directories. If not specified, default is the current directory.


### switch-to-nightly

You can use this command to switch your solution or project to latest [nightly](../release-info/nightly-builds.md) preview version of the ABP packages.

Usage:

````bash
abp switch-to-nightly [options]
````

#### Options

* `--directory` or `-d`: Specifies the directory. The solution or project should be in that directory or in any of its sub directories. If not specified, default is the current directory.

### switch-to-stable

If you're using the ABP preview packages (including nightly previews), you can switch back to latest stable version using this command.

Usage:

````bash
abp switch-to-stable [options]
````
#### Options

* `--directory` or `-d`: Specifies the directory. The solution or project should be in that directory or in any of its sub directories. If not specified, default is the current directory.

### switch-to-local

Changes all NuGet package references to local project references for all the .csproj files in the specified folder (and all its subfolders with any deep). It is not limited to ABP or Module packages.

Usage:

````bash
abp switch-to-local [options]
````
#### Options

* `--solution` or `-s`: Specifies the solution directory. The solution should be in that directory or in any of its sub directories. If not specified, default is the current directory.

* `--paths` or `-p`: Specifies the local paths that the projects are inside. You can use `|` character to separate the paths.

Examples:

````bash
abp switch-to-local --paths "D:\Github\abp|D:\Github\my-repo"
````

### upgrade

Upgrades the ABP modules to pro modules, such as upgrading [Identity](../modules/identity.md) to [Identity Pro](../modules/identity-pro.md). You can use this for [Single Layer Web Application](../solution-templates/single-layer-web-application/index.md) and [Layered Web Application](../solution-templates/layered-web-application/index.md) templates.
This command is specially designed for users who already started their development before having a license. Therefore this command requires license.

Usage:

````bash
abp upgrade [-t <template-name>] [options]
````

Examples:

````bash
abp upgrade -t app
abp upgrade -t app --language-management --gdpr --audit-logging-ui --text-template-management --openiddict-pro
abp upgrade -t app-nolayers --audit-logging-ui
abp upgrade -t app-nolayers -p D:\MyProjects\MyProject
````

#### Options

* `--path` or `-p`: Specifies the module path. The module should be in that directory. If not specified, the default is the current directory.
* `--gdpr`: Installs GDPR module too.
* `--language-management`: Installs Language Management module too.
* `--audit-logging-ui`: Installs Audit Logging Pro (UI) module too.
* `--text-template-management`: Installs Text Template Management module too.
* `--openiddict-pro`: Installs OpenIddict Pro (UI) module too.

### translate

Simplifies to translate [localization](../framework/fundamentals/localization.md) files when you have multiple JSON [localization](../framework/fundamentals/localization.md) files in a source control repository.

* This command will create a unified json file based on the reference culture. 
* It searches all the localization `JSON` files in the current directory and all subdirectories (recursively). Then creates a single file (named `abp-translation.json` by default) that includes all the entries need to be translated.
* Once you translate the entries in this file, you can then apply your changes to the original localization files using the `--apply` command.

> The main purpose of this command is to translate ABP localization files (since the [abp repository](https://github.com/abpframework/abp) has tens of localization files to be translated in different directories).

#### Creating the Translation File

First step is to create the unified translation file:

````bash
abp translate -c <culture> [options]
````

Examples:

````bash
abp translate -c de
````

This command created the unified translation file for the `de` (German) culture.

##### Additional Options

* `--reference-culture` or `-r`: Default `en`. Specifies the reference culture.
* `--output` or `-o`: Output file name. Default `abp-translation.json`.
* `--all-values` or `-all`: Include all keys to translate. By default, the unified translation file only includes the missing texts for the target culture. Specify this parameter if you may need to revise the values already translated before.

#### Applying Changes

Once you translate the entries in the unified translation file, you can apply your changes to the original localization files using the `--apply` parameter:

````bash
abp translate --apply  # apply all changes
abp translate -a       # shortcut for --apply
````

Then review changes on your source control system to be sure that it has changed the proper files and send a Pull Request if you've translated ABP resources. Thank you in advance for your contribution.

##### Additional Options

* `--file` or `-f`: Default: `abp-translation.json`. The translation file (use only if you've used the `--output` option before).

#### Online DeepL translate

The `translate` command also supports online translation. You need to provide your [DeepL Authentication Key](https://support.deepl.com/hc/en-us/articles/360020695820-Authentication-Key).

It will search all the `en.json(reference-culture)` files in the directory and sub-directory and then translate and generate the corresponding `zh-Hans.json(culture)` files.

````bash
abp translate -c zh-Hans --online --deepl-auth-key <auth-key>
````

### login

Some features of the CLI requires to be logged in to ABP Platform. To login with your username write:

```bash
abp login <username>                                  # Allows you to enter your password hidden
abp login <username> -p <password>                    # Specify the password as a parameter (password is visible)
abp login <username> --organization <organization>    # If you have multiple organizations, you need set your active organization
abp login <username> -p <password> -o <organization>  # You can enter both your password and organization in the same command
abp login <username> --device                         # Use device login flow
```

> When using the -p parameter, be careful as your password will be visible. It's useful for CI/CD automation pipelines.

A new login with an already active session overwrites the previous session.

### login-info

Shows your login information such as **Name**, **Surname**, **Username**, **Email Address** and **Organization**.

```bash
abp login-info
```

### logout

Logs you out by removing the session token from your computer.

```bash
abp logout
```

### bundle

This command generates script and style references for ABP Blazor WebAssembly and MAUI Blazor project and updates the **index.html** file. It helps developers to manage dependencies required by ABP modules easily.  In order ```bundle``` command to work, its **executing directory** or passed ```--working-directory``` parameter's directory must contain a Blazor or MAUI Blazor project file(*.csproj).

Usage:

````bash
abp bundle [options]
````

#### Options

* ```--working-directory``` or ```-wd```: Specifies the working directory. This option is useful when executing directory doesn't contain a Blazor project file.
* ```--force``` or ```-f```: Forces to build project before generating references.
* ```--project-type``` or ```-t```: Specifies the project type. Default type is `webassembly`. Available types:
  * `webassembly`
  * `maui-blazor`
* `--version` or `-v`: Specifies the ABP Framework version that the project is using. This is helpful for those who use central package management.

`bundle` command reads the `appsettings.json` file inside the Blazor and MAUI Blazor project for bundling options. For more details about managing style and script references in Blazor or MAUI Blazor apps, see [Managing Global Scripts & Styles](../framework/ui/blazor/global-scripts-styles.md)

### install-libs

This command install NPM Packages for MVC / Razor Pages and Blazor Server UI types. Its **executing directory** or passed ```--working-directory``` parameter's directory must contain a project file(*.csproj).

`install-libs` command reads the `abp.resourcemapping.js` file to manage package. For more details see [Client Side Package Management](../framework/ui/mvc-razor-pages/client-side-package-management.md).

Usage:

````bash
abp install-libs [options]
````

#### Options

* ```--working-directory``` or ```-wd```: Specifies the working directory. This option is useful when executing directory doesn't contain a project file.

### check-extensions

This command checks the installed ABP CLI extensions and updates them if necessary.

Usage:

````bash
abp check-extensions
````

### install-old-cli

This command installs the old ABP CLI with a specific version if it's specified. Otherwise, it installs the old ABP CLI with the latest stable version. Then, [you can directly use the old ABP CLI by simply passing the `--old` parameter at the end of the command or using the `abp-old` as the executing command](./differences-between-old-and-new-cli.md#using-the-old-abp-cli).

Usage:

```bash
abp install-old-cli [options]
```

#### Options

* ```--version``` or ```-v```: Specifies the version for ABP CLI to be installed.

## See Also

* [Examples for the new command](./new-command-samples.md)
* [Video tutorial](https://abp.io/video-courses/essentials/abp-cli)
