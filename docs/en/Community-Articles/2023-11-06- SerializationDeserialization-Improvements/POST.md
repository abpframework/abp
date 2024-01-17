# .NET 8: Serialization Improvements

The [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/api/system.text.json) namespace provides functionality for serializing to and deserializing from JSON. In .NET 8, there have been many improvements to the System.Text.Json serialization and deserialization functionality.

## Source generator

.NET 8 includes enhancements of the System.Text.Json source generator that are aimed at making the Native AOT experience on par with the reflection-based serializer. This is important because now you can select the source generator for System.Text.Json. To see a comparison of Reflection versus source generation in System.Text.Json, check out the comparison [documentation](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/reflection-vs-source-generation?pivots=dotnet-8-0#reflection).

## Interface hierarchies

.NET 8 adds support for serializing properties from interface hierarchies. Here is a code sample that shows this feature.

```csharp
ICar car = new MyCar { Color = "Red", NumberOfWheels = 4 };
JsonSerializer.Serialize(car); // {"Color":"Red","NumberOfWheels":4}

public interface IVehicle
{
    public string Color { get; set; }
}

public interface ICar : IVehicle
{
    public int NumberOfWheels { get; set; }
}

public class MyCar : ICar
{
    public string Color { get; set; }
    public int NumberOfWheels { get; set; }
}
```

## Naming policies

Two new naming policies `snake_case` and `kebab-case` have been added. You can use them as shown below:

```csharp
var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
JsonSerializer.Serialize(new { CreationTime = new DateTime(2023,11,6) }, options); // {"creation_time":"2023-11-06T00:00:00"}
```

## Read-only properties

With .NET 8, you can deserialize onto read-only fields or properties. This feature can be globally enabled by setting `PreferredObjectCreationHandling` to `JsonObjectCreationHandling.Populate`. You can also enable this feature for a class or one of its members by adding the `[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]` attribute. Here is a sample:

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

var book = JsonSerializer.Deserialize<Book>("""{"Contributors":["John Doe"],"Author":{"Name":"Sample Author"}}""")!;
Console.WriteLine(JsonSerializer.Serialize(book));

class Author
{
    public required string Name { get; set; }
}

[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
class Book
{
    // Both of these properties are read-only.
    public List<string> Contributors { get; } = new();
    public Author Author { get; } = new() {Name = "Undefined"};
}
```

Before .NET 8, the output was like this:

```json
{"Contributors":[],"Author":{"Name":"Undefined"}}
```

With .NET 8, the output now looks like this:

```json
{"Contributors":["John Doe"],"Author":{"Name":"Sample Author"}}
```

## Disable reflection-based default

One of the nice features about Serialization is, now you can disable using the reflection-based serializer by default. To disable default reflection-based serialization, set the `JsonSerializerIsReflectionEnabledByDefault` MSBuild property to `false` in your project file.

## Streaming deserialization APIs

.NET 8 includes new `IAsyncEnumerable<T>` streaming deserialization extension methods. The new extension methods invoke streaming APIs and return `IAsyncEnumerable<T>`. Here is a sample code that uses this new feature.

```csharp
const string RequestUri = "https://yourwebsite.com/api/saas/tenants?skipCount=0&maxResultCount=10";
using var client = new HttpClient();
IAsyncEnumerable<Tenant> tenants = client.GetFromJsonAsAsyncEnumerable<Tenant>(RequestUri);

await foreach (Tenant tenant in tenants)
{
    Console.WriteLine($"* '{tenant.name}' uses '{tenant.editionName
}' edition");
}
```

I have mentioned some of the items for Serialization Improvements in .NET 8. If you want to check the full list, you can read it in the .NET 8's [What's new document](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8#serialization).
