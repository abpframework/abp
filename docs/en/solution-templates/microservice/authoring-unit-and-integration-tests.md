# Microservice Solution: Authoring Unit and Integration Tests

Unit and integration tests are essential for ensuring the quality and reliability of your microservice system. In this document, you will learn how to write and run tests for your microservices. Each microservice has its own test projects to validate its functionality and interactions with other services. There are four main types of tests: `Entities`, `Repositories`, `Application Services`, and `Controllers`.

## Prepare the Test Environment

Before writing tests, you need to set up the test environment. Microservice solution templates include `MicroServiceNameTestsModule` and `MicroServiceNameTestBase` classes to help you create test classes and run tests. In the module class, you can seed test data, configure database, and set up the test environment. Test modules are used to register services and configure the test environment that's why it registers the microservice module as [AdditionalAssembly](../../framework/architecture/modularity/basics.md#additional-module-assemblies) to the test module. It doesn't depend on the actual module, because microservice module has configurations proper for development and production environments. Such as, it uses redis cache for distributed cache and rabbitmq for distributed event bus. But in the test environment, it's enough to use in-memory cache and in-memory event bus. The base class provides helper methods to access services and perform common test operations. 

## Unit Tests

Unit tests are used to test individual units of code to ensure that they work as expected. In a microservice system, a unit can be a method, a class, or a service. The goal of unit tests is to validate that each unit of the software performs as designed. 

### Writing Unit Tests

You can run unit tests for your microservice entities for testing the domain logic. You can also write unit tests for your application services to test the business logic however, if you have complex business logic, you may need to write integration tests to test the interaction between different parts of the system. Otherwise, you need to mock the dependencies of the application service to write unit tests.

Here is an example of a unit test for a `PermissionDefinitionRecord` entity:

```csharp
public class PermissionDefinitionRecord_Tests
{
    [Fact]
    public void Should_Change_Name()
    {
        // Arrange
        var permission = new PermissionDefinitionRecord(
            Guid.NewGuid(),
            "test",
            "test",
            null,
            "test"
        );
        permission.Name.ShouldBe("test");
        
        // Act
        permission.Patch(new PermissionDefinitionRecord(
            Guid.NewGuid(),
            "test",
            "test2",
            null,
            "test"));
        
        // Assert
        permission.Name.ShouldBe("test2");
    } 
}
```

In this example, `PermissionDefinitionRecord_Tests` class tests the `PermissionDefinitionRecord`entity by calling its `Patch` method. The test method `Should_Change_Name` arranges the test environment, calls the method, and asserts the result. Since it's a unit test we don't need take inheritance from *TestBase* classes.

### Running Unit Tests

You can run your unit tests using the Visual Studio Test Explorer or the `dotnet test` command. The `dotnet test` command runs all the tests in the solution.

```bash
dotnet test
```