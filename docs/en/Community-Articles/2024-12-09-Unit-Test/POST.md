# The new Unit Test structure in ABP application

A typical ABP modular project usually consists of three main projects: `Application`, `Domain`, and `EntityFrameworkCore/MongoDB`. In these projects, we may provide many services that require unit testing.

Using abstract unit test classes involves first writing tests in the `Application` and `Domain` layers that are independent of the storage technology, ensuring the correctness of core business logic. These abstract tests are then implemented in `EntityFrameworkCore` or `MongoDB`. The benefits of this approach include:

1. **Reduced Coupling**: Core logic tests do not depend on specific storage technologies, so switching databases does not require rewriting test code.
2. **Better Isolation**: Focuses on verifying business logic correctness, avoiding interference from database operations.
3. **Increased Reusability**: The same abstract tests can be reused with different storage implementations.
4. **Easier Maintenance and Extensibility**: Different storage implementations can be extended independently without breaking existing tests.
5. **Faster and More Reliable Tests**: Reduces dependency on databases, making tests faster and more stable.

## How to migrate old unit tests to the new unit test structure

Assume our project name is `MyCompanyName.MyProjectName`.

### Changes to the `MyCompanyName.MyProjectName.Application.Tests` project:

1. Remove the `MyCompanyName.MyProjectName.Application.Tests` project's `MyProjectNameApplicationCollection` class.
2. Modify the `MyCompanyName.MyProjectName.Application.Tests` project's `MyProjectNameApplicationTestBase` class.

```csharp
public abstract class MyProjectNameApplicationTestBase<TStartupModule> : MyProjectNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
	//...
}
```

3. Modify the `MyCompanyName.MyProjectName.Application.Tests` project's unit test classes to become abstract unit test classes, such as: `SampleAppServiceTests`.

```csharp
public abstract class SampleAppServiceTests<TStartupModule> : MyProjectNameApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    [Fact]
    public async Task Initial_Data_Should_Contain_Admin_User()
    {
        //...
    }
}
```

### Changes to the `MyCompanyName.MyProjectName.Domain.Tests` project:

1. Remove the `MyCompanyName.MyProjectName.Domain.Tests` project's `MyProjectNameDomainCollection` class.
2. Modify the `MyCompanyName.MyProjectName.Domain.Tests` project's `MyProjectNameDomainTestBase` class.

```csharp
public abstract class MyProjectNameDomainTestBase<TStartupModule> : MyProjectNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
	//...
}
```

3. Modify the `MyCompanyName.MyProjectName.Domain.Tests` project's unit test classes to become abstract unit test classes, such as: `SampleDomainTests`.

```csharp
public abstract class SampleDomainTests<TStartupModule> : MyProjectNameDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    [Fact]
    public async Task Should_Set_Email_Of_A_User()
    {
		//...
    }
}
```

4. Modify the `MyCompanyName.MyProjectName.Domain.Tests` project's `csproj` and module class. Remove references to `EntityFrameworkCore/MongoDB`.

`MyCompanyName.MyProjectName.Domain.Tests.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  //...

  <ItemGroup>
    <ProjectReference Include="..\..\src\MyCompanyName.MyProjectName.Domain\MyCompanyName.MyProjectName.Domain.csproj" />
    <ProjectReference Include="..\MyCompanyName.MyProjectName.TestBase\MyCompanyName.MyProjectName.TestBase.csproj" />
  </ItemGroup>
</Project>

```

`MyProjectNameDomainTestModule.cs`:

```csharp
[DependsOn(
    typeof(MyProjectNameDomainModule),
    typeof(MyProjectNameTestBaseModule)
)]
public class MyProjectNameDomainTestModule : AbpModule
{
	//...
}
```

### Changes to the `MyCompanyName.MyProjectName.EntityFrameworkCore.Tests` project:

Here, we need to create implementation classes for all abstract unit tests.

```csharp
[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<MyProjectNameEntityFrameworkCoreTestModule>
{
	//...
}
```

```csharp
[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<MyProjectNameEntityFrameworkCoreTestModule>
{
	//...
}
```

We also need to modify the project's dependencies and module class, which should directly or indirectly reference the `Application` and `Domain` test projects.

`MyCompanyName.MyProjectName.EntityFrameworkCore.Tests.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

   //...

  <ItemGroup>
    <ProjectReference Include="..\..\src\MyCompanyName.MyProjectName.EntityFrameworkCore\MyCompanyName.MyProjectName.EntityFrameworkCore.csproj" />
    <ProjectReference Include="..\MyCompanyName.MyProjectName.Application.Tests\MyCompanyName.MyProjectName.Application.Tests.csproj" />
    <ProjectReference Include="..\..\..\..\..\framework\src\Volo.Abp.EntityFrameworkCore.Sqlite\Volo.Abp.EntityFrameworkCore.Sqlite.csproj" />
  </ItemGroup>
</Project>
```

`MyProjectNameEntityFrameworkCoreTestModule.cs`:

```csharp
[DependsOn(
    typeof(MyProjectNameApplicationTestModule),
    typeof(MyProjectNameEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
public class MyProjectNameEntityFrameworkCoreTestModule : AbpModule
{
	//...
}
```

### Changes to the `MyCompanyName.MyProjectName.MongoDB.Tests` project (skip this step if not using MongoDB):

Like the `EntityFrameworkCore` project, we need to create implementation classes for all abstract unit tests and modify the project's dependencies and module class.

```csharp
[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class MongoDBSampleAppServiceTests : SampleAppServiceTests<MyProjectNameMongoDbTestModule>
{
	//...
}
```

```csharp
[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class MongoDBSampleDomainTests : SampleDomainTests<MyProjectNameMongoDbTestModule>
{
	//...
}
```

```xml
<Project Sdk="Microsoft.NET.Sdk">

  //...

  <ItemGroup>
    <ProjectReference Include="..\..\src\MyCompanyName.MyProjectName.MongoDB\MyCompanyName.MyProjectName.MongoDB.csproj" />
    <ProjectReference Include="..\MyCompanyName.MyProjectName.Application.Tests\MyCompanyName.MyProjectName.Application.Tests.csproj" />
  </ItemGroup>
</Project>
```

```csharp
[DependsOn(
    typeof(MyProjectNameApplicationTestModule),
    typeof(MyProjectNameMongoDbModule)
)]
public class MyProjectNameMongoDbTestModule : AbpModule
{
	//...
}
```

### Changes to the `MyCompanyName.MyProjectName.Web.Tests` project:

We need to reference the `EntityFrameworkCore/MongoDB` test projects in this test project.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  //...

  <ItemGroup>
    <ProjectReference Include="..\MyCompanyName.MyProjectName.Application.Tests\MyCompanyName.MyProjectName.Application.Tests.csproj" />
    <ProjectReference Include="..\..\src\MyCompanyName.MyProjectName.Web\MyCompanyName.MyProjectName.Web.csproj" />
    <ProjectReference Include="..\..\..\..\..\framework\src\Volo.Abp.AspNetCore.TestBase\Volo.Abp.AspNetCore.TestBase.csproj" />
    <ProjectReference Include="..\MyCompanyName.MyProjectName.EntityFrameworkCore.Tests\MyCompanyName.MyProjectName.EntityFrameworkCore.Tests.csproj" />
  </ItemGroup>
</Project>
```

```csharp
[DependsOn(
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(MyProjectNameWebModule),
    typeof(MyProjectNameApplicationTestModule),
    typeof(MyProjectNameEntityFrameworkCoreTestModule)
)]
public class MyProjectNameWebTestModule : AbpModule
{
	//...
}
```

We no longer need the `MyProjectNameWebCollection` class in this project. Please delete it and use `[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]` instead.

## Conclusion

This is our new unit test structure. Decoupling unit tests from storage technologies ensures the independence of business logic and allows easy switching between storage implementations. Abstract unit test classes improve test reusability, maintainability, and efficiency, reducing refactoring costs and providing flexibility for future tech updates.

## References

- [Unit Test](https://abp.io/docs/latest/testing/unit-tests)
- [Abstract all db-related unit tests](https://github.com/abpframework/abp/pull/17880)
