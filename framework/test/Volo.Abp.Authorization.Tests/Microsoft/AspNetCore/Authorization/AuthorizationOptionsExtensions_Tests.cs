using Shouldly;
using Xunit;

namespace Microsoft.AspNetCore.Authorization;

public class AuthorizationOptionsExtensions_Tests
{
    [Fact]
    public void GetPoliciesNames()
    {
        var options = new AuthorizationOptions();

        options.AddPolicy("TestPolicy1", policy => policy.RequireClaim("MyClaim"));
        options.AddPolicy("TestPolicy2", policy => policy.RequireRole("MyRole"));

        options.GetPoliciesNames().Count.ShouldBe(2);
    }
}
