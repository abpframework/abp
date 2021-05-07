# JSON
The ABP Framework provides an abstraction to work with JSON. Having such an abstraction has some benefits;

* You can write library independent code. Therefore, you can change the underlying library with the minimum effort and code change.
* You can use the predefined converters defined in the ABP without worrying about the underlying library's internal details.

> The JSON serialization system is implemented with the [Volo.Abp.Json](https://www.nuget.org/packages/Volo.Abp.Json) NuGet package. Most of the time, you don't need to manually [install it](https://abp.io/package-detail/Volo.Abp.Json) since it comes pre-installed with the [application startup template](Startup-Templates/Application.md).

## IJsonSerializer

You can inject `IJsonSerializer` and use it for JSON operations. Here is the available operations in the `IJsonSerializer` interface.

```csharp
public interface IJsonSerializer
{
    string Serialize(object obj, bool camelCase = true, bool indented = false);
    T Deserialize<T>(string jsonString, bool camelCase = true);
    object Deserialize(Type type, string jsonString, bool camelCase = true);
}
```
Usage Example:

```csharp
public class ProductManager
{
    public IJsonSerializer JsonSerializer { get; }

    public ProductManager(IJsonSerializer jsonSerializer)
    {
        JsonSerializer = jsonSerializer;
    }

    public void SendRequest(Product product)
    {
        var json=  JsonSerializer.Serialize(product);
        // Left blank intentionally for demo purposes...
    }
}
```

## Configuration

### AbpJsonOptions

`AbpJsonOptions` type provides options for the JSON operations in the ABP Framework.

Properties:
* **DefaultDateTimeFormat(`string`)**: Default `DateTime` format.
* **UseHybridSerializer(`bool`)**: True by default. Boolean field indicating whether the ABP Framework uses the hybrid approach or not. If the field is true, it will try to use `System.Json.Text` to handle JSON if it can otherwise use the `Newtonsoft.Json.`
* **Providers(`ITypeList<IJsonSerializerProvider>`)**: List of JSON serializer providers implementing the `IJsonSerializerProvider` interface. You can create and add custom serializers to the list, and the ABP Framework uses them automatically. When the `Serialize` or `Deserialize` method is called on the `IJsonSerializer` interface, the ABP Framework calls the `CanHandle` methods of the given providers in reverse order and uses the first provider that returns `true` to do the JSON operation.

### AbpSystemTextJsonSerializerOptions

`AbpSystemTextJsonSerializerOptions` provides options for  `System.Text.Json` usage. 

Properties:

- **JsonSerializerOptions(`System.Text.Json.JsonSerializerOptions`)**: Global options for System.Text.Json library operations. See [here](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions) for reference.
- **UnsupportedTypes(`ITypeList`)**: List of the unsupported types. You can add types of the unsupported types to the list and, the hybrid JSON serializer automatically uses the `Newtonsoft.Json` library instead of `System.Text.Json`.

