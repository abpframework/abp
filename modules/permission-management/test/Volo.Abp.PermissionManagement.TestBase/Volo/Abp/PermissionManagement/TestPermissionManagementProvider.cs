using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement;

public class TestPermissionManagementProvider : PermissionManagementProvider
{
    public override string Name => "Test";

    public List<KeyValuePair<string, bool>> SetCalls { get; } = new List<KeyValuePair<string, bool>>();

    public List<string[]> CheckCalls { get; } = new List<string[]>();

    public TestPermissionManagementProvider(
        IPermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    {

    }

    public override Task SetAsync(string name, string providerKey, bool isGranted)
    {
        SetCalls.Add(new KeyValuePair<string, bool>(name, isGranted));
        return base.SetAsync(name, providerKey, isGranted);
    }

    public override Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
    {
        CheckCalls.Add(names);
        return base.CheckAsync(names, providerName, providerKey);
    }
}
