using System;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public class HasExtraPropertiesExtensions_Tests
    {
        [Fact]
        public void Basic_Tests()
        {
            var city = new City(Guid.NewGuid(), "Adana");

            city.HasProperty("UnknownProperty").ShouldBeFalse();
            city.GetProperty("UnknownProperty").ShouldBeNull();
            city.GetProperty<int>("UnknownProperty").ShouldBe(0);
            city.GetProperty<int>("UnknownProperty", 42).ShouldBe(42);

            city.SetProperty("IsHot", true);
            city.HasProperty("IsHot").ShouldBeTrue();
            city.GetProperty<bool>("IsHot").ShouldBeTrue();
            city.GetProperty<bool>("IsHot").ShouldBeTrue();

            city.SetProperty("IsHot", false);
            city.HasProperty("IsHot").ShouldBeTrue();
            city.GetProperty<bool>("IsHot").ShouldBeFalse();
            city.GetProperty<bool>("IsHot", true).ShouldBeFalse();

            city.RemoveProperty("IsHot");
            city.HasProperty("IsHot").ShouldBeFalse();
            city.GetProperty<bool>("IsHot").ShouldBeFalse();
            city.GetProperty<bool>("IsHot", true).ShouldBeTrue();
        }
    }
}