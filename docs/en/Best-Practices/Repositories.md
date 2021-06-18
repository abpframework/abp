## Repository Best Practices & Conventions

### Repository Interfaces

* **Do** define repository interfaces in the **domain layer**.
* **Do** define a repository interface (like `IIdentityUserRepository`) and create its corresponding implementations for **each aggregate root**.
  * **Do** always use the created repository interface from the application code.
  * **Do not** use generic repository interfaces (like `IRepository<IdentityUser, Guid>`) from the application code.
  * **Do not** use `IQueryable<TEntity>` features in the application code (domain, application... layers).

For the example aggregate root:

````C#
public class IdentityUser : AggregateRoot<Guid>
{
    //...
}
````

Define the repository interface as below:

````C#
public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>
{
    //...
}
````

* **Do not** inherit the repository interface from the `IRepository<TEntity, TKey>` interface. Because it inherits the `IQueryable` and the repository should not expose `IQueryable` to the application.
* **Do** inherit the repository interface from `IBasicRepository<TEntity, TKey>` (as normally) or a lower-featured interface, like `IReadOnlyRepository<TEntity, TKey>` (if it's needed).
* **Do not** define repositories for entities those are **not aggregate roots**.

### Repository Methods

* **Do** define all repository methods as **asynchronous**.
* **Do** add an **optional** `cancellationToken` parameter to every method of the repository. Example:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    CancellationToken cancellationToken = default
);
````

* **Do** add an optional `bool includeDetails = true` parameter (default value is `true`) for every repository method which returns a **single entity**. Example:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    bool includeDetails = true,
    CancellationToken cancellationToken = default
);
````

This parameter will be implemented for ORMs to eager load sub collections of the entity.

* **Do** add an optional `bool includeDetails = false` parameter (default value is `false`) for every repository method which returns a **list of entities**. Example:

````C#
Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
    string normalizedRoleName, 
    bool includeDetails = false,
    CancellationToken cancellationToken = default
);
````

* **Do not** create composite classes to combine entities to get from repository with a single method call. Examples: *UserWithRoles*, *UserWithTokens*, *UserWithRolesAndTokens*. Instead, properly use `includeDetails` option to add all details of the entity when needed.
* **Avoid** to create projection classes for entities to get less property of an entity from the repository. Example: Avoid to create BasicUserView class to select a few properties needed for the use case needs. Instead, directly use the aggregate root class. However, there may be some exceptions for this rule, where:
  * Performance is so critical for the use case and getting the whole aggregate root highly impacts the performance.

### See Also

* [Entity Framework Core Integration](Entity-Framework-Core-Integration.md)
* [MongoDB Integration](MongoDB-Integration.md)
