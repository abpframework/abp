using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureManagementTestData : ISingletonDependency
    {
        public Guid User1Id { get; } = Guid.NewGuid();
    }
}
