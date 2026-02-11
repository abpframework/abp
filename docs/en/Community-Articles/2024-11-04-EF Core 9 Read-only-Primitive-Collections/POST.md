# EF Core 9 Read-only Primitive Collections

In this article, we will explore the new features introduced in EF Core 9, specifically focusing on Read-only Primitive Collections. EF Core 8 introduced support for mapping arrays and mutable lists of primitive types, and you can read more about it [here](https://abp.io/community/articles/ef-core-8-primitive-collections-ttn5b6xp). This has been expanded in EF Core 9 to include read-only collections/lists. Specifically, EF Core 9 supports collections typed as `IReadOnlyList`, `IReadOnlyCollection`, or `ReadOnlyCollection`.

## Introduction to EF Core 9 Read-only Primitive Collections

Entity Framework Core 9 introduces several enhancements, one of which is the support for Read-only Primitive Collections. This feature aims to provide better support for scenarios where collections of primitive types, such as `int`, `string`, or `bool`, need to be used in a read-only manner in your entity classes. Previously, developers had to use complex workarounds to ensure collections couldn't be modified, but EF Core 9 now provides a simpler, built-in solution to handle this more effectively.

### Why Read-only Primitive Collections Matter

Read-only Primitive Collections are particularly useful when you need to guarantee the integrity of certain data within your entities. For example, imagine you have a `Car` entity that has a collection of `Colors`, represented as a set of enums. You might not want these colors to be modified after they're initially set, ensuring that any business logic reliant on these values remains consistent.

EF Core 9 introduces a convenient way to define these collections as read-only, helping developers maintain stricter control over their data.

### How It Works

Defining a read-only primitive collection is quite straightforward in EF Core 9. You can use the `IReadOnlyList<T>`, `IReadOnlyCollection<T>`, or `ReadOnlyCollection<T>` types to declare your properties, ensuring a consistent read-only behavior. This helps maintain data integrity by preventing modifications after the collection is set. Below is an example that includes a `Car` class and a `Color` enum. The `Car` class has a `Colors` property that holds a read-only list of available colors, ensuring that these values cannot be modified after being initially set:

```csharp
public enum Color
{
    Black,
    White,
    Red,
    Blue
}

public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public IReadOnlyList<Color> Colors { get; private set; } = new List<Color> { Color.Black, Color.White }.AsReadOnly();
  
    protected Car()
    {
    	/* This constructor is for deserialization / ORM purpose */
    }

    public Car(string brand, string model, IEnumerable<Color> colors)
    {
        Brand = brand;
        Model = model;
        Colors = colors.ToList().AsReadOnly();
    }
}
```

In the example above, `Colors` is defined as a read-only list, preventing any accidental modifications once it is set. This ensures that data integrity is maintained without the need for manual validation.

To query cars with specific colors, you can use the following example:

```csharp
var colors = new List<Color> { Color.Black, Color.White };
var cars = await context.Cars
    .Where(c => c.Colors.Intersect(colors).Any())
    .ToListAsync();
```

The query selects all cars that have any of the specified colors in their `Colors` collection.

The SQL result looks like this; as you can see, it sends colors as parameters instead of adding them inline. It also uses the `json_each` function to deserialize on the database side:

```sql
SELECT "c"."id",
       "c"."brand",
       "c"."colors",
       "c"."model"
FROM   "cars" AS "c"
WHERE  EXISTS (SELECT 1
               FROM   (SELECT "c0"."value"
                       FROM   Json_each("c"."colors") AS "c0"
                       INTERSECT
                       SELECT "c1"."value"
                       FROM   Json_each(@__colors_0) AS "c1") AS "i") 
```

### Conclusion

Read-only primitive collections make it easier to enforce data integrity by preventing changes to your collection data. This feature helps simplify your code while ensuring that critical parts of your data remain consistent.

## References

- https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-9.0/whatsnew#read-only-primitive-collections
- https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#primitive-collections
- https://abp.io/community/articles/ef-core-8-primitive-collections-ttn5b6xp
