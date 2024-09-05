# Creating the Initial Ordering Module

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Building the Products Module",
    "Path": "tutorials/modular-crm/part-03"
  },
  "Next": {
    "Name": "Building the Ordering module",
    "Path": "tutorials/modular-crm/part-05"
  }
}
````

In this part, you will build a new module for placing orders and install it to the main CRM application.

## Creating an Empty Module

We have used the *DDD Module* template for the Product module. We will use the *Empty Module* template for the Ordering module in this part.

Right-click the `modules` folder on the *Solution Explorer* panel, and select the *Add* -> *New Module* -> *Empty Module* command:

![abp-studio-add-new-empty-module](images/abp-studio-add-new-empty-module.png)

That command opens a dialog to define the properties of the new module:

![abp-studio-add-new-empty-module-dialog](images/abp-studio-add-new-empty-module-dialog.png)

Set `ModularCrm.Ordering` as the *Module name*, leave the *Output folder* as is and click the *Create* button. It will create the new `ModularCrm.Ordering` module under the `modules` folder in the *Solution Explorer*:

![abp-studio-modular-crm-with-two-modules](images/abp-studio-modular-crm-with-two-modules.png)

Since we've created an empty module, it is really empty. If you open the `modules/modularcrm.ordering` in your file system, you can see the initial files:

![file-system-odering-module-initial-folder](images/file-system-odering-module-initial-folder.png)

## Creating the Module Packages

In this section, we will create packages under the Ordering module. The Products module was well layered based on Domain Driven Design principles. This time, we will keep the things very simple and will only create two packaged for the Ordering module:

* `ModularCrm.Ordering`: Contains all the module code without any layering. It will contains entities, database access code, services, controllers, UI pages and whatever we need to implement the *Ordering* module.
* `ModularCrm.Ordering.Contracts`: Contains the services and objects we want to share with other modules. `ModularCrm.Ordering` uses (and implements) this package.

> If your modules are relatively small, easy to maintain, they will only be used by your main application and you don't care about layering, you can create such simple module structures.

We will create and configure everything from scratch. It will take time, but you will have a good understanding of all the details. Let's begin...

### Creating the `ModularCrm.Ordering.Contracts` Package

We will start by creating the `ModularCrm.Ordering.Contracts` package. Right-click the `ModularCrm.Ordering` module on the *Solution Explorer* and select the *Add* -> *Package* -> *New Package* command as shown in the following figure:

![abp-studio-add-new-package](images/abp-studio-add-new-package.png)

That command opens a new dialog to create a new package:

![abp-studio-add-new-package-class-library](images/abp-studio-add-new-package-class-library.png)

With that dialog, you can build your module or application layer by layer. There are templates for any type of package. However, here we will go with the most simple one: *ABP Class Library*. ABP Class Library is an empty C# class library with the [core ABP package](https://www.nuget.org/packages/Volo.Abp.Core) dependency and a [module class](../../framework/architecture/modularity/basics.md).

Type `ModularCrm.Ordering.Contracts` as the *Package name* and click the *Create* button. It will add the package under the `ModularCrm.Ordering` module:

![abp-studio-new-package-under-the-module](images/abp-studio-new-package-under-the-module.png)

d