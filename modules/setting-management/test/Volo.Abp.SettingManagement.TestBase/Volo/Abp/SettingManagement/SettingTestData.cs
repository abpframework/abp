using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.SettingManagement
{
    public class SettingTestData : ISingletonDependency
    {
        public Guid User1Id { get; } = Guid.NewGuid();
        public Guid User2Id { get; } = Guid.NewGuid();

        public Guid SettingId { get; } = Guid.NewGuid();
    }
}