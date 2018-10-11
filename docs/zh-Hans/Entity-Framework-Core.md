## Entity Framework Core 集成

本文档介绍了如何将EF Core作为ORM提供程序集成到基于ABP的应用程序以及如何对其进行配置.

### 安装

`Volo.Abp.EntityFrameworkCore` 是EF Core 集成的主要nuget包. 将其安装到你的项目中(在分层应用程序中适用于 数据/基础设施层):

```
Install-Package Volo.Abp.EntityFrameworkCore
```

然后添加 `AbpEntityFrameworkCoreModule` 模块依赖项到 [module](Module-Development-Basics.cn.md)(项目中的Mudole类):

````C#
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

### 创建 DbContext

你可以平常一样创建DbContext,它需要继承自 `AbpDbContext<T>`. 如下所示:

````C#
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyCompany.MyProject
{
    public class MyDbContext : AbpDbContext<MyDbContext>
    {
        //...your DbSet properties

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }
}
````

### 将DbContext注册到依赖注入

在module中的ConfigureServices方法使用 `AddAbpDbContext` 在[依赖注入](Dependency-Injection.cn.md)系统注册DbContext类.

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyDbContext>();

            //...
        }
    }
}
````

#### 添加默认仓储

ABP会自动为DbContext中的实体创建仓储 (TODO: link). 需要在注册的时使用`AddDefaultRepositories()`:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

默认情况下为每个聚合根实体(从聚合体派生的类)创建一个仓储. 如果想要为其他实体也创建仓储
需要把`includeAllEntities` 设置为 `true`:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
````

然后你就可以在服务中注入和使用 `IRepository<TEntity>` 或 `IQueryableRepository<TEntity>`.

#### 添加自定义仓储

TODO ...

#### 为默认仓储设置Base DbContext类或接口

TODO ...

#### 替换其他仓储

TODO ...