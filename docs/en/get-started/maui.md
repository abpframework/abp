# MAUI Application Startup Template

This template is used to create a minimalist MAUI application project.

## How to Start With?

First, install the [ABP CLI](../cli) if you haven't installed before:

````bash
dotnet tool install -g Volo.Abp.Studio.Cli
````

Then use the `abp new` command in an empty folder to create a new solution:

````bash
abp new Acme.MyMauiApp -t maui
````

`Acme.MyMauiApp` is the solution name, like *YourCompany.YourProduct*. You can use single level, two-levels or three-levels naming.

## Solution Structure

After you use the above command to create a solution, you will have a solution like shown below:

![basic-maui-application-solution](../images/basic-maui-application-solution.png)

* `HelloWorldService` is a sample service that implements the `ITransientDependency` interface to register this service to the [dependency injection](../framework/fundamentals/dependency-injection.md) system.