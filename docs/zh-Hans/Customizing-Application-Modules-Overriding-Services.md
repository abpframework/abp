# 自定义应用模块: 重写服务

你可能想要**更改**依赖模块的**行为(业务逻辑)**. 在这种情况下,你可以使用[依赖注入](Dependency-Injection.md)的能力替换服务,控制器甚至页面模型到你自己的实现.

注册到依赖注入的任何类,包括ABP框架的服务都可以被**替换**.

你可以根据自己的需求使用不同的选项,下面的章节中将介绍这些选项.

> 请注意,某些服务方法可能不是virtual,你可能无法override,我们会通过设计将其virtual,如果你发现任何方法不可以被覆盖,请[创建一个issue](https://github.com/abpframework/abp/issues/new)或者你直接修改后并发送**pull request**到GitHub.

## 替换接口

如果给定的服务定义了接口,像 `IdentityUserAppService` 类实现了 `IIdentityUserAppService` 接口,你可以为这个接口创建自己的实现并且替换当前的实现. 例如:

````csharp
public class MyIdentityUserAppService : IIdentityUserAppService, ITransientDependency
{
    //...
}
````

`MyIdentityUserAppService` 通过命名约定替换了 `IIdentityUserAppService` 的当前实现. 如果你的类名不匹配,你需要手动公开服务接口:

````csharp
[ExposeServices(typeof(IIdentityUserAppService))]
public class TestAppService : IIdentityUserAppService, ITransientDependency
{
    //...
}
````

依赖注入系统允许为一个接口注册多个服务. 注入接口时会解析最后一个注入的服务. 显式的替换服务是一个好习惯.

示例:

````csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IIdentityUserAppService))]
public class TestAppService : IIdentityUserAppService, ITransientDependency
{
    //...
}
````

使用这种方法, `IIdentityUserAppService` 接口将只会有一个实现. 也可以使用以下方法替换服务:

````csharp
context.Services.Replace(
    ServiceDescriptor.Transient<IIdentityUserAppService, MyIdentityUserAppService>()
);
````

你可以在[模块](Module-Development-Basics.md)类的 `ConfigureServices` 方法编写替换服务代码.

## 重写一个服务类

大多数情况下,你会仅想改变服务当前实现的一个或几个方法. 重新实现完整的接口变的繁琐,更好的方法是继承原始类并重写方法.

### 示例: 重写服务方法

````csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IIdentityUserAppService), typeof(IdentityUserAppService), typeof(MyIdentityUserAppService))]
public class MyIdentityUserAppService : IdentityUserAppService
{
    //...
    public MyIdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IGuidGenerator guidGenerator
    ) : base(
        userManager,
        userRepository,
        guidGenerator)
    {
    }

    public async override Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        if (input.PhoneNumber.IsNullOrWhiteSpace())
        {
            throw new AbpValidationException(
                "Phone number is required for new users!",
                new List<ValidationResult>
                {
                    new ValidationResult(
                        "Phone number can not be empty!",
                        new []{"PhoneNumber"}
                    )
                }
            );        }

        return await base.CreateAsync(input);
    }
}
````

示例中**重写**了 `IdentityUserAppService` [应用程序](Application-Services.md) `CreateAsync` 方法检查手机号码. 然后调用了基类方法继续**基本业务逻辑**. 通过这种方法你可以在基本业务逻辑**之前**和**之后**执行其他业务逻辑.

你也可以完全**重写**整个业务逻辑去创建用户,而不是调用基类方法.

### 示例: 重写领域服务

````csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IdentityUserManager))]
public class MyIdentityUserManager : IdentityUserManager
{
    public MyIdentityUserManager(
        IdentityUserStore store,
        IIdentityRoleRepository roleRepository,
        IIdentityUserRepository userRepository,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<IdentityUserManager> logger,
        ICancellationTokenProvider cancellationTokenProvider) :
        base(store,
            roleRepository,
            userRepository,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger,
            cancellationTokenProvider)
    {
    }

    public async override Task<IdentityResult> CreateAsync(IdentityUser user)
    {
        if (user.PhoneNumber.IsNullOrWhiteSpace())
        {
            throw new AbpValidationException(
                "Phone number is required for new users!",
                new List<ValidationResult>
                {
                    new ValidationResult(
                        "Phone number can not be empty!",
                        new []{"PhoneNumber"}
                    )
                }
            );
        }

        return await base.CreateAsync(user);
    }
}
````

示例中类继承了 `IdentityUserManager` [领域服务](Domain-Services.md),并且重写了 `CreateAsync` 方法进行了与之前相同的手机号码检查. 结果也是一样的,但是这次我们在领域服务实现了它,假设这是我们系统的**核心领域逻辑**.

> 这里需要 `[ExposeServices(typeof(IdentityUserManager))]`  attribute,因为 `IdentityUserManager` 没有定义接口 (像 `IIdentityUserManager`) ,依赖注入系统并不会按照约定公开继承类的服务(如已实现的接口).

参阅[本地化系统](Localization.md)了解如何自定义错误消息.

### 重写其他服务

控制器,框架服务,视图组件类以及其他类型注册到依赖注入的类都可以像上面的示例那样被重写.

## 扩展数据传输对象

你可以如[扩展实体文档](Customizing-Application-Modules-Extending-Entities.md)所述扩展实体. 并使用上面介绍的重写相关服务**使用自定义属性****执行其他业务逻辑**.

应用程序使用的数据传输对象(**DTO**)同样可扩展. 这样你可以使服务返回其他属性并在UI(或其他客户端)得到其他属性.

### 示例

假设你已经按照[扩展实体文档](Customizing-Application-Modules-Extending-Entities.md)中的说明添加了 `SocialSecurityNumber` 并希望从 `IdentityUserAppService的GetListAsync` 方法获取用户列表时包括此属性.

你可以使用[对象扩展系统](Object-Extensions.md)将属性添加到 `IdentityUserDto`.  在应用程序启动模板带有的 `YourProjectNameDtoExtensions` 类中编写以下代码:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUserDto, string>(
        "SocialSecurityNumber"
    );
````

这段代码为 `IdentityUserDto` 类添加了 `string` 类型的 `SocialSecurityNumber` 属性. 现在你可以在RREST API客户端调用 `/api/identity/users` HTTP API(内部使用 `IdentityUserAppService`),你会在 `extraProperties` 部分看到 `SocialSecurityNumber` 值.

````json
{
    "totalCount": 1,
    "items": [{
        "tenantId": null,
        "userName": "admin",
        "name": "admin",
        "surname": null,
        "email": "admin@abp.io",
        "emailConfirmed": false,
        "phoneNumber": null,
        "phoneNumberConfirmed": false,
        "twoFactorEnabled": false,
        "lockoutEnabled": true,
        "lockoutEnd": null,
        "concurrencyStamp": "b4c371a0ab604de28af472fa79c3b70c",
        "isDeleted": false,
        "deleterId": null,
        "deletionTime": null,
        "lastModificationTime": "2020-04-09T21:25:47.0740706",
        "lastModifierId": null,
        "creationTime": "2020-04-09T21:25:46.8308744",
        "creatorId": null,
        "id": "8edecb8f-1894-a9b1-833b-39f4725db2a3",
        "extraProperties": {
            "SocialSecurityNumber": "123456789"
        }
    }]
}
````

手动添加了 `123456789` 值到数据库中.

所有预构建的模块都在DTO中支持额外属性,你可以对其轻松的配置.

### 定义检查

当为实体[定义](Customizing-Application-Modules-Extending-Entities.md)额外的属性时,由于安全性它不会自动出现在所有相关的DTO中. 额外属性可能包含敏感数据并且你可能不想默认公开给客户端.

因此如果要用于DTO,需要为相应的DTO显式定义相同的属性(如上所述).  如果要允许在用户创建时进行设置还需要为 `IdentityUserCreateDto` 定义.

如果属性并不是安全敏感,这可能会很枯燥. 对象扩展系统允许你忽略检查定义的属性. 参阅示例:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.MapEfCore(b => b.HasMaxLength(32));
            options.CheckPairDefinitionOnMapping = false;
        }
    );
````

这是定义实体属性的另一种方法( 有关 `ObjectExtensionManager` 更多信息,请参阅[文档](Object-Extensions.md)). 这次我们设置了 `CheckPairDefinitionOnMapping` 为false,在将实体映射到DTO时会跳过定义检查.

如果你不喜欢这种方法,但想简单的向多个对象(DTO)添加单个属性, `AddOrUpdateProperty` 可以使用类型数组添加额外的属性:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<string>(
        new[]
        {
            typeof(IdentityUserDto),
            typeof(IdentityUserCreateDto),
            typeof(IdentityUserUpdateDto)
        },
        "SocialSecurityNumber"
    );
````

### 关于用户界面

该系统允许你向实体和DTO添加额外的属性并执行自定义业务代码,但它与用户界面无关.

参阅 [重写用户界面](Customizing-Application-Modules-Overriding-User-Interface.md) 指南了解关于UI部分.

## 如何找到服务?

[模块文档](Modules/Index.md) 包含了定义的主要服务列表. 另外 你也可以查看[源码](https://github.com/abpframework/abp/tree/dev/modules)找到所有的服务.
