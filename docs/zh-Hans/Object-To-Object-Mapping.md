# 对象到对象映射

将对象映射到另一个对象是常用并且繁琐重复的工作,大部分情况下两个类都具有相同或相似的属性. 例如下面的 [应用服务](Application-Services.md)方法:

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

`CreateUserInput` 和 `User` 是一个简单的[DTO](Data-Transfer-Objects.md)和[实体](Entities.md)类. 上面的代码使用input对象创建了一个 `User` 实体. 上面的代码很简单,但在实际应用程序中 `User` 实体会拥有很多属性,手动创建实体乏味且容易出错. `User` 和 `CreateUserInput` 添加新属性时还需要再去修改代码.

我们需要一个库自动处理类到类的映射. ABP提供了对象到对象映射的抽象并集成了[AutoMapper](http://automapper.org/)做为对象映射器.

## IObjectMapper

`IObjectMapper` 接口 (在 [Volo.Abp.ObjectMapping](https://www.nuget.org/packages/Volo.Abp.ObjectMapping)包中) 定义了一个简单的 `Map` 方法. 上面的手动映射示例可以用以下方式重写:

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

> 示例中的 `ObjectMapper` 属性在 `ApplicationService` 基类中属性注入. 在其他地方也可以直接注入 `IObjectMapper` 接口.

Map方法有两个泛型参数: 第一个是源对象类型,第二个是目标对象类型.

如果想要设置现有对象属性,可以使用 `Map` 的重载方法:

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

必须先定义映射,然后才能映射对象. 请参阅AutoMapper集成部分了解如何定义映射.

## AutoMapper 集成

[AutoMapper](http://automapper.org/) 是最流行的对象到对象映射库之一. [Volo.Abp.AutoMapper](https://www.nuget.org/packages/Volo.Abp.AutoMapper)程序包使用AutoMapper实现了 `IObjectMapper`.

定义了以下部分的映射后就可以使 `IObjectMapper` 接口.

### 定义映射

AutoMapper提供了多种定义类之间映射的方法. 有关详细信息请参阅[AutoMapper的文档](https://docs.automapper.org).

其中定义一种映射的方法是创建一个[Profile](https://docs.automapper.org/en/stable/Configuration.html#profile-instances) 类. 例如:

````csharp
public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<User, UserDto>();
    }
}
````

然后使用`AbpAutoMapperOptions`注册配置文件:

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

`AddMaps` 注册给定类的程序集中所有的配置类,通常使用模块类. 它还会注册 [attribute 映射](https://docs.automapper.org/en/stable/Attribute-mapping.html).

### 配置验证

`AddMaps` 使用可选的 `bool` 参数控制[模块](Module-Development-Basics.md)的[配置验证](https://docs.automapper.org/en/stable/Configuration-validation.html):

````csharp
options.AddMaps<MyModule>(validate: true);
````

如果此选项默认是 `false` , 但最佳实践建议启用.

可以使用 `AddProfile` 而不是 `AddMaps` 来控制每个配置文件类的配置验证:

````csharp
options.AddProfile<MyProfile>(validate: true);
````

> 如果你有多个配置文件,并且只需要为其中几个启用验证,那么首先使用`AddMaps`而不进行验证,然后为你想要验证的每个配置文件使用`AddProfile`.

### 映射对象扩展

[对象扩展系统](Object-Extensions.md) 允许为已存在的类定义额外属性. ABP 框架提供了一个映射定义扩展可以正确的映射两个对象的额外属性.

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

如果两个类都是可扩展对象(实现了 `IHasExtraProperties` 接口),建议使用 `MapExtraProperties` 方法. 更多信息请参阅[对象扩展文档](Object-Extensions.md).

### 其他有用的扩展方法

有一些扩展方法可以简化映射代码.

#### 忽视审计属性

当你将一个对象映射到另一个对象时,通常会忽略审核属性.

假设你需要将 `ProductDto` ([DTO](Data-Transfer-Objects.md))映射到Product[实体](Entities.md),该实体是从 `AuditedEntity` 类继承的(该类提供了 `CreationTime`, `CreatorId`, `IHasModificationTime` 等属性).

从DTO映射时你可能想忽略这些基本属性,可以使用 `IgnoreAuditedObjectPropertie()` 方法忽略所有审计属性(而不是手动逐个忽略它们):

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

还有更多扩展方法, 如 `IgnoreFullAuditedObjectProperties()` 和 `IgnoreCreationAuditedObjectProperties()`,你可以根据实体类型使用.

> 请参阅[实体文档](Entities.md)中的"*基类和接口的审计属性*"部分了解有关审计属性的更多信息。

#### 忽视其他属性

在AutoMapper中,通常可以编写这样的映射代码来忽略属性:

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

我们发现它的长度是不必要的并且创建了 `Ignore()` 扩展方法:

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

## 高级主题

### IObjectMapper<TContext> 接口

假设你已经创建了一个**可重用的模块**,其中定义了AutoMapper配置文件,并在需要映射对象时使用 `IObjectMapper`. 根据[模块化](Module-Development-Basics.md)的性质,你的模块可以用于不同的应用程序.

`IObjectMapper` 是一个抽象,可以由最终应用程序替换使用另一个映射库. 这里的问题是你的可重用模块设计为使用AutoMapper,因为它为其定义映射配置文件. 这种情况下即使最终应用程序使用另一个默认对象映射库,你也要保证模块始终使用AutoMapper.

`IObjectMapper<TContext>`将对象映射器上下文化,你可以为不同的 模块/上下文 使用不同的库.

用法示例:

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

`UserAppService` 注入 `IObjectMapper<MyModule>`, 它是模块的特定对象映射器,用法与 `IObjectMapper` 完全相同.

上面的示例代码未使用 `ApplicationService` 中定义的 `ObjectMapper` 属性,而是注入了 `IObjectMapper<MyModule>`. 但是 `ApplicationService` 定义了可以在类构造函数中设置的 `ObjectMapperContext` 属性, 因此仍然可以使用基类属性. 示例可以进行以下重写:

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

虽然使用上下文化的对象映射器与普通的对象映射器相同, 但是也应该在模块的  `ConfigureServices` 方法中注册上下文化的映射器:

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

`IObjectMapper<MyModule>`是可重用模块的一项基本功能,可在多个应用程序中使用,每个模块可以使用不同的库进行对象到对象的映射. 所有预构建的ABP模块都在使用它. 但是对于最终应用程序,你可以忽略此接口,始终使用默认的 `IObjectMapper` 接口.

### IObjectMapper<TSource, TDestination> 接口

ABP允许自定义特定类的映射代码. 假设你要创建一个自定义类从 `User` 映射到 `UserDto`. 这种情况下,你可以创建一个实现 `IObjectMapper<User,UserDto>`的类 :

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

ABP会自动发现注册 `MyCustomUserMapper`, 在你使用IObjectMapper将用户映射到UserDto时会自动使用自定义映射.

一个类可以为不同的对象实现多个 `IObjectMapper<TSource,TDestination>`.

> 这种方法功能强大, `MyCustomUserMapper`可以注入任何其他服务并在Map方法中使用.