using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Xunit;

namespace Volo.Abp.Authorization;

public class PermissionChecker_Tests: AuthorizationTestBase
{
    private readonly IPermissionChecker _permissionChecker;
    
    public PermissionChecker_Tests()
    {
        _permissionChecker = GetRequiredService<IPermissionChecker>();
    }

    [Fact]
    public async Task IsGrantedAsync()
    {
        (await _permissionChecker.IsGrantedAsync("MyPermission5")).ShouldBe(true);
        (await _permissionChecker.IsGrantedAsync("UndefinedPermission")).ShouldBe(false);
    }

    [Fact]
    public async Task IsGranted_Multiple_Result_Async()
    {
        var result = await _permissionChecker.IsGrantedAsync(new []
        {
            "MyPermission1",
            "MyPermission2",
            "UndefinedPermission",
            "MyPermission3",
            "MyPermission4",
            "MyPermission5",
            "MyPermission6",
            "MyPermission7"
        });
        
        result.Result["MyPermission1"].ShouldBe(PermissionGrantResult.Undefined);
        result.Result["MyPermission2"].ShouldBe(PermissionGrantResult.Prohibited);
        result.Result["UndefinedPermission"].ShouldBe(PermissionGrantResult.Prohibited);
        result.Result["MyPermission3"].ShouldBe(PermissionGrantResult.Granted);
        result.Result["MyPermission4"].ShouldBe(PermissionGrantResult.Prohibited);
        result.Result["MyPermission5"].ShouldBe(PermissionGrantResult.Granted);
        result.Result["MyPermission6"].ShouldBe(PermissionGrantResult.Granted);
        result.Result["MyPermission7"].ShouldBe(PermissionGrantResult.Granted);
        
    }
}