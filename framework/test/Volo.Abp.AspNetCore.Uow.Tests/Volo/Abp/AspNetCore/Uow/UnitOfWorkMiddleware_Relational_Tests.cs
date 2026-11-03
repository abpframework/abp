using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Xunit;

namespace Volo.Abp.AspNetCore.Uow;

public class UnitOfWorkMiddleware_Relational_Tests : AbpWebApplicationFactoryIntegratedTest<Program>
{
    private void EnableCompleteOnResponseStarting()
    {
        ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreUnitOfWorkOptions>>()
            .Value.CompleteUnitOfWorkOnResponseStarting = true;
    }

    private async Task<int> CountAsync(string name)
    {
        var response = await Client.GetAsync("/api/uow-visibility/count?name=" + name);
        response.EnsureSuccessStatusCode();
        return int.Parse(await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Row_Written_During_Request_Is_Visible_From_An_Independent_Connection_On_Response_Start()
    {
        EnableCompleteOnResponseStarting();

        var response = await Client.GetAsync("/api/uow-visibility/insert-then-read");
        response.EnsureSuccessStatusCode();
        (await response.Content.ReadAsStringAsync()).ShouldBe("inserted:visible");
    }

    [Fact]
    public async Task Row_Written_During_Request_Is_Still_Committed_When_The_Feature_Is_Disabled()
    {
        var name = Guid.NewGuid().ToString("N");

        var insert = await Client.GetAsync("/api/uow-visibility/insert?name=" + name);
        insert.EnsureSuccessStatusCode();

        (await CountAsync(name)).ShouldBe(1);
    }

    [Fact]
    public async Task Exception_Before_Response_Rolls_Back_The_Written_Row()
    {
        EnableCompleteOnResponseStarting();
        var name = Guid.NewGuid().ToString("N");

        var insert = await Client.GetAsync("/api/uow-visibility/insert-then-throw?name=" + name);
        insert.IsSuccessStatusCode.ShouldBeFalse();

        (await CountAsync(name)).ShouldBe(0);
    }

    [Fact]
    public async Task Committed_Row_Survives_An_Exception_Raised_After_The_Response_Started()
    {
        EnableCompleteOnResponseStarting();
        var name = Guid.NewGuid().ToString("N");

        await Should.ThrowAsync<HttpRequestException>(async () =>
        {
            var response = await Client.GetAsync("/api/uow-visibility/insert-flush-then-throw?name=" + name);
            await response.Content.ReadAsStringAsync();
        });

        (await CountAsync(name)).ShouldBe(1);
    }
    [Fact]
    public async Task Committed_Row_Survives_A_Completed_Handler_Failing_On_Response_Start()
    {
        EnableCompleteOnResponseStarting();
        var name = Guid.NewGuid().ToString("N");

        // The handler's error must surface as-is (not masked by a second "already requested" completion);
        // the row is committed regardless, since the handler runs after commit.
        Exception surfaced = null;
        try
        {
            var response = await Client.GetAsync("/api/uow-visibility/insert-flush-throwing-completed-handler?name=" + name);
            await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            surfaced = ex;
        }

        surfaced.ShouldNotBeNull();
        surfaced.ToString().ShouldContain("boom in a completed handler");
        surfaced.ToString().ShouldNotContain("already");

        (await CountAsync(name)).ShouldBe(1);
    }
    [Fact]
    public async Task A_Failing_Commit_On_Response_Start_Does_Not_Persist_Data()
    {
        EnableCompleteOnResponseStarting();
        var name = Guid.NewGuid().ToString("N");

        Exception surfaced = null;
        try
        {
            var response = await Client.GetAsync("/api/uow-visibility/insert-then-fail-commit?name=" + name);
            await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            surfaced = ex;
        }

        // The commit fails on response start, so the request must surface an error and persist nothing.
        surfaced.ShouldNotBeNull();
        (await CountAsync(name)).ShouldBe(0);
    }

    [Fact]
    public async Task Result_Serialization_Failure_Rolls_Back_And_Does_Not_Commit_On_The_Error_Response()
    {
        EnableCompleteOnResponseStarting();
        var name = Guid.NewGuid().ToString("N");

        HttpResponseMessage response = null;
        Exception surfaced = null;
        try
        {
            response = await Client.GetAsync("/api/uow-visibility/insert-then-throw-in-serialization?name=" + name);
            await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            surfaced = ex;
        }

        // The action ran and saved the row, then serializing the result failed. The request must therefore
        // fail with a server error (not a 404 or a success), and the error response, written by the upstream
        // exception middleware after the request unit of work is disposed, must not commit the failed request.
        (surfaced != null || (response != null && (int)response.StatusCode >= 500)).ShouldBeTrue();
        (await CountAsync(name)).ShouldBe(0);
    }
}
