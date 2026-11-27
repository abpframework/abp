using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AgentTools.Sample;
using Volo.Abp.AgentTools.Sample.Governance;
using Volo.Abp.Authorization;
using Volo.Abp.DynamicProxy;
using Xunit;

namespace Volo.Abp.AgentTools;

// Governance edges: CRUD policy names, controller class-level [Authorize] on a derived controller, internal errors,
// proxied rollback, catalogue exclusions (stream inside a DTO, interface served by a controller).
public class AgentToolGovernanceTests : AgentToolsInvocationTestBase
{
    private static readonly Guid UserId = Guid.NewGuid();

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.Configure<AbpAgentToolOptions>(o => o.IncludedRemoteServiceNames.Add(GovernanceSampleConsts.RemoteServiceName));
    }

    [Fact]
    public async Task Crud_Service_Policy_Names_Filter_The_Tool_List_And_ABP_Refuses_The_Call()
    {
        using var _ = LoginAs(UserId);
        var arguments = new { input = new { maxResultCount = 10 } };

        (await GetToolNamesAsync()).ShouldNotContain("BookCrud_GetList"); // GetListPolicyName = Books.Default
        (await GetToolNamesAsync()).ShouldNotContain("BookCrud_Get");
        (await InvokeAsync("BookCrud_GetList", arguments)).ShouldStartWith($"ERROR {nameof(AbpAuthorizationException)}: ");

        PermissionStore.GrantToUser(UserId, SamplePermissions.Books.Default);

        var names = await GetToolNamesAsync();
        names.ShouldContain("BookCrud_GetList");
        names.ShouldContain("BookCrud_Get");
        names.ShouldNotContain("BookCrud_Create"); // CreatePolicyName = Books.Create, not granted
        Json(await InvokeAsync("BookCrud_GetList", arguments)).GetProperty("totalCount").GetInt64().ShouldBe(0);
    }

    [Fact]
    public async Task Class_Level_Authorize_Of_The_Actual_Controller_Counts_For_Actions_Declared_On_A_Base_Controller()
    {
        using var _ = LoginAs(UserId);

        (await GetToolNamesAsync()).ShouldNotContain("SampleVault_GetSecret");
        (await InvokeAsync("SampleVault_GetSecret")).ShouldStartWith($"ERROR {nameof(AbpAuthorizationException)}: ");

        PermissionStore.GrantToUser(UserId, SamplePermissions.Reports.Default);

        (await GetToolNamesAsync()).ShouldContain("SampleVault_GetSecret");
        Json(await InvokeAsync("SampleVault_GetSecret")).GetString().ShouldBe("42");
    }

    [Fact]
    public async Task Internal_Error_Message_Is_Hidden_And_The_Proxied_Write_Is_Rolled_Back()
    {
        ProxyHelper.IsProxy(GetRequiredService<ISampleVaultAppService>()).ShouldBeTrue();
        PermissionStore.GrantToUser(UserId, SamplePermissions.Reports.Default);
        using var _ = LoginAs(UserId);

        var result = await InvokeAsync("SampleVault_CreateBookThenCrash", new { input = new { name = "Vaulted" } });

        result.ShouldBe("ERROR InvalidOperationException: An internal error occurred during your request!");
        result.ShouldNotContain(SampleVaultAppService.InternalDetail);
        Observer.Records.Last().ErrorMessage.ShouldNotBeNull().ShouldNotContain(SampleVaultAppService.InternalDetail);
        InvokerLogger.Messages.ShouldContain(m => m.Contains("SampleVault_CreateBookThenCrash")); // the server log keeps the exception

        Json(await InvokeAsync("Book_GetList")).EnumerateArray().ShouldNotContain(b => b.GetProperty("name").GetString() == "Vaulted");
    }

    [Fact]
    public void Stream_Inside_An_Input_Dto_And_Interfaces_Served_By_Controllers_Are_Never_Tools()
    {
        var catalog = GetRequiredService<IAgentToolCatalog>();

        catalog.Find("SampleVault_Upload").ShouldBeNull();
        catalog.Skipped.ShouldContain(s => s.ControllerType == typeof(SampleVaultController).FullName
                                           && s.ActionName == nameof(ISampleVaultAppService.UploadAsync)
                                           && s.Reason.StartsWith("non-JSON input"));

        catalog.Skipped.ShouldContain(s => s.ControllerType == typeof(SampleSelfServedController).FullName
                                           && s.Reason.StartsWith("service interface resolves to a controller"));
    }

    private async Task<string[]> GetToolNamesAsync()
    {
        return (await Provider.GetToolsAsync(WithApproval)).Select(t => t.Definition.Name).ToArray();
    }
}
