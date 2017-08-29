using System;
using System.Collections.Generic;
using Volo.Abp.MemoryDb;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MemoryDb
{
    public class TestAppMemoryDbContext : MemoryDbContext
    {
        private static readonly Type[] EntityTypeList = new Type[]
        {
            typeof(Person)
        };

        public override IReadOnlyList<Type> GetEntityTypes()
        {
            return EntityTypeList;
        }
    }
}
