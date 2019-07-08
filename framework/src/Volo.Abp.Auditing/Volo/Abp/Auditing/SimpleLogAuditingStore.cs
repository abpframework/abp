using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing
{
    [Dependency(TryRegister = true)]
    public class SimpleLogAuditingStore : IAuditingStore, ISingletonDependency
    {
        public ILogger<SimpleLogAuditingStore> Logger { get; set; }

        public SimpleLogAuditingStore()
        {
            Logger = NullLogger<SimpleLogAuditingStore>.Instance;
        }

        public void Save(AuditLogInfo auditInfo)
        {
            Logger.LogInformation(auditInfo.ToString());
        }

        public Task SaveAsync(AuditLogInfo auditInfo)
        {
            Save(auditInfo);
            return Task.FromResult(0);
        }
    }
}