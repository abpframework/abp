```json
//[doc-seo]
{
    "Description": "Learn how to integrate FluentValidation with ABP Framework for enhanced validation capabilities in your applications."
}
```

# FluentValidation Integration

ABP [Validation](./validation.md) infrastructure is extensible. [Volo.Abp.FluentValidation](https://www.nuget.org/packages/Volo.Abp.FluentValidation) NuGet package extends the validation system to work with the [FluentValidation](https://fluentvalidation.net/) library.

## Installation

It is suggested to use the [ABP CLI](../../cli) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.FluentValidation
````

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.FluentValidation](https://www.nuget.org/packages/Volo.Abp.FluentValidation) NuGet package to your project:

   ````
   dotnet add package Volo.Abp.FluentValidation
   ````

2.  Add the `AbpFluentValidationModule` to the dependency list of your module:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpFluentValidationModule) //Add the FluentValidation module
    )]
public class YourModule : AbpModule
{
}
````

## Using the FluentValidation

Follow [the FluentValidation documentation](https://fluentvalidation.net/) to create validator classes.  Example:

````csharp
public class CreateUpdateBookDtoValidator : AbstractValidator<CreateUpdateBookDto>
{
    public CreateUpdateBookDtoValidator()
    {
        RuleFor(x => x.Name).Length(3, 10);
        RuleFor(x => x.Price).ExclusiveBetween(0.0f, 999.0f);
    }
}
````

ABP will automatically find this class and associate with the `CreateUpdateBookDto` on object validation.

## Exposing the Rules in the API Definition

ABP creates the API definition from the data annotation attributes of the DTO properties. So, a DTO that is only validated by FluentValidation has no constraints in the API definition, and the client proxy generators can not see them. The [Volo.Abp.Http.FluentValidation](https://www.nuget.org/packages/Volo.Abp.Http.FluentValidation) NuGet package adds the FluentValidation rules to the API definition, merged with the ones coming from the attributes.

### Installation

Open a command line window in the folder of the project (.csproj file) that hosts your HTTP API and type the following command:

````bash
abp add-package Volo.Abp.Http.FluentValidation
````

If you want to manually install, add the [Volo.Abp.Http.FluentValidation](https://www.nuget.org/packages/Volo.Abp.Http.FluentValidation) NuGet package to your project and add the `AbpHttpFluentValidationModule` to the dependency list of your module:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpHttpFluentValidationModule)
    )]
public class YourModule : AbpModule
{
}
````

`AbpHttpFluentValidationModule` already depends on `AbpFluentValidationModule`, so you don't need both.

### Mapped Rules

The following rules are mapped:

| FluentValidation rule | API definition |
|---|---|
| `NotNull()`, `NotEmpty()` | `IsRequired` |
| `Length(min, max)`, `MinimumLength(min)`, `MaximumLength(max)` | `MinLength`, `MaxLength` |
| `Matches(...)` | `Regex` |
| `GreaterThanOrEqualTo(...)`, `GreaterThan(...)` | `Minimum` (+ `MinimumIsExclusive`) |
| `LessThanOrEqualTo(...)`, `LessThan(...)` | `Maximum` (+ `MaximumIsExclusive`) |
| `InclusiveBetween(...)`, `ExclusiveBetween(...)` | `Minimum`, `Maximum` (+ the exclusive flags) |

`MinimumIsExclusive` and `MaximumIsExclusive` indicate whether the value can be equal to the bound. They are also filled from the `Range` attribute, so an exclusive bound is not lost when it is declared with an attribute.

When a rule and an attribute constrain the same property, the stricter bound is used: the higher minimum and the lower maximum. When both bounds have the same value, the exclusive one is used. The exclusivity always comes from the bound that is used, so `[Range(0, 100)]` with `GreaterThan(-5)` results in an inclusive `Minimum = 0`. A non-numeric bound, like a `Range` attribute on a `DateTime` property, is kept as-is. An existing `Regex` is also kept, because a single value can not express two patterns that both have to match.

### Rules That Are Not Mapped

The following rules are not mapped, because they don't apply to every instance of the DTO:

* Rules under `When(...)` / `Unless(...)` (both the chained and the block form) and their async variants, because the same property can be required for one instance and optional for another.
* Rules that only belong to a non-default rule set, because ABP validates with FluentValidation's default selector, which does not run them.
* `RuleForEach(...)` rules, because they constrain the items of a collection rather than the collection property.
* Comparisons against another property, and any bound that is not a number.

### Rules That Are Not Fully Expressed

The following rules are not fully expressed in the API definition:

* A zero length bound, from `MaximumLength(0)` or `Length(0, 0)`, is not published. Every length rule has a `Func<T, int>` form that reports the same zero on the descriptor, so the two can not be told apart.
* Rules that come from an `Include(...)` call are not published, because FluentValidation does not expose the included validator on its descriptor.
* A validator of a derived DTO can not add rules to a property declared by its base class, because each type describes only its own properties.
* A rule on a nested object, like `RuleFor(x => x.Address.City)`, is not published either. The nested type is described on its own, with its own validator, and its model is shared by every DTO that uses it.
* A validator of a closed generic DTO is not used, because the API definition describes the generic type definition, which is shared by all of its instantiations.
* `Matches(pattern, RegexOptions)` publishes the pattern without the options. This is the one case where a client can be stricter than the server, so avoid the overload if the client should not reject what the server accepts.

> The API definition describes a type, while the server runs the validation per action. So, a DTO that is only used as a return value, or that is sent to an action which doesn't validate its parameters, still declares its constraints here. This is also how the data annotation attributes have always been reported.

## See Also

* [Validation System](./validation.md)