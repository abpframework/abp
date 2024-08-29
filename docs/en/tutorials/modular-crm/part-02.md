## Building the Products Module

In this part, you will create a new module for product management and add it to the main application.

You can create folders and sub-folders in *Solution Explorer* to better organize your solution. Right-click to the solution root on the *Solution Explorer* panel, and select Add -> New Folder command:

![abp-studio-add-new-folder-command](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-add-new-folder-command.png)

That command opens a dialog where you can set the folder name:

![abp-studio-new-folder-dialog](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-new-folder-dialog.png)

Create `main` and `modules` folder using the *New Folder* command, then move the `ModularCrm` module into the `main` folder (simply by drag & drop). The *Solution Explorer* panel should look like below now:

![abp-studio-solution-explorer-with-folders](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-solution-explorer-with-folders.png)

Now, we can create a new module in our solution. There are two module templates provided by ABP Studio:

* **Empty Module**: You can use that module template to build your module structure from scratch.
* **DDD Module**: A Domain-Driven Design based layered module structure.

We will use the *DDD Module* template for the Product module. We will use the *Empty Module* template later in this tutorial.

Right-click the `modules` folder on the *Solution Explorer* panel, and select the *Add* -> *New Module* -> *DDD Module* command:

![abp-studio-add-new-dd-module](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-add-new-dd-module.png)

This command opens a new dialog to define properties of the new module. You can use the following values to create a new module named `ModularCrm.Products`:

![abp-studio-create-new-module-dialog](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-create-new-module-dialog.png)

When you click the *Next* button, you are redirected to the UI selection step. Here, you can select the UI type you want to support in your module:

![abp-studio-create-new-module-dialog-step-ui](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-create-new-module-dialog-step-ui.png)

A module;

* May not contain a UI part to leave the UI development to the final application.
* May contain a single UI implementation that is typically in the same technology with the main application.
* May contain more than one UI implementation if you want to create a reusable application module and you want to make that module usable by different applications with different UI technologies. For example, all of [pre-built ABP modules](https://abp.io/modules) support multiple UI options.

In this tutorial, we are selecting the MVC UI since we are building that module only for our `ModularCrm` solution and we are using the MVC UI in the main application. So, select the MVC UI and click the *Next* button.

The next step is to select the database provider (or providers) you want to support with your module:

![abp-studio-create-new-module-dialog-step-db](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-create-new-module-dialog-step-db.png)

Since our main application is using Entity Framework Core and we will use the `ModularCrm.Products` module only for that main application, we can select the *Entity Framework Core* option and click the *Create* button.

After adding the new module, the *Solution Explorer* panel should look like the following figure:

![abp-studio-solution-explorer-two-modules](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-solution-explorer-two-modules.png)

The new `ModularCrm.Products` module has been created and added to the solution. The `ModularCrm.Products` module has a separate and independent .NET solution. Right-click the `ModularCrm.Products` module and select the *Open with* -> *Explorer* command:

![abp-studio-open-in-explorer](D:\Github\abp\docs\en\tutorials\modular-crm\images\abp-studio-open-in-explorer.png)

This command opens the solution folder in your file system:

![product-module-folder](D:\Github\abp\docs\en\tutorials\modular-crm\images\product-module-folder.png)

You can open `ModularCrm.Product.sln` in your favorite IDE (e.g. Visual Studio):

![product-module-visual-studio](D:\Github\abp\docs\en\tutorials\modular-crm\images\product-module-visual-studio.png)

As seen in the preceding figure, the `ModularCrm.Product` solution consists of several layers, each has own responsibility.