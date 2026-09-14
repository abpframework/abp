using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Uow;

public class AspNetCoreUnitOfWorkTransactionBehaviourProvider_Tests
{
    private static AspNetCoreUnitOfWorkTransactionBehaviourProvider CreateProvider(string method)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = method;

        return new AspNetCoreUnitOfWorkTransactionBehaviourProvider(
            new HttpContextAccessor { HttpContext = httpContext },
            Microsoft.Extensions.Options.Options.Create(new AspNetCoreUnitOfWorkTransactionBehaviourProviderOptions()));
    }

    [Theory]
    [InlineData("GET", false)]
    [InlineData("QUERY", false)]
    [InlineData("query", false)]
    [InlineData("HEAD", true)]
    [InlineData("POST", true)]
    [InlineData("PUT", true)]
    [InlineData("DELETE", true)]
    public void IsTransactional_Should_Treat_Get_And_Query_As_Non_Transactional(string method, bool expected)
    {
        CreateProvider(method).IsTransactional.ShouldBe(expected);
    }
}
