using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement;

public class TestResourcePermissionManagementProvider : ResourcePermissionManagementProvider
{
    public override string Name => "Test";

    public TestResourcePermissionManagementProvider(
        IResourcePermissionGrantRepository resourcePermissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            resourcePermissionGrantRepository,
            guidGenerator,
            currentTenant)
    {

    }
}
