using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Authentication.OAuth.Claims;

public class MultipleClaimAction_Tests
{
    [Fact]
    public void Should_Set_Single_Value()
    {
        var jObject = JsonDocument.Parse(@"{
  ""sub"": ""71054539-0e48-af28-5e7a-39e4e42d8ea5"",
  ""role"": ""admin""
}");
        var claimsIdentity = new ClaimsIdentity();
        new MultipleClaimAction(AbpClaimTypes.Role, "role").Run(jObject.RootElement, claimsIdentity, null);
        var claims = claimsIdentity.FindAll(AbpClaimTypes.Role).ToList();
        claims.Count.ShouldBe(1);
        claims[0].Value.ShouldBe("admin");
    }

    [Fact]
    public void Should_Set_Multiple_Values()
    {
        var jObject = JsonDocument.Parse(@"{
  ""sub"": ""71054539-0e48-af28-5e7a-39e4e42d8ea5"",
  ""role"": [
    ""admin"",
    ""moderator""
  ]
}");

        var claimsIdentity = new ClaimsIdentity();
        new MultipleClaimAction(AbpClaimTypes.Role, "role").Run(jObject.RootElement, claimsIdentity, null);
        var claims = claimsIdentity.FindAll(AbpClaimTypes.Role).ToList();
        claims.Count.ShouldBe(2);
        claims[0].Value.ShouldBe("admin");
        claims[1].Value.ShouldBe("moderator");
    }
}
