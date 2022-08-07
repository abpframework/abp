using System;
using System.Collections.Generic;
using Volo.Abp.MemoryDb;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MemoryDb;

public class TestAppMemoryDbContext : MemoryDbContext
{
    private readonly static Type[] EntityTypeList = {
            typeof(Person),
            typeof(EntityWithIntPk)
        };

    public override IReadOnlyList<Type> GetEntityTypes()
    {
        return EntityTypeList;
    }
}
