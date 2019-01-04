## ASP.NET Core MVC 教程 - 第三章

### 关于本教程

这是本教程所有章节中的第三章.下面是所有的章节:

- [Part I: 创建项目和书籍列表页面](Part-I.md)
- [Part II: 创建,编辑,删除书籍](Part-II.md)
- **Part III: 集成测试(本章)**

你可以从 [这里](https://github.com/volosoft/abp/tree/master/samples/BookStore) 下载本程序的**源码**.

### 解决方案中的测试项目

本解决方案中有两个测试项目:

![bookstore-test-projects](images/bookstore-test-projects.png)

* `Acme.BookStore.Application.Tests` 项目用于单元测试和集成测试.你可以在这个项目中为Application Service方法写测试代码.这个项目使用了 **EF Core SQLite in-memory** 数据库.
* `Acme.BookStore.Web.Tests` 项目用于包含Web层的完整集成测试.所以,你也可以在这里写关于UI页面的测试.

测试项目使用了以下库:

* [xunit](https://xunit.github.io/) 作为主测试框架.
* [Shoudly](http://shouldly.readthedocs.io/en/latest/) 作为断言库.
* [NSubstitute](http://nsubstitute.github.io/) 作为模拟库.

### 添加测试用数据

起始模板在 `Acme.BookStore.Application.Tests` 项目中包含了 `BookStoreTestDataBuilder` 类,用于创建一些测试用数据. 相关代码如下所示:

````C#
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Acme.BookStore
{
    public class BookStoreTestDataBuilder : ITransientDependency
    {
        private readonly IIdentityDataSeeder _identityDataSeeder;

        public BookStoreTestDataBuilder(IIdentityDataSeeder identityDataSeeder)
        {
            _identityDataSeeder = identityDataSeeder;
        }

        public void Build()
        {
            AsyncHelper.RunSync(BuildInternalAsync);
        }

        public async Task BuildInternalAsync()
        {
            await _identityDataSeeder.SeedAsync("1q2w3E*");
        }
    }
}
````

* 这里直接使用了identity模块实现的 `IIdentityDataSeeder` 接口,创建了一个admin角色和admin用户.你同样可以在你的测试代码中直接使用这些代码.
* 你可以在 `BuildInternalAsync` 方法中添加你自己的测试数据.

按下方所示修改 `BookStoreTestDataBuilder` 类:

````C#
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Acme.BookStore
{
    public class BookStoreTestDataBuilder : ITransientDependency
    {
        private readonly IIdentityDataSeeder _identityDataSeeder;
        private readonly IRepository<Book, Guid> _bookRepository;

        public BookStoreTestDataBuilder(
            IIdentityDataSeeder identityDataSeeder,
            IRepository<Book, Guid> bookRepository)
        {
            _identityDataSeeder = identityDataSeeder;
            _bookRepository = bookRepository;
        }

        public void Build()
        {
            AsyncHelper.RunSync(BuildInternalAsync);
        }

        public async Task BuildInternalAsync()
        {
            await _identityDataSeeder.SeedAsync("1q2w3E*");

            await _bookRepository.InsertAsync(
                new Book
                {
                    Id = Guid.NewGuid(),
                    Name = "Test book 1",
                    Type = BookType.Fantastic,
                    PublishDate = new DateTime(2015, 05, 24),
                    Price = 21
                }
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    Id = Guid.NewGuid(),
                    Name = "Test book 2",
                    Type = BookType.Science,
                    PublishDate = new DateTime(2014, 02, 11),
                    Price = 15
                }
            );
        }
    }
}
````

* 通过构造函数注入 `IRepository<Book, Guid>`,在 `BuildInternalAsync` 方法中用它创建两个book实体.

### 测试 BookAppService

在 `Acme.BookStore.Application.Tests` 项目中创建一个名叫 `BookAppService_Tests` 的测试类:

````C#
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace Acme.BookStore
{
    public class BookAppService_Tests : BookStoreApplicationTestBase
    {
        private readonly IBookAppService _bookAppService;

        public BookAppService_Tests()
        {
            _bookAppService = GetRequiredService<IBookAppService>();
        }

        [Fact]
        public async Task Should_Get_List_Of_Books()
        {
            //Act
            var result = await _bookAppService.GetListAsync(
                new PagedAndSortedResultRequestDto()
            );

            //Assert
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(b => b.Name == "Test book 1");
        }
    }
}
````

* 测试方法 `Should_Get_List_Of_Books` 直接使用 `BookAppService.GetListAsync` 方法来获取用户列表,并执行检查.

新增测试方法,用以测试创建一个合法book实体的场景:

````C#
[Fact]
public async Task Should_Create_A_Valid_Book()
{
    //Act
    var result = await _bookAppService.CreateAsync(
        new CreateUpdateBookDto
        {
            Name = "New test book 42",
            Price = 10,
            PublishDate = DateTime.Now,
            Type = BookType.ScienceFiction
        }
    );

    //Assert
    result.Id.ShouldNotBe(Guid.Empty);
    result.Name.ShouldBe("New test book 42");
}
````

新增测试方法,用以测试创建一个非法book实体失败的场景:

````C#
[Fact]
public async Task Should_Not_Create_A_Book_Without_Name()
{
    var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
    {
        await _bookAppService.CreateAsync(
            new CreateUpdateBookDto
            {
                Name = "",
                Price = 10,
                PublishDate = DateTime.Now,
                Type = BookType.ScienceFiction
            }
        );
    });

    exception.ValidationErrors
        .ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
}
````

* 由于 `Name` 是空值, ABP 抛出一个 `AbpValidationException` 异常.

### 测试 Web 页面

TODO
