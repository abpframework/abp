using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MemoryDb;

public abstract class MemoryDbContext : ISingletonDependency
{
    private readonly static Type[] EmptyTypeList = new Type[0];

    public virtual IReadOnlyList<Type> GetEntityTypes()
    {
        return EmptyTypeList;
    }
}
