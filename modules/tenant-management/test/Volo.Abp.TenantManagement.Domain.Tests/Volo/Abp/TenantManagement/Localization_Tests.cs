using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.TenantManagement.Localization;
using Xunit;

namespace Volo.Abp.TenantManagement
{
    public class Localization_Tests : AbpTenantManagementDomainTestBase
    {
        private readonly IStringLocalizer<AbpTenantManagementResource> _stringLocalizer;

        public Localization_Tests()
        {
            _stringLocalizer = GetRequiredService<IStringLocalizer<AbpTenantManagementResource>>();
        }

        [Fact]
        public void Test()
        {
            _stringLocalizer["TenantDeletionConfirmationMessage"].Value
                .ShouldBe("Tenant '{0}' will be deleted. Do you confirm that?");
        }
    }
}
