using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AuditLogging
{
    public class AuditingTestDataBuilder : ITransientDependency
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly AuditingTestData _auditingTestData;

        public AuditingTestDataBuilder(IAuditLogRepository auditLogRepository, AuditingTestData auditingTestData )
        {
            _auditLogRepository = auditLogRepository;
            _auditingTestData = auditingTestData;
        }

        public void Build()
        {
            
        }
    }
}