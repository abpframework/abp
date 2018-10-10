## MongoDB 集成

本文会介绍如何将MongoDB集成到基于ABP的应用程序中以及如何配置它

### 安装

 集成MongoDB需要用到`Volo.Abp.MongoDB`这个包.将它安装到你的项目中(如果是多层架构,安装到数据层和基础设施层):

```
Install-Package Volo.Abp.MongoDB
```

然后添加 `AbpMongoDbModule` 依赖到你的 [模块](Module-Development-Basics.md)中:

```c#
using Volo.Abp.MongoDB;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
```

### 创建一个Mongo Db Context

ABP中引入了 **Mongo Db Context** 的概念(跟Entity Framework Core的DbContext很像)让使用和配置集合变得更简单.举个例子:

```c#
public class MyDbContext : AbpMongoDbContext
{
    public IMongoCollection<Question> Questions => Collection<Question>();

    public IMongoCollection<Category> Categories => Collection<Category>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(b =>
        {
            b.CollectionName = "Questions";
        });
    }
}
```

* 继承 `AbpMongoDbContext` 类
* 为每一个mongo集合添加一个公共的 `IMongoCollection<TEntity>` 属性.ABP默认使用这些属性创建默认的仓储
* 重写 `CreateModel` 方法,可以在方法中配置集合(如设置集合在数据库中的名字)

### 将 Db Context 注入到依赖注入中

在你的模块中使用 `AddAbpDbContext` 方法将Db Context注入到[依赖注入](Dependency-Injection.md)系统中.

```c#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MongoDB;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<MyDbContext>();

            //...
        }
    }
}
```

#### 添加默认的仓储

在注入的时候使用 `AddDefaultRepositories()`, ABP就能自动为Db Context中的每一个实体创建[仓储](Repositories.md):

````C#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

这样就会默认为每一个聚合根实体(继承自AggregateRoot的类)创建一个仓储.如果你也想为其他的实体创建仓储,将 `includeAllEntities` 设置为 `true`就可以了:

```c#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
```

现在可以在你的服务中注入并使用`IRepository<TEntity>` 或 `IQueryableRepository<TEntity>`了.