using System.ComponentModel.DataAnnotations;
using System.Linq;
using Shouldly;
using Xunit;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionManager_Tests
    {
        private readonly ObjectExtensionManager _objectExtensionManager;

        public ObjectExtensionManager_Tests()
        {
            _objectExtensionManager = new ObjectExtensionManager();
        }

        [Fact]
        public void Should_Not_Add_Same_Property_Multiple_Times()
        {
            _objectExtensionManager
                .AddOrUpdateProperty<MyExtensibleObject, string>("TestProp")
                .AddOrUpdateProperty<MyExtensibleObject, string>("TestProp");

            var objectExtension = _objectExtensionManager.GetOrNull<MyExtensibleObject>();
            objectExtension.ShouldNotBeNull();

            var properties = objectExtension.GetProperties();
            properties.Count.ShouldBe(1);
            properties.FirstOrDefault(p => p.Name == "TestProp").ShouldNotBeNull();
        }

        [Fact]
        public void Should_Update_Property_Configuration()
        {
            _objectExtensionManager
                .AddOrUpdateProperty<MyExtensibleObject, string>(
                    "TestProp",
                    options =>
                    {
                        options.Configuration["TestConfig1"] = "TestConfig1-Value";
                    }
                ).AddOrUpdateProperty<MyExtensibleObject, string>(
                    "TestProp",
                    options =>
                    {
                        options.Configuration["TestConfig2"] = "TestConfig2-Value";
                    }
                );

            var objectExtension = _objectExtensionManager.GetOrNull<MyExtensibleObject>();
            objectExtension.ShouldNotBeNull();

            var property = objectExtension.GetPropertyOrNull("TestProp");
            property.ShouldNotBeNull();
            property.Configuration["TestConfig1"].ShouldBe("TestConfig1-Value");
            property.Configuration["TestConfig2"].ShouldBe("TestConfig2-Value");
        }

        [Fact]
        public void Should_Automatically_Add_RequiredAttribute_To_Non_Nullable_Types_And_Enums()
        {
            _objectExtensionManager
                .AddOrUpdateProperty<MyExtensibleObject, int>("IntProp")
                .AddOrUpdateProperty<MyExtensibleObject, bool>("BoolProp")
                .AddOrUpdateProperty<MyExtensibleObject, int?>("NullableIntProp")
                .AddOrUpdateProperty<MyExtensibleObject, string>("StringProp")
                .AddOrUpdateProperty<MyExtensibleObject, MyTestEnum>("EnumProp");

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("IntProp")
                .Attributes
                .ShouldContain(x => x is RequiredAttribute);

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("BoolProp")
                .Attributes
                .ShouldContain(x => x is RequiredAttribute);

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("EnumProp")
                .Attributes
                .ShouldContain(x => x is RequiredAttribute);

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("NullableIntProp")
                .Attributes
                .ShouldNotContain(x => x is RequiredAttribute);

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("StringProp")
                .Attributes
                .ShouldNotContain(x => x is RequiredAttribute);
        }

        [Fact]
        public void Should_Automatically_Add_EnumDataTypeAttribute_For_Enums()
        {
            _objectExtensionManager
                .AddOrUpdateProperty<MyExtensibleObject, MyTestEnum>("EnumProp");

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("EnumProp")
                .Attributes
                .ShouldContain(x => x is EnumDataTypeAttribute);
        }

        [Fact]
        public void Should_Be_Able_To_Clear_Auto_Added_Attributes()
        {
            _objectExtensionManager
                .AddOrUpdateProperty<MyExtensibleObject, int>("IntProp", property =>
                {
                    property.Attributes.Clear();
                });

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("IntProp")
                .Attributes
                .ShouldNotContain(x => x is RequiredAttribute);
        }

        [Fact]
        public void Should_Set_DefaultValues()
        {
            _objectExtensionManager
                .AddOrUpdateProperty<MyExtensibleObject, int>("IntProp")
                .AddOrUpdateProperty<MyExtensibleObject, int>("IntPropWithCustomDefaultValue", property =>
                {
                    property.DefaultValue = 42;
                })
                .AddOrUpdateProperty<MyExtensibleObject, bool>("BoolProp")
                .AddOrUpdateProperty<MyExtensibleObject, int?>("NullableIntProp")
                .AddOrUpdateProperty<MyExtensibleObject, int?>("NullableIntPropWithCustomDefaultValueFactory", property =>
                {
                    property.DefaultValueFactory = () => 2;
                })
                .AddOrUpdateProperty<MyExtensibleObject, string>("StringProp")
                .AddOrUpdateProperty<MyExtensibleObject, string>("StringPropWithCustomDefaultValue", property =>
                {
                    property.DefaultValue = "custom-value";
                })
                .AddOrUpdateProperty<MyExtensibleObject, MyTestEnum>("EnumProp", property =>
                {
                    property.DefaultValue = MyTestEnum.EnumValue2;
                });

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("IntProp")
                .GetDefaultValue().ShouldBe(0);

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("IntPropWithCustomDefaultValue")
                .GetDefaultValue().ShouldBe(42);

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("BoolProp")
                .GetDefaultValue().ShouldBe(false);

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("NullableIntProp")
                .GetDefaultValue().ShouldBeNull();

            var propWithDefaultValueFactory = _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("NullableIntPropWithCustomDefaultValueFactory");
            propWithDefaultValueFactory.GetDefaultValue().ShouldBe(2);

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("StringProp")
                .GetDefaultValue().ShouldBeNull();

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("StringPropWithCustomDefaultValue")
                .GetDefaultValue().ShouldBe("custom-value");

            _objectExtensionManager
                .GetPropertyOrNull<MyExtensibleObject>("EnumProp")
                .GetDefaultValue().ShouldBe(MyTestEnum.EnumValue2);
        }

        private class MyExtensibleObject : ExtensibleObject
        {

        }

        private enum MyTestEnum
        {
            EnumValue1,
            EnumValue2,
        }
    }
}
