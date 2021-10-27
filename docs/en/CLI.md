# ABP CLI

ABP CLI (Command Line Interface) is a command line tool to perform some common operations for ABP based solutions.

## Installation

ABP CLI is a [dotnet global tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools). Install it using a command line window:

````bash
dotnet tool install -g Volo.Abp.Cli
````

To update an existing installation:

````bash
dotnet tool update -g Volo.Abp.Cli
````

## Global Options

While each command may have a set of options, there are some global options those can be used with any command;

* `--skip-cli-version-check`: Skips to check the latest version of the ABP CLI. If you don't specify, it will check the latest version and shows a warning message if there is a newer version of the ABP CLI.

## Commands

Here, the list of all available commands before explaining their details:

* **`help`**: Shows help on the usage of the ABP CLI.
* **`new`**: Generates a new solution based on the ABP [startup templates](Startup-Templates/Index.md).
* **`update`**: Automatically updates all ABP related NuGet and NPM packages in a solution.
* **`add-package`**: Adds an ABP package to a project.
* **`add-module`**: Adds a [multi-package application module](https://docs.abp.io/en/abp/latest/Modules/Index) to a solution.
* **`list-modules`**: Lists names of open-source application modules.
* **`get-source`**: Downloads the source code of a module.
* **`generate-proxy`**: Generates client side proxies to use HTTP API endpoints.
* **`remove-proxy`**: Removes previously generated client side proxies.
* **`switch-to-preview`**: Switches to the latest preview version of the ABP Framework.
* **`switch-to-nightly`**: Switches to the latest [nightly builds](Nightly-Builds.md) of the ABP related packages on a solution.
* **`switch-to-stable`**: Switches to the latest stable versions of the ABP related packages on a solution.
* **`translate`**: Simplifies to translate localization files when you have multiple JSON [localization](Localization.md) files in a source control repository.
* **`login`**: Authenticates on your computer with your [abp.io](https://abp.io/) username and password.
* **`logout`**: Logouts from your computer if you've authenticated before.
* **`bundle`**: Generates script and style references for an ABP Blazor project. 
* **`install-libs`**: Install NPM Packages for MVC / Razor Pages and Blazor Server UI types.

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

### new

Generates a new solution based on the ABP [startup templates](Startup-Templates/Index.md).

Usage:

````bash
abp new <solution-name> [options]
````

Example:

````bash
abp new Acme.BookStore
````

* `Acme.BookStore` is the solution name here.
* Common convention is to name a solution is like *YourCompany.YourProject*. However, you can use different naming like *YourProject* (single level namespacing) or *YourCompany.YourProduct.YourModule* (three levels namespacing).

For more samples, go to [ABP CLI Create Solution Samples](CLI-New-Command-Samples.md)

#### Options

* `--template` or `-t`: Specifies the template name. Default template name is `app`, which generates a web application. Available templates:
  * **`app`** (default): [Application template](Startup-Templates/Application.md). Additional options:
    * `--ui` or `-u`: Specifies the UI framework. Default framework is `mvc`. Available frameworks:
      * `mvc`: ASP.NET Core MVC. There are some additional options for this template:
        * `--tiered`: Creates a tiered solution where Web and Http API layers are physically separated. If not specified, it creates a layered solution which is less complex and suitable for most scenarios.
      * `angular`: Angular UI. There are some additional options for this template:
        * `--separate-identity-server`: The Identity Server project comes as a separate project and runs at a different endpoint. It separates the Identity Server from the API Host application. If not specified, you will have a single endpoint in the server side.
      * `blazor`: Blazor UI. There are some additional options for this template:
        * `--separate-identity-server`The Identity Server project comes as a separate project and runs at a different endpoint. It separates the Identity Server from the API Host application. If not specified, you will have a single endpoint in the server side.
      * `blazor-server`: Blazor Server UI. There are some additional options for this template:
        * `--tiered`: The Identity Server and the API Host project comes as separate projects and run at different endpoints. It has 3 startup projects: *HttpApi.Host*, *IdentityServer* and *Blazor* and and each runs on different endpoints. If not specified, you will have a single endpoint for your web project.
      * `none`: Without UI. No front-end layer will be created. There are some additional options for this template:
        * `--separate-identity-server`: The Identity Server project comes as a separate project and runs at a different endpoint. It separates the Identity Server from the API Host application. If not specified, you will have a single endpoint in the server side.
    * `--mobile` or `-m`: Specifies the mobile application framework. If not specified, no mobile application will be created. Available options:
      * `react-native`: React Native.
    * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
        * `ef`: Entity Framework Core.
        * `mongodb`: MongoDB.
  * **`module`**: [Module template](Startup-Templates/Module.md). Additional options:
    * `--no-ui`: Specifies to not include the UI. This makes possible to create service-only modules (a.k.a. microservices - without UI).
  * **`console`**: [Console template](Startup-Templates/Console.md).
* `--output-folder` or `-o`: Specifies the output folder. Default value is the current directory.
* `--version` or `-v`: Specifies the ABP & template version. It can be a [release tag](https://github.com/abpframework/abp/releases) or a [branch name](https://github.com/abpframework/abp/branches). Uses the latest release if not specified. Most of the times, you will want to use the latest version.
* `--preview`: Use latest preview version.
* `--template-source` or `-ts`: Specifies a custom template source to use to build the project. Local and network sources can be used(Like `D:\local-template` or `https://.../my-template-file.zip`).
* `--create-solution-folder` or `-csf`: Specifies if the project will be in a new folder in the output folder or directly the output folder.
* `--connection-string` or `-cs`:  Overwrites the default connection strings in all `appsettings.json` files. The default connection string is `Server=localhost;Database=MyProjectName;Trusted_Connection=True` for EF Core and it is configured to use the SQL Server. If you want to use the EF Core, but need to change the DBMS, you can change it as [described here](Entity-Framework-Core-Other-DBMS.md) (after creating the solution).
* `--database-management-system` or `-dbms`: Sets the database management system. Default is **SQL Server**. Supported DBMS's:
  * `SqlServer`
  * `MySQL`
  * `SQLite`
  * `Oracle`
  * `Oracle-Devart`
  * `PostgreSQL`
* `--local-framework-ref --abp-path`: Uses local projects references to the ABP framework instead of using the NuGet packages. This can be useful if you download the ABP Framework source code and have a local reference to the framework from your application.
* `--no-random-port`: Uses template's default ports.

See some [examples for the new command](CLI-New-Command-Samples.md) here.

### update

Updating all ABP related packages can be tedious since there are many packages of the framework and modules. This command automatically updates all ABP related NuGet and NPM packages in a solution or project to the latest versions.

Usage:

````bash
abp update [options]
````

* If you run in a directory with a .sln file, it updates all ABP related packages of the all projects of the solution to the latest versions.
* If you run in a directory with a .csproj file, it updates all ABP related packages of the project to the latest versions.

#### Options

* `--npm`: Only updates NPM packages.
* `--nuget`: Only updates NuGet packages.
* `--solution-path` or `-sp`: Specify the solution path. Use the current directory by default
* `--solution-name` or `-sn`: Specify the solution name. Search `*.sln` files in the directory by default.
* `--check-all`: Check the new version of each package separately. Default is `false`.
* `--version` or `-v`: Specifies the version to use for update. If not specified, latest version is used.

### add-package

Adds an ABP package to a project by,

* Adding related nuget package as a dependency to the project.
* Adding `[DependsOn(...)]` attribute to the module class in the project (see the [module development document](Module-Development-Basics.md)).

> Notice that the added module may require additional configuration which is generally indicated in the documentation of the related package.

Basic usage:

````bash
abp add-package <package-name> [options]
````

Example:

````
abp add-package Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic
````

* This example adds the Volo.Abp.MongoDB package to the project.

#### Options

* `--project` or `-p`: Specifies the project (.csproj) file path. If not specified, CLI tries to find a .csproj file in the current directory.
* `--with-source-code`: Downloads the source code of the package to your solution folder and uses local project references instead of NuGet/NPM packages.
* `--add-to-solution-file`: Adds the downloaded package to your solution file, so you will also see the package when you open the solution on a IDE. (only available when `--with-source-code` is True)

> Currently only the source code of the basic theme packages([MVC](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Basic-Theme) and [Blazor](https://docs.abp.io/en/abp/latest/UI/Blazor/Basic-Theme)) can be downloaded.
> - Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic
> - Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
> - Volo.Abp.AspNetCore.Components.Web.BasicTheme
> - Volo.Abp.AspNetCore.Components.Server.BasicTheme


### add-module

Adds a [multi-package application module](Modules/Index) to a solution by finding all packages of the module, finding related projects in the solution and adding each package to the corresponding project in the solution.

It can also create a new module for your solution and add it to your solution. See `--new` option.

> A business module generally consists of several packages (because of layering, different database provider options or other reasons). Using `add-module` command dramatically simplifies adding a module to a solution. However, each module may require some additional configurations which is generally indicated in the documentation of the related module.

Usage

````bash
abp add-module <module-name> [options]
````

Examples:

```bash
abp add-module Volo.Blogging
```

* This example adds the `Volo.Blogging` module to the solution.

```bash
abp add-module ProductManagement --new --add-to-solution-file
```

* This command creates a fresh new module customized for your solution (named `ProductManagement`) and adds it to your solution.


#### Options

* `--solution` or `-s`: Specifies the solution (.sln) file path. If not specified, CLI tries to find a .sln file in the current directory.
* `--skip-db-migrations`: For EF Core database provider, it automatically adds a new code first migration (`Add-Migration`) and updates the database (`Update-Database`) if necessary. Specify this option to skip this operation.
* `-sp` or `--startup-project`: Relative path to the project folder of the startup project. Default value is the current folder.
* `--new`: Creates a fresh new module (customized for your solution) and adds it to your solution.
* `--with-source-code`: Downloads the source code of the module to your solution folder and uses local project references instead of NuGet/NPM packages. This options is always `True` if `--new` is used.
* `--add-to-solution-file`: Adds the downloaded/created module to your solution file, so you will also see the projects of the module when you open the solution on a IDE. (only available when `--with-source-code` is `True`.)

### list-modules

Lists names of open-source application modules.

Usage

````bash
abp list-modules [options]
````

Example:

```bash
abp list-modules
```

#### Options

* `--include-pro-modules`: Includes commercial (pro) modules in the output.

### get-source

Downloads the source code of a module to your computer.

Usage

````bash
abp get-source <module-name> [options]
````

Example:

```bash
abp get-source Volo.Blogging

abp get-source Volo.Blogging --local-framework-ref --abp-path D:\GitHub\abp
```

#### Options

* `--output-folder` or `-o`: Specifies the directory that source code will be downloaded in. If not specified, current directory is used.
* `--version` or `-v`: Specifies the version of the  source code that will be downloaded. If not specified, latest version is used.
* `--preview`: If no version option is specified, this option specifies if latest [preview version](Previews.md) will be used instead of latest stable version.
* `--local-framework-ref --abp-path`: Path of [ABP Framework GitHub repository](https://github.com/abpframework/abp) in your computer. This will be used for converting project references to your local system. If this is not specified, project references will be converted to NuGet references.

### generate-proxy

Generates Angular service proxies for your HTTP APIs to make easy to consume your services from the client side. Your host (server) application must be up and running before running this command.

Usage:

````bash
abp generate-proxy
````

#### Options

* `--module` or `-m`: Specifies the name of the backend module you wish to generate proxies for. Default value: `app`.
* `--api-name` or `-a`: The name of the API endpoint defined in the `/src/environments/environment.ts`. Default value: `default`.
* `--source` or `-s`: Specifies the Angular project name to resolve the root namespace & API definition URL from. Default value: `defaultProject`.
* `--target` or `-t`: Specifies the Angular project name to place generated code in. Default value: `defaultProject`.
* `--prompt` or `-p`: Asks the options from the command line prompt (for the unspecified options).

> See the [Angular Service Proxies document](UI/Angular/Service-Proxies.md) for more.

### remove-proxy

Removes previously generated proxy code from the Angular application. Your host (server) application must be up and running before running this command.

This can be especially useful when you generate proxies for multiple modules before and need to remove one of them later.

Usage:

````bash
abp remove-proxy
````

#### Options

* `--module` or `-m`: Specifies the name of the backend module you wish to remove proxies for. Default value: `app`.
* `--api-name` or `-a`: The name of the API endpoint defined in the `/src/environments/environment.ts`. Default value: `default`.
* `--source` or `-s`: Specifies the Angular project name to resolve the root namespace & API definition URL from. Default value: `defaultProject`.
* `--target` or `-t`: Specifies the Angular project name to place generated code in. Default value: `defaultProject`.
* `--prompt` or `-p`: Asks the options from the command line prompt (for the unspecified options).

> See the [Angular Service Proxies document](UI/Angular/Service-Proxies.md) for more.

### switch-to-preview

You can use this command to switch your project to latest preview version of the ABP framework.

Usage:

````bash
abp switch-to-preview [options]
````

#### Options

* `--solution-directory` or `-sd`: Specifies the directory. The solution should be in that directory or in any of its sub directories. If not specified, default is the current directory.


### switch-to-nightly

You can use this command to switch your project to latest [nightly](Nightly-Builds.md) preview version of the ABP framework packages.

Usage:

````bash
abp switch-to-nightly [options]
````

#### Options

* `--solution-directory` or `-sd`: Specifies the directory. The solution should be in that directory or in any of its sub directories. If not specified, default is the current directory.

### switch-to-stable

If you're using the ABP Framework preview packages (including nightly previews), you can switch back to latest stable version using this command.

Usage:

````bash
abp switch-to-stable [options]
````
#### Options

* `--solution-directory` or `-sd`: Specifies the directory. The solution should be in that directory or in any of its sub directories. If not specified, default is the current directory.

### translate

Simplifies to translate [localization](Localization.md) files when you have multiple JSON [localization](Localization.md) files in a source control repository.

* This command will create a unified json file based on the reference culture. 
* It searches all the localization `JSON` files in the current directory and all subdirectories (recursively). Then creates a single file (named `abp-translation.json` by default) that includes all the entries need to be translated.
* Once you translate the entries in this file, you can then apply your changes to the original localization files using the `--apply` command.

> The main purpose of this command is to translate ABP Framework localization files (since the [abp repository](https://github.com/abpframework/abp) has tens of localization files to be translated in different directories).

#### Creating the Translation File

First step is to create the unified translation file:

````bash
abp translate -c <culture> [options]
````

Example:

````bash
abp translate -c de-DE
````

This command created the unified translation file for the `de-DE` (German) culture.

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

Then review changes on your source control system to be sure that it has changed the proper files and send a Pull Request if you've translated ABP Framework resources. Thank you in advance for your contribution.

##### Additional Options

* `--file` or `-f`: Default: `abp-translation.json`. The translation file (use only if you've used the `--output` option before).

### login

Some features of the CLI requires to be logged in to abp.io platform. To login with your username write:

```bash
abp login <username>                                  # Allows you to enter your password hidden
abp login <username> -p <password>                    # Specify the password as a parameter (password is visible)
abp login <username> --organization <organization>    # If you have multiple organizations, you need set your active organization
abp login <username> -p <password> -o <organization>  # You can enter both your password and organization in the same command
```

> When using the -p parameter, be careful as your password will be visible. It's useful for CI/CD automation pipelines.

A new login with an already active session overwrites the previous session.

### logout

Logs you out by removing the session token from your computer.

```
abp logout
```

#### Options

* ```--working-directory``` or ```-wd```: Specifies the working directory. This option is useful when the command is executed outside of a GIT repository or when executing directory doesn't contain a .NET solution file.
* ```--build-name``` or ```-n```: Specifies a name for the build. This option is useful when same repository is used for more than one different builds. 
* ```--dotnet-build-arguments``` or ```-a```: Arguments to pass ```dotnet build``` when building project files.  This parameter must be passed like ```"\"{params}\""``` .
* ```--force``` or ```-f```: Forces to build projects even they are not changed from the last successful build.

For more details, see [build command documentation](CLI-BuildCommand.md).


### bundle

This command generates script and style references for an ABP Blazor WebAssembly project and updates the **index.html** file. It helps developers to manage dependencies required by ABP modules easily.  In order ```bundle``` command to work, its **executing directory** or passed ```--working-directory``` parameter's directory must contain a Blazor project file(*.csproj).

Usage:

````bash
abp bundle [options]
````

#### Options

* ```--working-directory``` or ```-wd```: Specifies the working directory. This option is useful when executing directory doesn't contain a Blazor project file.
* ```--force``` or ```-f```: Forces to build project before generating references.

`bundle` command reads the `appsettings.json` file inside the Blazor project for bundling options. For more details about managing style and script references in Blazor apps, see [Managing Global Scripts & Styles](UI/Blazor/Global-Scripts-Styles.md)

### install-libs

This command install NPM Packages for MVC / Razor Pages and Blazor Server UI types. Its **executing directory** or passed ```--working-directory``` parameter's directory must contain a project file(*.csproj).

`install-libs` command reads the `abp.resourcemapping.js` file to manage package. For more details see [Client Side Package Management](UI/AspNetCore/Client-Side-Package-Management.md).

Usage:

````bash
abp install-libs [options]
````

#### Options

* ```--working-directory``` or ```-wd```: Specifies the working directory. This option is useful when executing directory doesn't contain a project file.

## See Also

* [Examples for the new command](CLI-New-Command-Samples.md)
