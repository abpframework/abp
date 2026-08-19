using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Uow;

public class UnitOfWorkMiddleware_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task Get_Actions_Should_Not_Be_Transactional()
    {
        await GetResponseAsStringAsync("/api/unitofwork-test/ActionRequiresUow");
    }

    [Fact]
    public async Task Non_Get_Actions_Should_Be_Transactional()
    {
        var result = await Client.PostAsync("/api/unitofwork-test/ActionRequiresUowPost", null);
        result.IsSuccessStatusCode.ShouldBeTrue();
    }

    [Fact]
    public async Task Query_Actions_Should_Not_Be_Transactional()
    {
        using var requestMessage = new HttpRequestMessage(new HttpMethod("QUERY"), "/api/unitofwork-test/ActionRequiresUowQuery");
        var result = await Client.SendAsync(requestMessage);
        result.IsSuccessStatusCode.ShouldBeTrue();
    }

    [Fact]
    public async Task Ambient_Uow_Should_Be_Completed_Before_Response_Is_Flushed()
    {
        var result = await GetResponseAsStringAsync("/api/unitofwork-test/CommitBeforeResponseFlush");
        result.ShouldBe("first:completed");
    }

    [Fact]
    public async Task Exception_After_Response_Flush_Should_Not_Undo_Committed_Work()
    {
        // Once the response has started, an exception can't turn it into an error response
        // (the connection is reset). What matters: the uow was committed before the throw.
        await Should.ThrowAsync<HttpRequestException>(async () =>
        {
            var response = await Client.GetAsync("/api/unitofwork-test/CommitThenThrowAfterResponseFlush");
            await response.Content.ReadAsStringAsync();
        });

        ServiceProvider.GetRequiredService<TestUnitOfWorkConfig>()
            .UowCompletedAfterResponseFlush.ShouldBe(true);
    }

    [Fact]
    public async Task Repository_Access_After_Response_Flush_Runs_Outside_The_Request_Uow()
    {
        // After the response starts the request uow is gone; a repository still works via its
        // own implicit uow (ambient=null), so it no longer joins the request transaction.
        var body = await GetResponseAsStringAsync("/api/unitofwork-test/ReadRepositoryAfterResponseFlush");
        body.ShouldBe("before=ok(1);after=ok(1,ambient=null)");
    }

    [Fact]
    public async Task Raw_Database_Provider_After_Response_Flush_Throws()
    {
        // Unlike repositories, raw provider access after the response started has no uow and throws.
        var body = await GetResponseAsStringAsync("/api/unitofwork-test/RawDatabaseProviderAfterResponseFlush");
        body.ShouldBe("first:threw-AbpException");
    }
}
