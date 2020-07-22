# Web Application Development Tutorial - Part 10: Book to Author Relation
````json
//[doc-params]
{
    "UI": ["MVC","NG"],
    "DB": ["EF","Mongo"]
}
````
{{
if UI == "MVC"
  UI_Text="mvc"
else if UI == "NG"
  UI_Text="angular"
else
  UI_Text="?"
end
if DB == "EF"
  DB_Text="Entity Framework Core"
else if DB == "Mongo"
  DB_Text="MongoDB"
else
  DB_Text="?"
end
}}

## About This Tutorial

In this tutorial series, you will build an ABP based web application named `Acme.BookStore`. This application is used to manage a list of books and their authors. It is developed using the following technologies:

* **{{DB_Text}}** as the ORM provider. 
* **{{UI_Value}}** as the UI Framework.

This tutorial is organized as the following parts;

- [Part 1: Creating the server side](Part-1.md)
- [Part 2: The book list page](Part-2.md)
- [Part 3: Creating, updating and deleting books](Part-3.md)
- [Part 4: Integration tests](Part-4.md)
- [Part 5: Authorization](Part-5.md)
- [Part 6: Authors: Domain layer](Part-6.md)
- [Part 7: Authors: Database Integration](Part-7.md)
- [Part 8: Authors: Application Layer](Part-8.md)
- [Part 9: Authors: User Interface](Part-9.md)
- **Part 10: Book to Author Relation (this part)**

### Download the Source Code

This tutorials has multiple versions based on your **UI** and **Database** preferences. We've prepared two combinations of the source code to be downloaded:

* [MVC (Razor Pages) UI with EF Core](https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore)
* [Angular UI with MongoDB](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)

## Introduction

We have created `Book` and `Author` functionalities for the book store application. However, currently there is no relation between these entities.

In this tutorial, we will establish a **1 to N** relation between the `Book` and the `Author`.

## Add Relation to The Book Entity

Open the `Books/Book.cs` in the `Acme.BookStore.Domain` project and add the following property to the `Book` entity:

````csharp
public Guid AuthorId { get; set; }
````

## Database Migration

Added a new, required `AuthorId` property to the `Book` entity. But, what about the existing books on the database? They currently don't have `AuthorId`s and this will be a problem when we try to run the application.

This is a typical migration problem and the decision depends on your case;

* If you haven't published your application to the production yet, you can just delete existing books in the database, or you can even delete the entire database in your development environment.
* You can do it programmatically on data migration or seed phase.
* You can manually handle it on the database.

We prefer to drop the database (run the `Drop-Database` in the *Package Manager Console*) since this is just an example project and data loss is not important. Since this topic is not related to the ABP Framework, we don't go deeper for all scenarios.

{{if DB=="EF"}}

### Update the EF Core Mapping

Open the `BookStoreDbContextModelCreatingExtensions` class under the `EntityFrameworkCore` folder of the `Acme.BookStore.EntityFrameworkCore` project and change the `builder.Entity<Book>` part as shown below:

````csharp
builder.Entity<Book>(b =>
{
    b.ToTable(BookStoreConsts.DbTablePrefix + "Books", BookStoreConsts.DbSchema);
    b.ConfigureByConvention(); //auto configure for the base class props
    b.Property(x => x.Name).IsRequired().HasMaxLength(128);
    
    // ADD THE MAPPING FOR THE RELATION
    b.HasOne<Author>().WithMany().HasForeignKey(x => x.AuthorId).IsRequired();
});
````

### Add New EF Core Migration

Run the following command in the Package Manager Console (of the Visual Studio) to add a new database migration:

````bash
Add-Migration "Added_AuthorId_To_Book"
````

This should create a new migration class with the following code in its `Up` method:

````csharp
migrationBuilder.AddColumn<Guid>(
    name: "AuthorId",
    table: "AppBooks",
    nullable: false,
    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

migrationBuilder.CreateIndex(
    name: "IX_AppBooks_AuthorId",
    table: "AppBooks",
    column: "AuthorId");

migrationBuilder.AddForeignKey(
    name: "FK_AppBooks_AppAuthors_AuthorId",
    table: "AppBooks",
    column: "AuthorId",
    principalTable: "AppAuthors",
    principalColumn: "Id",
    onDelete: ReferentialAction.Cascade);
````

* Adds an `AuthorId` field to the `AppBooks` table.
* Creates an index on the `AuthorId` field.
* Declares the foreign key to the `AppAuthors` table.

{{end}}

## Change the Data Seeder

Since the `AuthorId` is a required property of the `Book` entity, current data seeder code can not work. Open the `BookStoreDataSeederContributor` in the `Acme.BookStore.Domain` project and change as the following:

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
            if (await _bookRepository.GetCountAsync() > 0)
            {
                return;
            }

            var orwell = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "George Orwell",
                    new DateTime(1903, 06, 25),
                    "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
                )
            );

            var douglas = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "Douglas Adams",
                    new DateTime(1952, 03, 11),
                    "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
                )
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    AuthorId = orwell.Id, // SET THE AUTHOR
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
                    AuthorId = douglas.Id, // SET THE AUTHOR
                    Name = "The Hitchhiker's Guide to the Galaxy",
                    Type = BookType.ScienceFiction,
                    PublishDate = new DateTime(1995, 9, 27),
                    Price = 42.0f
                },
                autoSave: true
            );
        }
    }
}
````

The only change is that we set the `AuthorId` properties of the `Book` entities.

{{if DB=="EF"}}

You can now run the `.DbMigrator` console application to **migrate** the **database schema** and **seed** the initial data.

{{else if DB="Mongo"}}

You can now run the `.DbMigrator` console application to **seed** the initial data.

{{end}}

## Application Layer

We will change the `BookAppService` to support the Author relation.

### Data Transfer Objects

Let's begin from the DTOs.

#### BookDto

Open the `BookDto` class in the `Books` folder of the `Acme.BookStore.Application.Contracts` project and add the following properties:

```csharp
public Guid AuthorId { get; set; }
public string AuthorName { get; set; }
```

The final `BookDto` class should be following:

```csharp
using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public class BookDto : AuditedEntityDto<Guid>
    {
        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
    }
}
```

#### CreateUpdateBookDto

Open the `CreateUpdateBookDto` class in the `Books` folder of the `Acme.BookStore.Application.Contracts` project and add an `AuthorId` property as shown:

````csharp
public Guid AuthorId { get; set; }
````

#### AuthorLookupDto

Create a new class, `AuthorLookupDto`, inside the `Books` folder of the `Acme.BookStore.Application.Contracts` project:

````csharp
using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public class AuthorLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
````

This will be used in a new method will be added to the `IBookAppService`.

### IBookAppService

Open the `IBookAppService` interface in the `Books` folder of the `Acme.BookStore.Application.Contracts` project and add a new method, named `GetAuthorLookupAsync`, as shown below:

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    public interface IBookAppService :
        ICrudAppService< //Defines CRUD methods
            BookDto, //Used to show books
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateBookDto> //Used to create/update a book
    {
        // ADD the NEW METHOD
        Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync();
    }
}
````

This new method will be used from the UI to get a list of authors and fill a dropdown list to select the author of a book.

### BookAppService

Open the `BookAppService` interface in the `Books` folder of the `Acme.BookStore.Application` project and replace the file content with the following code:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Books
{
    [Authorize(BookStorePermissions.Books.Default)]
    public class BookAppService :
        CrudAppService<
            Book, //The Book entity
            BookDto, //Used to show books
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateBookDto>, //Used to create/update a book
        IBookAppService //implement the IBookAppService
    {
        private readonly IAuthorRepository _authorRepository;

        public BookAppService(
            IRepository<Book, Guid> repository,
            IAuthorRepository authorRepository)
            : base(repository)
        {
            _authorRepository = authorRepository;
            GetPolicyName = BookStorePermissions.Books.Default;
            GetListPolicyName = BookStorePermissions.Books.Default;
            CreatePolicyName = BookStorePermissions.Books.Create;
            UpdatePolicyName = BookStorePermissions.Books.Edit;
            DeletePolicyName = BookStorePermissions.Books.Create;
        }

        public override async Task<BookDto> GetAsync(Guid id)
        {
            //Prepare a query to join books and authors
            var query = from book in Repository
                join author in _authorRepository on book.AuthorId equals author.Id
                where book.Id == id
                select new { book, author };

            //Execute the query and get the book with author
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Book), id);
            }

            var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
            bookDto.AuthorName = queryResult.author.Name;
            return bookDto;
        }

        public override async Task<PagedResultDto<BookDto>>
            GetListAsync(PagedAndSortedResultRequestDto input)
        {
            //Prepare a query to join books and authors
            var query = from book in Repository
                join author in _authorRepository on book.AuthorId equals author.Id
                orderby input.Sorting
                select new {book, author};

            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            //Execute the query and get a list
            var queryResult = await AsyncExecuter.ToListAsync(query);

            //Convert the query result to a list of BookDto objects
            var bookDtos = queryResult.Select(x =>
            {
                var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
                bookDto.AuthorName = x.author.Name;
                return bookDto;
            }).ToList();

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<BookDto>(
                totalCount,
                bookDtos
            );
        }

        public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
        {
            var authors = await _authorRepository.GetListAsync();

            return new ListResultDto<AuthorLookupDto>(
                ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors)
            );
        }
    }
}
```

Let's see the changes we've done:

* Added `[Authorize(BookStorePermissions.Books.Default)]` to authorize the methods we've newly added/overrode (remember, authorize attribute is valid for all the methods of the class when it is declared for a class).
* Injected `IAuthorRepository` to query from the authors.
* Overrode the `GetAsync` method of the base `CrudAppService`, which returns a single `BookDto` object with the given `id`.
  * Used a simple LINQ expression to join books and authors and query them together for the given book id.
  * Used `AsyncExecuter.FirstOrDefaultAsync(...)` to execute the query and get a result. `AsyncExecuter` was previously used in the `AuthorAppService`. Check the [repository documentation](../Repositories.md) to understand why we've used it.
  * Throws an `EntityNotFoundException` which results an `HTTP 404` (not found) result if requested book was not present in the database.
  * Finally, created a `BookDto` object using the `ObjectMapper`, then assigning the `AuthorName` manually.
* Overrode the `GetListAsync` method of the base `CrudAppService`, which returns a list of books. The logic is similar to the previous method, so you can easily understand the code.
* Created a new method: `GetAuthorLookupAsync`. This simple gets all the authors. The UI uses this method to fill a dropdown list and select and author while creating/editing books.





### Object to Object Mapping Configuration

