# Web应用程序开发教程 - 第八章: 作者: 应用服务层
````json
//[doc-params]
{
    "UI": ["MVC","Blazor","BlazorServer","NG"],
    "DB": ["EF","Mongo"]
}
````
## 关于本教程

在本系列教程中, 你将构建一个名为 `Acme.BookStore` 的用于管理书籍及其作者列表的基于ABP的应用程序.  它是使用以下技术开发的:

* **{{DB_Value}}** 做为ORM提供程序.
* **{{UI_Value}}** 做为UI框架.

本教程分为以下部分:

- [Part 1: 创建服务端](Part-1.md)
- [Part 2: 图书列表页面](Part-2.md)
- [Part 3: 创建,更新和删除图书](Part-2.md)
- [Part 4: 集成测试](Part-4.md)
- [Part 5: 授权](Part-5.md)
- [Part 6: 作者: 领域层](Part-6.md)
- [Part 7: 作者: 数据库集成](Part-7.md)
- **Part 8: 作者: 应用服务层 (本章)**
- [Part 9: 作者: 用户页面](Part-9.md)
- [Part 10: 图书到作者的关系](Part-10.md)

## 下载源码

本教程根据你的**UI** 和 **数据库**偏好有多个版本,我们准备了几种可供下载的源码组合:

* [MVC (Razor Pages) UI 与 EF Core](https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore)
* [Blazor UI 与 EF Core](https://github.com/abpframework/abp-samples/tree/master/BookStore-Blazor-EfCore)
* [Angular UI 与 MongoDB](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)

> 如果你在Windows中遇到 "文件名太长" or "解压错误", 很可能与Windows最大文件路径限制有关. Windows文件路径的最大长度为250字符. 为了解决这个问题,参阅 [在Windows 10中启用长路径](https://docs.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=cmd#enable-long-paths-in-windows-10-version-1607-and-later).

> 如果你遇到与Git相关的长路径错误, 尝试使用下面的命令在Windows中启用长路径. 参阅 https://github.com/msysgit/msysgit/wiki/Git-cannot-create-a-file-or-directory-with-a-long-path
> `git config --system core.longpaths true`

## 简介

这章阐述如何为前一章介绍的 `作者` 实体创建应用服务层.

## IAuthorAppService

我们首先创建 [应用服务](../Application-Services.md) 接口和相关的 [DTO](../Data-Transfer-Objects.md)s. 在 `Acme.BookStore.Application.Contracts` 项目的 `Authors` 命名空间 (文件夹) 创建一个新接口 `IAuthorAppService`:

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Authors
{
    public interface IAuthorAppService : IApplicationService
    {
        Task<AuthorDto> GetAsync(Guid id);

        Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input);

        Task<AuthorDto> CreateAsync(CreateAuthorDto input);

        Task UpdateAsync(Guid id, UpdateAuthorDto input);

        Task DeleteAsync(Guid id);
    }
}
````

* `IApplicationService` 是一个常规接口, 所有应用服务都继承自它, 所以 ABP 框架可以识别它们.
* 在 `Author` 实体中定义标准方法用于CRUD操作.
* `PagedResultDto` 是一个ABP框架中预定义的 DTO 类. 它拥有一个 `Items` 集合 和一个 `TotalCount` 属性, 用于返回分页结果.
* 优先从 `CreateAsync` 方法返回 `AuthorDto` (新创建的作者), 虽然在这个程序中没有这么做 - 这里只是展示一种不同用法.

这个类使用下面定义的DTOs (为你的项目创建它们).

### AuthorDto

````csharp
using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Authors
{
    public class AuthorDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }
    }
}
````

* `EntityDto<T>` 只有一个类型为指定泛型参数的 `Id` 属性. 你可以自己创建 `Id` 属性, 而不是继承自 `EntityDto<T>`.

### GetAuthorListDto

````csharp
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Authors
{
    public class GetAuthorListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
````

* `Filter` 用于搜索作者. 它可以是 `null` (或空字符串) 以获得所有用户.
* `PagedAndSortedResultRequestDto` 具有标准分页和排序属性: `int MaxResultCount`, `int SkipCount` 和 `string Sorting`.

> ABP 框架拥有这些基本的DTO类以简化并标准化你的DTOs. 参阅 [DTO 文档](../Data-Transfer-Objects.md) 获得所有DTO类的详细信息.

### CreateAuthorDto

````csharp
using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore.Authors
{
    public class CreateAuthorDto
    {
        [Required]
        [StringLength(AuthorConsts.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }
    }
}
````

数据标记特性可以用来验证DTO. 参阅 [验证文档](../Validation.md) 获得详细信息.

### UpdateAuthorDto

````csharp
using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore.Authors
{
    public class UpdateAuthorDto
    {
        [Required]
        [StringLength(AuthorConsts.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }
    }
}
````

> 我们可以在创建和更新操作间分享 (重用) 相同的DTO. 虽然可以这么做, 但我们推荐为这些操作创建不同的DTOs, 因为我们发现随着时间的推移, 它们通常会变得有差异. 所以, 与紧耦合相比, 代码重复也是合理的.

## AuthorAppService

是时候实现 `IAuthorAppService` 接口了. 在 `Acme.BookStore.Application` 项目的 `Authors` 命名空间 (文件夹) 中创建一个新类 `AuthorAppService` :

````csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Authors
{
    [Authorize(BookStorePermissions.Authors.Default)]
    public class AuthorAppService : BookStoreAppService, IAuthorAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly AuthorManager _authorManager;

        public AuthorAppService(
            IAuthorRepository authorRepository,
            AuthorManager authorManager)
        {
            _authorRepository = authorRepository;
            _authorManager = authorManager;
        }

        //...SERVICE METHODS WILL COME HERE...
    }
}
````

* `[Authorize(BookStorePermissions.Authors.Default)]` 是一个检查权限(策略)的声明式方法, 用来给当前用户授权. 参阅 [授权文档](../Authorization.md) 获得详细信息. `BookStorePermissions` 类在后文会被更新, 现在不需要担心编译错误.
* 由 `BookStoreAppService` 派生, 这个类是一个简单基类, 可以做为模板. 它继承自标准的 `ApplicationService` 类.
* 实现上面定义的 `IAuthorAppService` .
* 注入 `IAuthorRepository` 和 `AuthorManager` 以使用服务方法.

现在, 我们逐个介绍服务方法. 复制这些方法到 `AuthorAppService` 类.

### GetAsync

````csharp
public async Task<AuthorDto> GetAsync(Guid id)
{
    var author = await _authorRepository.GetAsync(id);
    return ObjectMapper.Map<Author, AuthorDto>(author);
}
````

这个方法根据 `Id` 获得 `Author` 实体, 使用 [对象到对象映射](../Object-To-Object-Mapping.md) 转换为 `AuthorDto`. 这需要配置AutoMapper, 后面会介绍.

### GetListAsync

````csharp
public async Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input)
{
    if (input.Sorting.IsNullOrWhiteSpace())
    {
        input.Sorting = nameof(Author.Name);
    }

    var authors = await _authorRepository.GetListAsync(
        input.SkipCount,
        input.MaxResultCount,
        input.Sorting,
        input.Filter
    );

    var totalCount = input.Filter == null
        ? await _authorRepository.CountAsync()
        : await _authorRepository.CountAsync(
            author => author.Name.Contains(input.Filter));

    return new PagedResultDto<AuthorDto>(
        totalCount,
        ObjectMapper.Map<List<Author>, List<AuthorDto>>(authors)
    );
}
````

* 为处理客户端没有设置的情况, 在方法的开头设置默认排序是 "根据作者名".
* 使用 `IAuthorRepository.GetListAsync` 从数据库中获得分页的, 排序的和过滤的作者列表. 我们已经在教程的前一章中实现了它. 再一次强调, 实际上不需要创建这个方法, 因为我们可以从数据库中直接查询, 这里只是演示如何创建自定义repository方法.
* 直接查询 `AuthorRepository` , 得到作者的数量. 如果客户端发送了过滤条件, 会得到过滤后的作者数量.
* 最后, 通过映射 `Author` 列表到 `AuthorDto` 列表, 返回分页后的结果.

### CreateAsync

````csharp
[Authorize(BookStorePermissions.Authors.Create)]
public async Task<AuthorDto> CreateAsync(CreateAuthorDto input)
{
    var author = await _authorManager.CreateAsync(
        input.Name,
        input.BirthDate,
        input.ShortBio
    );

    await _authorRepository.InsertAsync(author);

    return ObjectMapper.Map<Author, AuthorDto>(author);
}
````

* `CreateAsync` 需要 `BookStorePermissions.Authors.Create` 权限 (另外包括 `AuthorAppService` 类声明的 `BookStorePermissions.Authors.Default` 权限).
* 使用 `AuthorManager` (领域服务) 创建新作者.
* 使用 `IAuthorRepository.InsertAsync` 插入新作者到数据库.
* 使用 `ObjectMapper` 返回 `AuthorDto` , 代表新创建的作者.

> **DDD提示**: 一些开发者可能会发现可以在 `_authorManager.CreateAsync` 插入新实体. 我们认为把它留给应用层是更好的设计, 因为应用层更了解应该何时插入实体到数据库(在插入实体前可能需要额外的工作. 如果在领域层插入, 可能需要额外的更新操作). 但是, 你拥有最终的决定权.

### UpdateAsync

````csharp
[Authorize(BookStorePermissions.Authors.Edit)]
public async Task UpdateAsync(Guid id, UpdateAuthorDto input)
{
    var author = await _authorRepository.GetAsync(id);

    if (author.Name != input.Name)
    {
        await _authorManager.ChangeNameAsync(author, input.Name);
    }

    author.BirthDate = input.BirthDate;
    author.ShortBio = input.ShortBio;

    await _authorRepository.UpdateAsync(author);
}
````

* `UpdateAsync` 需要额外的 `BookStorePermissions.Authors.Edit` 权限.
* 使用 `IAuthorRepository.GetAsync` 从数据库中获得作者实体. 如果给定的id没有找到作者, `GetAsync` 抛出 `EntityNotFoundException`, 这在web应用程序中导致一个 `404` HTTP 状态码. 在更新操作中先获取实体再更新它, 是一个好的实践.
* 如果客户端请求, 使用 `AuthorManager.ChangeNameAsync` (领域服务方法) 修改作者姓名.
* 因为没有任何业务逻辑, 直接更新 `BirthDate` 和 `ShortBio`, 它们可以接受任何值.
* 最后, 调用 `IAuthorRepository.UpdateAsync` 更新实体到数据库.

{{if DB == "EF"}}

> **EF Core 提示**: Entity Framework Core 拥有 **change tracking** 系统并在unit of work 结束时 **自动保存** 任何修改到实体 (你可以简单地认为APB框架在方法结束时自动调用 `SaveChanges`). 所以, 即使你在方法结束时没有调用 `_authorRepository.UpdateAsync(...)` , 它依然可以工作. 如果你不考虑以后修改EF Core, 你可以移除这一行.

{{end}}

### DeleteAsync

````csharp
[Authorize(BookStorePermissions.Authors.Delete)]
public async Task DeleteAsync(Guid id)
{
    await _authorRepository.DeleteAsync(id);
}
````

* `DeleteAsync` 需要额外的 `BookStorePermissions.Authors.Delete` 权限.
* 直接使用repository的 `DeleteAsync` 方法.

## 权限定义

你还不能编译代码, 因为它需要 `BookStorePermissions` 类定义中一些常数.

打开 `Acme.BookStore.Application.Contracts` 项目中的 `BookStorePermissions` 类 (在 `Permissions` 文件夹中), 修改为如下代码:

````csharp
namespace Acme.BookStore.Permissions
{
    public static class BookStorePermissions
    {
        public const string GroupName = "BookStore";

        public static class Books
        {
            public const string Default = GroupName + ".Books";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        // *** ADDED a NEW NESTED CLASS ***
        public static class Authors
        {
            public const string Default = GroupName + ".Authors";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
````

然后打开同一项目中的 `BookStorePermissionDefinitionProvider`, 在 `Define` 方法的结尾加入以下行:

````csharp
var authorsPermission = bookStoreGroup.AddPermission(
    BookStorePermissions.Authors.Default, L("Permission:Authors"));

authorsPermission.AddChild(
    BookStorePermissions.Authors.Create, L("Permission:Authors.Create"));

authorsPermission.AddChild(
    BookStorePermissions.Authors.Edit, L("Permission:Authors.Edit"));

authorsPermission.AddChild(
    BookStorePermissions.Authors.Delete, L("Permission:Authors.Delete"));
````

最后, 在 `Acme.BookStore.Domain.Shared` 项目中的 `Localization/BookStore/en.json` 加入以下项, 用以本地化权限名称:

````csharp
"Permission:Authors": "Author Management",
"Permission:Authors.Create": "Creating new authors",
"Permission:Authors.Edit": "Editing the authors",
"Permission:Authors.Delete": "Deleting the authors"
````

> 简体中文翻译请打开`zh-Hans.json`文件 ,并将"Texts"对象中对应的值替换为中文.

## 对象到对象映射

`AuthorAppService` 使用 `ObjectMapper` 将 `Author` 对象 转换为 `AuthorDto` 对象. 所以, 我们需要在 AutoMapper 配置中定义映射.

打开 `Acme.BookStore.Application` 项目中的 `BookStoreApplicationAutoMapperProfile` 类, 加入以下行到构造函数:

````csharp
CreateMap<Author, AuthorDto>();
````

## 数据种子

如同图书管理部分所做的, 在数据库中生成一些初始作者实体. 不仅当第一次运行应用程序时是有用的, 对自动化测试也是很有用的.

打开 `Acme.BookStore.Domain` 项目中的 `BookStoreDataSeederContributor`, 修改文件内容如下:

````csharp
using System;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore
{
    public class BookStoreDataSeederContributor
        : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly AuthorManager _authorManager;

        public BookStoreDataSeederContributor(
            IRepository<Book, Guid> bookRepository,
            IAuthorRepository authorRepository,
            AuthorManager authorManager)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _authorManager = authorManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _bookRepository.GetCountAsync() <= 0)
            {
                await _bookRepository.InsertAsync(
                    new Book
                    {
                        Name = "1984",
                        Type = BookType.Dystopia,
                        PublishDate = new DateTime(1949, 6, 8),
                        Price = 19.84f
                    },
                    autoSave: true
                );

                await _bookRepository.InsertAsync(
                    new Book
                    {
                        Name = "The Hitchhiker's Guide to the Galaxy",
                        Type = BookType.ScienceFiction,
                        PublishDate = new DateTime(1995, 9, 27),
                        Price = 42.0f
                    },
                    autoSave: true
                );
            }

            // ADDED SEED DATA FOR AUTHORS

            if (await _authorRepository.GetCountAsync() <= 0)
            {
                await _authorRepository.InsertAsync(
                    await _authorManager.CreateAsync(
                        "George Orwell",
                        new DateTime(1903, 06, 25),
                        "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
                    )
                );

                await _authorRepository.InsertAsync(
                    await _authorManager.CreateAsync(
                        "Douglas Adams",
                        new DateTime(1952, 03, 11),
                        "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
                    )
                );
            }
        }
    }
}
````

{{if DB=="EF"}}

你现在可以运行 `.DbMigrator` 控制台应用程序, **迁移** **数据库 schema** 并生成 **种子** 初始数据.

{{else if DB=="Mongo"}}

你现在可以运行 `.DbMigrator` 控制台应用程序, **迁移** **数据库 schema** 并生成 **种子** 初始数据.

{{end}}

## 测试作者应用服务

最后, 你可以为 `IAuthorAppService` 写一些测试. 在 `Acme.BookStore.Application.Tests` 项目的 `Authors` 命名空间(文件夹)中加入一个名为 `AuthorAppService_Tests` 新类:

````csharp
using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Acme.BookStore.Authors
{ {{if DB=="Mongo"}}
    [Collection(BookStoreTestConsts.CollectionDefinitionName)]{{end}}
    public class AuthorAppService_Tests : BookStoreApplicationTestBase
    {
        private readonly IAuthorAppService _authorAppService;

        public AuthorAppService_Tests()
        {
            _authorAppService = GetRequiredService<IAuthorAppService>();
        }

        [Fact]
        public async Task Should_Get_All_Authors_Without_Any_Filter()
        {
            var result = await _authorAppService.GetListAsync(new GetAuthorListDto());

            result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            result.Items.ShouldContain(author => author.Name == "George Orwell");
            result.Items.ShouldContain(author => author.Name == "Douglas Adams");
        }

        [Fact]
        public async Task Should_Get_Filtered_Authors()
        {
            var result = await _authorAppService.GetListAsync(
                new GetAuthorListDto {Filter = "George"});

            result.TotalCount.ShouldBeGreaterThanOrEqualTo(1);
            result.Items.ShouldContain(author => author.Name == "George Orwell");
            result.Items.ShouldNotContain(author => author.Name == "Douglas Adams");
        }

        [Fact]
        public async Task Should_Create_A_New_Author()
        {
            var authorDto = await _authorAppService.CreateAsync(
                new CreateAuthorDto
                {
                    Name = "Edward Bellamy",
                    BirthDate = new DateTime(1850, 05, 22),
                    ShortBio = "Edward Bellamy was an American author..."
                }
            );

            authorDto.Id.ShouldNotBe(Guid.Empty);
            authorDto.Name.ShouldBe("Edward Bellamy");
        }

        [Fact]
        public async Task Should_Not_Allow_To_Create_Duplicate_Author()
        {
            await Assert.ThrowsAsync<AuthorAlreadyExistsException>(async () =>
            {
                await _authorAppService.CreateAsync(
                    new CreateAuthorDto
                    {
                        Name = "Douglas Adams",
                        BirthDate = DateTime.Now,
                        ShortBio = "..."
                    }
                );
            });
        }

        //TODO: Test other methods...
    }
}
````

完成应用服务方法的测试, 它们应该很容易理解.

## 下一章

查看本教程的[下一章](Part-9.md).
