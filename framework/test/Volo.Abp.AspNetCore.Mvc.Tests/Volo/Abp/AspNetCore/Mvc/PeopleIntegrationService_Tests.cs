using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc;

public class PeopleIntegrationService_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task GetValueAsync()
    {
        var result = await GetResponseAsStringAsync("/integration-api/app/people/value");
        result.ShouldBe("42");
    }
}