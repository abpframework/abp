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

        private class MyExtensibleObject : ExtensibleObject
        {

        }
    }
}
