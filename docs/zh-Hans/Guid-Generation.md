# GUID 生成

GUID是数据库管理系统中使用的常见**主键类型**, ABP框架更喜欢GUID作为预构建[应用模块](Modules/Index.md)的主要对象. `ICurrentUser.Id` 属性([参见文档](CurrentUser.md))是GUID类型,这意味着ABP框架假定用户ID始终是GUID,

## 为什么偏爱GUID?

GUID有优缺点. 你可以在网上找到许多与此主题相关的文章,因此我们不再赘述,而是列出了最基本的优点:

* 它可在所有数据库提供程序中**使用**.
* 它允许在客户端**确定主键**,而不需要通过**数据库往返**来生成Id值. 在向数据库插入新记录时,这可以提高性能并允许我们在与数据库交互之前知道PK.
* GUID是**自然唯一的**在以下情况下有一些优势;
  * 你需要与**外部**系统集成,
  * 你需要**拆分或合并**不同的表.
  * 你正在创建**分布式系统**
* GUID是无法猜测的,因此在某些情况下与自动递增的Id值相比,GUID**更安全**.

尽管存在一些缺点(只需在Web上搜索),但在设计ABP框架时我们发现这些优点更为重要.

## IGuidGenerator

GUID的最重要问题是**默认情况下它不是连续的**. 当你将GUID用作主键并将其设置为表的**聚集索引**(默认设置)时,这会在**插入时带来严重的性能问题**(因为插入新记录可能需要对现有记录进行重新排序).

所以,**永远不要为你的实体使用 `Guid.NewGuid()` 创建ID**!.

这个问题的一个好的解决方案是生成**连续的GUID**,由ABP框架提供的开箱即用的. `IGuidGenerator` 服务创建顺序的GUID(默认由 `SequentialGuidGenerator` 实现). 当需要手动设置[实体](Entities.md)的Id时,请使用 `IGuidGenerator.Create()`.

**示例: 具有GUID主键的实体并创建该实体**

假设你有一个具有 `Guid` 主键的 `Product` [实体](Entities.md):

````csharp
using System;
using Volo.Abp.Domain.Entities;

namespace AbpDemo
{
    public class Product : AggregateRoot<Guid>
    {
        public string Name { get; set; }

        private Product() { /* This constructor is used by the ORM/database provider */ }

        public Product(Guid id, string name)
            : base(id)
        {
            Name = name;
        }
    }
}
````

然后你想要创建一个产品:

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace AbpDemo
{
    public class MyProductService : ITransientDependency
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IGuidGenerator _guidGenerator;

        public MyProductService(
            IRepository<Product, Guid> productRepository,
            IGuidGenerator guidGenerator)
        {
            _productRepository = productRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task CreateAsync(string productName)
        {
            var product = new Product(_guidGenerator.Create(), productName);

            await _productRepository.InsertAsync(product);
        }
    }
}
````

该服务将 `IGuidGenerator` 注入构造函数中. 如果你的类是[应用服务](Application-Services.md)或派生自其他基类之一,可以直接使用 `GuidGenerator` 基类属性,该属性是预先注入的 `IGuidGenerator` 实例.

## Options

### AbpSequentialGuidGeneratorOptions

`AbpSequentialGuidGeneratorOptions` 是用于配置顺序生成GUID的[选项类](Options.md). 它只有一个属性:

* `DefaultSequentialGuidType` (`SequentialGuidType` 类型的枚举): 生成GUID值时使用的策略.

数据库提供程序在处理GUID时的行为有所不同,你应根据数据库提供程序进行设置. `SequentialGuidType` 有以下枚举成员:

* `SequentialAtEnd` (**default**) 用于[SQL Server](Entity-Framework-Core.md).
* `SequentialAsString` 用于[MySQL](Entity-Framework-Core-MySQL.md)和[PostgreSQL](Entity-Framework-Core-PostgreSQL.md).
* `SequentialAsBinary` 用于[Oracle](Entity-Framework-Core-Oracle.md).

在你的[模块](Module-Development-Basics.md)的 `ConfigureServices` 方法配置选项,如下:

````csharp
Configure<AbpSequentialGuidGeneratorOptions>(options =>
{
    options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsBinary;
});
````

> EF Core[集成包](https://docs.abp.io/en/abp/latest/Entity-Framework-Core-Other-DBMS)已为相关的DBMS设置相应的值. 如果你正在使用这些集成包,在大多数情况下则无需设置此选项.
