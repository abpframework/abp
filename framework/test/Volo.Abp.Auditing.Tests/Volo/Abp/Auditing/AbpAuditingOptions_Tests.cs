using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.Auditing;

public class AbpAuditingOptions_Tests : AbpAuditingTestBase
{
    private const string ApplicationName = "TEST_APP_NAME";
    
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        base.SetAbpApplicationCreationOptions(options);
        options.ApplicationName = ApplicationName;
    }
    
    [Fact]
    public void Should_Set_Application_Name_From_Global_Application_Name_By_Default()
    {
        var options = GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
        options.ApplicationName.ShouldBe(ApplicationName);
    }
}