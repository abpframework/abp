# Application Services

Application services are used to implement the **use cases** of an application. They are used to **expose domain logic to the presentation layer**.

An Application Service is called from the presentation layer (optionally) with a **DTO (Data Transfer Object)** as the parameter. It uses domain objects to **perform some specific business logic** and (optionally) returns a DTO back to the presentation layer. Thus, the presentation layer is completely **isolated** from domain layer.

## Example

### Book Entity

Assume that you have a `Book` entity (actually, an aggregate root) defined as shown below:

````csharp
public class Book : AggregateRoot<Guid>
{
    public const int MaxNameLength = 128;

    public virtual string Name { get; protected set; }

    public virtual BookType Type { get; set; }

    public virtual float? Price { get; set; }

    protected Book()
    {
        
    }

    public Book(Guid id, [NotNull] string name, BookType type, float? price = 0)
    {
        Id = id;
        Name = CheckName(name);
        Type = type;
        Price = price;
    }

    public virtual void ChangeName([NotNull] string name)
    {
        Name = CheckName(name);
    }

    private static string CheckName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"name can not be empty or white space!");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ArgumentException($"name can not be longer than {MaxNameLength} chars!");
        }

        return name;
    }
}
````

* `Book` entity has a `MaxNameLength` that defines the maximum length of the `Name` property. 
* `Book` constructor and `ChangeName` method to ensure that the `Name` is always a valid value. Notice that `Name`'s  setter is not `public`.

> ABP does not force you to design your entities like that. It just can have public get/set for all properties. It's your decision to full implement DDD practices.

### IBookAppService Interface

In ABP, an application service should implement the `IApplicationService` interface. It's good to create an interface for each application service:

````csharp
public interface IBookAppService : IApplicationService
{
    Task CreateAsync(CreateBookDto input);
}
````

A Create method will be implemented as the example. `CreateBookDto` is defined like that:

````csharp
public class CreateBookDto
{
    [Required]
    [StringLength(Book.MaxNameLength)]
    public string Name { get; set; }

    public BookType Type { get; set; }

    public float? Price { get; set; }
}
````

> See [data transfer objects document](Data-Transfer-Objects.md) for more about DTOs.

### BookAppService (Implementation)

````csharp
public class BookAppService : ApplicationService, IBookAppService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookAppService(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task CreateAsync(CreateBookDto input)
    {
        var book = new Book(
            GuidGenerator.Create(),
            input.Name,
            input.Type,
            input.Price
        );

        await _bookRepository.InsertAsync(book);
    }
}
````

* `BookAppService` inherits from the `ApplicationService` base class. It's not required, but the `ApplicationService` class provides helpful properties for common application service requirements like `GuidGenerator` used in this service. If we didn't inherit from it, we would need to inject the `IGuidGenerator` service manually (see [guid generation](Guid-Generation.md) document).
* `BookAppService` implements the `IBookAppService` as expected.
* `BookAppService` [injects](Dependency-Injection.md) `IRepository<Book, Guid>` (see [repositories](Repositories.md)) and uses it inside the `CreateAsync` method to insert a new entity to the database.
* `CreateAsync` uses the constructor of the `Book` entity to create a new book from the properties of given `input`.

## Data Transfer Objects

Application services gets and returns DTOs instead of entities. ABP does not force this rule. However, exposing entities to presentation layer (or to remote clients) have significant problems and not suggested. 

See the [DTO documentation](Data-Transfer-Objects.md) for more.

## Object to Object Mapping

The `CreateAsync` method above manually creates a `Book` entity from given `CreateBookDto` object. Because the `Book` entity enforces it (we designed it like that).

However, in many cases, it's very practical to use **auto object mapping** to set properties of an object from a similar object. ABP provides an [object to object mapping](Object-To-Object-Mapping.md) infrastructure to make this even easier.

Let's create another method to get a book. First, define the method in the `IBookAppService` interface:

````csharp
public interface IBookAppService : IApplicationService
{
    Task CreateAsync(CreateBookDto input);

    Task<BookDto> GetAsync(Guid id); //New method
}
````

`BookDto` is a simple [DTO](Data-Transfer-Objects.md) class defined as below:

````csharp
[AbpAutoMapFrom(typeof(Book))] //Defines the mapping
public class BookDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public BookType Type { get; set; }

    public float? Price { get; set; }
}
````

* `BookDto` defines `[AbpAutoMapFrom(typeof(Book))]` attribute to create the object mapping from `Book` to `BookDto`.

Then you can implement the `GetAsync` method as shown below:

````csharp
public async Task<BookDto> GetAsync(Guid id)
{
    var book = await _bookRepository.GetAsync(id);
    return book.MapTo<BookDto>();
}
````

`MapTo` extension method converts `Book` object to `BookDto` object by copying all properties with the same naming.

An alternative to the `MapTo` is using the `IObjectMapper` service:

````csharp
public async Task<BookDto> GetAsync(Guid id)
{
    var book = await _bookRepository.GetAsync(id);
    return ObjectMapper.Map<Book, BookDto>(book);
}
````

While the second syntax is a bit harder to write, it better works if you write unit tests.

See the [object to object mapping document](Object-To-Object-Mapping.md) for more.

## Validation

Inputs of application service methods are automatically validated (like ASP.NET Core controller actions). You can use the standard data annotation attributes or custom validation method to perform the validation. ABP also ensures that the input is not null.

See the [validation document](Validation.md) for more.

## Authorization

It's possible to use declarative and imperative authorization for application service methods.

See the [authorization document](Authorization.md) for more.

## CRUD Application Services

If you need to create a simple **CRUD application service** which has Create, Update, Delete and Get methods, you can use ABP's **base classes** to easily build your services. You can either inherit from `CrudAppService` or `AsyncCrudAppService`.

### Example

Create an `IBookAppService` interface inheriting from the `IAsyncCrudAppService` interface.

````csharp
public interface IBookAppService : 
    IAsyncCrudAppService< //Defines CRUD methods
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting on getting a list of books
        CreateUpdateBookDto, //Used to create a new book
        CreateUpdateBookDto> //Used to update a book
{
}
````

* `IAsyncCrudAppService` has generic arguments to get the primary key type of the entity and the DTO types for the CRUD operations (it does not get the entity type since the entity type is not exposed to the clients use this interface).

`IAsyncCrudAppService` declares the following methods:

````csharp
public interface IAsyncCrudAppService<
    TEntityDto,
    in TKey,
    in TGetListInput,
    in TCreateInput,
    in TUpdateInput>
    : IApplicationService
    where TEntityDto : IEntityDto<TKey>
{
    Task<TEntityDto> GetAsync(TKey id);

    Task<PagedResultDto<TEntityDto>> GetListAsync(TGetListInput input);

    Task<TEntityDto> CreateAsync(TCreateInput input);

    Task<TEntityDto> UpdateAsync(TKey id, TUpdateInput input);

    Task DeleteAsync(TKey id);
}
````

DTO classes used in this example are `BookDto` and `CreateUpdateBookDto`:

````csharp
[AbpAutoMapFrom(typeof(Book))]
public class BookDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public BookType Type { get; set; }

    public float Price { get; set; }
}

[AbpAutoMapTo(typeof(Book))]
public class CreateUpdateBookDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [Required]
    public BookType Type { get; set; } = BookType.Undefined;

    [Required]
    public float Price { get; set; }
}
````

* `CreateUpdateBookDto` is shared by create and update operations, but you could use separated DTO classes as well.

And finally, the `BookAppService` implementation is very simple:

````csharp
public class BookAppService : 
    AsyncCrudAppService<Book, BookDto, Guid, PagedAndSortedResultRequestDto,
                        CreateUpdateBookDto, CreateUpdateBookDto>,
    IBookAppService
{
    public BookAppService(IRepository<Book, Guid> repository) 
        : base(repository)
    {
    }
}
````

`AsyncCrudAppService` implements all methods declared in the `IAsyncCrudAppService` interface. You can then add your own custom methods or override and customize base methods.

## Lifetime

Lifetime of application services are [transient](Dependency-Injection.md) and they are automatically registered to the dependency injection system.

