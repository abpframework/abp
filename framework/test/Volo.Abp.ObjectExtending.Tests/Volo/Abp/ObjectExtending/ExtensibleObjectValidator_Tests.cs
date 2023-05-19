using System.ComponentModel.DataAnnotations;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Threading;
using Volo.Abp.Validation;
using Xunit;

namespace Volo.Abp.ObjectExtending;

public class ExtensibleObjectValidator_Tests
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    static ExtensibleObjectValidator_Tests()
    {
        OneTimeRunner.Run(() =>
        {
            ObjectExtensionManager.Instance
                .AddOrUpdate<ExtensiblePersonObject>(options =>
                {
                    options.AddOrUpdateProperty<string>("Name", propertyInfo =>
                    {
                        propertyInfo.Attributes.Add(new RequiredAttribute());
                        propertyInfo.Attributes.Add(new StringLengthAttribute(64) { MinimumLength = 2 });
                    });

                    options.AddOrUpdateProperty<string>("Address", propertyInfo =>
                    {
                        propertyInfo.Attributes.Add(new StringLengthAttribute(255));
                    });

                    options.AddOrUpdateProperty<byte>("Age", propertyInfo =>
                    {
                        propertyInfo.Attributes.Add(new RequiredAttribute());
                        propertyInfo.Attributes.Add(new RangeAttribute(18, 99));
                    });

                    options.AddOrUpdateProperty<bool>("IsMarried", propertyInfo =>
                    {

                    });

                    options.AddOrUpdateProperty<string>("Password", propertyInfo =>
                    {
                    });

                    options.AddOrUpdateProperty<string>("PasswordRepeat", propertyInfo =>
                    {
                        propertyInfo.Validators.Add(context =>
                        {
                            if (context.ValidatingObject.HasProperty("Password"))
                            {
                                if (context.ValidatingObject.GetProperty<string>("Password") !=
                                    context.Value as string)
                                {
                                    context.ValidationErrors.Add(
                                        new ValidationResult(
                                            "If you specify a password, then please correctly repeat it!",
                                            new[] { "Password", "PasswordRepeat" }
                                        )
                                    );
                                }
                            }
                        });
                    });

                    options.Validators.Add(context =>
                    {
                        if (context.ValidatingObject.GetProperty<string>("Name") == "BadValue")
                        {
                            context.ValidationErrors.Add(
                                new ValidationResult(
                                    "Name can not be 'BadValue', sorry :(",
                                    new[] { "Name" }
                                )
                            );
                        }
                    });
                });
        });
    }

    [Fact]
    public void Should_Validate_If_The_Properties_Are_Valid()
    {
        ExtensibleObjectValidator
            .GetValidationErrors(
                new ExtensiblePersonObject()
                    .SetProperty("Name", "John", validate: false)
                    .SetProperty("Age", 42, validate: false)
            ).Count.ShouldBe(0); //All Valid
    }

    [Fact]
    public void Should_Not_Validate_If_The_Properties_Are_Not_Valid()
    {
        ExtensibleObjectValidator
            .GetValidationErrors(
                new ExtensiblePersonObject()
            ).Count.ShouldBe(2); // Name & Age

        ExtensibleObjectValidator
            .GetValidationErrors(
                new ExtensiblePersonObject()
                    .SetProperty("Address", new string('x', 256), validate: false)
            ).Count.ShouldBe(3); // Name, Age & Address

        ExtensibleObjectValidator
            .GetValidationErrors(
                new ExtensiblePersonObject()
                    .SetProperty("Age", 42, validate: false)
            ).Count.ShouldBe(1); // Name

        ExtensibleObjectValidator
            .GetValidationErrors(
                new ExtensiblePersonObject()
                    .SetProperty("Address", new string('x', 256), validate: false)
                    .SetProperty("Age", 100, validate: false)
            ).Count.ShouldBe(3); // Name, Age & Address

        ExtensibleObjectValidator
            .GetValidationErrors(
                new ExtensiblePersonObject()
                    .SetProperty("Name", "John", validate: false)
                    .SetProperty("Age", 42, validate: false)
                    .SetProperty("Password", "123", validate: false)
                    .SetProperty("PasswordRepeat", "1256", validate: false)
            ).Count.ShouldBe(1); // PasswordRepeat != Password

        ExtensibleObjectValidator
            .GetValidationErrors(
                new ExtensiblePersonObject()
                    .SetProperty("Name", "BadValue", validate: false)
                    .SetProperty("Age", 42, validate: false)
            ).Count.ShouldBe(1); //Name is 'BadValue'!
    }

    [Fact]
    public void Should_Check_Validation_On_SetProperty()
    {
        Assert.Throws<AbpValidationException>(() =>
        {
            new ExtensiblePersonObject()
                .SetProperty("Address", new string('x', 256));
        });
    }

    private class ExtensiblePersonObject : ExtensibleObject
    {

    }
}
