using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Users
{
    public class AbpUsersLocalTestData : ISingletonDependency
    {
        public AbpUser John { get; }
        public AbpUser David { get; }

        public AbpUsersLocalTestData()
        {
            John = new AbpUser(Guid.NewGuid(), "john");
            David = new AbpUser(Guid.NewGuid(), "david", "david@abp.io");
        }
    }
}