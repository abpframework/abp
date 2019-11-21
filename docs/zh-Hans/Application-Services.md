## 应用服务

应用服务实现应用程序的**用例**, 将**领域层逻辑公开给表示层**.

从表示层(可选)调用应用服务,**DTO (数据传对象)** 作为参数. 返回(可选)DTO给表示层.

## 示例

### 图书实体

假设你有一个`Book`实体(聚合根), 如下所示:

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

* `Book`实体中定义`MaxNameLength`限制`Name`属性的最大长度.
* `Book`构造函数与`ChangeName`确保`Name`属性值的有效性. 请注意, `Name`的setter不是`public`.

ABP不会强制开发者这样设计实体, 可以将所有的属性设置Public set/get. 由你来决定是否全面实施DDD.

### IBookAppService接口

在ABP中应用程序服务应该实现`IApplicationService接口`. 推荐每个应用程序服务创建一个接口:

````csharp
public interface IBookAppService : IApplicationService
{
    Task CreateAsync(CreateBookDto input);
}
````

我们将实现Create方法作为示例. CreateBookDto定义如下:

```csharp
public class CreateBookDto
{
    [Required]
    [StringLength(Book.MaxNameLength)]
    public string Name { get; set; }

    public BookType Type { get; set; }

    public float? Price { get; set; }
}
```

有关DTO更的教程,请参见[数据传输对象文档](Entities.md)

### BookAppService(实现)

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

* `BookAppService`继承了基类`ApplicationService`· 这不是必需的, 但是`ApplicationService`提供了应用服务常见的需求(比如本示例服务中使用的`GuidGenerator`). 如果不继承它, 我们需要在服务中手动注入`IGuidGenerator`(参见[Guid生成](Guid-Generation.md)文档)
* `BookAppService`按照预期实现了`IBookAppService`
* `BookAppService` 注入了 `IRepository<Book, Guid>`(请参见[仓储](Repositories.md))在CreateAsync方法内部使用仓储将新实体插入数据库.
* `CreateAsync`使用`Book`实体的构造函数从给定的Input值创建新的`Book`对象

### 数据传输对象

应用服务使用并返回DTO而不是实体. ABP不会强制执行此规则. 但是将实体暴露给表示层(或远程客户端)存在重大问题, 所以不建议返回实体.

有关更多信息, 请参见[DTO文档](Entities.md).

### 对象到对象映射

`CreateBook`方法使用参数`CreateBookDto`对象手动创建`Book`实体. 因为`Book`实体的构造函数强制执行(我们是这样设计的).

但是在很多情况下使用**自动对象映射**从相似对象设置对象的属性更加方便实用. ABP提供了一个[对象到对象映射](Object-To-Object-Mapping.md)基础设施,使其变得更加容易.

让我们创建另一种获取`Book`的方法. 首先,在`IBookAppService`接口中定义方法:

````csharp
public interface IBookAppService : IApplicationService
{
    Task CreateAsync(CreateBookDto input);

    Task<BookDto> GetAsync(Guid id); //New method
}
````

`BookDto`是一个简单的[DTO](Data-Transfer-Objects.md)类, 定义如下:

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

* `BookDto`定义了`[AbpAutoMapFrom(typeof(Book))]`属性来从创建对象映射Book到BookDto.

然后你可以实现`GetAsync`方法. 如下所示:

````csharp
public async Task<BookDto> GetAsync(Guid id)
{
    var book = await _bookRepository.GetAsync(id);
    return book.MapTo<BookDto>();
}
````

`MapTo`扩展方法通过复制具有相同命名的所有属性将`Book`对象转换为`BookDto`对象.

`MapTo`的另一种替代方法是使用`IObjectMapper`服务:

````csharp
public async Task<BookDto> GetAsync(Guid id)
{
    var book = await _bookRepository.GetAsync(id);
    return ObjectMapper.Map<Book, BookDto>(book);
}
````

虽然第二种语法编写起来有点困难,但是如果你编写单元测试,它会更好地工作.
有关更多信息,请参阅[对象到对象映射](Object-To-Object-Mapping)文档.

### 验证

自动验证应用服务方法的输入(如ASP.NET Core 控制器的actions). 你可以使用标准数据注释属性或自定义验证方法来执行验证. ABP还确保输入不为空.

请参阅[验证](Validation.md)文档了解更多信息.

### 授权

可以对应用程序服务方法使用声明性和命令式授权.

请参阅[授权](Authorization.md)文档了解更多信息.

### CRUD应用服务

如果需要创建具有Create,Update,Delete和Get方法的简单CRUD应用服务,则可以使用ABP的基类轻松构建服务. 你可以继承CrudAppService或 AsyncCrudAppService.

示例:

创建继承`IAsyncCrudAppService`接口的`IBookAppService`接口.

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

* IAsyncCrudAppService有泛型参数来获取实体的主键类型和CRUD操作的DTO类型(它不获取实体类型,因为实体类型未向客户端公开使用此接口).

`IAsyncCrudAppService`声明以下方法:

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

示例中使用的DTO类是`BookDto`和`CreateUpdateBookDto`:

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

* `CreateUpdateBookDto`由创建和更新操作共享,但你也可以使用单独的DTO类.

最后`BookAppService`实现非常简单:

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

`AsyncCrudAppService`实现了`IAsyncCrudAppService`接口中声明的所有方法. 然后,你可以添加自己的自定义方法或覆盖和自定义实现.

### 生命周期

应用服务的生命周期是[transient](Dependency-Injection)的，它们会自动注册到依赖注入系统.

