# Automated Testing

````json
//[doc-nav]
{
  "Next": {
    "Name": "Unit tests",
    "Path": "testing/unit-tests"
  }
}
````

## Introduction

ABP has been designed with testability in mind. There are some different levels of automated testing;

* **Unit Tests**: You typically test a single class (or a very few classes together). These tests will be fast. However, you generally need to deal with mocking for the dependencies of your service(s).
* **Integration Tests**: You typically test a service, but this time you don't mock the fundamental infrastructure and services to see if they properly working together.
* **UI Tests**: You test the UI of the application, just like the users interact with your application.

### Unit Tests vs Integration Tests

Integration tests have some significant **advantages** compared to unit tests;

* **Easier to write** since you don't work to establish mocking and dealing with the dependencies.
* Your test code runs with all the real services and infrastructure (including database mapping and queries), so it is much closer to the **real application test**.

While they have some drawbacks;

* They are **slower** compared to unit tests since all the infrastructure is prepared for each test case.
* A bug in a service may make multiple test cases broken, so it may be **harder to find the real problem** in some cases.

We suggest to go mixed: Write unit or integration test where it is necessary and you find effective to write and maintain it.

## The Application Startup Template

The [Application Startup Template](../solution-templates/layered-web-application/index.md) comes with the test infrastructure properly installed and configured for you.

### The Test Projects

See the following solution structure in the Visual Studio:

![solution-test-projects](../images/solution-test-projects.png)

There are more than one test project, organized by the layers;

* `Domain.Tests` is used to test your Domain Layer objects (like [Domain Services](../framework/architecture/best-practices/domain-services.md) and [Entities](../framework/architecture/domain-driven-design/entities.md)).
* `Application.Tests` is used to test your Application Layer (like [Application Services](../framework/architecture/domain-driven-design/application-services.md)).
* `EntityFrameworkCore.Tests` is used to implement abstract test classes and test your custom repository implementations or EF Core mappings (this project will be different if you use another [Database Provider](../framework/data/index.md)).
* `Web.Tests` is used to test the UI Layer (like Pages, Controllers and View Components). This project does only exist for MVC / Razor Page applications.
* `TestBase` contains some classes those are shared/used by the other projects.

> `HttpApi.Client.ConsoleTestApp` is not an automated test application. It is an example Console Application that shows how to consume your HTTP APIs from a .NET Console Application.

The following sections will introduce the base classes and other infrastructure included in these projects.

### The Test Infrastructure

The startup solution has the following libraries already installed;

* [xUnit](https://xunit.net/) as the test framework.
* [NSubstitute](https://nsubstitute.github.io/) as the mocking library.
* [Shouldly](https://github.com/shouldly/shouldly) as the assertion library.

While you are free to replace them with your favorite tools, this document and examples will be base on these tooling.

## The Test Explorer

You can use the Test Explorer to view and run the tests in Visual Studio. For other IDEs, see their own documentation.

### Open the Test Explorer

Open the *Test Explorer*, under the *Tests* menu, if it is not already open:

![vs-test-explorer](../images/vs-test-explorer.png)

### Run the Tests

Then you can click to the *Run All* or *Run* buttons to run the tests. The initial startup template has some sample tests for you: 

![vs-startup-template-tests](../images/vs-startup-template-tests.png)

### Run Tests In Parallel

The test infrastructure is compatible to run the tests in parallel. It is **strongly suggested** to run all the tests in parallel, which is pretty faster then running them one by one.

To enable it, click to the caret icon near to the settings (gear) button and select the *Run Tests In Parallel*.

![vs-run-tests-in-parallel](../images/vs-run-tests-in-parallel.png)

