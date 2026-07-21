using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

public class AbpPermissionManagementApplicationTestBase : PermissionManagementTestBase<AbpPermissionManagementApplicationTestModule>
{
    protected Guid? CurrentUserId { get; set; }

    protected AbpPermissionManagementApplicationTestBase()
    {
        CurrentUserId = Guid.NewGuid();
    }
    protected override void AfterAddApplication(IServiceCollection services)
    {
        var fakePermissionChecker = new FakePermissionChecker();
        services.AddSingleton(fakePermissionChecker);
        services.Replace(ServiceDescriptor.Singleton<IPermissionChecker>(fakePermissionChecker));
    }
}
