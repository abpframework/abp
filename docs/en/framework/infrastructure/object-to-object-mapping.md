```json
//[doc-seo]
{
    "Description": "Learn how to simplify object-to-object mapping in ABP Framework, reducing repetitive code and enhancing efficiency in your applications."
}
```

# Object To Object Mapping

It's common to map an object to another similar object. It's also tedious and repetitive since generally both classes have the same or similar properties mapped to each other. Imagine a typical [application service](../architecture/domain-driven-design/application-services.md) method below:

```csharp
public class UserAppService : ApplicationService
{
    private readonly IRepository<User, Guid> _userRepository;

    public UserAppService(IRepository<User, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateUser(CreateUserInput input)
    {
        //Manually creating a User object from the CreateUserInput object
        var user = new User
        {
            Name = input.Name,
            Surname = input.Surname,
            EmailAddress = input.EmailAddress,
            Password = input.Password
        };

        await _userRepository.InsertAsync(user);
    }
}
```

`CreateUserInput` is a simple [DTO](../architecture/domain-driven-design/data-transfer-objects.md) class and the `User` is a simple [entity](../architecture/domain-driven-design/entities.md). The code above creates a `User` entity from the given input object. The `User` entity will have more properties in a real-world application and manually creating it will become tedious and error-prone. You also have to change the mapping code when you add new properties to `User` and `CreateUserInput` classes.

We can use a library to automatically handle these kind of mappings. ABP provides abstractions for object to object mapping and has an integration package to use [AutoMapper](http://automapper.org/) as the object mapper. 

## IObjectMapper

`IObjectMapper` interface (in the [Volo.Abp.ObjectMapping](https://www.nuget.org/packages/Volo.Abp.ObjectMapping) package) defines a simple `Map` method. The example code introduced before can be re-written as shown below:

````csharp
public class UserAppService : ApplicationService
{
    private readonly IRepository<User, Guid> _userRepository;

    public UserAppService(IRepository<User, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateUser(CreateUserInput input)
    {
        //Automatically creating a new User object using the CreateUserInput object
        var user = ObjectMapper.Map<CreateUserInput, User>(input);

        await _userRepository.InsertAsync(user);
    }
}
````

> `ObjectMapper` is defined in the `ApplicationService` base class in this example. You can directly inject the `IObjectMapper` interface when you need it somewhere else.

Map method has two generic argument: First one is the source object type while the second one is the destination object type.

If you need to set properties of an existing object, you can use the second overload of the `Map` method:

````csharp
public class UserAppService : ApplicationService
{
    private readonly IRepository<User, Guid> _userRepository;

    public UserAppService(IRepository<User, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task UpdateUserAsync(Guid id, UpdateUserInput input)
    {
        var user = await _userRepository.GetAsync(id);

        //Automatically set properties of the user object using the UpdateUserInput
        ObjectMapper.Map<UpdateUserInput, User>(input, user);

        await _userRepository.UpdateAsync(user);
    }
}
````

You should have defined the mappings before to be able to map objects. See the AutoMapper/Mapperly integration section to learn how to define mappings.

## AutoMapper Integration

[AutoMapper](http://automapper.org/) is one of the most popular object to object mapping libraries. [Volo.Abp.AutoMapper](https://www.nuget.org/packages/Volo.Abp.AutoMapper) package defines the AutoMapper integration for the `IObjectMapper`.

Once you define mappings described as below, you can use the `IObjectMapper` interface just like explained before.

### Define Mappings

AutoMapper provides multiple ways of defining mapping between classes. Refer to [its own documentation](https://docs.automapper.org) for all details.

One way to define object mappings is creating a [Profile](https://docs.automapper.org/en/stable/Configuration.html#profile-instances) class. Example:

````csharp
public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<User, UserDto>();
    }
}
````

You should then register profiles using the `AbpAutoMapperOptions`:

````csharp
[DependsOn(typeof(AbpAutoMapperModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            //Add all mappings defined in the assembly of the MyModule class
            options.AddMaps<MyModule>();
        });
    }
}
````

`AddMaps` registers all profile classes defined in the assembly of the given class, typically your module class. It also registers for the [attribute mapping](https://docs.automapper.org/en/stable/Attribute-mapping.html).

### Configuration Validation

`AddMaps` optionally takes a `bool` parameter to control the [configuration validation](https://docs.automapper.org/en/stable/Configuration-validation.html) for your [module](../architecture/modularity/basics.md):

````csharp
options.AddMaps<MyModule>(validate: true);
````

While this option is `false` by default, it is suggested to enable configuration validation as a best practice.

Configuration validation can be controlled per profile class using `AddProfile` instead of `AddMaps`:

````csharp
options.AddProfile<MyProfile>(validate: true);
````

> If you have multiple profiles and need to enable validation only for a few of them, first use `AddMaps` without validation, then use `AddProfile` for each profile you want to validate.

### Mapping the Object Extensions

[Object extension system](../fundamentals/object-extensions.md) allows to define extra properties for existing classes. ABP provides a mapping definition extension to properly map extra properties of two objects.

````csharp
public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<User, UserDto>()
            .MapExtraProperties();
    }
}
````

It is suggested to use the `MapExtraProperties()` method if both classes are extensible objects (implement the `IHasExtraProperties` interface). See the [object extension document](../fundamentals/object-extensions.md) for more.

### Other Useful Extension Methods

There are some more extension methods those can simplify your mapping code.

#### Ignoring Audit Properties

It is common to ignore audit properties when you map an object to another.

Assume that you need to map a `ProductDto` ([DTO](../architecture/domain-driven-design/data-transfer-objects.md)) to a `Product` [entity](../architecture/domain-driven-design/entities.md) and the entity is inheriting from the `AuditedEntity` class (which provides properties like `CreationTime`, `CreatorId`, `IHasModificationTime`... etc).

You probably want to ignore these base properties while mapping from the DTO. You can use `IgnoreAuditedObjectProperties()` method to ignore all audit properties (instead of manually ignoring them one by one):

````csharp
public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<ProductDto, Product>()
            .IgnoreAuditedObjectProperties();
    }
}
````

There are more extension methods like `IgnoreFullAuditedObjectProperties()` and `IgnoreCreationAuditedObjectProperties()` those can be used based on your entity type.

> See the "*Base Classes & Interfaces for Audit Properties*" section in the [entities document](../architecture/domain-driven-design/entities.md) to know more about auditing properties.

#### Ignoring Other Properties

In AutoMapper, you typically write such a mapping code to ignore a property:

````csharp
public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<SimpleClass1, SimpleClass2>()
            .ForMember(x => x.CreationTime, map => map.Ignore());
    }
}
````

We found it unnecessarily long and created the `Ignore()` extension method:

````csharp
public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<SimpleClass1, SimpleClass2>()
            .Ignore(x => x.CreationTime);
    }
}
````

## Mapperly Integration

[Mapperly](https://github.com/riok/mapperly) is a .NET source generator for generating object mappings. [Volo.Abp.Mapperly](https://www.nuget.org/packages/Volo.Abp.Mapperly) package defines the Mapperly integration for the `IObjectMapper`.

Once you define mappings class as below, you can use the `IObjectMapper` interface just like explained before.

### Define Mapping Classes

You can define a mapper class by using the `Mapper` attribute. The class and methods must be `partial` to allow the Mapperly to generate the implementation during the build process:

````csharp
[Mapper]
public partial class UserToUserDtoMapper : MapperBase<User, UserDto>
{
    public override partial UserDto Map(User source);
    public override partial void Map(User source, UserDto destination);
}
````

If you also want to map `UserDto` to `User`, you can inherit from the `TwoWayMapperBase<User, UserDto>` class:

````csharp
[Mapper]
public partial class UserToUserDtoMapper : TwoWayMapperBase<User, UserDto>
{
    public override partial UserDto Map(User source);
    public override partial void Map(User source, UserDto destination);

    public override partial User ReverseMap(UserDto destination);
    public override partial void ReverseMap(UserDto destination, User source);
}
````

### Before and After Mapping Methods

The base class provides `BeforeMap` and `AfterMap` methods that can be overridden to perform actions before and after the mapping:

````csharp
[Mapper]
public partial class UserToUserDtoMapper : TwoWayMapperBase<User, UserDto>
{
    public override partial UserDto Map(User source);
    public override partial void Map(User source, UserDto destination);

    public override partial void BeforeMap(User source)
    {
        //TODO: Perform actions before the mapping
    }

    public override partial void AfterMap(User source, UserDto destination)
    {
        //TODO: Perform actions after the mapping
    }

    public override partial User ReverseMap(UserDto destination);
    public override partial void ReverseMap(UserDto destination, User source);

    public override partial void BeforeReverseMap(UserDto destination)
    {
        //TODO: Perform actions before the reverse mapping
    }

    public override partial void AfterReverseMap(UserDto destination, User source)
    {
        //TODO: Perform actions after the reverse mapping
    }
}
````

### Mapping the Object Extensions

[Object extension system](../fundamentals/object-extensions.md) allows to define extra properties for existing classes. ABP provides a mapping definition extension to properly map extra properties of two objects:

````csharp
[Mapper]
[MapExtraProperties]
public partial class UserToUserDtoMapper : MapperBase<User, UserDto>
{
    public override partial UserDto Map(User source);
    public override partial void Map(User source, UserDto destination);
}
````

It is suggested to use the `MapExtraPropertiesAttribute` attribute if both classes are extensible objects (implement the `IHasExtraProperties` interface). See the [object extension document](../fundamentals/object-extensions.md) for more.

### Property Setter Method

Mapperly requires that properties of both source and destination objects have `setter` methods. Otherwise, the property will be ignored. You can use `protected set` or `private set` to control the visibility of the `setter` method, but each property must have a `setter` method.

### Deep Cloning

By default, Mapperly does not create deep copies of objects to improve performance. If an object can be directly assigned to the target, it will do so (e.g., if the source and target type are both `List<T>`, the list and its entries will not be cloned). To create deep copies, set the `UseDeepCloning` property on the `MapperAttribute` to `true`.

````csharp
[Mapper(UseDeepCloning = true)]
public partial class UserToUserDtoMapper : MapperBase<User, UserDto>
{
    public override partial UserDto Map(User source);
    public override partial void Map(User source, UserDto destination);
}
````

### Lists and Arrays Support

ABP Mapperly integration also supports mapping lists and arrays as explained in the [IObjectMapper<TSource, TDestination> Interface](#iobjectmappertsource-tdestination-interface) section. 

**Example**:

````csharp
[Mapper]
public partial class UserToUserDtoMapper : MapperBase<User, UserDto>
{
    public override partial UserDto Map(User source);
    public override partial void Map(User source, UserDto destination);
}

var users = await _userRepository.GetListAsync(); // returns List<User>
var dtos = ObjectMapper.Map<List<User>, List<UserDto>>(users); // creates List<UserDto>
````

> When mapping a collection property, if the source value is null Mapperly will keep the destination value as null. This is different from AutoMapper, which will map the destination field to an empty collection.

### Nested Mapping

When working with nested object mapping, there's an important limitation to be aware of. If you have separate mappers for nested types like in the example below, the parent mapper (`SourceTypeToDestinationTypeMapper`) will not automatically use the nested mapper (`SourceNestedTypeToDestinationNestedTypeMapper`) to handle the mapping of nested properties. This means that configurations like the `MapperIgnoreTarget` attribute on the nested mapper will be ignored during the parent mapping operation.

````csharp
public class SourceNestedType
{
    public string Name { get; set; }

    public string Ignored { get; set; }
}

public class SourceType
{
    public string Name { get; set; }

    public SourceNestedType Nested { get; set; }
}

public class DestinationNestedType
{
    public string Name { get; set; }

    public string Ignored { get; set; }
}

public class DestinationType
{
    public string Name { get; set; }

    public DestinationNestedType Nested { get; set; }
}

[Mapper]
public partial class SourceTypeToDestinationTypeMapper : MapperBase<SourceType, DestinationType>
{
    public override partial DestinationType Map(SourceType source);
    public override partial void Map(SourceType source, DestinationType destination);
}

[Mapper]
public partial class SourceNestedTypeToDestinationNestedTypeMapper : MapperBase<SourceNestedType, DestinationNestedType>
{
    [MapperIgnoreTarget(nameof(SourceNestedType.Ignored))]
    public override partial DestinationNestedType Map(SourceNestedType source);

    [MapperIgnoreTarget(nameof(SourceNestedType.Ignored))]
    public override partial void Map(SourceNestedType source, DestinationNestedType destination);
}
````

There are several ways to solve this nested mapping issue. Choose the approach that best fits your specific requirements:

#### Solution 1: Multi-Interface Implementation

Implement both mapping interfaces (`IAbpMapperlyMapper<SourceType, DestinationType>` and `IAbpMapperlyMapper<SourceNestedType, DestinationNestedType>`) in a single mapper class. This approach consolidates all related mapping logic into one class.

**Important:** Remember to implement `ITransientDependency` to register the mapper class with the dependency injection container.

````csharp
[Mapper]
public partial class SourceTypeToDestinationTypeMapper : IAbpMapperlyMapper<SourceType, DestinationType>, IAbpMapperlyMapper<SourceNestedType, DestinationNestedType>, ITransientDependency
{
    public partial DestinationType Map(SourceType source);
    public partial void Map(SourceType source, DestinationType destination);
    public void BeforeMap(SourceType source)
    {
    }

    public void AfterMap(SourceType source, DestinationType destination)
    {
    }

    [MapperIgnoreTarget(nameof(SourceNestedType.Ignored))]
    public partial DestinationNestedType Map(SourceNestedType source);

    [MapperIgnoreTarget(nameof(SourceNestedType.Ignored))]
    public partial void Map(SourceNestedType source, DestinationNestedType destination);

    public void BeforeMap(SourceNestedType source)
    {
    }

    public void AfterMap(SourceNestedType source, DestinationNestedType destination)
    {
    }
}
````

#### Solution 2: Consolidate Mapping Methods

Copy the nested mapping methods from `SourceNestedTypeToDestinationNestedTypeMapper` to the parent `SourceTypeToDestinationTypeMapper` class. This ensures all mapping logic is contained within a single mapper.

Example:

````csharp
[Mapper]
public partial class SourceTypeToDestinationTypeMapper : MapperBase<SourceType, DestinationType>
{
    public override partial DestinationType Map(SourceType source);
    public override partial void Map(SourceType source, DestinationType destination);

    [MapperIgnoreTarget(nameof(SourceNestedType.Ignored))]
    public override partial DestinationNestedType Map(SourceNestedType source);
    [MapperIgnoreTarget(nameof(SourceNestedType.Ignored))]
    public override partial void Map(SourceNestedType source, DestinationNestedType destination);
}

[Mapper]
public partial class SourceNestedTypeToDestinationNestedTypeMapper : MapperBase<SourceNestedType, DestinationNestedType>
{
    [MapperIgnoreTarget(nameof(SourceNestedType.Ignored))]
    public override partial DestinationNestedType Map(SourceNestedType source);

    [MapperIgnoreTarget(nameof(SourceNestedType.Ignored))]
    public override partial void Map(SourceNestedType source, DestinationNestedType destination);
}
````

#### Solution 3: Dependency Injection Approach

Inject the nested mapper as a dependency into the parent mapper and use it in the `AfterMap` method to handle nested object mapping manually.

Example:

````csharp
[Mapper]
public partial class SourceTypeToDestinationTypeMapper : MapperBase<SourceType, DestinationType>
{
    private readonly SourceNestedTypeToDestinationNestedTypeMapper _sourceNestedTypeToDestinationNestedTypeMapper;

    public SourceTypeToDestinationTypeMapper(SourceNestedTypeToDestinationNestedTypeMapper sourceNestedTypeToDestinationNestedTypeMapper)
    {
        _sourceNestedTypeToDestinationNestedTypeMapper = sourceNestedTypeToDestinationNestedTypeMapper;
    }

    public override partial DestinationType Map(SourceType source);
    public override partial void Map(SourceType source, DestinationType destination);

    public override void AfterMap(SourceType source, DestinationType destination)
    {
        if (source.Nested != null)
        {
            destination.Nested = _sourceNestedTypeToDestinationNestedTypeMapper.Map(source.Nested);
        }
    }
}
````

#### Choosing the Right Solution

Each solution has its own advantages:

- **Solution 1** consolidates all mapping logic in one place and works well when mappings are tightly related.
- **Solution 2** is simple but can lead to code duplication if you need the nested mapper elsewhere.
- **Solution 3** maintains separation of concerns and reusability but requires manual mapping in the `AfterMap` method.

Choose the approach that best aligns with your application's architecture and maintainability requirements.

### More Mapperly Features

Most of Mapperly's features such as `Ignore` can be configured through its attributes. See the [Mapperly documentation](https://mapperly.riok.app/docs/intro/) for more details.

## Advanced Topics

### IObjectMapper<TContext> Interface

Assume that you have created a **reusable module** which defines AutoMapper/Mapperly profiles and uses `IObjectMapper` when it needs to map objects. Your module then can be used in different applications, by nature of the [modularity](../architecture/modularity/basics.md).

`IObjectMapper` is an abstraction and can be replaced by the final application to use another mapping library. The problem here that your reusable module is designed to use the AutoMapper/Mapperly library, because it only defines mappings for it. In such a case, you will want to guarantee that your module always uses AutoMapper/Mapperly even if the final application uses another default object mapping library.

`IObjectMapper<TContext>` is used to contextualize the object mapper, so you can use different libraries for different modules/contexts.

Example usage:

````csharp
public class UserAppService : ApplicationService
{
    private readonly IRepository<User, Guid> _userRepository;   
    
    private readonly IObjectMapper<MyModule> _objectMapper;

    public UserAppService(
        IRepository<User, Guid> userRepository, 
        IObjectMapper<MyModule> objectMapper) //Inject module specific mapper
    {
        _userRepository = userRepository;
        _objectMapper = objectMapper;
    }

    public async Task CreateUserAsync(CreateUserInput input)
    {
        //Use the module specific mapper
        var user = _objectMapper.Map<CreateUserInput, User>(input);

        await _userRepository.InsertAsync(user);
    }
}
````

`UserAppService` injects the `IObjectMapper<MyModule>`, the specific object mapper for this module. It's usage is exactly same of the `IObjectMapper`.

The example code above don't use the `ObjectMapper` property defined in the `ApplicationService`, but injects the `IObjectMapper<MyModule>`. However, it is still possible to use the base property since the `ApplicationService` defines an `ObjectMapperContext` property that can be set in the class constructor. So, the example about can be re-written as like below:

````csharp
public class UserAppService : ApplicationService
{
    private readonly IRepository<User, Guid> _userRepository;

    public UserAppService(IRepository<User, Guid> userRepository)
    {
        _userRepository = userRepository;
        //Set the object mapper context
        ObjectMapperContext = typeof(MyModule);
    }

    public async Task CreateUserAsync(CreateUserInput input)
    {
        var user = ObjectMapper.Map<CreateUserInput, User>(input);

        await _userRepository.InsertAsync(user);
    }
}
````

While using the contextualized object mapper is same as the normal object mapper, you should register the contextualized mapper in your module's `ConfigureServices` method:

When using AutoMapper:

````csharp
[DependsOn(typeof(AbpAutoMapperModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Use AutoMapper for MyModule
        context.Services.AddAutoMapperObjectMapper<MyModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<MyModule>(validate: true);
        });
    }
}
````

When using Mapperly:

````csharp
[DependsOn(typeof(AbpMapperlyModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Use Mapperly for MyModule
        context.Services.AddMapperlyObjectMapper<MyModule>();
    }
}
````

`IObjectMapper<MyModule>` is an essential feature for a reusable module where it can be used in multiple applications each may use a different library for object to object mapping. All pre-built ABP modules are using it. But, for the final application, you can ignore this interface and always use the default `IObjectMapper` interface.

### IObjectMapper<TSource, TDestination> Interface

ABP allows you to customize the mapping code for specific classes. Assume that you want to create a custom class to map from `User` to `UserDto`. In this case, you can create a class that implements the `IObjectMapper<User, UserDto>`:

````csharp
public class MyCustomUserMapper : IObjectMapper<User, UserDto>, ITransientDependency
{
    public UserDto Map(User source)
    {
        //TODO: Create a new UserDto
    }

    public UserDto Map(User source, UserDto destination)
    {
        //TODO: Set properties of an existing UserDto
        return destination;
    }
}
````

ABP automatically discovers and registers the `MyCustomUserMapper` and it is automatically used whenever you use the `IObjectMapper` to map `User` to `UserDto`. A single class may implement more than one `IObjectMapper<TSource, TDestination>` each for a different object pairs.

> This approach is powerful since `MyCustomUserMapper` can inject any other service and use in the `Map` methods.

Once you implement `IObjectMapper<User, UserDto>`, ABP can automatically convert a collection of `User` objects to a collection of `UserDto` objects. The following generic collection types are supported:

* `IEnumerable<T>`
* `ICollection<T>`
* `Collection<T>`
* `IList<T>`
* `List<T>`
* `T[]` (array)

**Example:**

````csharp
var users = await _userRepository.GetListAsync(); // returns List<User>
var dtos = ObjectMapper.Map<List<User>, List<UserDto>>(users); // creates List<UserDto>
````
