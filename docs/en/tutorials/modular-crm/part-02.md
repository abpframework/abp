# Building the Products Module

In this part, you will build a new module for product management and install it to the main CRM application.

## Creating Solution Folders

You can create solution folders and sub-folders in *Solution Explorer* to better organize your solution components. Right-click to the solution root on the *Solution Explorer* panel, and select *Add* -> *New Folder* command:

![abp-studio-add-new-folder-command](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-add-new-folder-command.png)

That command opens a dialog where you can set the folder name:

![abp-studio-new-folder-dialog](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-new-folder-dialog.png)

Create `main` and `modules` folder using the *New Folder* command, then move the `ModularCrm` module into the `main` folder (simply by drag & drop). The *Solution Explorer* panel should look like the following figure now:

![abp-studio-solution-explorer-with-folders](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-solution-explorer-with-folders.png)

## Creating the Products Module

There are two module templates provided by ABP Studio:

* **Empty Module**: You can use that module template to build your module structure from scratch.
* **DDD Module**: A Domain-Driven Design based layered module structure.

We will use the *DDD Module* template for the Product module. We will use the *Empty Module* template later in this tutorial.

Right-click the `modules` folder on the *Solution Explorer* panel, and select the *Add* -> *New Module* -> *DDD Module* command:

![abp-studio-add-new-dd-module](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-add-new-dd-module.png)

This command opens a new dialog to define properties of the new module. You can use the following values to create a new module named `ModularCrm.Products`:

![abp-studio-create-new-module-dialog](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-create-new-module-dialog.png)

When you click the *Next* button, you are redirected to the UI selection step.

### Selecting the UI Type

Here, you can select the UI type you want to support in your module:

![abp-studio-create-new-module-dialog-step-ui](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-create-new-module-dialog-step-ui.png)

A module;

* May not contain a UI and leaves the UI development to the final application.
* May contain a single UI implementation that is typically in the same technology with the main application.
* May contain more than one UI implementation if you want to create a reusable application module and you want to make that module usable by different applications with different UI technologies. For example, all of [pre-built ABP modules](https://abp.io/modules) support multiple UI options.

In this tutorial, we are selecting the MVC UI since we are building that module only for our `ModularCrm` solution and we are using the MVC UI in our application. So, select the MVC UI and click the *Next* button.

### Selecting the Database Provider

The next step is to select the database provider (or providers) you want to support with your module:

![abp-studio-create-new-module-dialog-step-db](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-create-new-module-dialog-step-db.png)

Since our main application is using Entity Framework Core and we will use the `ModularCrm.Products` module only for that main application, we can select the *Entity Framework Core* option and click the *Create* button.

### Exploring the New Module

After adding the new module, the *Solution Explorer* panel should look like the following figure:

![abp-studio-solution-explorer-two-modules](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-solution-explorer-two-modules.png)

The new `ModularCrm.Products` module has been created and added to the solution. The `ModularCrm.Products` module has a separate and independent .NET solution. Right-click the `ModularCrm.Products` module and select the *Open with* -> *Explorer* command:

![abp-studio-open-in-explorer](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-open-in-explorer.png)

This command opens the solution folder in your file system:

![product-module-folder](D:\Github\abp\docs\en\tutorials\modular-crm\images\product-module-folder.png)

You can open `ModularCrm.Product.sln` in your favorite IDE (e.g. Visual Studio):

![product-module-visual-studio](D:\Github\abp\docs\en\tutorials\modular-crm\images\product-module-visual-studio.png)

As seen in the preceding figure, the `ModularCrm.Product` solution consists of several layers, each has own responsibility.

### Installing the Product Module to the Main Application

A module does not contain an executable application inside. The `Modular.Products.Web` project is just a class library project, not an executable web application. A module should be installed to an executable application in order to run it.

The product module has no relation to the main application yet. Right-click to the `ModularCrm` module (inside the `main` folder) and select the *Import Module* command:

![abp-studio-import-module](images/abp-studio-import-module.png)

The *Import Module* command opens a dialog as shown below:

![abp-studio-import-module-dialog](images/abp-studio-import-module-dialog.png)

Select the `ModularCrm.Products` module and check the *Install this module* option. If you don't check that option, it only imports the module but doesn't setup project dependencies. Importing a module without installation can be used to manually setup your project dependencies. Here, we want to make it automatically, so checking the *Install this module* option.

When you click the OK button, ABP Studio opens the *Install Module* dialog:

![abp-studio-module-installation-dialog](images/abp-studio-module-installation-dialog.png)

This dialog simplifies installing a multi-layer module to a multi-layer application.