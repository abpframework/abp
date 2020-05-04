using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding
{
    public class ExtraPropertyBindingHelper_Tests
    {
        [Fact]
        public void ExtractExtraPropertyName_Tests()
        {
            ExtraPropertyBindingHelper.ExtractExtraPropertyName(
                "MyObject.UserInfo.ExtraProperties[SocialSecurityNumber]"
            ).ShouldBe("SocialSecurityNumber");

            ExtraPropertyBindingHelper.ExtractExtraPropertyName(
                "UserInfo.ExtraProperties[SocialSecurityNumber]"
            ).ShouldBe("SocialSecurityNumber");

            ExtraPropertyBindingHelper.ExtractExtraPropertyName(
                "ExtraProperties[SocialSecurityNumber]"
            ).ShouldBe("SocialSecurityNumber");

            ExtraPropertyBindingHelper.ExtractExtraPropertyName(
                "SocialSecurityNumber"
            ).ShouldBeNull();
        }

        [Fact]
        public void ExtractContainerName_Tests()
        {
            ExtraPropertyBindingHelper.ExtractContainerName(
                "MyObject.UserInfo.ExtraProperties[SocialSecurityNumber]"
            ).ShouldBe("MyObject.UserInfo");

            ExtraPropertyBindingHelper.ExtractContainerName(
                "UserInfo.ExtraProperties[SocialSecurityNumber]"
            ).ShouldBe("UserInfo");

            ExtraPropertyBindingHelper.ExtractContainerName(
                "ExtraProperties[SocialSecurityNumber]"
            ).ShouldBe("");

            ExtraPropertyBindingHelper.ExtractContainerName(
                "SocialSecurityNumber"
            ).ShouldBeNull();
        }
    }
}
