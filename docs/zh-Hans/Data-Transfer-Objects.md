# 数据传输对象

## 介绍

**数据传输对象**(DTO)用于在**应用层**和**表示层**或其他类型的客户端之间传输数据.

通常用**DTO**作为参数在表示层(可选)调用[应用服务](Application-Services.md). 它使用领域对象执行某些**特定的业务逻辑**,并(可选)将DTO返回到表示层.因此表示层与领域层完全**隔离**.

### DTO的需求

> 如果你感觉你已经知道并确认使用DTO的好处,你可以**跳过这一节**.

首先为每个应用程序服务方法创建DTO类可能被看作是一项冗长而耗时的工作. 但是如果正确使用它们,它们可以保存在应用程序. 为什么和如何>

#### 领域层的抽象

DTO提供了一种从表示层**抽象领域对象**的有效方法. 实际上你的**层**被正确地分开了. 如果希望完全更改表示层,可以继续使用现有的应用程序层和领域层. 或者你可以重写领域层完全更改数据库架构,实体和O/RM框架,而无需更改表示层. 当然前提是应用程序服务的契约(方法签名和dto)保持不变.

#### 数据隐藏

假设你有一个具有属性Id,名称,电子邮件地址和密码的 `User` 实体. 如果 `UserAppService` 的 `GetAllUsers()` 方法返回 `List<User>`,任何人都可以访问你所有用户的密码,即使你没有在屏幕上显示它. 这不仅关乎安全,还关乎数据隐藏. 应用程序服务应该只返回表示层(或客户端)所需要的内容,不多也不少.

#### 序列化和延迟加载问题

当你将数据(一个对象)返回到表示层时,它很可能是序列化的. 例如在返回JSON的REST API中,你的对象将被序列化为JSON并发送给客户端. 在这方面将实体返回到表示层可能会有问题,尤其是在使用关系数据库和像Entity Framework Core这样的ORM提供者时.

在真实的应用程序中你的实体可以相互引用.  `User` 实体可以引用它的角色. 如果你想序列化用户,它的角色也必须是序列化的. `Role` 类可以有 `List <Permission>`,而 `Permission` 类可以有一个对 `PermissionGroup` 类的引用,依此类推...想象一下所有这些对象都被立即序列化了. 你可能会意外地序列化整个数据库! 同样,如果你的对象具有循环引用,则它们可能根本**不会**序列化成功.

有什么解决方案? 将属性标记为 `NonSerialized` 吗? 不,你不知道什么时候应该序列化什么时候应该序列化. 一个应用程序服务方法可能需要它,而另一个则不需要. 在这种情况下返回安全,可序列化且经过特殊设计的DTO是一个不错的选择.

几乎所有的O/RM框架都支持延迟加载. 此功能可在需要时从数据库加载实体. 假设 `User` 类具有对 `Role` 类的引用. 当你从数据库中获取用户时,`Role` 属性(或集合)不会被立即填充. 首次读取 `Role` 属性时,它是从数据库加载的. 因此如果将这样的实体返回到表示层,它将通过执行额外的查询从数据库中检索额外的实体. 如果序列化工具读取实体,它会递归读取所有属性,并且可以再次检索整个数据库(如果实体之间存在关系).

如果在表示层中使用实体,可能会出现更多问题.**最好不要在表示层中引用领域/业务层程序集**.

如果你确定使用DTO,我们可以继续讨论ABP框架提供的关于dto的建议.

> ABP并不强迫你使用DTO,但是**强烈建议将DTO作为最佳实践**.

## 标准接口和基类

DTO是一个没有依赖性的简单类,你可以用任何方式进行设计. 但是ABP引入了一些**接口**来确定命名**标准属性**和**基类**的**约定**,以免在声明**公共属性**时**重复工作**.

**它们都不是必需的**,但是使用它们可以**简化和标准化**应用程序代码.

### 实体相关DTO

通常你需要创建与你的实体相对应的DTO,从而生成与实体类似的类. ABP框架在创建DTO时提供了一些基类来简化.

#### EntityDto

`IEntityDto<TKey>` 是一个只定义 `Id` 属性的简单接口. 你可以实现它或从 `EntityDto<TKey>` 继承.

**Example:**

````csharp
using System;
using Volo.Abp.Application.Dtos;

namespace AbpDemo
{
    public class ProductDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        //...
    }
}
````

#### 审计DTO

如果你的实体继承自被审计的实体类(或实现审计接口)可以使用以下基类来创建DTO:

* `CreationAuditedEntityDto`
* `CreationAuditedEntityWithUserDto`
* `AuditedEntityDto`
* `AuditedEntityWithUserDto`
* `FullAuditedEntityDto`
* `FullAuditedEntityWithUserDto`

#### 可扩展的DTO

如果你想为你的DTO使用[对象扩展系统](Object-Extensions.md),你可以使用或继承以下DTO类:

* `ExtensibleObject` 实现 `IHasExtraProperties` (其它类继承这个类).
* `ExtensibleEntityDto`
* `ExtensibleCreationAuditedEntityDto`
* `ExtensibleCreationAuditedEntityWithUserDto`
* `ExtensibleAuditedEntityDto`
* `ExtensibleAuditedEntityWithUserDto`
* `ExtensibleFullAuditedEntityDto`
* `ExtensibleFullAuditedEntityWithUserDto`

### 列表结果

通常将DTO列表返回给客户端. `IListResult<T>` 接口和 `ListResultDto<T>` 类用于使其成为标准.

`IListResult<T>` 接口的定义:

````csharp
public interface IListResult<T>
{
    IReadOnlyList<T> Items { get; set; }
}
````

**示例: 返回产品列表**

````csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpDemo
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductAppService(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ListResultDto<ProductDto>> GetListAsync()
        {
            //Get entities from the repository
            List<Product> products = await _productRepository.GetListAsync();

            //Map entities to DTOs
            List<ProductDto> productDtos =
                ObjectMapper.Map<List<Product>, List<ProductDto>>(products);

            //Return the result
            return new ListResultDto<ProductDto>(productDtos);
        }
    }
}
````

你可以简单地返回 `productDtos` 对象(并更改方法的返回类型), 这也没有错. 返回一个 `ListResultDto` 会使`List<ProductDto>` 做为 `Item` 属性包装到另一个对象中. 这具有一个优点:以后可以在不破坏远程客户端的情况下(当它们作为JSON结果获得值时)在返回值中添加更多属性. 在开发可重用的应用程序模块时特别建议使用这种方式.

### 分页 & 排序列表结果

从服务器请求分页列表并将分页列表返回给客户端是更常见的情况. ABP定义了一些接口和类来对其进行标准化:

#### 输入 (请求) 类型

以下接口和类用于标准化客户端发送的输入.

* `ILimitedResultRequest`: 定义 `MaxResultCount`(`int`) 属性从服务器请求指定数量的结果.
* `IPagedResultRequest`: 继承自 `ILimitedResultRequest` (所以它具有 `MaxResultCount` 属性)并且定义了 `SkipCount` (`int`)用于请求服务器的分页结果时跳过计数.
* `ISortedResultRequest`:  定义 `Sorting` (`string`)属性以请求服务器的排序结果. 排序值可以是“名称”，"*Name*", "*Name DESC*", "*Name ASC, Age DESC*"... 等.
* `IPagedAndSortedResultRequest` 继承自 `IPagedResultRequest` 和 `ISortedResultRequest`,所以它有上述所有属性.

建议你继承以下基类DTO类之一,而不是手动实现接口:

* `LimitedResultRequestDto` 实现了 `ILimitedResultRequest`.
* `PagedResultRequestDto` 实现了 `IPagedResultRequest` (和继承自 `LimitedResultRequestDto`).
* `PagedAndSortedResultRequestDto` 实现了 `IPagedAndSortedResultRequest` (和继承自 `PagedResultRequestDto`).

##### 最大返回数量

`LimitedResultRequestDto`(和其它固有的)通过以下规则限制和验证 `MaxResultCount`;

* 如果客户端未设置 `MaxResultCount`,则假定为**10**(默认页面大小). 可以通过设置 `LimitedResultRequestDto.DefaultMaxResultCoun` t静态属性来更改此值.
* 如果客户端发送的 `MaxResultCount` 大于*1,000**,则会产生**验证错误**. 保护服务器免受滥用服务很重要. 如果需要可以通过设置 `LimitedResultRequestDto.MaxMaxResultCount` 静态属性来更改此值.

建议在应用程序启动时设置静态属性,因为它们是静态的(全局).

#### 输出 (响应) 类型

以下接口和类用于标准化发送给客户端的输出.

* `IHasTotalCount` 定义 `TotalCount`(`long`)属性以在分页的情况下返回记录的总数.
* `IPagedResult<T>` 集成自 `IListResult<T>` 和 `IHasTotalCount`, 所以它有 `Items` 和 `TotalCount` 属性.

建议你继承以下基类DTO类之一,而不是手动实现接口:

* `PagedResultDto<T>` 继承自 `ListResultDto<T>` 和实现了 `IPagedResult<T>`.

**示例: 从服务器请求分页和排序的结果并返回分页列表**

````csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpDemo
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductAppService(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PagedResultDto<ProductDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            //Create the query
            var query = _productRepository
                .OrderBy(input.Sorting);

            //Get total count from the repository
            var totalCount = await query.CountAsync();
            
            //Get entities from the repository
            List<Product> products = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            //Map entities to DTOs
            List<ProductDto> productDtos =
                ObjectMapper.Map<List<Product>, List<ProductDto>>(products);

            //Return the result
            return new PagedResultDto<ProductDto>(totalCount, productDtos);
        }
    }
}
````

ABP框架还定义了一种 `PageBy` 扩展方法(与`IPagedResultRequest`兼容),可用于代替 `Skip` + `Take`调用:

````csharp
var query = _productRepository
    .OrderBy(input.Sorting)
    .PageBy(input);
````

> 注意我们将`Volo.Abp.EntityFrameworkCore`包添加到项目中以使用 `ToListAsync` 和 `CountAsync` 方法,因为它们不包含在标准LINQ中,而是由Entity Framework Core定义.

如果你不了解示例代码,另请参阅[仓储文档](Repositories.md).

## 相关话题

### 验证

[应用服务](Application-Services.md)方法,控制器操作,页面模型输入...的输入会自动验证. 你可以使用标准数据注释属性或自定义验证方法来执行验证.

参阅[验证文档](Validation.md)了解更多.

### 对象到对象的映射

创建与实体相关的DTO时通常需要映射这些对象. ABP提供了一个对象到对象的映射系统简化映射过程. 请参阅以下文档:

* [对象到对象映射文档](Object-To-Object-Mapping.md)介绍了这些功能.
* [应用服务文档](Application-Services.md)提供了完整的示例.

## 最佳实践

你可以自由设计DTO类,然而这里有一些你可能想要遵循的最佳实践和建议.

### 共同原则

* DTO应该是**可序列化的**,因为它们通常是序列化和反序列化的(JSON或其他格式). 如果你有另一个带参数的构造函数,建议使用空(无参数)的公共构造函数.
* 除某些[验证](Validation.md)代码外,DTO**不应包含任何业务逻辑**.
* DTO不要继承实体,也**不要引用实体**. [应用程序启动模板](Startup-Templates/Application.md)已经通过分隔项目来阻止它.
* 如果你使用自动[对象到对象](Object-To-Object-Mapping.md)映射库,如AutoMapper,请启用**映射配置验证**以防止潜在的错误.

### 输入DTO原则

* 只定义用例**所需的属性**. 不要包含不用于用例的属性,这样做会使开发人员感到困惑.

* **不要在**不同的应用程序服务方法之间重用输入DTO. 因为不同的用例将需要和使用DTO的不同属性,从而导致某些属性在某些情况下没有使用,这使得理解和使用服务更加困难,并在将来导致潜在的错误.

### 输出DTO原则

* 如果在所有情况下填充**所有属性**,就可以**重用输出DTO**.
