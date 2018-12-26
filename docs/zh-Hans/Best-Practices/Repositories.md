## 仓储最佳实践 & 约定

### 仓储接口

* **推荐** 在**领域层**中定义仓储接口.
* **推荐** 为**每个聚合根**定义仓储接口(如 `IIdentityUserRepository`)并创建相应的实现.
  * **推荐** 在应用代码中使用仓储时应该注入仓储接口.
  * **不推荐** 在应用代码中使用泛型仓储接口(如 `IRepository<IdentityUser, Guid>`).
  * **不推荐** 在应用代码(领域, 应用... 层)中使用 `IQueryable<TEntity>` 特性.

聚合根的示例:

````C#
public class IdentityUser : AggregateRoot<Guid>
{
    //...
}
````

定义仓储接口, 如下所示:

````C#
public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>
{
    //...
}
````

* **不推荐** 仓储接口继承 `IRepository<TEntity, TKey>` 接口. 因为它继承了 `IQueryable` 而仓储不应该将`IQueryable`暴漏给应用.
* **推荐** 通常仓储接口继承自 `IBasicRepository<TEntity, TKey>` 或更低级别的接口, 如 `IReadOnlyRepository<TEntity, TKey>` (在需要的时候).
* **不推荐** 为实体定义仓储接口,因为它们**不是聚合根**.

### 仓储方法

* **推荐** 所有的仓储方法定义为 **异步**.
* **推荐** 为仓储的每个方法添加 **可选参数** `cancellationToken` . 例:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    CancellationToken cancellationToken = default
);
````

* **推荐** 为仓储的每个异步方法创建一个 **同步扩展** 方法. 示例:

````C#
public static class IdentityUserRepositoryExtensions
{
    public static IdentityUser FindByNormalizedUserName(
        this IIdentityUserRepository repository,
        [NotNull] string normalizedUserName)
    {
        return AsyncHelper.RunSync(
            () => repository.FindByNormalizedUserNameAsync(normalizedUserName)
        );
    }
}
````

对于同步方法而言, 这会让它们更方便的调用仓储方法.

* **推荐** 为仓储中返回**单个实体**的方法添加一个可选参数 `bool includeDetails = true`  (默认值为`true`). 示例:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    bool includeDetails = true,
    CancellationToken cancellationToken = default
);
````

该参数由ORM实现, 用来加载实体子集合.

* **推荐** 为仓储中返回**实体列表**的方法添加一个可选参数 `bool includeDetails = false` (默认值为`false`). 示例:

````C#
Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
    string normalizedRoleName, 
    bool includeDetails = false,
    CancellationToken cancellationToken = default
);
````

* **不推荐** 创建复合类通过调用仓储单个方法返回组合实体. 比如: *UserWithRoles*, *UserWithTokens*, *UserWithRolesAndTokens*. 相反, 正确的使用 `includeDetails` 选项, 在需要时加载实体所有的详细信息.
* **避免** 为了从仓储中获取实体的部分属性而为实体创建投影类. 比如: 避免通过创建BasicUserView来选择所需的一些属性. 相反可以直接使用聚合根类. 不过这条规则有例外情况:
  * 性能对于用例来说非常重要,而且使用整个聚合根对性能的影响非常大.

### 另外请参阅

* [Entity Framework Core 集成](Entity-Framework-Core-Integration.md)
* [MongoDB 集成](MongoDB-Integration.md)
