```json
//[doc-params]
{
    "BlazorUI": ["Blazorise", "MudBlazor"]
}
```

```json
//[doc-seo]
{
    "Description": "Learn how to implement form validation in ABP Blazor UI using Blazorise or MudBlazor with practical examples."
}
```

# Blazor UI: Forms & Validation

{{if BlazorUI == "Blazorise"}}

ABP Blazor UI is based on the [Blazorise](https://blazorise.com/docs) and does not have a built-in form validation infrastructure. However, you can use the [Blazorise validation infrastructure](https://blazorise.com/docs/components/validation) to validate your forms.

## Sample

_The example is provided by official Blazorise documentation._

```html
<Validation Validator="ValidationRule.IsNotEmpty">
    <TextEdit Placeholder="Enter name">
        <Feedback>
            <ValidationNone>Please enter the name.</ValidationNone>
            <ValidationSuccess>Name is good.</ValidationSuccess>
            <ValidationError>Enter valid name!</ValidationError>
        </Feedback>
    </TextEdit>
</Validation>

<Validation Validator="ValidateEmail">
    <TextEdit Placeholder="Enter email">
        <Feedback>
            <ValidationNone>Please enter the email.</ValidationNone>
            <ValidationSuccess>Email is good.</ValidationSuccess>
            <ValidationError>Enter valid email!</ValidationError>
        </Feedback>
    </TextEdit>
</Validation>
@code{
    void ValidateEmail( ValidatorEventArgs e )
    {
        var email = Convert.ToString( e.Value );

        e.Status = string.IsNullOrEmpty( email ) ? ValidationStatus.None :
            email.Contains( "@" ) ? ValidationStatus.Success : ValidationStatus.Error;
    }
}
```

> Check the [Blazorise documentation](https://blazorise.com/docs/components/validation) for more information and examples.

{{end}}

{{if BlazorUI == "MudBlazor"}}

ABP Blazor UI built on top of [MudBlazor](https://mudblazor.com) uses MudBlazor's built-in form components and validation infrastructure. MudBlazor accepts a `ValidationAttribute` (e.g. `[Required]`, `[EmailAddress]` from ASP.NET Core's `DataAnnotations`) on the input's `Validation` parameter, plus custom `Func<T, string>` / `Func<T, IEnumerable<string>>` delegates. FluentValidation can be plugged in the same way.

## Sample

The most common pattern is wrapping inputs in a `<MudForm>` and binding the form's validation state through `IsValid`:

> Standard MudBlazor and ABP usings (`@using MudBlazor`, `@using Volo.Abp.MudBlazorUI`, etc.) come from the project's `_Imports.razor`. The example below only adds the additional usings needed for validation.

```razor
@using System.ComponentModel.DataAnnotations

<MudForm @ref="_form" @bind-IsValid="@_isValid" Model="@_model">
    <MudStack Spacing="3">
        <MudTextField @bind-Value="_model.Name"
                      Label="Name"
                      Required="true"
                      RequiredError="Please enter the name." />

        <MudTextField @bind-Value="_model.Email"
                      Label="Email"
                      Required="true"
                      Validation="@(new EmailAddressAttribute() { ErrorMessage = "Enter a valid email." })" />

        <MudButton OnClick="@SubmitAsync"
                   Disabled="@(!_isValid)"
                   Variant="Variant.Filled"
                   Color="Color.Primary">
            Submit
        </MudButton>
    </MudStack>
</MudForm>

@code {
    private MudForm _form;
    private bool _isValid;
    private SampleModel _model = new();

    private async Task SubmitAsync()
    {
        await _form.Validate();
        if (_isValid)
        {
            // ...
        }
    }

    public class SampleModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
```

### Inputs Used in CRUD Pages

ABP's MudBlazor CRUD pages (see `AbpMudCrudPageBase`) use a `<MudDialog>` containing a `<MudForm>` and standard MudBlazor inputs:

* `<MudTextField>` / `<MudTextField Lines="N">` for text and multi-line text
* `<MudSelect>` / `<MudSelectItem>` for dropdowns
* `<MudCheckBox>` for booleans
* `<MudDatePicker>` / `<MudTimePicker>` for date and time
* `<MudNumericField>` for numbers

`AbpMudCrudPageBase.CreateEntityAsync` and `UpdateEntityAsync` validate the form for you (`CreateFormRef.Validate()` / `EditFormRef.Validate()`) and only call the corresponding hook when the form is valid. To inject custom logic before the application service call, override `OnCreatingEntityAsync` / `OnUpdatingEntityAsync` (do **not** re-validate inside the override):

```csharp
protected override Task OnCreatingEntityAsync()
{
    // mutate NewEntity here if needed
    return base.OnCreatingEntityAsync();
}
```

> Check the [MudBlazor documentation](https://mudblazor.com/components/form) for the full list of validation modes and the [MudBlazor inputs reference](https://mudblazor.com/components/textfield).

{{end}}