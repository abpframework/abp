using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.AspNetCore.Uow;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Uow;

public class UnitOfWorkMiddleware_Tests : AspNetCoreMvcTestBase
{
    private AbpAspNetCoreUnitOfWorkOptions Options =>
        ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreUnitOfWorkOptions>>().Value;

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
    public async Task Ambient_Uow_Should_Be_Completed_Before_Response_Is_Flushed_When_Enabled()
    {
        Options.CompleteUnitOfWorkOnResponseStarting = true;

        var result = await GetResponseAsStringAsync("/api/unitofwork-test/CommitBeforeResponseFlush");
        result.ShouldBe("first:completed");
    }

    [Fact]
    public async Task Ambient_Uow_Is_Not_Completed_On_Response_Start_By_Default()
    {
        var result = await GetResponseAsStringAsync("/api/unitofwork-test/CommitBeforeResponseFlush");
        result.ShouldBe("first:not-completed");
    }

    [Fact]
    public async Task Ambient_Uow_Is_Already_Completed_When_An_Exception_Is_Raised_After_The_Response_Started()
    {
        Options.CompleteUnitOfWorkOnResponseStarting = true;

        // Once the response has started, an exception can't turn it into an error response (the
        // connection is reset). Database-level rollback/commit is covered by the relational tests.
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
        Options.CompleteUnitOfWorkOnResponseStarting = true;

        var body = await GetResponseAsStringAsync("/api/unitofwork-test/ReadRepositoryAfterResponseFlush");
        body.ShouldBe("before=ok(1);after=ok(1,ambient=null)");
    }

    [Fact]
    public async Task Raw_Database_Provider_After_Response_Flush_Throws()
    {
        Options.CompleteUnitOfWorkOnResponseStarting = true;

        var body = await GetResponseAsStringAsync("/api/unitofwork-test/RawDatabaseProviderAfterResponseFlush");
        body.ShouldBe("first:threw-AbpException");
    }

    [Fact]
    public async Task Response_Flush_Inside_Nested_Uow_Should_Not_Complete_The_Nested_Uow()
    {
        Options.CompleteUnitOfWorkOnResponseStarting = true;

        var body = await GetResponseAsStringAsync("/api/unitofwork-test/NestedUowDuringResponseFlush");
        body.ShouldBe("first:outer-not-completed:nested-completed-by-owner");
    }

    [Fact]
    public async Task Completing_The_Uow_In_The_Action_Still_Fails_At_End_Of_Pipeline_By_Default()
    {
        var response = await Client.GetAsync("/api/unitofwork-test/CompleteCurrentUow");
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Opt_In_Url_Enables_The_Feature_For_A_Matching_Path()
    {
        Options.CompleteUnitOfWorkOnResponseStartingUrls.Add("/api/unitofwork-test/CommitBeforeResponseFlush");

        var result = await GetResponseAsStringAsync("/api/unitofwork-test/CommitBeforeResponseFlush");
        result.ShouldBe("first:completed");
    }

    [Fact]
    public async Task Opt_In_Url_With_A_Trailing_Slash_Still_Matches()
    {
        Options.CompleteUnitOfWorkOnResponseStartingUrls.Add("/api/unitofwork-test/");

        var result = await GetResponseAsStringAsync("/api/unitofwork-test/CommitBeforeResponseFlush");
        result.ShouldBe("first:completed");
    }

    [Fact]
    public async Task Response_Flush_Inside_A_Child_Uow_Scope_Should_Not_Complete_The_Request_Uow()
    {
        Options.CompleteUnitOfWorkOnResponseStarting = true;

        // A child scope (Begin without requiresNew) shares the request unit of work, so completing
        // it on response start would commit under the still-active scope; it is left to the end of
        // the pipeline instead, like a nested (requiresNew) unit of work.
        var body = await GetResponseAsStringAsync("/api/unitofwork-test/ChildUowDuringResponseFlush");
        body.ShouldBe("first:request-not-completed");
    }

    [Fact]
    public async Task An_Event_Handler_Starting_The_Response_During_The_End_Of_Pipeline_Completion_Should_Not_Fail()
    {
        Options.CompleteUnitOfWorkOnResponseStarting = true;

        // The response does not start during the pipeline here, so the middleware completes the
        // unit of work at its end; the event handler then starts the response from inside that
        // completion. The OnStarting callback must not attempt a second completion (which would
        // throw "Completion has already been requested for this unit of work").
        var body = await GetResponseAsStringAsync("/api/unitofwork-test/PublishEventThatWritesResponseOnCompletion");
        body.ShouldBe("event-written");
    }
}
