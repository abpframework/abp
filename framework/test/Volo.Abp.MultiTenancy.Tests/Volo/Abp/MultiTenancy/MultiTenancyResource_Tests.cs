using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy.Localization;
using Xunit;

namespace Volo.Abp.MultiTenancy;

public class MultiTenancyResource_Tests : MultiTenancyTestBase
{
    [Fact]
    public void MultiTenancyResource_Test()
    {
        var q = GetRequiredService<IStringLocalizer<AbpMultiTenancyResource>>();
        using (CultureHelper.Use("en"))
        {
            GetRequiredService<IStringLocalizer<AbpMultiTenancyResource>>()["TenantNotFoundMessage"].Value.ShouldBe("Tenant not found!");
        }

        using (CultureHelper.Use("tr"))
        {
            GetRequiredService<IStringLocalizer<AbpMultiTenancyResource>>()["TenantNotFoundMessage"].Value.ShouldBe("Kiracı bulunamadı!");
        }
    }
}
