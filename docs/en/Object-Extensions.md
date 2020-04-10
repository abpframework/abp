# Object Extensions

ABP Framework provides an **object extension system** to allow you to **add extra properties** to an existing object **without modifying** the related class. This allows to extend functionalities implemented by a depended [application module](Modules/Index.md), especially when you want to [extend entities](Customizing-Application-Modules-Extending-Entities.md) and [DTOs](Customizing-Application-Modules-Overriding-Services.md) defined by the module.

> Object extension system is not normally not needed for your own objects since you can easily add regular properties to your own classes.

## IHasExtraProperties Interface

This is the interface to make a class extensible. It simply defines a `Dictionary` property:

````csharp
Dictionary<string, object> ExtraProperties { get; }
````

Then you can add or get extra properties using this dictionary.

### Base Classes

`IHasExtraProperties` interface is implemented by several base classes by default:

* Implemented by the `AggregateRoot` class (see [entities](Entities.md)).
* Implemented by `ExtensibleEntityDto`, `ExtensibleAuditedEntityDto`... base [DTO](Data-Transfer-Objects.md) classes.
* Implemented by the `ExtensibleObject`, which is a simple base class can be inherited for any type of object.

So, if you inherit from these classes, your class will also be extensible. If not, you can always implement it manually.

### Fundamental Extension Methods

While you can directly use the `ExtraProperties` property of a class, it is suggested to use the following extension methods while working with the extra properties.

#### SetProperty

Used to set the value of an extra property:

````csharp
user.SetProperty("Title", "My Title");
user.SetProperty("IsSuperUser", true);
````

`SetProperty` returns the same object, so you can chain it:

````csharp
user.SetProperty("Title", "My Title")
    .SetProperty("IsSuperUser", true);
````

#### GetProperty

Used to read the value of an extra property:

````csharp
var title = user.GetProperty<string>("Title");

if (user.GetProperty<bool>("IsSuperUser"))
{
    //...
}
````

* `GetProperty` is a generic method and takes the object type as the generic parameter.
* Returns the default value if given property was not set before (default value is `0` for `int`, `false` for `bool`... etc).

##### Non Primitive Property Types

If your property type is not a primitive (int, bool, enum, string... etc) type, then you need to use non-generic version of the `GetProperty` which returns an `object`.

#### HasProperty

Used to check if the object has a property set before.

#### RemoveProperty

Used to remove a property from the object. Use this methods instead of setting a `null` value for the property.

### Some Best Practices

Using magic strings for the property names is dangerous since you can easily type the property name wrong - it is not type safe. Instead;

* Define a constant for your extra property names
* Create extension methods to easily set your extra properties.

Example:

````csharp
public static class IdentityUserExtensions
{
    private const string TitlePropertyName = "Title";

    public static void SetTitle(this IdentityUser user, string title)
    {
        user.SetProperty(TitlePropertyName, title);
    }

    public static string GetTitle(this IdentityUser user)
    {
        return user.GetProperty<string>(TitlePropertyName);
    }
}
````

Then you can easily set or get the `Title` property:

````csharp
user.SetTitle("My Title");
var title = user.GetTitle();
````

