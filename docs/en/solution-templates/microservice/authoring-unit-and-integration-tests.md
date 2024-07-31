# Microservice Solution: Authoring Unit and Integration Tests

> You must have an ABP Business or a higher license to be able to create a microservice solution.

Unit and integration tests are essential for ensuring the quality and reliability of your microservice system. In this document, you will learn how to write and run tests for your microservices. Each microservice has its own test projects to validate its functionality and interactions with other services. There are four main types of tests: `Entities`, `Repositories`, `Application Services`, and `Controllers`.

## Prepare the Test Environment

Before writing tests, you need to set up the test environment. Microservice solution templates include `MicroServiceNameTestsModule` and `MicroServiceNameTestBase` classes to help you create test classes and run tests. In the module class, you can seed test data, configure the database, and set up the test environment. Test modules are used to register services and configure the test environment, registering the microservice module as [AdditionalAssembly](../../framework/architecture/modularity/basics.md#additional-module-assemblies) to the test module. It doesn't depend on the actual microservice module, because the microservice module has configurations proper for development and production environments. For instance, it uses Redis for distributed caching and RabbitMQ for the distributed event bus. However, in the test environment, it's sufficient to use in-memory cache and in-memory event bus. The base class provides helper methods to access services and perform common test operations. Test projects use the [xUnit](https://xunit.net/) and [Shouldly](https://github.com/shouldly/shouldly) packages for writing and running tests.

## Unit Tests

Unit tests are used to test individual units of code to ensure that they work as expected. In a microservice system, a unit can be a method, a class, or a service. The goal of unit tests is to validate that each unit of the software performs as designed.

### Writing Unit Tests

You can run unit tests for your microservice entities to test the domain logic. You can also write unit tests for your application services to test the business logic. However, if you have complex business logic, you may need to write integration tests to test the interaction between different parts of the system. Otherwise, you need to mock the dependencies of the application service to write unit tests.

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

In this example, the `PermissionDefinitionRecord_Tests` class tests the `PermissionDefinitionRecord` entity by calling its `Patch` method. The test method `Should_Change_Name` arranges the test environment, calls the method, and asserts the result. Since it's a unit test, we don't need to inherit from *TestBase* classes.

## Integration Tests

Integration tests are used to test the interaction between different parts of the system to ensure that they work together correctly. You can write integration tests for your repositories, application services, controllers, and other components to validate their interactions.

### Writing Integration Tests

Microservice templates provide a test base class for writing integration tests. You can use the test base class to access services and perform common test operations.

Here is an example of an integration test for a repository:

```csharp
public class PermissionGrantRepository_Tests : AdministrationServiceIntegrationTestBase
{
    private readonly IPermissionGrantRepository _permissionGrantRepository;

    public PermissionGrantRepository_Tests()
    {
        _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
    }
    
    [Fact]
    public async Task Should_Get_Permissions_By_Role_Name()
    {
        var permissionGrants = await _permissionGrantRepository.GetListAsync(RolePermissionValueProvider.ProviderName, "admin");
        
        permissionGrants.ShouldNotBeNull();
        permissionGrants.Count.ShouldBeGreaterThanOrEqualTo(1);
    }
}
```

In this example, the `PermissionGrantRepository_Tests` class tests the `Should_Get_Permissions_By_Role_Name` method of the `PermissionGrantRepository` by calling its `GetListAsync` method. To access the services, the test class inherits from the `AdministrationServiceIntegrationTestBase` class and uses the `GetRequiredService` method to get the service implementation. Since the administration microservice test module is seeded with test data on application startup, we can directly try to get the admin role's permissions.

Similarly, you can write Application Service test classes to test the business logic of the application services:

```csharp
public class PermissionAppService_Tests : AdministrationServiceIntegrationTestBase
{
    private readonly IPermissionAppService _permissionAppService;

    public PermissionAppService_Tests()
    {
        _permissionAppService = GetRequiredService<IPermissionAppService>();
    }

    [Fact]
    public async Task Should_Get_Permissions()
    {
        var permissions= await _permissionAppService.GetAsync(RolePermissionValueProvider.ProviderName, "admin");
      
        permissions.ShouldNotBeNull();
        permissions.EntityDisplayName.ShouldBe("admin");
        permissions.Groups.Count.ShouldBeGreaterThanOrEqualTo(1);
        permissions.Groups.SelectMany(x => x.Permissions).Count().ShouldBeGreaterThanOrEqualTo(1);
    }
}
```

In this example, `PermissionAppService_Tests` class tests the `Should_Get_Permissions` method of the `PermissionAppService` by calling its `GetAsync` method. The test class inherits from the `AdministrationServiceIntegrationTestBase` class to access the services.

To test the controllers, you can write test classes for the controllers and test the actions:

```csharp
public class DemoController_Tests : AdministrationServiceIntegrationTestBase
{
    [Fact]
    public async Task HelloWorld()
    {
        var response = await GetResponseAsStringAsync("/api/administration/demo/hello");
        response.ShouldBe("Hello World!");
    }
}
```

In this example, `DemoController_Tests` class tests the `HelloWorld` action of the `DemoController` by calling the `GetResponseAsStringAsync` method to get the response content. The test class inherits from the `AdministrationServiceIntegrationTestBase` class to access the services and helper methods such as `GetResponseAsStringAsync`.

## Running Tests

You can run your unit or integration tests using the Visual Studio Test Explorer or the `dotnet test` command. The `dotnet test` command runs all the tests in the solution.

```bash
dotnet test
```