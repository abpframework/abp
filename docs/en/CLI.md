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

## Commands

### new

Generates a new solution based on the ABP [startup templates](Startup-Templates/Index.md).

Basic usage:

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
  * `app` (default): [Application template](Startup-Templates/Application.md). Additional options:
    * `--ui` or `-u`: Specifies the UI framework. Default framework is `mvc`. Available frameworks:
      * `mvc`: ASP.NET Core MVC. There are some additional options for this template:
        * `--tiered`: Creates a tiered solution where Web and Http API layers are physically separated. If not specified, it creates a layered solution which is less complex and suitable for most scenarios.
      * `angular`: Angular. There are some additional options for this template:
        * `--separate-identity-server`: Separates the identity server application from the API host application. If not specified, you will have a single endpoint in the server side.
    * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
  *  `module`: [Module template](Startup-Templates/Module.md). Additional options:
      * `--no-ui`: Specifies to not include the UI. This makes possible to create service-only modules (a.k.a. microservices - without UI).
* `--output-folder` or `-o`: Specifies the output folder. Default value is the current directory.
* `--version` or `-v`: Specifies the ABP & template version. It can be a [release tag](https://github.com/abpframework/abp/releases) or a [branch name](https://github.com/abpframework/abp/branches). Uses the latest release if not specified. Most of the times, you will want to use the latest version.

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

Basic usage:

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

### help

Writes basic usage information of the CLI.

Usage:

````bash
abp help [command-name]
````

Examples:

````bash
abp help        # Shows a general help.
abp help new    # Shows help about the "new" command.
````

