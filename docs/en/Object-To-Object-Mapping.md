# Object To Object Mapping

It's common to map an object to another similar object. It's also tedious and repetitive since generally both classes have the same or similar properties mapped to each other. Imagine a typical [application service](Application-Services.md) method below:

```csharp
public class UserAppService : ApplicationService
{
    private readonly IRepository<User, Guid> _userRepository;

    public UserAppService(IRepository<User, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    public void CreateUser(CreateUserInput input)
    {
        //Manually creating a User object from the CreateUserInput object
        var user = new User
        {
            Name = input.Name,
            Surname = input.Surname,
            EmailAddress = input.EmailAddress,
            Password = input.Password
        };

        _userRepository.Insert(user);
    }
}
```

`CreateUserInput ` is a simple [DTO](Data-Transfer-Objects.md) class and the `User` is a simple [entity](Entities.md). The code above creates a `User` entity from the given input object. The `User` entity will have more properties in a real-world application and manually creating it will become tedious and error-prone. You also have to change the mapping code when you add new properties to `User` and `CreateUserInput` classes.

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

    public void CreateUser(CreateUserInput input)
    {
        //Automatically creating a new User object using the CreateUserInput object
        var user = ObjectMapper.Map<CreateUserInput, User>(input);

        _userRepository.Insert(user);
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

You should have defined the mappings before to be able to map objects. See the AutoMapper integration section to learn how to define mappings.

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

`AddMaps` optionally takes a `bool` parameter to control the [configuration validation](https://docs.automapper.org/en/stable/Configuration-validation.html) for your [module](Module-Development-Basics.md):

````csharp
options.AddMaps<MyModule>(validate: true);
````

While this option is `false` by default, it is suggested to enable configuration validation as a best practice.

Configuration validation can be controlled per profile class using `AddProfile` instead of `AddMaps`:

````csharp
options.AddProfile<MyProfile>(validate: true);
````

> If you have multiple profiles and need to enable validation only for a few of them, first use `AddMaps` without validation, then use `AddProfile` for each profile you want to validate.

## Advanced Topics

### IObjectMapper<TContext> Interface

Assume that you have created a **reusable module** which defines AutoMapper profiles and uses `IObjectMapper` when it needs to map objects. Your module then can be used in different applications, by nature of the [modularity](Module-Development-Basics.md).

`IObjectMapper` is an abstraction and can be replaced by the final application to use another mapping library. The problem here that your reusable module is designed to use the AutoMapper library, because it only defines mappings for it. In such a case, you will want to guarantee that your module always uses AutoMapper even if the final application uses another default object mapping library.

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

`UserAppService ` injects the `IObjectMapper<MyModule>`, the specific object mapper for this module. It's usage is exactly same of the `IObjectMapper`.

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

ABP automatically discovers and registers the `MyCustomUserMapper` and it is automatically used whenever you use the `IObjectMapper` to map `User` to `UserDto`.

A single class may implement more than one `IObjectMapper<TSource, TDestination>` each for a different object pairs.

> This approach is powerful since `MyCustomUserMapper` can inject any other service and use in the `Map` methods.

