using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.AuditLogging
{
    public class AuditingStore : IAuditingStore, ITransientDependency
    {
        public ILogger<AuditingStore> Logger { get; set; }
        protected IAuditLogRepository AuditLogRepository { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected AbpAuditingOptions Options { get; }
        protected IAuditLogInfoToAuditLogConverter Converter { get; }
        public AuditingStore(
            IAuditLogRepository auditLogRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IOptions<AbpAuditingOptions> options,
            IAuditLogInfoToAuditLogConverter converter)
        {
            AuditLogRepository = auditLogRepository;
            UnitOfWorkManager = unitOfWorkManager;
            Converter = converter;
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
                await AuditLogRepository.InsertAsync(await Converter.ConvertAsync(auditInfo));
                await uow.CompleteAsync();
            }
        }
    }
}
