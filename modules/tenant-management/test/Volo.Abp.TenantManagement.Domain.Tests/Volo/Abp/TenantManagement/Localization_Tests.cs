using System.Globalization;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Volo.Abp.TenantManagement.Localization;
using Xunit;

namespace Volo.Abp.TenantManagement;

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
        using (CultureHelper.Use("en"))
        {
            _stringLocalizer["TenantDeletionConfirmationMessage"].Value
                .ShouldBe("Tenant '{0}' will be deleted. Do you confirm that?");
        }

        using (CultureHelper.Use("en-gb"))
        {
            _stringLocalizer["TenantDeletionConfirmationMessage"].Value
                .ShouldBe("Tenant '{0}' will be deleted. Is that OK?");
        }

        using (CultureHelper.Use("tr"))
        {
            _stringLocalizer["TenantDeletionConfirmationMessage"].Value
                .ShouldBe("'{0}' isimli müşteri silinecektir. Onaylıyor musunuz?");
        }
    }
}
