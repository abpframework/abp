# JSON
The ABP Framework provides an abstraction to work with JSON. Having such an abstraction has some benefits;

* You can write library independent code. Therefore, you can change the underlying library with the minimum effort and code change.
* You can use the predefined converters defined in the ABP without worrying about the underlying library's internal details.

> The JSON serialization system is implemented with the [Volo.Abp.Json](https://www.nuget.org/packages/Volo.Abp.Json) NuGet package([Volo.Abp.Json.SystemTextJson](https://www.nuget.org/packages/Volo.Abp.Json.SystemTextJson) is the default implementation). Most of the time, you don't need to manually [install it](https://abp.io/package-detail/Volo.Abp.Json) since it comes pre-installed with the [application startup template](Startup-Templates/Application.md).

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
* **InputDateTimeFormats(`List<string>`)**: Formats of input JSON date, Empty string means default format. You can provide multiple formats to parse the date.
* **OutputDateTimeFormat(`string`)**: Format of output json date, Null or empty string means default format.

## System Text Json

### AbpSystemTextJsonSerializerOptions

- **JsonSerializerOptions(`System.Text.Json.JsonSerializerOptions`)**: Global options for System.Text.Json library operations. See [here](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions) for reference.

### AbpSystemTextJsonSerializerModifiersOptions

- **Modifiers(`List<Action<JsonTypeInfo>>`)**: Configure `Modifiers` of `DefaultJsonTypeInfoResolver`. See [here](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-6/#json-contract-customization) for reference.


## Newtonsoft

Add [Volo.Abp.Json.Newtonsoft](https://www.nuget.org/packages/Volo.Abp.Json.Newtonsoft) package and depends on `AbpJsonNewtonsoftModule` to replace the `System Text Json`.

#### AbpNewtonsoftJsonSerializerOptions

- **JsonSerializerSettings(`Newtonsoft.Json.JsonSerializerSettings`)**: Global options for Newtonsoft library operations. See [here](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) for reference.

## Configuring JSON options in ASP.NET Core

You can change the JSON behavior in ASP.NET Core by configuring [JsonOptions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.jsonoptions) or
[MvcNewtonsoftJsonOptions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.mvcnewtonsoftjsonoptions)(if you use `Newtonsoft.Json`)

