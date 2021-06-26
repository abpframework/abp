# Integração do MongoDB

Este documento explica como integrar e configurar o MongoDB como um provedor de banco de dados para aplicações baseadas no ABP.

## Instalação

`Volo.Abp.MongoDB` é o pacote nuget principal para a integração do MongoDB. Instale-o em seu projeto (para uma aplicação em camadas, será sua camada de dados ou infraestrutura):

```
Install-Package Volo.Abp.MongoDB
```

Então adicione a dependência de módulo `AbpMongoDbModule` para o seu [module](Module-Development-Basics.md):

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

## Criando um Mongo Db Context

ABP apresenta o conceito **Mongo Db Context** (que é semelhante ao Entity Framework Core DbContext) para tornar mais fácil usar coleções e configurá-las. Um exemplo é mostrado abaixo:

```c#
public class MyDbContext : AbpMongoDbContext
{
    public IMongoCollection<Question> Questions => Collection<Question>();

    public IMongoCollection<Category> Categories => Collection<Category>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        //Personalize a configuração para as suas collections.
    }
}
```

* É derivado da classe `AbpMongoDbContext`.
* Adiciona uma propriedade pública `IMongoCollection<TEntity>` para cada mongo collection. Por padrão ABP usa essas propriedades para criar repositórios padrão.
* Substituir o método `CreateModel` permite definir a configuração da collection.

### Configurar o Mapeamento para uma Collection

ABP registra automaticamente entidades MongoDB client library para todas propriedades `IMongoCollection<TEntity>` em seu DbContext. Para o exemplo acima, as entidades `Question` e `Category` são registradas automaticamente.

Para cada entidade registrada, chama `AutoMap()` e configura propriedades conhecidas de sua entidade. Por exemplo, se sua entidade implementa uma interface `IHasExtraProperties` (que já está implementada para cada raiz agregada por padrão), ele configura automaticamente `ExtraProperties`.

Portanto, na maioria das vezes, você não precisa configurar explicitamente o registro de suas entidades. No entanto, se você precisar, pode fazer isso sobrescrevendo o método `CreateModel` em seu DbContext. Exemplo:

````csharp
protected override void CreateModel(IMongoModelBuilder modelBuilder)
{
    base.CreateModel(modelBuilder);

    modelBuilder.Entity<Question>(b =>
    {
        b.CollectionName = "MyQuestions"; //Define o nome da collection
        b.BsonMap.UnmapProperty(x => x.MyProperty); //Ignora 'MyProperty'
    });
}
````

Este exemplo altera o nome da collection mapeada para 'MyQuestions' no banco de dados e ignora uma propriedade na classe `Question`.

Se você só precisa configurar o nome da collection, você também pode usar o atributo `[MongoCollection]` para a collection em seu DbContext. Exemplo:

````csharp
[MongoCollection("MyQuestions")] //Define o nome da collection
public IMongoCollection<Question> Questions => Collection<Question>();
````

### Configurar a Seleção da String de Conexão

Se você tiver vários bancos de dados em seu aplicativo, você pode configurar o nome da string de conexão para o seu DbContext usando o atributo `[ConnectionStringName]`. Exemplo:

````csharp
[ConnectionStringName("MySecondConnString")]
public class MyDbContext : AbpMongoDbContext
{

}
````

Se você não configurar, a string de conexão `Default` é usada. Se você configurar um nome de string de conexão, mas não definir este nome da string de conexão na configuração da aplicação, então ele retorna para a string de conexão `Default`.

## Registrando DbContext para Injeção de Dependência

Use o método `AddAbpDbContext` em seu module para registrar sua classe DbContext para o sistema de [injeção de dependência](Dependency-Injection.md).

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

### Adicionar Repositories Padrão

O ABP pode criar automaticamente [repositories genéricas](Repositories.md) padrão para as entidades em seu DbContext. Basta usar a opção `AddDefaultRepositories()` no registro:

````C#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

Isso criará uma repository para cada [aggregate root entity](Entities.md) (classes derivadas de `AggregateRoot`) por padrão. Se você quiser criar repositories para outras entidades também, em seguida defina `includeAllEntities` para `true`:

```c#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
```

Então você pode injetar e usar `IRepository<TEntity, TPrimaryKey>` nas suas services. Suponha que você tenha uma entidade `Book` com a chave primária `Guid`:

```csharp
public class Book : AggregateRoot<Guid>
{
    public string Name { get; set; }

    public BookType Type { get; set; }
}
```

(`BookType` é um simples `enum` aqui) E você deseja criar uma nova entidade `Book` em uma [domain service](Domain-Services.md):

```csharp
public class BookManager : DomainService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookManager(IRepository<Book, Guid> bookRepository) //injetar repositório padrão
    {
        _bookRepository = bookRepository;
    }

    public async Task<Book> CreateBook(string name, BookType type)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var book = new Book
        {
            Id = GuidGenerator.Create(),
            Name = name,
            Type = type
        };

        await _bookRepository.InsertAsync(book); //Use um método de repositório padrão

        return book;
    }
}
```

Este exemplo usa o método `InsertAsync` para inserir uma nova entity no banco de dados.

### Adicionar Repositories Personalizadas

Repositories genéricas padrão são poderosas e suficiente na maioria dos casos (uma vez que implementam `IQueryable`). No entanto, pode ser necessário criar uma repository customizada para adicionar seus próprios métodos de repository.

Suponha que você deseja excluir todos os books por type. É sugerido definir uma interface para sua repository personalizada:

```csharp
public interface IBookRepository : IRepository<Book, Guid>
{
    Task DeleteBooksByType(
        BookType type,
        CancellationToken cancellationToken = default(CancellationToken)
    );
}
```

Você geralmente vai querer derivar de `IRepository` para herdar métodos de repository padrão. No entanto, você não precisa. As interfaces de repository são definidas na camada de domínio de uma aplicação em camadas. Elas são implementadas na camada de dados ou infraestrutura (projeto `MongoDB` em um [startup template](https://abp.io/Templates)).

Exemplo de implementação da interface `IBookRepository`:

```csharp
public class BookRepository :
    MongoDbRepository<BookStoreMongoDbContext, Book, Guid>,
    IBookRepository
{
    public BookRepository(IMongoDbContextProvider<BookStoreMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task DeleteBooksByType(
        BookType type,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var collection = await GetCollectionAsync(cancellationToken);
        await collection.DeleteManyAsync(
            Builders<Book>.Filter.Eq(b => b.Type, type),
            cancellationToken
        );
    }
}
```

Agora é possível [injetar](Dependency-Injection.md) a `IBookRepository` e usar o método `DeleteBooksByType` quando necessário.

#### Substituir Repository Genérica Padrão

Mesmo se você criar uma repository personalizada, você ainda pode injetar a repository genérica padrão (`IRepository<Book, Guid>` para este exemplo). A implementação da repository padrão não usará a classe que você criou.

Se você querer substituir a implementação da repository padrão pela sua repository personalizada, faça dentro das opções `AddMongoDbContext`:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.AddDefaultRepositories();
    options.AddRepository<Book, BookRepository>(); //Replaces IRepository<Book, Guid>
});
```

This is especially important when you want to **override a base repository method** to customize it. For instance, you may want to override `DeleteAsync` method to delete an entity in a more efficient way:

```csharp
public async override Task DeleteAsync(
    Guid id,
    bool autoSave = false,
    CancellationToken cancellationToken = default)
{
    //TODO: Custom implementation of the delete method
}
```

### Access to the MongoDB API

In most cases, you want to hide MongoDB APIs behind a repository (this is the main purpose of the repository). However, if you want to access the MongoDB API over the repository, you can use `GetDatabaseAsync()`, `GetCollectionAsync()` or `GetAggregateAsync()` extension methods. Example:

```csharp
public class BookService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookService(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task FooAsync()
    {
        IMongoDatabase database = await _bookRepository.GetDatabaseAsync();
        IMongoCollection<Book> books = await _bookRepository.GetCollectionAsync();
        IAggregateFluent<Book> bookAggregate = await _bookRepository.GetAggregateAsync();
    }
}
```

> Important: You must reference to the `Volo.Abp.MongoDB` package from the project you want to access to the MongoDB API. This breaks encapsulation, but this is what you want in that case.

### Transactions

MongoDB supports multi-document transactions starting from the version 4.0 and the ABP Framework supports it. However, the [startup template](Startup-templates/Index.md) **disables** transactions by default. If your MongoDB **server** supports transactions, you can enable the it in the *YourProjectMongoDbModule* class:

```csharp
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
});
```

> Or you can delete this code since this is already the default behavior.

### Tópicos Avançados

### Controlling the Multi-Tenancy

If your solution is [multi-tenant](Multi-Tenancy.md), tenants may have **separate databases**, you have **multiple** `DbContext` classes in your solution and some of your `DbContext` classes should be usable **only from the host side**, it is suggested to add `[IgnoreMultiTenancy]` attribute on your `DbContext` class. In this case, ABP guarantees that the related `DbContext` always uses the host [connection string](Connection-Strings.md), even if you are in a tenant context.

**Example:**

````csharp
[IgnoreMultiTenancy]
public class MyDbContext : AbpMongoDbContext
{
    ...
}
````

Do not use the `[IgnoreMultiTenancy]` attribute if any one of your entities in your `DbContext` can be persisted in a tenant database.

> When you use repositories, ABP already uses the host database for the entities don't implement the `IMultiTenant` interface. So, most of time you don't need to `[IgnoreMultiTenancy]` attribute if you are using the repositories to work with the database.

#### Set Default Repository Classes

Default generic repositories are implemented by `MongoDbRepository` class by default. You can create your own implementation and use it for default repository implementation.

First, define your repository classes like that:

```csharp
public class MyRepositoryBase<TEntity>
    : MongoDbRepository<BookStoreMongoDbContext, TEntity>
    where TEntity : class, IEntity
{
    public MyRepositoryBase(IMongoDbContextProvider<BookStoreMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}

public class MyRepositoryBase<TEntity, TKey>
    : MongoDbRepository<BookStoreMongoDbContext, TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public MyRepositoryBase(IMongoDbContextProvider<BookStoreMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
```

First one is for [entities with composite keys](Entities.md), second one is for entities with single primary key.

It's suggested to inherit from the `MongoDbRepository` class and override methods if needed. Otherwise, you will have to implement all standard repository methods manually.

Now, you can use `SetDefaultRepositoryClasses` option:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.SetDefaultRepositoryClasses(
        typeof(MyRepositoryBase<,>),
        typeof(MyRepositoryBase<>)
    );
    //...
});
```

#### Definir classe base MongoDbContext ou Interface para Repositories padrão

If your MongoDbContext inherits from another MongoDbContext or implements an interface, you can use that base class or interface as the MongoDbContext for default repositories. Example:

```csharp
public interface IBookStoreMongoDbContext : IAbpMongoDbContext
{
    Collection<Book> Books { get; }
}
```

`IBookStoreMongoDbContext` is implemented by the `BookStoreMongoDbContext` class. Then you can use generic overload of the `AddDefaultRepositories`:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.AddDefaultRepositories<IBookStoreMongoDbContext>();
    //...
});
```

Now, your custom `BookRepository` can also use the `IBookStoreMongoDbContext` interface:

```csharp
public class BookRepository
    : MongoDbRepository<IBookStoreMongoDbContext, Book, Guid>,
      IBookRepository
{
    //...
}
```

One advantage of using interface for a MongoDbContext is then it becomes replaceable by another implementation.

#### Substituir Outros DbContexts

Once you properly define and use an interface for a MongoDbContext , then any other implementation can use the following ways to replace it:

**ReplaceDbContextAttribute**

```csharp
[ReplaceDbContext(typeof(IBookStoreMongoDbContext))]
public class OtherMongoDbContext : AbpMongoDbContext, IBookStoreMongoDbContext
{
    //...
}
```

**ReplaceDbContext option**

```csharp
context.Services.AddMongoDbContext<OtherMongoDbContext>(options =>
{
    //...
    options.ReplaceDbContext<IBookStoreMongoDbContext>();
});
```

In this example, `OtherMongoDbContext` implements `IBookStoreMongoDbContext`. This feature allows you to have multiple MongoDbContext (one per module) on development, but single MongoDbContext (implements all interfaces of all MongoDbContexts) on runtime.

### Personalizar Operações em Massa

If you have better logic or using an external library for bulk operations, you can override the logic via implementing `IMongoDbBulkOperationProvider`.

- Você pode usar o modelo de exemplo abaixo:

```csharp
public class MyCustomMongoDbBulkOperationProvider
    : IMongoDbBulkOperationProvider, ITransientDependency
{
    public async Task DeleteManyAsync<TEntity>(
        IMongoDbRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        IClientSessionHandle sessionHandle,
        bool autoSave,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity
    {
        // Your logic here.
    }

    public async Task InsertManyAsync<TEntity>(
        IMongoDbRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        IClientSessionHandle sessionHandle,
        bool autoSave,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity
    {
        // Your logic here.
    }

    public async Task UpdateManyAsync<TEntity>(
        IMongoDbRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        IClientSessionHandle sessionHandle,
        bool autoSave,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity
    {
        // Your logic here.
    }
}
```

## Veja Também

* [Entities](Entities.md)
* [Repositories](Repositories.md)