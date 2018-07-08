using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Volo.Abp.Auditing
{
    public interface IAuditingHelper
    {
        bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false);

        AuditInfo CreateAuditInfo(Type type, MethodInfo method, object[] arguments);

        AuditInfo CreateAuditInfo(Type type, MethodInfo method, IDictionary<string, object> arguments);

        void Save(AuditInfo auditInfo);

        Task SaveAsync(AuditInfo auditInfo);
    }
}