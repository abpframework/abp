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

- Create your filter dto:

```csharp
public class BookFilterDto : PaginationFilterBase
{

    [StringFilterOptions(StringFilterOption.Contains)]
    public string Name { get; set; }

    [ArraySearchFilter]
    public BookType[] Type { get; set; }

    public Range<DateTime> PublishDate { get; set; }

    public Range<float> Price { get; set; }
}
```

- Replace your `CrudAppService` inheritance with `CrudAutoFiltererAppService` and send your newly created `BookFilterDto` as generic parameter for **TGetListInput**.


```csharp
/* Old State: */
public class BookAppService :
    CrudAppService<
        Book,
        BookDto,
        Guid, 
        PagedAndSortedResultRequestDto,
        CreateUpdateBookDto>,
    IBookAppService
{
    protected BookAppService(IRepository<Book, Guid> repository) : base(repository)
    {
    }
}
```

```csharp
/* New State: */
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

Follow [the AutoFilterer documentation](https://fluentvalidation.net/) to create filter classes.  Example:

ABP will automatically apply filters with your given parameters. You don't need to call `ApplyFilter()` method for AutoFilterer.
