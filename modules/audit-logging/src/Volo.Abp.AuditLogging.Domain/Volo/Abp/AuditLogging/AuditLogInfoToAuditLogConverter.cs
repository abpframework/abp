using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace Volo.Abp.AuditLogging
{
    public class AuditLogInfoToAuditLogConverter : IAuditLogInfoToAuditLogConverter, ITransientDependency
    {
        protected IGuidGenerator GuidGenerator { get; }
        protected IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter { get; }
        protected IJsonSerializer JsonSerializer { get; }

        public AuditLogInfoToAuditLogConverter(IGuidGenerator guidGenerator, IExceptionToErrorInfoConverter exceptionToErrorInfoConverter, IJsonSerializer jsonSerializer)
        {
            GuidGenerator = guidGenerator;
            ExceptionToErrorInfoConverter = exceptionToErrorInfoConverter;
            JsonSerializer = jsonSerializer;
        }

        public virtual Task<AuditLog> ConvertAsync(AuditLogInfo auditLogInfo)
        {
            var auditLogId = GuidGenerator.Create();

            var extraProperties = new ExtraPropertyDictionary();
            if (auditLogInfo.ExtraProperties != null)
            {
                foreach (var pair in auditLogInfo.ExtraProperties)
                {
                    extraProperties.Add(pair.Key, pair.Value);
                }
            }

            var entityChanges = auditLogInfo
                                    .EntityChanges?
                                    .Select(entityChangeInfo => new EntityChange(GuidGenerator, auditLogId, entityChangeInfo, tenantId: auditLogInfo.TenantId))
                                    .ToList()
                                ?? new List<EntityChange>();

            var actions = auditLogInfo
                              .Actions?
                              .Select(auditLogActionInfo => new AuditLogAction(GuidGenerator.Create(), auditLogId, auditLogActionInfo, tenantId: auditLogInfo.TenantId))
                              .ToList()
                          ?? new List<AuditLogAction>();

            var remoteServiceErrorInfos = auditLogInfo.Exceptions?.Select(exception => ExceptionToErrorInfoConverter.Convert(exception, true))
                                          ?? new List<RemoteServiceErrorInfo>();

            var exceptions = remoteServiceErrorInfos.Any()
                ? JsonSerializer.Serialize(remoteServiceErrorInfos, indented: false)
                : null;

            var comments = auditLogInfo
                .Comments?
                .JoinAsString(Environment.NewLine);

            var auditLog = new AuditLog(
                auditLogId,
                auditLogInfo.ApplicationName,
                auditLogInfo.TenantId,
                auditLogInfo.TenantName,
                auditLogInfo.UserId,
                auditLogInfo.UserName,
                auditLogInfo.ExecutionTime,
                auditLogInfo.ExecutionDuration,
                auditLogInfo.ClientIpAddress,
                auditLogInfo.ClientName,
                auditLogInfo.ClientId,
                auditLogInfo.CorrelationId,
                auditLogInfo.BrowserInfo,
                auditLogInfo.HttpMethod,
                auditLogInfo.Url,
                auditLogInfo.HttpStatusCode,
                auditLogInfo.ImpersonatorUserId,
                auditLogInfo.ImpersonatorTenantId,
                extraProperties,
                entityChanges,
                actions,
                exceptions,
                comments
            );

            return Task.FromResult(auditLog);
        }
    }
}
