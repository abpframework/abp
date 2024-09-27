# Microservice Tutorial Part 03: Building the Catalog Microservice

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Creating the initial Catalog Microservice",
    "Path": "tutorials/microservice/part-02"
  },
  "Next": {
    "Name": "Creating the initial Ordering Microservice",
    "Path": "tutorials/microservice/part-04"
  }
}
````

In the previous part, we've created a new microservice named Catalog. In this part, we will build functionality to create and manage products in our system.

In this part, we will use [ABP Suite](../../suite/index.md) to automatically create all the necessary code for us. So, you will see how to use ABP Suite in a microservice solution. We will do everything manually while we will create the Ordering microservice in next parts, so you will learn the details better. We suggest to use ABP Suite wherever it is possible, because it saves a lot of time. You can then investigate the changes done by ABP Suite to understand what it produced.

## Opening the ABP Suite

First of all, **stop all the applications** in ABP Studio's *Solution Runner* panel, because ABP Suite will make changes in the solution and it will also needs to build the solution in some steps. Running the solution prevents to build it.

Now, select the *ABP Suite* -> *Open* command on the main menu to open ABP Suite:

![abp-studio-open-abp-suite](images/abp-studio-open-abp-suite.png)

It will ask to you which module you want to use:

![abp-studio-open-abp-suite-select-module](images/abp-studio-open-abp-suite-select-module.png)

The `CloudCrm` microservice solution contains more than one .NET solution. Typically, each ABP Studio module represents a separate .NET solution (see the [concepts](../../studio/concepts.md) document).

ABP Suite works on a single .NET solution to generate code, and an ABP Studio module represents a separate .NET solution.

