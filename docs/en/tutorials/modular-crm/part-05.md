# Building the Ordering Module

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Creating the Initial Ordering Module",
    "Path": "tutorials/modular-crm/part-04"
  }
}
````

In the previous part, we've created a custom Ordering module and installed it into the main application. However, the Ordering module has no functionality now. In this part, we will create an `Order` entity and add functionality to create and list the orders.

## Creating an `Order` Entity

Open the `ModularCrm.Ordering` .NET solution in your IDE.

> Tip: You can open the folder of a module's .NET solution by right-clicking the related module in ABP Studio, selecting the *Open with* -> *Explorer* command.

The following figure shows the `ModularCrm.Ordering` module in the *Solution Explorer* panel of Visual Studio:

![visual-studio-ordering-module-initial](images/visual-studio-ordering-module-initial.png)

### Adding `Volo.Abp.Ddd.Domain` Package Reference

As you see in the preceding figure, the solution structure is very minimal. It also has a minimal ABP dependency. ABP Framework has [hundreds of NuGet packages](https://abp.io/packages), but the `ModularCrm.Ordering` project only has the [`Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared`](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared) package reference:

![visual-studio-ordering-ui-package-dependency](images/visual-studio-ordering-ui-package-dependency.png)

TODO

![abp-studio-add-package-reference-2](images/abp-studio-add-package-reference-2.png)

f