# Many to Many Relationship with ABP and EF Core

## Introduction 

In this article, we'll create a **BookStore** application like in [the ABP tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC&DB=EF) and add an extra `Category` feature to demonstrate how we can manage the many-to-many relationship with ABP-based applications (by following DDD rules).

You can see the ER Diagram of our application under below. This diagram will be helpful for us to demonstrate the relations between our entities.

![ER-Diagram](./er-diagram.png)

When we've examined the ER Diagram, we can see the one-to-many relationship between **Author** and **Book** tables and also the many-to-many relationship (**BookCategory** table) between **Book** and **Category** tables. Cause, there can be more than one category on each book and vice-versa in our scenario.

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

* Our project boilerplate will be ready after the download is finished. Then, we can open the solution and starts to the development.

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

* In our scenerio, a book can have more than one category and a category can have more 
than one book so it's obvious to we need to establish many-to-many relationship between them.

* For achieve this we will create an **join entity** named `BookCategory`, and this class
will simply have variables named `BookId` and `CategoryId`.

* To manage this **join entity**, we can add it as sub-collection to the **Book** entity, as we do above. We add this sub-collection
to **Book** class instead of **Category** class, because a book can have tens or mostly hundreds of category in it but in other
view a category can have more than hundred (or even way much) book inside of it.

* It is a significant performance problem to load thousands of items whenever you query a category. Therefore it's make much sense to add that sub-collection to `Book` entity. (Don't forget: **An aggregate (with the root entity and sub-collections) should
be serializable and transferrable on the wire as a single unit.**)

* Notice that, `BookCategory` is not an **Aggregate Root** so we are not violating the one of the base rule about Aggregate Root (Rule: "Reference Other Aggregates Only by ID").

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

* Here, as you can notice we've defined the `BookCategory` as the **Join Table/Entity** for our many-to-many relationship and ensure the required properties (BookId and CategoryId) must be set to create this object.

* And also we've derived this class from `Entity` class and therefore we've had to override the **GetKeys** method of this class to define **Composite Key**.

> The composite key is composed of `BookId` and `CategoryId` in our case. And they are unique with together.

>  For more information about **Entities with Composite Keys**, you can read the relavant section from [Entites documentation](https://docs.abp.io/en/abp/latest/Entities#entities-with-composite-keys)

//TODO: add BookManager class later?

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

### Step 2 - (Database Integration)

### Step 3 - (Database Migration)

### Step 4 - (Create Application Services)

### Step 5 - (UI)
