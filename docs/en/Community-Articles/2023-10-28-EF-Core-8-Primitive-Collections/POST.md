# EF Core 8 Primitive collections

What can we do when we want to store a list of primitive types? Before EF Core 8, there were two options:

- Create a wrapper class and a related table, then add a foreign key linking each value to its owner of the collection.
- Use [value converter](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions) to serialize-deserialize as JSON.

The first option covers most scenarios if we need to add some additional properties to this type but let's say we're never gonna need this type additionality. 

## Which collection types are supported ?

EF Core has the capability to map the `IEnumerable<T>` public properties that have both a getter and a setter, with the `T` representing a primitive type

```csharp
public class PrimitiveCollections
{
    public IEnumerable<int> Ints { get; set; }
    public ICollection<string> Strings { get; set; }
    public ISet<DateTime> DateTimes { get; set; }
    public IList<DateOnly> Dates { get; set; }
    public uint[] UnsignedInts { get; set; }
    public List<bool> Booleans { get; set; }
    public List<Uri> Urls { get; set; }
}
```

> Some generic arguments are not considered primitive on the database side, such as `uint` and `Uri`. However, these types are also considered as primitive because there are [built-in value converters](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#built-in-converters).

## Demo 

In this sample, we have a `Car` class with a `Color` enum, and the `Car` class has a `Colors` property.

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
    public ISet<Color> Colors { get; set; } = new HashSet<Color>();

    public Car(string brand, string model)
    {
        Brand = brand;
        Model = model;
    }
}
```

When we want to list the cars if they have any of the specific colors.

```csharp
var colors = new HashSet<Color> { Color.Blue, Color.White };
var cars = await context
    .Cars
    .Where(x=> x.Colors.Intersect(colors).Any())
    .ToListAsync();
```

The SQL result looks like this; as you can see, it sends colors as parameters instead of adding them inline. It also uses the `json_each` function to deserialize on the database side.

```sql
SELECT "c"."Id", "c"."Brand", "c"."Colors", "c"."Model"
      FROM "Cars" AS "c"
      WHERE EXISTS (
          SELECT 1
          FROM (
              SELECT "c0"."value"
              FROM json_each("c"."Colors") AS "c0"
              INTERSECT
              SELECT "c1"."value"
              FROM json_each(@__colors_0) AS "c1"
          ) AS "t")

```

When we insert to the car table.

```csharp
var car = new Car("Maserati", "GranTurismo")
{
    Colors = new HashSet<Color>()
    {
        Color.Black,
        Color.Blue
    }
};
context.Cars.Add(car);
await context.SaveChangesAsync();
```

The SQL statement looks like this, and as you can see, it automatically serializes into the Colors parameter as JSON.

```sql
 Executed DbCommand (0ms) [Parameters=
 [@p0='Maserati' (Nullable = false) (Size = 8),
  @p1='[0,3]' (Nullable = false) (Size = 5),
  @p2='GranTurismo' (Nullable = false) (Size = 4)
 ], CommandType='Text', CommandTimeout='30']
      INSERT INTO "Cars" ("Brand", "Colors", "Model")
      VALUES (@p0, @p1, @p2)
      RETURNING "Id";
```
## Conclusion

We don't need to do anything if we just use a collection of primitive types. It serializes and deserializes them as JSON automatically. Additionally, it sends the primitive collection as a parameter to cache the query.

## References

- [EF Core 8 Primitive collections](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#primitive-collection-properties)
- [.NET Data Community Standup - Collections of primitive values in EF Core](https://www.youtube.com/watch?v=AUS2OZjsA2I)
