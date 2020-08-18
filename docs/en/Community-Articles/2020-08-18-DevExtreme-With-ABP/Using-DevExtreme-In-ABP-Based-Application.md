## Using DevExtreme in ABP Based Application

Hi, in this article, I will explain some cases of DevExtreme usages in ABP based application.

This article will reference [this](https://github.com/abpframework/abp-samples/tree/master/DevExtreme-Mvc) sample.

## Integrate DevExtreme Packages to ABP Framework Based Applications

Please follow [Intergrate DevExtreme documentation](Intergrate-DevExtreme-To-ABP-Based-Application.md) to see each step. This article will not include setup informations.

## Data Storage

We will use an in-memory list for using data storage for this sample.

There is a `SampleDataService.cs` file in `Data` folder at `*.Application.Contracts` project. We store all sample data here.

We did not create `Entities` etc. Because we want to show "How to use DevExtreme?", because of that, in this sample we focused to application and UI layer.

## JSON Serialization

You can see some `[JsonProperty(Name = "OrderId")]` attributes at DTO's. In this sample, we use that attribute on DTO's properties because DevExtreme official resource is suggesting to _disable the conversion in the JSON serializer_ [(ref)](https://js.devexpress.com/Documentation/19_1/Guide/Angular_Components/Visual_Studio_Integration/Add_DevExtreme_to_an_ASP.NET_Core_Angular_Application/#Troubleshooting). **DO NOT DO THAT!**

If you change **the conversion in the JSON serializer**, some pre-build abp modules may occur a problem.

## MVC

You can use some DevExtreme functions to create UI. The following code blocks show you how you can use it with ABP Applicaion Services.

```csharp
Html.DevExtreme().DataGrid<Order>()
            .DataSource(d => d.Mvc()
                .Controller("Order") // Application Service Name 'without **AppService**'
                .LoadAction("GetOrders") // Method Name 'without **Async**'
                .InsertAction("InsertOrder")
                .UpdateAction("UpdateOrder")
                .DeleteAction("DeleteOrder")
                .Key("OrderID")
            )
```

```csharp
public class OrderAppService : DevExtremeSampleAppService, IOrderAppService
{
    public async Task<LoadResult> GetOrdersAsync(DataSourceLoadOptions loadOptions)
    {
        ...
    }

    public async Task<Order> InsertOrder(string values)
    {
        ...
    }
    ...
}
```
