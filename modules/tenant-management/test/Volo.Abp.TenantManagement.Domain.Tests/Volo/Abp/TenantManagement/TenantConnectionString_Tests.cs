using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.TenantManagement;

public class TenantConnectionString_Tests
{
    [Theory]
    [InlineData("aaa")]
    [InlineData("bbb")]
    public void SetValue(string value)
    {
        var tenantConnectionString =
            new TenantConnectionString(Guid.NewGuid(), "MyConnString", "MyConnString-Value");
        tenantConnectionString.SetValue(value);
        tenantConnectionString.Value.ShouldBe(value);
    }
}
