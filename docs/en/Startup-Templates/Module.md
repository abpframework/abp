# Module Startup Template

This template can be used to create a **reusable [application module](../Modules/Index.md)** based on the [module development best practices & conventions](../Best-Practices/Index.md). It is also suitable for creating **microservices** (with or without UI).

## How to Start With?

You can use the [ABP CLI](../CLI.md) to create a new project using this startup template. Alternatively, you can directly create & download from the [Get Started](https://abp.io/get-started) page. CLI approach is used here.

First, install the ABP CLI if you haven't installed before:

```bash
dotnet tool install -g Volo.Abp.Cli
```

Then use the `abp new` command in an empty folder to create a new solution:

```bash
abp new Acme.IssueManagement -t module
```

- `Acme.IssueManagement` is the solution name, like *YourCompany.YourProduct*. You can use single level, two-levels or three-levels naming.

### Specify the UI Framework

This template provides multiple UI frameworks:

* `mvc`: ASP.NET Core MVC UI with Razor Pages (default)
* `blazor`: Blazor UI
* `angular`: Angular UI

Use `-u` or `--ui` option to specify the UI framework:

````bash
abp new Acme.IssueManagement -t module -u angular
````

### Without User Interface

The template comes with an MVC UI by default. You can use `--no-ui` option to not include the UI layer.

````bash
abp new Acme.IssueManagement -t module --no-ui
````

## Solution Structure

Based on the options you've specified, you will get a slightly different solution structure. If you don't specify any option, you will have a solution like shown below:

![issuemanagement-module-solution](../images/issuemanagement-module-solution.png)

Projects are organized as `src`, `test` and `host` folders:

* `src` folder contains the actual module which is layered based on [DDD](../Domain-Driven-Design.md) principles.
* `test` folder contains unit & integration tests.
* `host` folder contains applications with different configurations to demonstrate how to host the module in an application. These are not a part of the module, but useful on development.

The diagram below shows the layers & project dependencies of the module:

![layered-project-dependencies-module](../images/layered-project-dependencies-module.png)

Each section below will explain the related project & its dependencies.

### .Domain.Shared Project

This project contains constants, enums and other objects these are actually a part of the domain layer, but needed to be used by all layers/projects in the solution.

An `IssueType` enum and an `IssueConsts` class (which may have some constant fields for the `Issue` entity, like `MaxTitleLength`) are good candidates for this project.

- This project has no dependency to other projects in the solution. All other projects depend on this directly or indirectly.

### .Domain Project

This is the domain layer of the solution. It mainly contains [entities, aggregate roots](../Entities.md), [domain services](../Domain-Services.md), value types, [repository interfaces](../Repositories.md) and other domain objects.

An `Issue` entity, an `IssueManager` domain service and an `IIssueRepository` interface are good candidates for this project.

- Depends on the `.Domain.Shared` because it uses constants, enums and other objects defined in that project.

### .Application.Contracts Project

This project mainly contains [application service](../Application-Services.md) **interfaces** and [Data Transfer Objects](../Data-Transfer-Objects.md) (DTO) of the application layer. It does exists to separate interface & implementation of the application layer. In this way, the interface project can be shared to the clients as a contract package.

An `IIssueAppService` interface and an `IssueCreationDto` class are good candidates for this project.

- Depends on the `.Domain.Shared` because it may use constants, enums and other shared objects of this project in the application service interfaces and DTOs.

### .Application Project

This project contains the [application service](../Application-Services.md) **implementations** of the interfaces defined in the `.Application.Contracts` project.

An `IssueAppService` class is a good candidate for this project.

- Depends on the `.Application.Contracts` project to be able to implement the interfaces and use the DTOs.
- Depends on the `.Domain` project to be able to use domain objects (entities, repository interfaces... etc.) to perform the application logic.

### .EntityFrameworkCore Project

This is the integration project for EF Core. It defines the `DbContext` and implements repository interfaces defined in the `.Domain` project.

- Depends on the `.Domain` project to be able to reference to entities and repository interfaces.

> You can delete this project if you don't want to support EF Core for your module.

### .MongoDB Project

This is the integration project for MongoDB.

- Depends on the `.Domain` project to be able to reference to entities and repository interfaces.

> You can delete this project if you don't want to support MongoDB for your module.

### Test Projects

The solution has multiple test projects, one for each layer:

- `.Domain.Tests` is used to test the domain layer.
- `.Application.Tests` is used to test the application layer.
- `.EntityFrameworkCore.Tests` is used to test EF Core configuration and custom repositories.
- `.MongoDB.Tests` is used to test MongoDB configuration and custom repositories.
- `.TestBase` is a base (shared) project for all tests.

In addition, `.HttpApi.Client.ConsoleTestApp` is a console application (not an automated test project) which demonstrate the usage of HTTP APIs from a Dotnet application.

Test projects are prepared for integration testing;

- It is fully integrated to ABP framework and all services in your application.
- It uses SQLite in-memory database for EF Core. For MongoDB, it uses the [Mongo2Go](https://github.com/Mongo2Go/Mongo2Go) library.
- Authorization is disabled, so any application service can be easily used in tests.

You can still create unit tests for your classes which will be harder to write (because you will need to prepare mock/fake objects), but faster to run (because it only tests a single class and skips all initialization process).

> Domain & Application tests are using EF Core. If you remove EF Core integration or you want to use MongoDB for testing these layers, you should manually change project references & module dependencies.

### Host Projects

The solution has a few host applications to run your module. Host applications are used to run your module in a fully configured application. It is useful on development. Host applications includes some other modules in addition to the module being developed:

Host applications support two types of scenarios.

#### Single (Unified) Application Scenario

If your module has a UI, then `.Web.Unified` application is used to host the UI and API on a single point. It has its own `appsettings.json` file (that includes the database connection string) and EF Core database migrations.

For the `.Web.Unified` application, there is a single database, named `YourProjectName_Unified` (like *IssueManagement_Unified* for this sample).

> If you've selected the `--no-ui` option, this project will not be in your solution.

##### How to Run?

Set `host/YourProjectName.Web.Unified` as the startup project, run `Update-Database` command for the EF Core from Package Manager Console and run your application. Default username is `admin` and password is `1q2w3E*`.

#### Separated Deployment & Databases Scenario

In this scenario, there are three applications;

* `.IdentityServer` application is an authentication server used by other applications. It has its own `appsettings.json` that contains database connection and other configurations.
* `.HttpApi.Host` hosts the HTTP API of the module. It has its own `appsettings.json` that contains database connections and other configurations.
* `.Web.Host` host the UI of the module. This project contains an `appsettings.json` file, but it does not have a connection string because it never connects to the database. Instead, it mainly contains endpoint of the remote API server and the authentication server.

The diagram below shows the relation of the applications:

![tiered-solution-applications](../images/tiered-solution-applications.png)

`.Web.Host` project uses OpenId Connect Authentication to get identity and access tokens for the current user from the `.IdentityServer`. Then uses the access token to call the `.HttpApi.Host`. HTTP API server uses bearer token authentication to obtain claims from the access token to authorize the current user.

##### Pre-requirements

* [Redis](https://redis.io/): The applications use Redis as as distributed cache. So, you need to have Redis installed & running.

##### How to Run?

You should run the application with the given order:

- First, run the `.IdentityServer` since other applications depends on it.
- Then run the `.HttpApi.Host` since it is used by the `.Web.Host` application.
- Finally, you can run the `.Web.Host` project and login to the application using `admin` as the username and `1q2w3E*` as the password.

## UI

### Angular UI

If you choose `Angular` as the UI framework (using the `-u angular` option), the solution will have a folder called `angular` in it. This is where the client-side code is located. When you open that folder in an IDE, the folder structure will look like below:

![Folder structure of ABP Angular module project](../images/angular-module-folder-structure.png)

* _angular/projects/issue-management_ folder contains the Angular module project.
* _angular/projects/dev-app_ folder contains a development application that runs your module.

The server-side is similar to the solution described above. `*.HttpApi.Host` project serves the API and the `Angular` demo application consumes it. You will not need to run the `.Web.Host` project though.

#### How to Run the Angular Development App

For module development, you will need the `dev-app` project up and running. So, here is how we can start the development server.

First, we need to install dependencies:

1. Open your terminal at the root folder, i.e. `angular`.
2. Run `yarn` or `npm install`.

The dependencies will be installed and some of them are ABP modules published as NPM packages. To see all ABP packages, you can run the following command in the `angular` folder:

```bash
yarn list --pattern abp
```

> There is no equivalent of this command in npm.

The module you will develop depends on two of these ABP packages: _@abp/ng.core_ and _@abp/ng.theme.shared_. Rest of the ABP modules are included in _package.json_ because of the `dev-app` project.

Once all dependencies are installed, follow the steps below to serve your development app:

1. Make sure `.IdentityServer` and `*.HttpApi.Host` projects are up and running.
2. Open your terminal at the root folder, i.e. `angular`.
3. Run `yarn start` or `npm start`.

![ABP Angular module dev-app project](../images/angular-module-dev-app-project.png)

The issue management page is empty in the beginning. You may change the content in `IssueManagementComponent` at the _angular/projects/issue-management/src/lib/issue-management.component.ts_ path and observe that the view changes accordingly.

Now, let's have a closer look at some key elements of your project.

#### Main Module

`IssueManagementModule` at the _angular/projects/issue-management/src/lib/issue-management.module.ts_ path is the main module of your module project. There are a few things worth mentioning in it:

- Essential ABP modules, i.e. `CoreModule` and `ThemeSharedModule`, are imported.
- `IssueManagementRoutingModule` is imported.
- `IssueManagementComponent` is declared.
- It is prepared for configurability. The `forLazy` static method enables [a configuration to be passed to the module when it is loaded by the router](https://volosoft.com/blog/how-to-configure-angular-modules-loaded-by-the-router).


#### Main Routing Module

`IssueManagementRoutingModule` at the _angular/projects/issue-management/src/lib/issue-management-routing.module.ts_ path is the main routing module of your module project. It currently does two things:

- Loads `DynamicLayoutComponent` at base path it is given.
- Loads `IssueManagementComponent` as child to the layout, again at the given base path.

You can rearrange this module to load more than one component at different routes, but you need to update the route provider at _angular/projects/issue-management/config/src/providers/route.provider.ts_ to match the new routing structure with the routes in the menu. Please check [Modifying the Menu](../UI/Angular/Modifying-the-Menu.md) to see how route providers work.

#### Config Module

There is a config module at the _angular/projects/issue-management/config/src/issue-management-config.module.ts_ path. The static `forRoot` method of this module is supposed to be called at the route level. So, you may assume the following will take place:

```js
@NgModule({
  imports: [
    /* other imports */

    IssueManagementConfigModule.forRoot(),
  ],

  /* rest of the module meta data */
})
export class AppModule {}
```

You can use this static method to configure an application that uses your module project. An example of such configuration is already implemented and the `ISSUE_MANAGEMENT_ROUTE_PROVIDERS` token is provided here. The method can take options which enables further configuration possibilities.

The difference between the `forRoot` method of the config module and the `forLazy` method of the main module is that, for smallest bundle size, the former should only be used when you have to configure an app before your module is even loaded.

#### Testing Angular UI

Please see the [testing document](../UI/Angular/Testing.md).
