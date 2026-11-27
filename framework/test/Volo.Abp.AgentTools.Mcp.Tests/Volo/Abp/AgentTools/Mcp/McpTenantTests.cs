using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AgentTools.Mcp.Sample;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.AgentTools.Mcp;

public class McpTenantTests : AgentToolsMcpTestBase
{
    [Fact]
    public async Task Each_Token_Sees_Only_Its_Own_Tenants_Data_And_A_Host_Token_The_Hosts()
    {
        await SeedNotesAsync(TenantA, "a1");
        await SeedNotesAsync(TenantB, "b1");
        await SeedNotesAsync(null, "h1");

        (await GetNotesAsync(NewUser(TenantA))).ShouldBe("[\"a1\"]");
        (await GetNotesAsync(NewUser(TenantB))).ShouldBe("[\"b1\"]");
        (await GetNotesAsync(NewUser(null))).ShouldBe("[\"h1\"]");
    }

    [Theory]
    [InlineData("header", "tenant-b")]
    [InlineData("header", "0b0b0b0b-0000-0000-0000-00000000000b")]
    [InlineData("query", "tenant-b")]
    [InlineData("cookie", "tenant-b")]
    public async Task Tenant_Token_With_Another_Tenant_Named_In_The_Request_Is_Refused(string where, string tenant)
    {
        var response = await PostWithTenantAsync(NewUser(TenantA), where, tenant);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await response.Content.ReadAsStringAsync()).ShouldContain("other than the token's tenant");
    }

    [Fact]
    public async Task Host_Token_With_A_Tenant_Named_In_The_Request_Is_Refused()
    {
        (await PostWithTenantAsync(NewUser(null), "header", "tenant-a")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task The_Tokens_Own_Tenant_Named_In_The_Request_Is_Accepted()
    {
        (await PostWithTenantAsync(NewUser(TenantA), "header", "tenant-a")).StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Token_Of_An_Unknown_Tenant_Is_Refused()
    {
        var response = await PostInitializeAsync(CreateHttpClient(NewUser(Guid.NewGuid())));

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await response.Content.ReadAsStringAsync()).ShouldContain("does not exist or is not active");
    }

    [Fact]
    public async Task Token_Of_An_Inactive_Tenant_Is_Refused()
    {
        var response = await PostInitializeAsync(CreateHttpClient(NewUser(AgentToolsMcpTestModule.InactiveTenant)));

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await response.Content.ReadAsStringAsync()).ShouldContain("does not exist or is not active");
    }

    [Fact]
    public async Task Cookie_Session_Of_Another_Tenants_User_Changes_Neither_User_Nor_Tenant()
    {
        await SeedNotesAsync(TenantA, "a1");
        await SeedNotesAsync(TenantB, "b1");
        var token = NewUser(TenantA);
        var http = CreateHttpClient(token);
        http.DefaultRequestHeaders.Add("Cookie", await LoginWithCookieAsync(Guid.NewGuid(), TenantB));
        await using var client = await ConnectAsync(http);

        Text(await CallAsync(client, McpSampleAppService.GetNotes)).ShouldBe("[\"a1\"]");
        Observer.Records.ShouldHaveSingleItem().UserId.ShouldBe(token.UserId);
    }

    [Fact]
    public async Task Feature_Off_For_The_Tenant_Gives_An_Empty_List_And_Refuses_By_Name()
    {
        FeatureStore.SetForTenant(TenantA, AgentToolsFeatures.Enable, "false");
        await using var client = await ConnectAsync(NewUser(TenantA));

        (await client.ListToolsAsync()).ShouldBeEmpty();
        var result = await CallAsync(client, McpSampleAppService.GetNotes);
        result.IsError.ShouldBe(true);
        Text(result).ShouldBe($"ERROR BusinessException: Agent tools are not enabled for the current tenant. (code: {AgentToolsErrorCodes.FeatureDisabled})");

        await using var other = await ConnectAsync(NewUser(TenantB));
        (await other.ListToolsAsync()).ShouldNotBeEmpty();
    }

    private async Task<string> GetNotesAsync(Token token)
    {
        await using var client = await ConnectAsync(token);
        var result = await CallAsync(client, McpSampleAppService.GetNotes);
        result.IsError.ShouldNotBe(true, Text(result));
        return Text(result);
    }

    private Task<System.Net.Http.HttpResponseMessage> PostWithTenantAsync(Token token, string where, string tenant)
    {
        var client = CreateHttpClient(token);
        switch (where)
        {
            case "header":
                client.DefaultRequestHeaders.Add("__tenant", tenant);
                return PostInitializeAsync(client);
            case "cookie":
                client.DefaultRequestHeaders.Add("Cookie", "__tenant=" + tenant);
                return PostInitializeAsync(client);
            default:
                return PostInitializeAsync(client, "/mcp?__tenant=" + tenant);
        }
    }
}

/// <summary>
/// The domain resolver ("{0}.example.test"). ABP stops at it whenever it runs (it marks the context handled even without a
/// match), so with it the header, query and cookie are not consulted at all; hence a host of its own.
/// </summary>
public class McpDomainTenantTests : AgentToolsMcpTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpTenantResolveOptions>(options => options.AddDomainTenantResolver("{0}.example.test"));
    }

    [Fact]
    public async Task Tenant_Token_On_Another_Tenants_Domain_Is_Refused_And_On_Its_Own_Domain_Accepted()
    {
        var response = await PostInitializeAsync(CreateHttpClient(NewUser(TenantA)), "http://tenant-b.example.test/mcp");
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await response.Content.ReadAsStringAsync()).ShouldContain("other than the token's tenant");

        (await PostInitializeAsync(CreateHttpClient(NewUser(TenantA)), "http://tenant-a.example.test/mcp")).StatusCode
            .ShouldBe(HttpStatusCode.OK);
    }
}
