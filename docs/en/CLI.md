﻿# ABP CLI

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

## Commands

Here, the list of all available commands before explaining their details:

* **`help`**: Shows help on the usage of the ABP CLI.
* **`new`**: Generates a new solution based on the ABP [startup templates](Startup-Templates/Index.md).
* **`update`**: Automatically updates all ABP related NuGet and NPM packages in a solution.
* **`add-package`**: Adds an ABP package to a project.
* **`add-module`**: Adds a [multi-package application module](https://docs.abp.io/en/abp/latest/Modules/Index) to a solution.
* **`generate-proxy`**: Generates client side proxies to use HTTP API endpoints on the server.
* **`switch-to-preview`**: Switches to the latest [nightly builds](Nightly-Builds.md) of the ABP related packages on a solution.
* **`switch-to-stable`**: Switches to the latest stable versions of the ABP related packages on a solution.
* **`translate`**: Simplifies to translate localization files when you have multiple JSON [localization](Localization.md) files in a source control repository.
* **`login`**: Authenticates on your computer with your [abp.io](https://abp.io/) username and password.
* **`logout`**: Logouts from your computer if you've authenticated before.

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

#### Options

* `--template` or `-t`: Specifies the template name. Default template name is `app`, which generates a web application. Available templates:
  * **`app`** (default): [Application template](Startup-Templates/Application.md). Additional options:
    * `--ui` or `-u`: Specifies the UI framework. Default framework is `mvc`. Available frameworks:
      * `mvc`: ASP.NET Core MVC. There are some additional options for this template:
        * `--tiered`: Creates a tiered solution where Web and Http API layers are physically separated. If not specified, it creates a layered solution which is less complex and suitable for most scenarios.
      * `angular`: Angular. There are some additional options for this template:
        * `--separate-identity-server`: Separates the identity server application from the API host application. If not specified, you will have a single endpoint in the server side.
      * `none`: Without UI. There are some additional options for this template:
        * `--separate-identity-server`: Separates the identity server application from the API host application. If not specified, you will have a single endpoint in the server side.
    * `--mobile` or `-m`: Specifies the mobile application framework. Default framework is `react-native`. Available frameworks:
      * `none`: no mobile application.
      * `react-native`: React Native.
	  * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
        * `ef`: Entity Framework Core.
        * `mongodb`: MongoDB.
  * **`module`**: [Module template](Startup-Templates/Module.md). Additional options:
    * `--no-ui`: Specifies to not include the UI. This makes possible to create service-only modules (a.k.a. microservices - without UI).
  * **`console`**: [Console template](Startup-Templates/Console.md).
* `--output-folder` or `-o`: Specifies the output folder. Default value is the current directory.
* `--version` or `-v`: Specifies the ABP & template version. It can be a [release tag](https://github.com/abpframework/abp/releases) or a [branch name](https://github.com/abpframework/abp/branches). Uses the latest release if not specified. Most of the times, you will want to use the latest version.
* `--template-source` or `-ts`: Specifies a custom template source to use to build the project. Local and network sources can be used(Like `D\localTemplate` or `https://<your url>.zip`).
* `--create-solution-folder` or `-csf`: Specifies if the project will be in a new folder in the output folder or directly the output folder.
* `--connection-string` or `-cs`:  Overwrites the default connection strings in all `appsettings.json` files. The default connection string is `Server=localhost;Database=MyProjectName;Trusted_Connection=True;MultipleActiveResultSets=true`. You can set your own connection string if you don't want to use the default. Be aware that the default database provider is `SQL Server`, therefore you can only enter connection string for SQL Server!
* `--local-framework-ref --abp-path`: keeps local references to projects instead of replacing with NuGet package references.

### update

Updating all ABP related packages can be tedious since there are many packages of the framework and modules. This command automatically updates all ABP related NuGet and NPM packages in a solution or project to the latest versions.

Usage:

````bash
abp update [options]
````

* If you run in a directory with a .sln file, it updates all ABP related packages of the all projects of the solution to the latest versions.
* If you run in a directory with a .csproj file, it updates all ABP related packages of the project to the latest versions.

#### Options

* `--include-previews` or `-p`: Includes preview, beta and rc packages while checking the latest versions.
* `--npm`: Only updates NPM packages.
* `--nuget`: Only updates NuGet packages.
* `--solution-path` or `-sp`: Specify the solution path. Use the current directory by default
* `--solution-name` or `-sn`: Specify the solution name. Search `*.sln` files in the directory by default.
* `--check-all`: Check the new version of each package separately. Default is `false`.

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
abp add-package Volo.Abp.MongoDB
````

* This example adds the Volo.Abp.MongoDB package to the project.

#### Options

* `--project` or `-p`: Specifies the project (.csproj) file path. If not specified, CLI tries to find a .csproj file in the current directory.

### add-module

Adds a [multi-package application module](Modules/Index) to a solution by finding all packages of the module, finding related projects in the solution and adding each package to the corresponding project in the solution.

> A business module generally consists of several packages (because of layering, different database provider options or other reasons). Using `add-module` command dramatically simplifies adding a module to a solution. However, each module may require some additional configurations which is generally indicated in the documentation of the related module.

Usage

````bash
abp add-module <module-name> [options]
````

Example:

```bash
abp add-module Volo.Blogging
```

* This example add the Volo.Blogging module to the solution.

#### Options

* `--solution` or `-s`: Specifies the solution (.sln) file path. If not specified, CLI tries to find a .sln file in the current directory.
* `--skip-db-migrations`: For EF Core database provider, it automatically adds a new code first migration (`Add-Migration`) and updates the database (`Update-Database`) if necessary. Specify this option to skip this operation.
* `-sp` or `--startup-project`: Relative path to the project folder of the startup project. Default value is the current folder.
* `--with-source-code`: Add source code of the module instead of NuGet/NPM packages.

### generate-proxy

Generates client proxies for your HTTP APIs to make easy to consume your services from the client side. Before running `generate-proxy` command, your host must be up and running.

Usage:

````bash
abp generate-proxy [options] 
````

#### Options

* `--apiUrl` or `-a`: Specifies the root URL of the HTTP API. The default value is being retrieved from the `environment.ts` file for the Angular application. Make sure your host is up and running before running `abp generate-proxy`.
* `--ui` or `-u`: Specifies the UI framework. Default value is `angular` and it is the only UI framework supported for now. Creates TypeScript code.
* `--module` or `-m`: Specifies the module name. Default module name is `app`, which indicates your own application (you typically want this since every module is responsible to maintain its own client proxies). Set `all` for to generate proxies for all the modules.

Example usage with the options:

````bash
abp generate-proxy --apiUrl https://localhost:44305 --ui angular --module all
````


### switch-to-preview

You can use this command to switch your project to latest preview version of the ABP framework packages.

Usage:

````bash
abp switch-to-preview [options]
````

#### Options

`--solution-directory` or `-sd`: Specifies the directory. The solution should be in that directory or in any of its sub directories. If not specified, default is the current directory.

### switch-to-stable

If you're using the ABP Framework preview packages, you can switch back to stable version using this command.

Usage:

````bash
abp switch-to-stable [options]
````
#### Options

`--solution-directory` or `-sd`: Specifies the directory. The solution should be in that directory or in any of its sub directories. If not specified, default is the current directory.

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
abp login <username>                # Asks password separately
abp login <username> -p <password>  # Specify the password as a parameter
```

> Using `-p` parameter might not be safe if someone is watching your screen :) It can be useful for automation purposes.

A new login with an already active session overwrites the previous session.

### logout

Logs you out by removing the session token from your computer.

```
abp logout
```

