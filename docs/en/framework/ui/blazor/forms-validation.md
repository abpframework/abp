# Blazor UI: Forms & Validation

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