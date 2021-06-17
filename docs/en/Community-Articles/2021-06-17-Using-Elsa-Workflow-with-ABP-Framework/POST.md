# Using Elsa Workflow with ABP Framework

**Elsa Core** is an open-source workflows library that can be used in any kind of .NET Core application. Using such a workflow library can be useful to implement a business rules visually or programmatically.

![elsa-overview](./elsa-overview.gif)

This article shows how we can use this workflow library within our ABP-based application. We will start with a couple of examples and then we will integrate the **Elsa Dashboard** (you can see it in the above gif) into our application to be able to design our workflows visually.

## Source Code

You can find source code of the sample used in this article [here](https://github.com/abpframework/abp-samples/tree/elsa-demo).

## Create the Project

In this article, I will create a new startup template with EF Core as a database provider and MVC/Razor-Pages for UI framework.

> If you already have a project with MVC/Razor-Pages or Blazor UI, you don't need to create a new startup template, you can directly implement the following steps to your existing project. In other words you can skip this section.

* Before starting to the development, we will create a new solution named `ElsaDemo` (or whatever you want). We will create a new startup template with EF Core as a database provider and MVC/Razor-Pages for UI framework by using the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI):

```bash
abp new ElsaDemo
```

* Our project boilerplate will be ready after the download is finished. Then, we can open the solution in the Visual Studio (or any other IDE).

* First of all we need to create database migrations and apply these migrations to our database. So, open your terminal in the `ElsaDemo.EntityFrameworkCore.DbMigrations` project directory and run the following command to create the initial migration.

```bash
dotnet ef migrations add Initial
```

* After the initial migration created, we can run the `ElsaDemo.DbMigrator` project to apply migration into our database and seed initial data.

* After database and initial data created, we can run the `ElsaDemo.Web` to see our UI working properly.

> Default admin username is **admin** and password is **1q2w3E***

## Creating First Workflow (Hello World to Workflows)

We can start with creating our first workflow. Let's start with creating basic hello-world workflow.

### Install Packages

We need to install two packages: `Elsa` and `Elsa.Activities.Console` into our `ElsaDemo.Web` project. You can add these two packages with the following command:

```bash
dotnet add package Elsa
dotnet add package Elsa.Activities.Console
```

* After the packages installed, we can define our first workflow. To do this, create a folder named **Workflows** and in this folder create a class named `HelloWorld`.

```csharp
using Elsa.Activities.Console;
using Elsa.Builders;

namespace ElsaDemo.Web.Workflows
{
    public class HelloWorld : IWorkflow
    {
        public void Build(IWorkflowBuilder builder) => builder.WriteLine("Hello World from Elsa!");
    }
}
```

* In here we've basically implemented the `IWorkflow` interface which only has one method named **Build**. In this method, we can define our workflow's executing steps (activities). 

* As you can see in the example above, we've used a activity named **WriteLine**, which writes a line of text to the console. We could also chain the activities one to another.

> "An activity is an atomic building block that represents a single executable step on the workflow." - [Elsa Core Definition](https://elsa-workflows.github.io/elsa-core/docs/next/concepts/concepts-workflows#activity)

* After defining our workflow, we need to register it. To do that, open your `ElsaDemoWebModule` class and update your `ElsaDemoWebModule` with the following lines. Most of the codes are abbreviated for simplicity.

```csharp
using ElsaDemo.Web.Workflows;
using Elsa.Services;

public override void ConfigureServices(ServiceConfigurationContext context)
{
    var hostingEnvironment = context.Services.GetHostingEnvironment();
    var configuration = context.Services.GetConfiguration();

    //...

    ConfigureElsa(context); //add this line for Elsa's service registrations
}

private void ConfigureElsa(ServiceConfigurationContext context)
{
    context.Services.AddElsa(options =>
    {
        options
            .AddConsoleActivities()
            .AddWorkflow<HelloWorld>();
    });
}

public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    //...

    var workflowRunner = context.ServiceProvider.GetRequiredService<IBuildsAndStartsWorkflow>();
    workflowRunner.BuildAndStartWorkflowAsync<HelloWorld>();
}
```

* Here we basically, configured Elsa's services in our `ConfigureServices` method and after that in our `OnApplicationInitialization` method we get workflow runner (**IBuildsAndStartsWorkflow**) and started the our `HelloWorld` workflow.

* If we run the application and examine the console outputs, we should see the message that we defined in our workflow.

![hello-world-workflow](./hello-world-workflow.jpg)

