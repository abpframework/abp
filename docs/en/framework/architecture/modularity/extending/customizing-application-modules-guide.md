# Customizing the Existing Modules

ABP has been designed to support to build fully [modular applications](../../modularity/basics.md) and systems. It also provides some [pre-built application modules](../../../../modules) those are **ready to use** in any kind of application.

For example, you can **re-use** the [Identity Management Module](../../../../modules/identity.md) to add user, role and permission management to your application. The [application startup template](../../../../solution-templates/layered-web-application) already comes with Identity and some other modules **pre-installed**.

## Re-Using an Application Module

You have two options to re-use an application module.

### As Package References

You can add **NuGet** & **NPM** package references of the related module to your application and configure the module (based on its documentation) to integrate to your application.

As mentioned before, the [application startup template](../../../../solution-templates/layered-web-application) already comes with some **fundamental modules pre-installed**. It uses the modules as NuGet & NPM package references.

This approach has the following benefits:

* Your solution will be **clean** and only contains your **own application code**.
* You can **easily upgrade** a module when a new version is available. `abp update` [CLI](../../../../cli/index.md) command makes it even easier. In this way, you can continue to get **new features and bug fixes**.

However, there is a drawback:

* You may not able to **customize** the module because the module source is not in your solution.

This document explains **how to customize or extend** a depended module without need to change its source code. While it is limited compared to a full source code change opportunity, there are still some good ways to make some customizations.

If you don't think to make huge changes on the pre-built modules, re-using them as package reference is the recommended way.

### Including the Source Code

If you want to make **huge changes** or add **major features** on a pre-built module, but the available extension points are not enough, you can consider to directly work the source code of the depended module.

In this case, you typically **add the source code** of the module to your solution and replace **every** package reference in the solution with its corresponding local project references.  **[ABP CLI](../../../../cli/index.md)**'s `add-module` command automates this process for you with the `--with-source-code` parameter. This command can also replace a module by its source code if the module already installed as NuGet packages.


#### Separating the Module Solution

You may prefer to not include the module source code **directly into your solution**. Every module consists of 10+ project files and adding **multiple modules** may impact on the **size** of your solution **load & development time.** Also, you may have different development teams working on different modules, so you don't want to make the module code available to the application development team.

In any case, you can create a **separate solution** for the desired module and depend on the module as project references out of the solution. We do it like that for the [abp repository](https://github.com/abpframework/abp/).

> One problem we see is  Visual Studio doesn't play nice with this kind of approach (it doesn't support well to have references to local projects out of the solution directory). If you get error while building the application (depends on an external module), run `dotnet restore` in the command line after opening the application's solution in the Visual Studio.

#### Publishing the Customized Module as Packages

One alternative scenario could be re-packaging the module source code (as NuGet/NPM packages) and using as package references. You can use a local private NuGet/NPM server for your company, for example.

## Module Customization / Extending Approaches

This section suggests some approaches if you decided to use pre-built application modules as NuGet/NPM package references. The following documents explain how to customize/extend existing modules in different ways.

### Module Entity Extension System

> Module entity extension system is the **main and high level extension system** that allows you to **define new properties** for existing entities of the depended modules. It automatically **adds properties to the entity, database, HTTP API and the user interface** in a single point.

See the [Module Entity Extensions document](./module-entity-extensions.md) to learn how to use it.

### Extending Entities

If you only need to get/set extra data on an existing entity, follow the [Extending Entities](./customizing-application-modules-extending-entities.md) document.

### Overriding Services/Components

In addition to the extensibility systems, you can partially or completely override any service or user interface page/component.

* [Overriding Services](./customizing-application-modules-overriding-services.md)
* [Overriding the User Interface](./overriding-user-interface.md)

### Additional UI Extensibility Points

There are some low level systems that you can control entity actions, table columns and page toolbar of a page defined by a module.

#### Entity Actions

Entity action extension system allows you to add a new action to the action menu for an entity on the user interface;

* [Entity Action Extensions for ASP.NET Core UI](../../../ui/mvc-razor-pages/entity-action-extensions.md)
* [Entity Action Extensions for Blazor UI](../../../ui/blazor/entity-action-extensions.md)
* [Entity Action Extensions for Angular](../../../ui/angular/entity-action-extensions.md)

#### Data Table Column Extensions

Data table column extension system allows you to add a new column in the data table on the user interface;

* [Data Table Column Extensions for ASP.NET Core UI](../../../ui/mvc-razor-pages/data-table-column-extensions.md)
* [Data Table Column Extensions for Blazor UI](../../../ui/blazor/data-table-column-extensions.md)
* [Data Table Column Extensions for Angular](../../../ui/angular/data-table-column-extensions.md)

#### Page Toolbar

Page toolbar system allows you to add components to the toolbar of a page;

* [Page Toolbar Extensions for ASP.NET Core UI](../../../ui/mvc-razor-pages/page-toolbar-extensions.md)
* [Page Toolbar Extensions for Blazor UI](../../../ui/blazor/page-toolbar-extensions.md)
* [Page Toolbar Extensions for Angular](../../../ui/angular/page-toolbar-extensions.md)

#### Others

* [Dynamic Form Extensions for Angular](../../../ui/angular/dynamic-form-extensions.md)

## See Also

Also, see the following documents:

* See [the localization document](../../../fundamentals/localization.md) to learn how to extend existing localization resources.
* See [the settings document](../../../infrastructure/settings.md) to learn how to change setting definitions of a depended module.
* See [the authorization document](../../../fundamentals/authorization.md) to learn how to change permission definitions of a depended module.
