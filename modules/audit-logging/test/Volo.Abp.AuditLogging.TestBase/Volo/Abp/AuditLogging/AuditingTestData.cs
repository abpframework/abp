using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AuditLogging
{
    public class AuditingTestData : ISingletonDependency
    {
        public Guid UserId { get; } = Guid.NewGuid();

        public Guid TenantId { get; } = Guid.NewGuid();

        public Guid ImpersonatorUserId { get; } = Guid.NewGuid();

        public Guid ImpersonatorTenantId { get; } = Guid.NewGuid();
    }
}
