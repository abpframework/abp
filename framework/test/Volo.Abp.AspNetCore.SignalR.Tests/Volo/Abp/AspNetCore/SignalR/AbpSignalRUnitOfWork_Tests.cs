using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.SignalR;

public class AbpSignalRUnitOfWork_Tests : AbpAspNetCoreTestBase<Program>
{
    [Theory]
    [InlineData("/signalr-hubs/regular-abp/negotiate?negotiateVersion=1")]
    [InlineData("/custom-route-hub/negotiate?negotiateVersion=1")]
    [InlineData("/manually-mapped-hub/negotiate?negotiateVersion=1")]
    public async Task Should_Not_Reserve_A_Unit_Of_Work_For_The_Hub_Requests(string url)
    {
        var response = await Client.PostAsync(url, null);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        IsUnitOfWorkReserved(response).ShouldBeFalse();
    }

    [Theory]
    [InlineData("/not-a-hub")]
    [InlineData("/custom-route-hub-export")]
    public async Task Should_Reserve_A_Unit_Of_Work_For_The_Other_Requests(string url)
    {
        var response = await Client.GetAsync(url);

        IsUnitOfWorkReserved(response).ShouldBeTrue();
    }

    private static bool IsUnitOfWorkReserved(HttpResponseMessage response)
    {
        return bool.Parse(response.Headers.GetValues(AbpAspNetCoreSignalRTestModule.UnitOfWorkReservedHeaderName).Single());
    }
}
