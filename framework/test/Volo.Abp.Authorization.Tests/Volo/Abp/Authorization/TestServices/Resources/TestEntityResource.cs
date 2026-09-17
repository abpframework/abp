using System;
using System.Collections.Generic;
using Volo.Abp.Authorization.Permissions.Resources;

namespace Volo.Abp.Authorization.TestServices.Resources;

public class TestEntityResource : IHasResourcePermissions
{
    public static readonly string ResourceName = typeof(TestEntityResource).FullName;

    public static readonly string ResourceKey1 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey2 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey3 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey4 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey5 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey6 = Guid.NewGuid().ToString();
    public static readonly string ResourceKey7 = Guid.NewGuid().ToString();

    private string Id { get; }

    public TestEntityResource(string id)
    {
        Id = id;
    }

    public string GetObjectKey()
    {
        return Id;
    }

    public Dictionary<string, bool> ResourcePermissions { get; set; }
}

public class TestEntityResource2
{
    public static readonly string ResourceName = typeof(TestEntityResource2).FullName;
}
