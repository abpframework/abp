using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Volo.Abp.AuditLogging
{
    public class AuditingStore : IAuditingStore, ITransientDependency
    {
        public ILogger<AuditingStore> Logger { get; set; }

        protected IAuditLogRepository AuditLogRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected AbpAuditingOptions Options { get; }

        public AuditingStore(
            IAuditLogRepository auditLogRepository,
            IGuidGenerator guidGenerator,
            IUnitOfWorkManager unitOfWorkManager,
            IOptions<AbpAuditingOptions> options)
        {
            AuditLogRepository = auditLogRepository;
            GuidGenerator = guidGenerator;
            UnitOfWorkManager = unitOfWorkManager;
            Options = options.Value;

            Logger = NullLogger<AuditingStore>.Instance;
        }

        public virtual async Task SaveAsync(AuditLogInfo auditInfo)
        {
            if (!Options.HideErrors)
            {
                await SaveLogAsync(auditInfo);
                return;
            }

            try
            {
                await SaveLogAsync(auditInfo);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not save the audit log object: " + Environment.NewLine + auditInfo.ToString());
                Logger.LogException(ex, LogLevel.Error);
            }
        }

        protected virtual async Task SaveLogAsync(AuditLogInfo auditInfo)
        {
            using (var uow = UnitOfWorkManager.Begin(true))
            {
                await AuditLogRepository.InsertAsync(new AuditLog(GuidGenerator, auditInfo));
                await uow.CompleteAsync();
            }
        }
    }
}