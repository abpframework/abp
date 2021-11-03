# Many to Many Relationship with ABP and EF Core

## Introduction 

In this article, we'll create a **BookStore** application like in [the ABP tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC&DB=EF) and add an extra `Category` feature to demonstrate how we can manage the many-to-many relationship with ABP-based applications (by following DDD rules).

You can see the ER Diagram of our application below. This diagram will be helpful for us to demonstrate the relations between our entities.

![ER-Diagram](./er-diagram.png)

When we've examined the ER Diagram, we can see the one-to-many relationship between **Author** and **Book** tables and also the many-to-many relationship (**BookCategory** table) between **Book** and **Category** tables. There can be more than one category on each book and vice-versa in our scenario.

### Source Code

You can find the source code of the application at https://github.com/EngincanV/ABP-Many-to-Many-Relationship-Demo .

### Screenshot of The Final Application

At the end of this article, we will have created an application as in the below image.

![Book Homepage](./demo.png)

## Creating the Solution

In this article, I will create a new startup template with EF Core as a database provider and MVC for UI framework.

* We can create a new startup template by using the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI):

```bash
abp new BookStore -t app --version 5.0.0-beta.2
```

* Our project boilerplate will be ready after the download is finished. Then, we can open the solution and starts the development.

## Starting the Development

Let's start with creating our Domain Entities. 

### Step 1 - (Creating the Domain Entities)

We can create a folder-structure under the `BookStore.Domain` project like in the below image.

![Domain-Layer-Folder-Structure](./domain-file-structure.png)

Open the entity classes and add the following codes to each of these classes.

* **Author.cs**

```csharp
using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BookStore.Authors
{
    public class Author : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        
        public DateTime BirthDate { get; set; }
        
        public string ShortBio { get; set; }
        
        /* This constructor is for deserialization / ORM purpose */
        private Author()
        {
        }

        public Author(Guid id, [NotNull] string name, DateTime birthDate, [CanBeNull] string shortBio = null)
            : base(id)
        {
            SetName(name);
            BirthDate = birthDate;
            ShortBio = shortBio;
        }

        public void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AuthorConsts.MaxNameLength
            );
        }
    }
}
```

> We'll create the `AuthorConsts` class later in this step.

* **Book.cs**

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BookStore.Books
{
    public class Book : FullAuditedAggregateRoot<Guid>
    {
        public Guid AuthorId { get; set; }

        public string Name { get; private set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }

        public ICollection<BookCategory> Categories { get; private set; }

        private Book()
        {
        }

        public Book(Guid id, Guid authorId, string name, DateTime publishDate, float price) 
            : base(id)
        {
            AuthorId = authorId;
            SetName(name);
            PublishDate = publishDate;
            Price = price;

            Categories = new Collection<BookCategory>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), BookConsts.MaxNameLength);
        }

        public void AddCategory(Guid categoryId)
        {
            Check.NotNull(categoryId, nameof(categoryId));

            if (IsInCategory(categoryId))
            {
                return;
            }
            
            Categories.Add(new BookCategory(bookId: Id, categoryId));
        }

        public void RemoveCategory(Guid categoryId)
        {
            Check.NotNull(categoryId, nameof(categoryId));

            if (!IsInCategory(categoryId))
            {
                return;
            }

            Categories.RemoveAll(x => x.CategoryId == categoryId);
        }

        public void RemoveAllCategoriesExceptGivenIds(List<Guid> categoryIds)
        {
            Check.NotNullOrEmpty(categoryIds, nameof(categoryIds));
            
            Categories.RemoveAll(x => !categoryIds.Contains(x.CategoryId));
        }

        public void RemoveAllCategories()
        {
            Categories.RemoveAll(x => x.BookId == Id);
        }

        private bool IsInCategory(Guid categoryId)
        {
            return Categories.Any(x => x.CategoryId == categoryId);
        }
    }
}
```

* In our scenario, a book can have more than one category and a category can have more than one book so we need to create a many-to-many relationship between them.

* For achieving this, we will create a **join entity** named `BookCategory`, and this class will simply have variables named `BookId` and `CategoryId`.

* To manage this **join entity**, we can add it as a sub-collection to the **Book** entity, as we do above. We add this sub-collection
to **Book** class instead of **Category** class, because a book can have tens (or mostly hundreds) of categories but on the other perspective a category can have more than a hundred (or even way much) books inside of it.

* It is a significant performance problem to load thousands of items whenever you query a category. Therefore it makes much more sense to add that sub-collection to the `Book` entity. (Don't forget: **An aggregate (with the root entity and sub-collections) should be serializable and transferrable on the wire as a single unit.**)

* Notice that, `BookCategory` is not an **Aggregate Root** so we are not violating one of the base rules about Aggregate Root (Rule: "Reference Other Aggregates Only by ID").

* If we examine the methods in the `Book` class (such as **RemoveAllCategories**, **RemoveAllCategoriesExceptGivenIds** and **AddCategory**) we will manage our sub-collection `Categories` (**BookCategory** - join table/entity) through them. (Adds or removes categories for books)

> We'll create the `BookCategory` and `BookConsts` classes later in this step.

* **BookCategory.cs**

```csharp
using System;
using Volo.Abp.Domain.Entities;

namespace BookStore.Books
{
    public class BookCategory : Entity
    {
        public Guid BookId { get; protected set; }

        public Guid CategoryId { get; protected set; }

        /* This constructor is for deserialization / ORM purpose */
        private BookCategory()
        {
        }

        public BookCategory(Guid bookId, Guid categoryId)
        {
            BookId = bookId;
            CategoryId = categoryId;
        }
        
        public override object[] GetKeys()
        {
            return new object[] {BookId, CategoryId};
        }
    }
}
```

* Here, as you can notice we've defined the `BookCategory` as the **Join Table/Entity** for our many-to-many relationship and ensure the required properties (BookId and CategoryId) must be set in the constructor method of this class to create this object.

* And also we've derived this class from the `Entity` class and therefore we've had to override the **GetKeys** method of this class to define **Composite Key**.

> The composite key is composed of `BookId` and `CategoryId` in our case. And they are unique together.

>  For more information about **Entities with Composite Keys**, you can read the relavant section from [Entites documentation](https://docs.abp.io/en/abp/latest/Entities#entities-with-composite-keys)

* **BookManager.cs**

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Categories;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace BookStore.Books
{
    public class BookManager : DomainService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;

        public BookManager(IBookRepository bookRepository, IRepository<Category, Guid> categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task CreateAsync(Guid authorId, string name, DateTime publishDate, float price, [CanBeNull]string[] categoryNames)
        {
            var book = new Book(GuidGenerator.Create(), authorId, name, publishDate, price);

            await SetCategoriesAsync(book, categoryNames);
            
            await _bookRepository.InsertAsync(book);
        }

        public async Task UpdateAsync(
            Book book, 
            Guid authorId,
            string name, 
            DateTime publishDate, 
            float price,
            [CanBeNull] string[] categoryNames
        )
        {
            book.AuthorId = authorId;
            book.SetName(name);
            book.PublishDate = publishDate;
            book.Price = price;
            
            await SetCategoriesAsync(book, categoryNames);

            await _bookRepository.UpdateAsync(book);
        }
        
        private async Task SetCategoriesAsync(Book book, [CanBeNull] string[] categoryNames)
        {
            if (categoryNames == null || !categoryNames.Any())
            {
                book.RemoveAllCategories();
                return;
            }

            var query = (await _categoryRepository.GetQueryableAsync())
                .Where(x => categoryNames.Contains(x.Name))
                .Select(x => x.Id)
                .Distinct();

            var categoryIds = await AsyncExecuter.ToListAsync(query);
            if (!categoryIds.Any())
            {
                return;
            }

            book.RemoveAllCategoriesExceptGivenIds(categoryIds);

            foreach (var categoryId in categoryIds)
            {
                book.AddCategory(categoryId);
            }
        }
    }
}
```

* If we examine the codes in the `BookManager` class, we can see that we've managed the `BookCategory` class (our join table/entity)
by using some methods that we've defined in the `Book` class such as **RemoveAllCategories**, **RemoveAllCategoriesExceptGivenIds** and **AddCategory**.

* These methods basically add or remove categories related to the book by conditions.

* In the `CreateAsync` method, if the category names are specified we are retrieving their ids from the database and by using the **AddCategory** method that we've defined in the `Book` class, we're adding them.

* In the `UpdateAsync` method, the same logic is also valid. But in this case, the user could want to remove some categories from books, so if the user sends us an empty **categoryNames** array, we remove all categories from the book he wants to update. If the user sends us some category names, we remove the excluded ones and add the new ones according to **categoryNames** array.

* **BookWithDetails.cs**

```csharp
using System;
using Volo.Abp.Auditing;

namespace BookStore.Books
{
    public class BookWithDetails : IHasCreationTime
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }

        public string AuthorName { get; set; }

        public string[] CategoryNames { get; set; }
        
        public DateTime CreationTime { get; set; }
    }
}
```

* We will use this class to retrieve books with their sub-categories and author names.

* **IBookRepository.cs**

```csharp
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace BookStore.Books
{
    public interface IBookRepository : IRepository<Book, Guid>
    {
        Task<List<BookWithDetails>> GetListAsync(
            string sorting,
            int skipCount,
            int maxResultCount,
            CancellationToken cancellationToken = default
        );

        Task<BookWithDetails> GetAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
```

* We need to create two methods named **GetListAsync** and **GetAsync** and specify their return type as `BookWithDetails`. So by implementing these methods, we will return the book/books by their details (author name and categories).

* **Category.cs**

```csharp
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BookStore.Categories
{
    public class Category : AuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private Category()
        {
        }

        public Category(Guid id, string name) : base(id)
        {
            SetName(name);
        }

        public Category SetName(string name)
        { 
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CategoryConsts.MaxNameLength);
            return this;
        }
    }
}
```

### Step 2 - (Define Consts)

We can create a folder-structure under the `BookStore.Domain.Shared` project like in the below image.

![Domain Shared File Structure](./domain-shared-file-structure.png)

* **AuthorConsts.cs**

```csharp
namespace BookStore.Authors
{
    public class AuthorConsts
    {
        public const int MaxNameLength = 128;

        public const int MaxShortBioLength = 256;
    }
}
```

* **BookConsts.cs**

```csharp
namespace BookStore.Books
{
    public class BookConsts
    {
        public const int MaxNameLength = 128;
    }
}
```

* **CategoryConsts.cs**

```csharp
namespace BookStore.Categories
{
    public class CategoryConsts
    {
        public const int MaxNameLength = 64;
    }
}
```

* In these classes, we've defined max text length for our entity properties that we will use in the **Database Integration** section to specify limits for our properties. (E.g. varchar(128) for BookName)

### Step 3 - (Database Integration)

* After defining our entities, we can configure them for the database integration. 
Open the `BookStoreDbContext` class in the `BookStore.EntityFrameworkCore` project and update with the following code blocks.

```csharp
namespace BookStore.EntityFrameworkCore
{
    [ReplaceDbContext(typeof(IIdentityDbContext))]
    [ReplaceDbContext(typeof(ITenantManagementDbContext))]
    [ConnectionStringName("Default")]
    public class BookStoreDbContext : 
        AbpDbContext<BookStoreDbContext>,
        IIdentityDbContext,
        ITenantManagementDbContext
    {
        //...

        //DbSet properties for our Aggregate Roots
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        //NOTE: We don't need to add DbSet<BookCategory>, because we will be query it via using the Book entity
        // public DbSet<BookCategory> BookCategories { get; set; }

        //...

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //...

            /* Configure your own tables/entities inside here */
            builder.Entity<Author>(b =>
            {
                b.ToTable(BookStoreConsts.DbTablePrefix + "Authors" + BookStoreConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Name)
                    .HasMaxLength(AuthorConsts.MaxNameLength)
                    .IsRequired();

                b.Property(x => x.ShortBio)
                    .HasMaxLength(AuthorConsts.MaxShortBioLength)
                    .IsRequired();
            });

            builder.Entity<Book>(b =>
            {
                b.ToTable(BookStoreConsts.DbTablePrefix + "Books" + BookStoreConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Name)
                    .HasMaxLength(BookConsts.MaxNameLength)
                    .IsRequired();

                //one-to-many relationship with Author table
                b.HasOne<Author>().WithMany().HasForeignKey(x => x.AuthorId).IsRequired();

                //many-to-many relationship with Category table => BookCategories
                b.HasMany(x => x.Categories).WithOne().HasForeignKey(x => x.BookId).IsRequired();
            });

            builder.Entity<Category>(b =>
            {
                b.ToTable(BookStoreConsts.DbTablePrefix + "Categories" + BookStoreConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Name)
                    .HasMaxLength(CategoryConsts.MaxNameLength)
                    .IsRequired();
            });

            builder.Entity<BookCategory>(b =>
            {
                b.ToTable(BookStoreConsts.DbTablePrefix + "BookCategories" + BookStoreConsts.DbSchema);
                b.ConfigureByConvention();

                //define composite key
                b.HasKey(x => new { x.BookId, x.CategoryId });

                //many-to-many configuration
                b.HasOne<Book>().WithMany(x => x.Categories).HasForeignKey(x => x.BookId).IsRequired();
                b.HasOne<Category>().WithMany().HasForeignKey(x => x.CategoryId).IsRequired();
                
                b.HasIndex(x => new { x.BookId, x.CategoryId });
            });
        }
    }
}
```

* In this class, we've defined **DbSet** properties for our **Aggregate Roots** (**Book**, **Author** and **Category**). Notice, we didn't define **DbSet** for `BookCategory` class (our join table/entity). Because, the `Book` aggregate is responsible to manage it via sub-collection.

* After that, we can use the **FluentAPI** to configure our tables in the `OnModelCreating` method of this class.

```csharp
builder.Entity<Book>(b =>
{
    //...

    //one-to-many relationship with Author table
    b.HasOne<Author>().WithMany().HasForeignKey(x => x.AuthorId).IsRequired();

    //many-to-many relationship with Category table => BookCategories
    b.HasMany(x => x.Categories).WithOne().HasForeignKey(x => x.BookId).IsRequired();
});
```

* Here, we have provided the one-to-many relationship between the **Book** and the **Author** in the above code-block. 

```csharp
builder.Entity<BookCategory>(b =>
{
    //...

    //define composite key
    b.HasKey(x => new { x.BookId, x.CategoryId });

    //many-to-many configuration
    b.HasOne<Book>().WithMany(x => x.Categories).HasForeignKey(x => x.BookId).IsRequired();
    b.HasOne<Category>().WithMany().HasForeignKey(x => x.CategoryId).IsRequired();
                
    b.HasIndex(x => new { x.BookId, x.CategoryId });
});
```

* Here, firstly we've defined the composite key for our `BookCategory` entity. `BookId` and `CategoryId` are together composite keys for the `BookCategory` table. Then we've configured the many-to-many relationship between `Book` and `Category` table like in the above code-block.

#### Implementing the `IBookRepository` Interface

* After making the relevant configurations for database integration, we can now implement the `IBookRepository` interface. To do this, create a folder named `Books` in the `BookStore.EntityFrameworkCore` project and inside of this folder create a class named `EfCoreBookRepository` and update this class with the following code. 

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using BookStore.Authors;
using BookStore.Categories;
using BookStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BookStore.Books
{
    public class EfCoreBookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
    {
        public EfCoreBookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<BookWithDetails>> GetListAsync(
            string sorting, 
            int skipCount, 
            int maxResultCount, 
            CancellationToken cancellationToken = default
        )
        {
            var query = await ApplyFilterAsync();
            
            return await query
                .OrderBy(!string.IsNullOrWhiteSpace(sorting) ? sorting : nameof(Book.Name))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<BookWithDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilterAsync();
            
            return await query
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        private async Task<IQueryable<BookWithDetails>> ApplyFilterAsync()
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync())
                .Include(x => x.Categories)
                .Join(dbContext.Set<Author>(), book => book.AuthorId, author => author.Id,
                    (book, author) => new {book, author})
                .Select(x => new BookWithDetails
                {
                    Id = x.book.Id,
                    Name = x.book.Name,
                    Price = x.book.Price,
                    PublishDate = x.book.PublishDate,
                    CreationTime = x.book.CreationTime,
                    AuthorName = x.author.Name,
                    CategoryNames = (from bookCategories in x.book.Categories
                        join category in dbContext.Set<Category>() on bookCategories.CategoryId equals category.Id
                        select category.Name).ToArray()
                });
        }

        public override Task<IQueryable<Book>> WithDetailsAsync()
        {
            return base.WithDetailsAsync(x => x.Categories);
        }
    }
}
```

* Here we've implemented our custom repository methods and returned the book with details (author name and categories).

### Step 4 - (Database Migration)

* We've integrated our entities with the database in the previous step, now we can create a new database migration and apply it to the database. So let's do that.

* Open the `BookStore.EntityFrameworkCore` project in the terminal. And create a new database migration by using the following command.

```bash
dotnet ef migrations add <Migration_Name>
```

* Then, run the `BookStore.DbMigrator` application to create the database.

### Step 5 - (Create Application Services)



### Step 6 - (UI)
