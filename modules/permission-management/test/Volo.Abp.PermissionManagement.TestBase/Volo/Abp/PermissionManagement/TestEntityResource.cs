using System;

namespace Volo.Abp.PermissionManagement;

public class TestEntityResource
{
    public static readonly string ResourceName = typeof(TestEntityResource).FullName;

    public static readonly string ResourceKey1 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey2 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey3 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey4 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey5 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey6 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey7 = Guid.NewGuid().ToString();
}
