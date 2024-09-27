# Microservice Tutorial Part 02: Creating the initial Catalog Microservice

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Creating the initial solution",
    "Path": "tutorials/microservice/part-01"
  },
  "Next": {
    "Name": "Building the Catalog service",
    "Path": "tutorials/microservice/part-03"
  }
}
````

In this tutorial, you will create a new Catalog service and integrate it to the solution.

## Creating the Service

Right-click the `services` folder in the *Solution Explorer* panel, select the *Add* -> *New Module* -> *Microservice* command:

![abp-studio-add-new-microservice-command](images/abp-studio-add-new-microservice-command.png)

This command opens a new dialog to define the properties of the new microservice. You can use the following values to create a new microservice named `Catalog`:

![abp-studio-add-new-microservice-dialog](images/abp-studio-add-new-microservice-dialog.png)

When you click the *Next* button, you are redirected to the database provider selection step.

### Selecting the Database Type

Here, you can select the database provider to be used by the new microservice:

![abp-studio-add-new-microservice-dialog-database-step](images/abp-studio-add-new-microservice-dialog-database-step.png)

Select *Entity Framework Core* option and proceed the *Next* step.

### Integrating to the Solution

In this step, we can select the options for integrating the new microservice to the rest of the solution components:

![abp-studio-add-new-microservice-dialog-integration-step](D:\Github\abp\docs\en\tutorials\microservice\images\abp-studio-add-new-microservice-dialog-integration-step.png)

ABP Studio intelligently selects the right values for you, but you should still check them carefully since they directly affect what we will do in the next parts of this tutorial.

**Ensure the options are configured the same as in the preceding figure**, and click the *Create* button.

That's all, ABP Studio creates the new microservice and arranges all the integration and configuration for you.

## Exploring the New Microservice

The new microservice is added under the `services` folder in the `CloudCrm` solution:

![abp-studio-new-catalog-service-in-solution-explorer](images/abp-studio-new-catalog-service-in-solution-explorer.png)

The new microservice has its own separate .NET solution that includes three .NET projects:

* `CloudCrm.Catalog` is the main project that you will implement your service. It typically contains your [entities](../../framework/architecture/domain-driven-design/entities.md), [repositories](../../framework/architecture/domain-driven-design/repositories.md), [application services](../../framework/architecture/domain-driven-design/application-services.md), API controllers, etc.
* `CloudCrm.Catalog.Contracts` project can be shared with the other services and applications. It typically contains interfaces of your [application services](../../framework/architecture/domain-driven-design/application-services.md), [data transfer objects](../../framework/architecture/domain-driven-design/data-transfer-objects.md), and some other types you may want to share with the clients of this microservice.
* `CloudCrm.Catalog.Tests` is for building your unit and integration tests for this microservice.

f