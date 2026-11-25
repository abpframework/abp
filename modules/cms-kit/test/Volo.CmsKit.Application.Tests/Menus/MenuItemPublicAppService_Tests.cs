using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.CmsKit.Public.Menus;
using Xunit;

namespace Volo.CmsKit.Menus;

public class MenuItemPublicAppService_Tests : CmsKitApplicationTestBase
{
    private const string RequiredPermissionName = "MyPermission";

    protected IMenuItemPublicAppService MenuPublicAppService { get; }
    protected IMenuItemRepository MenuRepository { get; }
    protected CmsKitTestData TestData { get; }
    private IPermissionChecker _permissionChecker;

    public MenuItemPublicAppService_Tests()
    {
        MenuPublicAppService = GetRequiredService<IMenuItemPublicAppService>();
        MenuRepository = GetRequiredService<IMenuItemRepository>();
        TestData = GetRequiredService<CmsKitTestData>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _permissionChecker = Substitute.For<IPermissionChecker>();
        SetPermissionGrantResult(PermissionGrantResult.Prohibited);
        services.AddSingleton(_permissionChecker);
    }

    [Fact]
    public async Task GetListAsync_ShouldWorkProperly()
    {
        var result = await MenuPublicAppService.GetListAsync();

        result.ShouldNotBeNull();
        result.Count.ShouldBe(6);
    }

    [Fact]
    public async Task GetListAsync_ShouldNotReturn_InactiveItems_And_TheirChildren()
    {
        var inactiveItemId = Guid.NewGuid();
        var activeChildOfInactiveItemId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await MenuRepository.InsertAsync(new MenuItem(inactiveItemId, "Inactive", "/inactive", isActive: false));
            await MenuRepository.InsertAsync(new MenuItem(activeChildOfInactiveItemId, "Child", "/inactive/child", parentId: inactiveItemId));
        });

        var result = await MenuPublicAppService.GetListAsync();

        result.Count.ShouldBe(6);
        result.ShouldNotContain(x => x.Id == inactiveItemId);
        result.ShouldNotContain(x => x.Id == activeChildOfInactiveItemId);
    }

    [Fact]
    public async Task GetListAsync_ShouldNotReturn_PermissionProtectedItems_IfPermissionIsNotGranted()
    {
        var protectedItemId = Guid.NewGuid();
        var childOfProtectedItemId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await MenuRepository.InsertAsync(new MenuItem(protectedItemId, "Protected", "/protected", requiredPermissionName: RequiredPermissionName));
            await MenuRepository.InsertAsync(new MenuItem(childOfProtectedItemId, "Child", "/protected/child", parentId: protectedItemId));
        });

        var result = await MenuPublicAppService.GetListAsync();

        result.Count.ShouldBe(6);
        result.ShouldNotContain(x => x.Id == protectedItemId);
        result.ShouldNotContain(x => x.Id == childOfProtectedItemId);
    }

    [Fact]
    public async Task GetListAsync_ShouldReturn_PermissionProtectedItems_IfPermissionIsGranted()
    {
        var protectedItemId = Guid.NewGuid();
        var childOfProtectedItemId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await MenuRepository.InsertAsync(new MenuItem(protectedItemId, "Protected", "/protected", requiredPermissionName: RequiredPermissionName));
            await MenuRepository.InsertAsync(new MenuItem(childOfProtectedItemId, "Child", "/protected/child", parentId: protectedItemId));
        });

        var hidden = await MenuPublicAppService.GetListAsync();
        hidden.ShouldNotContain(x => x.Id == protectedItemId);

        SetPermissionGrantResult(PermissionGrantResult.Granted);

        var result = await MenuPublicAppService.GetListAsync();

        result.Count.ShouldBe(8);
        result.ShouldContain(x => x.Id == protectedItemId);
        result.ShouldContain(x => x.Id == childOfProtectedItemId);

        SetPermissionGrantResult(PermissionGrantResult.Prohibited);
        (await MenuPublicAppService.GetListAsync()).ShouldNotContain(x => x.Id == protectedItemId);
    }

    private void SetPermissionGrantResult(PermissionGrantResult grantResult)
    {
        _permissionChecker
            .IsGrantedAsync(Arg.Any<string[]>())
            .Returns(callInfo => Task.FromResult(new MultiplePermissionGrantResult(callInfo.Arg<string[]>(), grantResult)));
    }
}
