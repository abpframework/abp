# Object Extensions

ABP Framework provides an **object extension system** to allow you to **add extra properties** to an existing object **without modifying** the related class. This allows to extend functionalities implemented by a depended [application module](Modules/Index.md), especially when you want to [extend entities](Customizing-Application-Modules-Extending-Entities.md) and [DTOs](Customizing-Application-Modules-Overriding-Services.md) defined by the module.

> Object extension system normally is not needed for your own objects since you can easily add regular properties to your own classes.

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

## Object Extension Manager

While you can set arbitrary properties to an extensible object (which implements the `IHasExtraProperties` interface), `ObjectExtensionManager` is used to explicitly define extra properties for extensible classes.

Explicitly defining an extra property has some use cases:

* Allows to control how the extra property is handled on object to object mapping (see the section below).
* Allows to define metadata for the property. For example, you can map an extra property to a table field in the database while using the [EF Core](Entity-Framework-Core.md).

> `ObjectExtensionManager` implements the singleton pattern (`ObjectExtensionManager.Instance`) and you should define object extensions before your application startup. The [application startup template](Startup-Templates/Application.md) has some pre-defined static classes to safely define object extensions inside.

### AddOrUpdate

`AddOrUpdate` is the main method to define a extra properties or update extra properties for an object.

Example: Define extra properties for the `IdentityUser` entity:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdate<IdentityUser>(options =>
        {
            options.AddOrUpdateProperty<string>("SocialSecurityNumber");
            options.AddOrUpdateProperty<bool>("IsSuperUser");
        }
    );
````

### AddOrUpdateProperty

While `AddOrUpdateProperty` can be used on the `options` as shown before, if you want to define a single extra property, you can use the shortcut extension method too:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>("SocialSecurityNumber");
````

Sometimes it would be practical to define a single extra property to multiple types. Instead of defining one by one, you can use the following code:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<string>(
        new[]
        {
            typeof(IdentityUserDto),
            typeof(IdentityUserCreateDto),
            typeof(IdentityUserUpdateDto)
        },
        "SocialSecurityNumber"
    );
````

### Property Configuration

`AddOrUpdateProperty` can also get an action that can perform additional configuration on the property definition:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        options =>
        {
            //Configure options...
        });
````

> `options` has a dictionary, named `Configuration` which makes the object extension definitions even extensible. It is used by the EF Core to map extra properties to table fields in the database. See the [extending entities](Customizing-Application-Modules-Extending-Entities.md) document.

The following sections explain the fundamental property configuration options.

#### Default Value

A default value is automatically set for the new property, which is the natural default value for the property type, like `null` for `string`, `false` for `bool` or `0` for `int`.

There are two ways to override the default value:

##### DefaultValue Option

`DefaultValue` option can be set to any value:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, int>(
        "MyIntProperty",
        options =>
        {
            options.DefaultValue = 42;
        });
````

##### DefaultValueFactory Options

`DefaultValueFactory` can be set to a function that returns the default value:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, DateTime>(
        "MyDateTimeProperty",
        options =>
        {
            options.DefaultValueFactory = () => DateTime.Now;
        });
````

`options.DefaultValueFactory` has a higher priority than the `options.DefaultValue` .

> Tip: Use `DefaultValueFactory` option only if the default value may change over the time (like `DateTime.Now` in this example). If it is a constant value, then use the `DefaultValue` option.

#### CheckPairDefinitionOnMapping

Controls how to check property definitions while mapping two extensible objects. See the "Object to Object Mapping" section to understand the `CheckPairDefinitionOnMapping` option better.

## Validation

You may want to add some **validation rules** for the extra properties you've defined. `AddOrUpdateProperty` method options allows two ways of performing validation:

1. You can add **data annotation attributes** for a property.
2. You can write an action (code block) to perform a **custom validation**.

Validation works when you use the object in a method that is **automatically validated** (e.g. controller actions, page handler methods, application service methods...). So, all extra properties are validated whenever the extended object is being validated.

### Data Annotation Attributes

All of the standard data annotation attributes are valid for extra properties. Example:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUserCreateDto, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.Attributes.Add(new RequiredAttribute());
            options.Attributes.Add(
                new StringLengthAttribute(32) {
                    MinimumLength = 6 
                }
            );
        });
````

With this configuration, `IdentityUserCreateDto` objects will be invalid without a valid `SocialSecurityNumber` value provided.

#### Default Validation Attributes

There are some attributes **automatically added** when you create certain type of properties;

* `RequiredAttribute` is added for non nullable primitive property types (e.g. `int`, `bool`, `DateTime`...) and `enum` types.
* `EnumDataTypeAttribute` is added for enum types, to prevent to set invalid enum values.

Use `options.Attributes.Clear();` if you don't want these attributes.

### Custom Validation

If you need, you can add a custom action that is executed to validate the extra properties. Example:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUserCreateDto, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.Validators.Add(context =>
            {
                var socialSecurityNumber = context.Value as string;

                if (socialSecurityNumber == null ||
                    socialSecurityNumber.StartsWith("X"))
                {
                    context.ValidationErrors.Add(
                        new ValidationResult(
                            "Invalid social security number: " + socialSecurityNumber,
                            new[] { "SocialSecurityNumber" }
                        )
                    );
                }
            });
        });
````

`context.ServiceProvider` can be used to resolve a service dependency for advanced scenarios.

In addition to add custom validation logic for a single property, you can add a custom validation logic that is executed in object level. Example:

````csharp
ObjectExtensionManager.Instance
.AddOrUpdate<IdentityUserCreateDto>(objConfig =>
{
    //Define two properties with their own validation rules
    
    objConfig.AddOrUpdateProperty<string>("Password", propertyConfig =>
    {
        propertyConfig.Attributes.Add(new RequiredAttribute());
    });

    objConfig.AddOrUpdateProperty<string>("PasswordRepeat", propertyConfig =>
    {
        propertyConfig.Attributes.Add(new RequiredAttribute());
    });

    //Write a common validation logic works on multiple properties
    
    objConfig.Validators.Add(context =>
    {
        if (context.ValidatingObject.GetProperty<string>("Password") !=
            context.ValidatingObject.GetProperty<string>("PasswordRepeat"))
        {
            context.ValidationErrors.Add(
                new ValidationResult(
                    "Please repeat the same password!",
                    new[] { "Password", "PasswordRepeat" }
                )
            );
        }
    });
});
````

## Object to Object Mapping

Assume that you've added an extra property to an extensible entity object and used auto [object to object mapping](Object-To-Object-Mapping.md) to map this entity to an extensible DTO class. You need to be careful in such a case, because the extra property may contain a **sensitive data** that should not be available to clients.

This section offers some **good practices** to control your extra properties on object mapping.

### MapExtraPropertiesTo

`MapExtraPropertiesTo` is an extension method provided by the ABP Framework to copy extra properties from an object to another in a controlled manner. Example usage:

````csharp
identityUser.MapExtraPropertiesTo(identityUserDto);
````

`MapExtraPropertiesTo` **requires to define properties** (as described above) in **both sides** (`IdentityUser` and `IdentityUserDto` in this case) in order to copy the value to the target object. Otherwise, it doesn't copy the value even if it does exists in the source object (`identityUser` in this example). There are some ways to overload this restriction.

#### MappingPropertyDefinitionChecks

`MapExtraPropertiesTo` gets an additional parameter to control the definition check for a single mapping operation:

````csharp
identityUser.MapExtraPropertiesTo(
    identityUserDto,
    MappingPropertyDefinitionChecks.None
);
````

> Be careful since `MappingPropertyDefinitionChecks.None` copies all extra properties without any check. `MappingPropertyDefinitionChecks` enum has other members too.

If you want to completely disable definition check for a property, you can do it while defining the extra property (or update an existing definition) as shown below:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.CheckPairDefinitionOnMapping = false;
        });
````

#### Ignored Properties

You may want to ignore some properties on a specific mapping operation:

````csharp
identityUser.MapExtraPropertiesTo(
    identityUserDto,
    ignoredProperties: new[] {"MySensitiveProp"}
);
````

Ignored properties are not copied to the target object.

#### AutoMapper Integration

If you're using the [AutoMapper](https://automapper.org/) library, the ABP Framework also provides an extension method to utilize the `MapExtraPropertiesTo` method defined above.

You can use the `MapExtraProperties()` method inside your mapping profile.

````csharp
public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<IdentityUser, IdentityUserDto>()
            .MapExtraProperties();
    }
}
````

It has the same parameters with the `MapExtraPropertiesTo` method.

## Entity Framework Core Database Mapping

If you're using the EF Core, you can map an extra property to a table field in the database. Example:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.MapEfCore(b => b.HasMaxLength(32));
        }
    );
````

See the [Entity Framework Core Integration document](Entity-Framework-Core.md) for more.

## See Also

* [Module Entity Extensions](Module-Entity-Extensions.md)