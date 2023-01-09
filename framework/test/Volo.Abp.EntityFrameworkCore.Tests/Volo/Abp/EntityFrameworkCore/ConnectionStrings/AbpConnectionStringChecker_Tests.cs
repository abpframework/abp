using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.ConnectionStrings;

public class AbpConnectionStringChecker_Tests : EntityFrameworkCoreTestBase
{
    [Fact]
    public async Task IsValidAsync()
    {
        var connectionStringChecker = GetRequiredService<IAbpConnectionStringChecker>();
        var result = await connectionStringChecker.CheckAsync(@"Data Source=:memory:");
        result.Connected.ShouldBeTrue();
        result.DatabaseExists.ShouldBeTrue();
    }
}
