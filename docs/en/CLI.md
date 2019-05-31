# ABP CLI

ABP CLI (Command Line Interface) is a command line tool to perform some common operations for ABP based solutions.

## new

Generates a new solution based on the ABP [startup templates](Startup-Templates/Index.md).

Basic usage:

````bash
abp new <solution-name>
````

Example:

````bash
abp new Acme.BookStore
````

* Acme.BookStore is the solution name here.
* Common convention is to name a solution is like *YourCompany.YourProject*. However, you can use different naming like *YourProject* (single level namespacing) or *YourCompany.YourProduct.YourModule* (three levels namespacing).

### Options

* `--template` or `-t`: Specifies the template name. Default template name is `mvc`. Available templates:
  * `mvc` (default): ASP.NET Core [MVC application template](Startup-Templates/Mvc.md). Additional options:
    * `--database-provider` or `-d`: Specifies the database provider. Default provider is `ef`. Available providers:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
    * `--tiered`: Creates a tiered solution where Web and Http API layers are physically separated. If not specified, it creates a layered solution which is less complex and suitable for most scenarios.
  *  `mvc-module`: ASP.NET Core [MVC module template](Startup-Templates/Mvc-Module.md). Additional options:
    * `--no-ui`: Specifies to not include the UI. This makes possible to create service-only modules (a.k.a. microservices - without UI).
* `--output-folder` or `-o`: Specifies the output folder. Default value is the current directory.

## add-package

Adds a new ABP package to a project by,

* Adding related nuget package as a dependency to the project.
* Adding `[DependsOn(...)]` attribute to the module class in the project (see the [module development document](Module-Development-Basics.md)).

> Notice that the added module may require additional configuration which is generally indicated in the documentation of the related package.

Basic usage:

````bash
abp add-package <package-name>
````

Example:

````
abp add-package Volo.Abp.MongoDB
````

* This example adds the Volo.Abp.MongoDB package to the project.

### Options

* `--project` or `-p`: Specifies the project (.csproj) file path. If not specified, CLI tries to find a .csproj file in the current directory.

## add-module

Adds a multi-package module to a solution by finding all packages of the module, finding related projects in the solution and adding each package to the corresponding project in the solution.

> A business module generally consists of several packages (because of layering, different database providr options or other reasons). Using `add-module` command dramatically simplifies adding a module to a solution. However, each module may require some additional configurations which is generally indicated in the documentation of the related module.

Basic usage:

````bash
abp add-module <module-name>
````

Example:

```bash
abp add-module Volo.Blogging
```

* This example add the Volo.Blogging module to the solution.

### Options

* `--solution` or `-s`: Specifies the solution (.sln) file path. If not specified, CLI tries to find a .sln file in the current directory.
* `--skip-db-migrations`: For EF Core database provider, it automatically adds a new code first migration (`Add-Migration`) and updates the database (`Update-Database`) if necessary. Specify this option to skip this operation.

## update

Updating all ABP related packages can be tedious since there are many packages of the framework and modules. This command automatically updates all ABP related packages in a solution or project to the latest versions.

Usage:

````bash
abp update
````

* If you run in a directory with a .sln file, it updates all ABP related packages of the all projects of the solution to the latest versions.
* If you run in a directory with a .csproj file, it updates all ABP related packages of the project to the latest versions.

### Options

* `--include-previews` or `-p`: Includes preview, beta and rc packages while checking the latest versions.

## help

Writes basic usage information of the CLI.

Usage:

````bash
abp help
````

