using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.TenantManagement
{
    public class Localization_Tests : AbpIdentityDomainTestBase
    {
        private readonly IStringLocalizer<IdentityResource> _stringLocalizer;

        public Localization_Tests()
        {
            _stringLocalizer = GetRequiredService<IStringLocalizer<IdentityResource>>();
        }

        [Fact]
        public void Test()
        {
            using (CultureHelper.Use("en"))
            {
                _stringLocalizer["PersonalSettingsSavedMessage"].Value
                .ShouldBe("Your personal settings has been saved successfully.");
            }
        }
    }
}