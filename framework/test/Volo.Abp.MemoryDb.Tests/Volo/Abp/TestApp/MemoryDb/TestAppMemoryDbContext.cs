using System;
using System.Collections.Generic;
using Volo.Abp.MemoryDb;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;

namespace Volo.Abp.TestApp.MemoryDb;

public class TestAppMemoryDbContext : MemoryDbContext
{
    private static readonly Type[] EntityTypeList = {
            typeof(Person),
            typeof(EntityWithIntPk),
            typeof(Product)
    };

    public override IReadOnlyList<Type> GetEntityTypes()
    {
        return EntityTypeList;
    }
}
