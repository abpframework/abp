using System.ComponentModel.DataAnnotations;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Threading;
using Xunit;

namespace Volo.Abp.ObjectExtending
{
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
                            propertyInfo.ValidationAttributes.Add(new RequiredAttribute());
                            propertyInfo.ValidationAttributes.Add(new StringLengthAttribute(64) { MinimumLength = 2 });
                        });

                        options.AddOrUpdateProperty<string>("Address", propertyInfo =>
                        {
                            propertyInfo.ValidationAttributes.Add(new StringLengthAttribute(255));
                        });

                        options.AddOrUpdateProperty<byte>("Age", propertyInfo =>
                        {
                            propertyInfo.ValidationAttributes.Add(new RequiredAttribute());
                            propertyInfo.ValidationAttributes.Add(new RangeAttribute(18, 99));
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
                                                new[] {"Password", "PasswordRepeat"}
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
                    new ExtensiblePersonObject
                    {
                        ExtraProperties =
                        {
                            {"Name", "John"},
                            {"Age", "42"},
                        }
                    }
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
                    new ExtensiblePersonObject
                    {
                        ExtraProperties =
                        {
                            {"Address", new string('x', 256) }
                        }
                    }
                ).Count.ShouldBe(3); // Name, Age & Address

            ExtensibleObjectValidator
                .GetValidationErrors(
                    new ExtensiblePersonObject
                    {
                        ExtraProperties =
                        {
                            {"Age", "42" }
                        }
                    }
                ).Count.ShouldBe(1); // Name

            ExtensibleObjectValidator
                .GetValidationErrors(
                    new ExtensiblePersonObject
                    {
                        ExtraProperties =
                        {
                            {"Address", new string('x', 256) },
                            {"Age", "100" }
                        }
                    }
                ).Count.ShouldBe(3); // Name, Age & Address

            ExtensibleObjectValidator
                .GetValidationErrors(
                    new ExtensiblePersonObject
                    {
                        ExtraProperties =
                        {
                            {"Name", "John"},
                            {"Age", "42"},
                            {"Password", "123"},
                            {"PasswordRepeat", "1256"}
                        }
                    }
                ).Count.ShouldBe(1); // PasswordRepeat != Password

            ExtensibleObjectValidator
                .GetValidationErrors(
                    new ExtensiblePersonObject
                    {
                        ExtraProperties =
                        {
                            {"Name", "BadValue"},
                            {"Age", "42"},
                        }
                    }
                ).Count.ShouldBe(1); //Name is 'BadValue'!
        }

        private class ExtensiblePersonObject : ExtensibleObject
        {

        }
    }
}
