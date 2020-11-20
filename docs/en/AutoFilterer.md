# AutoFilterer Integration

You can use [AutoFilterer](https://github.com/enisn/AutoFilterer) library to filter your entities without writing any LINQ code. [Volo.Abp.AutoFilterer](https://www.nuget.org/packages/Volo.Abp.AutoFilterer) NuGet package allows to use AutoFilterer with ABP Framework.

## Installation

It is suggested to use the [ABP CLI](CLI.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.AutoFilterer
````

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.AutoFilterer](https://www.nuget.org/packages/Volo.Abp.AutoFilterer) NuGet package to your project:

   ````
   Install-Package Volo.Abp.AutoFilterer
   ````

2.  Add the `AbpAutoFiltererModule` to the dependency list of your module:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpAutoFiltererModule) // <-- Add the Autofilterer module.
    )]
public class YourModule : AbpModule
{
}
````

## Using the AutoFilterer

### AbpFilterBase

- Create your filter dto via inheriting from `AbpFilterBase` instead of AutoFilterer's default `FilterBase`.

```csharp
public class BookFilterDto : AbpFilterBase // <-- Careful here
{
    [CompareTo("Title", "Description")] // Properties of Entity to compare.
    [StringFilterOptions(StringFilterOption.Contains)] // Use Contains method instead of exact value.
    public string Filter { get; set; }

    [ArraySearchFilter] // Gets only these types of books.
    public BookType[] Type { get; set; }

    public Range<float> Price { get; set; } // To filter between Price range.

    /* ... 
     * Any other properties to filter.
     */
}
```

- Replace your `CrudAppService` inheritance with `CrudAutoFiltererAppService` and send your newly created `BookFilterDto` as generic parameter for **TGetListInput**.


```csharp
public class BookAppService :
    CrudAutoFiltererAppService<  // Change inheritance
        Book,
        BookDto,
        Guid, 
        BookFilterDto,  // Send your filter dto as generic parameter
        CreateUpdateBookDto>,
    IBookAppService
{
    protected BookAppService(IRepository<Book, Guid> repository) : base(repository)
    {
    }
}
```

---

### AbpPaginationFilterBase

- Even you can use `AbpPaginationFilterBase` to use AutoFilter's paging & sorting algorithm. When you use `AbpFilterBase`, paging and sorting will be provided over **IPagedAndSortedResultRequest**  by Abp Framework.

```csharp
// ** You should define possible sorting parameters:
[PossibleSortings(typeof(BookDto))] // Each property of return dto.
// [PossibleSortings("Title", "Price", "CreationTime")] // <-- Or one by one
public class BookFilterDto : AbpPaginationFilterBase // <-- Inherit from
{
    [CompareTo("Title", "Description")] // Properties of Entity to compare.
    [StringFilterOptions(StringFilterOption.Contains)] // Use Contains method instead of exact value.
    public string Filter { get; set; }

    [ArraySearchFilter] // Gets only these types of books.
    public BookType[] Type { get; set; }

    public Range<float> Price { get; set; } // To filter between Price range.

    /* ... 
     * Any other properties to filter.
     */
}
```
---

Follow [the AutoFilterer documentation](https://github.com/enisn/AutoFilterer/wiki) to create filter classes.  Example:

ABP will automatically apply filters with your given parameters. You don't need to call `ApplyFilter()` method for AutoFilterer.
