using System.Linq;
using Shouldly;
using Volo.Abp.AspNetCore.SignalR.SampleHubs;
using Xunit;

namespace Volo.Abp.AspNetCore.SignalR;

public class HubConfigList_Tests
{
    [Fact]
    public void Should_Apply_ConfigAction_For_Generic_Overload()
    {
        var list = new HubConfigList();

        list.AddOrUpdate<RegularHub>(config => config.RoutePattern = "/custom-route");

        var hubConfig = list.Single(c => c.HubType == typeof(RegularHub));
        hubConfig.RoutePattern.ShouldBe("/custom-route");
    }

    [Fact]
    public void Should_Update_Existing_Hub_For_Generic_Overload()
    {
        var list = new HubConfigList();

        list.AddOrUpdate<RegularHub>();
        list.AddOrUpdate<RegularHub>(config => config.RoutePattern = "/custom-route");

        list.Count.ShouldBe(1);
        list.Single(c => c.HubType == typeof(RegularHub)).RoutePattern.ShouldBe("/custom-route");
    }
}
