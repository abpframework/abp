# Dynamic Widget

CMS kit provides a widget system to allow generate dynamic widgets in pages and blog posts. Here is a completed example widget screenshot for the poll on the `Page` side.

> Important Note: The poll widget in ABP Commercial is the only widget implemented

![cmskit-example-output-on-page](../../images/cmskit-example-output-on-page.png)

> Also, you can do the same thing for other widgets. It's just an example.

To see the above image, you should go page or blogpost to create or update one. Then click the `W` button to add a dynamic widget like the below image. Don't forget please this is design mode and you need to view your page in view mode after saving. Also `Preview` tab on the editor will be ready to check your output easily for widget configurations in the next features.

![cmskit-add-widget-on-page](../../images/cmskit-add-widget-on-page.png)

In this image, after choosing your poll (on the other case, it changes automatically up to your configuration, mine is the poll widget. Its parameter name `editorWidgetName`) you will see the next widget. Enter input values or choose them and click `Add`. You will see the below output

> [Widget Type="Poll" Code="SelectedValue"]

You can edit this output manually if do any wrong coding for that (wrong value or typo) you won't see the widget, even so, your page will be viewed successfully. 

## Options 
To see the above image must have been configured in YourModule.cs 

```csharp
Configure<CmsKitContentWidgetOptions>(options =>
    {
        options.AddWidget("widgetKey", "widgetName", "editorWidgetName");
    }); 
```

Let's look at these parameters in detail
* `widgetKey` is used for end-user and more readable names. The following bold word represents widgetKey.
    [Widget Type="**Poll**" Code="SelectedValue"]

* `widgetName` is used for your widget name used in code via [Widget] attribute.
```csharp
[Widget]
public class WidgetNameViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
    
```
* `editorWidgetName` is used the for editor component side to see on the `Add Widget` modal.
After choosing the widget type from listbox and renders this widget automatically.

If you need more detail on creating and using widgets please click [here](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Widgets).