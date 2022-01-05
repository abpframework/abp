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
        // ...
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

        // Personalize a configuração para as suas collections.
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
        b.CollectionName = "MyQuestions"; // Define o nome da collection
        b.BsonMap.UnmapProperty(x => x.MyProperty); // Ignora 'MyProperty'
    });
}
````

Este exemplo altera o nome da collection mapeada para 'MyQuestions' no banco de dados e ignora uma propriedade na classe `Question`.

Se você só precisa configurar o nome da collection, você também pode usar o atributo `[MongoCollection]` para a collection em seu DbContext. Exemplo:

````csharp
[MongoCollection("MyQuestions")] // Define o nome da collection
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

            // ...
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

    public BookManager(IRepository<Book, Guid> bookRepository) // injetar repositório padrão
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

        await _bookRepository.InsertAsync(book); // Use um método de repositório padrão

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

Isso é especialmente importante quando você deseja dar **override em um método da base repository** para customizar. Por exemplo, você pode querer substituir o método `DeleteAsync` para excluir uma entidade de uma forma mais eficiente:

```csharp
public async override Task DeleteAsync(
    Guid id,
    bool autoSave = false,
    CancellationToken cancellationToken = default)
{
    // TODO: Implementação customizada do método delete
}
```

### Acesso à API MongoDB

Na maioria dos casos, você vai querer ocultar APIs do MongoDB atrás de uma repository (este é o objetivo principal da repository). No entanto, se você quiser acessar a API MongoDB através da Repository, você pode usar os métodos de extensão `GetDatabaseAsync()`, `GetCollectionAsync()` ou `GetAggregateAsync()`. Exemplo:

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

> Importante: Você deve fazer referência ao pacote `Volo.Abp.MongoDB` do projeto que deseja acessar a API MongoDB. Isso quebra o encapsulamento, mas é o que você deseja nesse caso.

### Transactions

O MongoDB oferece suporte multi-document transactions a partir da versão 4.0 e o ABP Framework oferece suporte para isso. No entanto, o [startup template](Startup-templates/Index.md) **desativa** transactions por padrão. Se o seu **servidor** MongoDB suportar transactions, você pode habilitar esse recurso na classe *YourProjectMongoDbModule*:

```csharp
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
});
```

> Ou você pode excluir este código, pois este já é o comportamento padrão.

### Tópicos Avançados

### Controlando o Multi-Tenancy

Se sua solução for [multi-tenant](Multi-Tenancy.md), os tenants podem ter **bancos de dados separados**, você tem **múltiplas** classes `DbContext` em sua solução e algumas de suas classes `DbContext` devem ser utilizáveis **apenas do lado do host**, é recomendado adicionar o atributo `[IgnoreMultiTenancy]` em sua classe `DbContext`. Nesse caso, a ABP garante que o `DbContext` relacionado sempre usa a [connection string](Connection-Strings.md) do host, mesmo se você estiver em um tenant context.

**Exemplo:**

````csharp
[IgnoreMultiTenancy]
public class MyDbContext : AbpMongoDbContext
{
    ...
}
````

Não use o atributo `[IgnoreMultiTenancy]` se qualquer uma de suas entidades em seu `DbContext` puder ser persistida em um outro banco de dados de um tenant.

> When you use repositories, ABP already uses the host database for the entities don't implement the `IMultiTenant` interface. So, most of time you don't need to `[IgnoreMultiTenancy]` attribute if you are using the repositories to work with the database.

#### Definir Classes Repository Padrão

Repositories genéricas padrão são implementadas pela classe `MongoDbRepository` por padrão. Você pode criar sua própria implementação e usá-la para implementação da repository padrão.

Primeiro, defina suas classes de repository assim:

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

O primeiro é para [entities com chaves compostas](Entities.md), o segundo é para entities com uma única chave primária.

É sugerido herdar da classe `MongoDbRepository` e substituir os métodos, se necessário. Caso contrário, você terá que implementar todos os métodos de repository padrão manualmente.

Agora, você pode usar a opção `SetDefaultRepositoryClasses`:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.SetDefaultRepositoryClasses(
        typeof(MyRepositoryBase<,>),
        typeof(MyRepositoryBase<>)
    );
    // ...
});
```

#### Definir classe base MongoDbContext ou Interface para Repositories padrão

Se seu MongoDbContext herda de outro MongoDbContext ou implementa uma interface, você pode usar essa classe base ou interface como o MongoDbContext para repositories padrão. Exemplo:

```csharp
public interface IBookStoreMongoDbContext : IAbpMongoDbContext
{
    Collection<Book> Books { get; }
}
```

`IBookStoreMongoDbContext` é implementado pela classe `BookStoreMongoDbContext`. Então você pode usar sobrecarga genérica do `AddDefaultRepositories`:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.AddDefaultRepositories<IBookStoreMongoDbContext>();
    // ...
});
```

Agora, seu `BookRepository` personalizado também pode usar a interface `IBookStoreMongoDbContext`:

```csharp
public class BookRepository
    : MongoDbRepository<IBookStoreMongoDbContext, Book, Guid>,
      IBookRepository
{
    // ...
}
```

Uma vantagem de usar a interface para um MongoDbContext é que ela pode ser substituída por outra implementação.

#### Substituir Outros DbContexts

Depois de definir e usar adequadamente uma interface para um MongoDbContext, qualquer outra implementação pode usar as seguintes maneiras de substituí-lo:

**ReplaceDbContextAttribute**

```csharp
[ReplaceDbContext(typeof(IBookStoreMongoDbContext))]
public class OtherMongoDbContext : AbpMongoDbContext, IBookStoreMongoDbContext
{
    // ...
}
```

**Opção ReplaceDbContext**

```csharp
context.Services.AddMongoDbContext<OtherMongoDbContext>(options =>
{
    // ...
    options.ReplaceDbContext<IBookStoreMongoDbContext>();
});
```

Neste exemplo, `OtherMongoDbContext` implementa `IBookStoreMongoDbContext`. Este recurso permite que você tenha vários MongoDbContext (um por módulo) no desenvolvimento, mas um único MongoDbContext (implementa todas as interfaces de todos os MongoDbContexts) no tempo de execução.

### Personalizar Operações em Massa

Se você tiver uma lógica melhor ou usar uma biblioteca externa para operações em massa, pode substituir a lógica por meio da implementação de `IMongoDbBulkOperationProvider`.

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
        // Sua lógica aqui.
    }

    public async Task InsertManyAsync<TEntity>(
        IMongoDbRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        IClientSessionHandle sessionHandle,
        bool autoSave,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity
    {
        // Sua lógica aqui.
    }

    public async Task UpdateManyAsync<TEntity>(
        IMongoDbRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        IClientSessionHandle sessionHandle,
        bool autoSave,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity
    {
        // Sua lógica aqui.
    }
}
```

## Veja Também

* [Entities](Entities.md)
* [Repositories](Repositories.md)