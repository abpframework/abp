```json
//[doc-params]
{
    "BlazorUI": ["Blazorise", "MudBlazor"]
}
```

```json
//[doc-seo]
{
    "Description": "Explore the submit button component in Blazor UI, designed for easy form submissions with localization support and loading indicators."
}
```

# Blazor UI: SubmitButton Component

{{if BlazorUI == "Blazorise"}}

`SubmitButton` is a simple wrapper around `Button` component. It is used to be placed inside of page Form or Modal dialogs where it can response to user actions and to be activated as a default button by pressing an ENTER key. Once clicked it will go into the `disabled` state and also it will show a small loading indicator until clicked event is finished.

## Quick Example

```html
<SubmitButton Clicked="@YourSaveOperation" />
```

Notice that we didn't specify any text, like `Save Changes`. This is because `SubmitButton` will by default pull text from the localization. If you want to change that you either specify a localization key or you can add custom content.

### With localization key

```html
<SubmitButton Clicked="@YourSaveOperation" SaveResourceKey="YourSaveName" />
```

### With custom content

```html
<SubmitButton Clicked="@YourSaveOperation">
    @L["Save"]
</SubmitButton>
```

{{end}}

{{if BlazorUI == "MudBlazor"}}

The MudBlazor variant of ABP UI does not ship a dedicated `SubmitButton` wrapper. Use the standard `MudButton` together with the typical `Processing`/`Disabled` pattern to disable the button and show a progress indicator while the click handler is running.

## Quick Example

```razor
<MudButton OnClick="@SaveAsync"
           Variant="Variant.Filled"
           Color="Color.Primary"
           Disabled="@_processing">
    @if (_processing)
    {
        <MudProgressCircular Size="Size.Small" Indeterminate="true" Class="me-2" />
    }
    @L["Save"]
</MudButton>

@code {
    private bool _processing;

    private async Task SaveAsync()
    {
        _processing = true;
        try
        {
            // ... your save operation
        }
        finally
        {
            _processing = false;
        }
    }
}
```

## Submit on Enter

When the button is placed inside a `<MudForm>` or a `<MudDialog>`, pressing ENTER inside an input control submits the form. To run validation before saving, call `_form.Validate()` first. See the [Forms & Validation](../forms-validation.md) page for details.

## Use Inside `AbpMudCrudPageBase`

The MudBlazor CRUD page base (`AbpMudCrudPageBase`) already wires up the standard create/update buttons inside its dialogs and shows a progress indicator while the application service call is running. In most cases you don't need to author a save button by hand; override `OnCreatingEntityAsync` / `OnUpdatingEntityAsync` instead.

> Check the [MudBlazor button documentation](https://mudblazor.com/components/button) for all available options.

{{end}}