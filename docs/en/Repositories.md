# Repositories

"*Mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects*" (Martin Fowler).

Repositories, in practice, are used to perform database operations for domain objects (see [Entities](Entities.md)). Generally, a separated repository is used for each **aggregate root** or entity.

## Generic Repositories

ABP can provide a **default generic repository** for each aggregate root or entity. You can [inject](Dependency-Injection.md) `IRepository<TEntity, TKey>` into your service and perform standard **CRUD** operations. Example usage:

````C#
public class PersonAppService : ApplicationService
{
    private readonly IRepository<Person, Guid> _personRepository;

    public PersonAppService(IRepository<Person, Guid> personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task Create(CreatePersonDto input)
    {
        var person = new Person { Name = input.Name, Age = input.Age };

        await _personRepository.InsertAsync(person);
    }

    public List<PersonDto> GetList(string nameFilter)
    {
        var people = _personRepository
            .Where(p => p.Name.Contains(nameFilter))
            .ToList();

        return people
            .Select(p => new PersonDto {Id = p.Id, Name = p.Name, Age = p.Age})
            .ToList();
    }
}
````

In this example;

* `PersonAppService` simply injects `IRepository<Person, Guid>` in it's constructor.
* `Create` method uses `InsertAsync` to save a newly created entity.
* `GetList` method uses the standard LINQ `Where` and `ToList` methods to filter and get a list of people from the data source.

> The example above uses hand-made mapping between [entities](Entities.md) and [DTO](Data-Transfer-Objects.md)s. See [object to object mapping document](Object-To-Object-Mapping.md) for an automatic way of mapping.

Generic Repositories provides some standard CRUD features out of the box:

* Provides `Insert` method to save a new entity.
* Provides `Update` and `Delete` methods to update or delete an entity by entity object or it's id.
* Provides `Delete` method to delete multiple entities by a filter.
* Implements `IQueryable<TEntity>`, so you can use LINQ and extension methods like `FirstOrDefault`, `Where`, `OrderBy`, `ToList` and so on...

### Basic Repositories

Standard `IRepository<TEntity, TKey>` interface extends standard `IQueryable<TEntity>` and you can freely query using standard LINQ methods. However, some ORM providers or database systems may not support standard `IQueryable` interface.

ABP provides `IBasicRepository<TEntity, TPrimaryKey>` and `IBasicRepository<TEntity>` interfaces to support such scenarios. You can extend these interfaces (and optionally derive from `BasicRepositoryBase`) to create custom repositories for your entities.

Depending on `IBasicRepository` but not depending on `IRepository` has an advantage to make possible to work with all data sources even if they don't support `IQueryable`. But major vendors, like Entity Framework, NHibernate or MongoDb already support `IQueryable`.

So, working with `IRepository` is the **suggested** way for typical applications. But reusable module developers may consider `IBasicRepository` to support a wider range of data sources.

### Read Only Repositories

There are also `IReadOnlyRepository<TEntity, TKey>` and `IReadOnlyBasicRepository<Tentity, TKey>` interfaces for who only want to depend on querying capabilities of the repositories.

### Generic Repository without a Primary Key

If your entity does not have an Id primary key (it may have a composite primary key for instance) then you cannot use the `IRepository<TEntity, TKey>` (or basic/readonly versions) defined above. In that case, you can inject and use `IRepository<TEntity>` for your entity.

> `IRepository<TEntity>` has a few missing methods those normally works with the `Id` property of an entity. Because of the entity has no `Id` property in that case, these methods are not available. One example is the `Get` method that gets an id and returns the entity with given id. However, you can still use `IQueryable<TEntity>` features to query entities by standard LINQ methods.

## Custom Repositories

Default generic repositories will be sufficient for most cases. However, you may need to create a custom repository class for your entity.

### Custom Repository Example

ABP does not force you to implement any interface or inherit from any base class for a repository. It can be just a simple POCO class. However, it's suggested to inherit existing repository interface and classes to make your work easier and get the standard methods out of the box.

#### Custom Repository Interface

First, define an interface in your domain layer:

```c#
public interface IPersonRepository : IRepository<Person, Guid>
{
    Task<Person> FindByNameAsync(string name);
}
```

This interface extends `IRepository<Person, Guid>` to take advantage of pre-built repository functionality.

#### Custom Repository Implementation

A custom repository is tightly coupled to the data access tool type you are using. In this example, we will use Entity Framework Core:

````C#
public class PersonRepository : EfCoreRepository<MyDbContext, Person, Guid>, IPersonRepository
{
    public PersonRepository(IDbContextProvider<TestAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {

    }

    public async Task<Person> FindByNameAsync(string name)
    {
        return await DbContext.Set<Person>()
            .Where(p => p.Name == name)
            .FirstOrDefaultAsync();
    }
}
````

You can directly access the data access provider (`DbContext` in this case) to perform operations. See [entity framework integration document](Entity-Framework-Core.md) for more about custom repositories based on EF Core.

