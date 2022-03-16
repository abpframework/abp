using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AuditLogging;

public class AuditLogEtoMapper : IAuditLogEtoMapper, ITransientDependency
{
    public Task<AuditLogInfoEto> CovertToAuditLogInfoEtoAsync(AuditLogInfo auditLogInfo)
    {
        return Task.FromResult(new AuditLogInfoEto {
            Actions = MapActionsToEtoList(auditLogInfo.Actions),
            EntityChanges = MapEntityChangesToEtoList(auditLogInfo.EntityChanges),
            Comments = new List<string>(auditLogInfo.Comments),
            Exceptions = auditLogInfo.Exceptions,
            Url = auditLogInfo.Url,
            ApplicationName = auditLogInfo.ApplicationName,
            BrowserInfo = auditLogInfo.BrowserInfo,
            ClientId = auditLogInfo.ClientId,
            ClientName = auditLogInfo.ClientName,
            CorrelationId = auditLogInfo.CorrelationId,
            ExecutionDuration = auditLogInfo.ExecutionDuration,
            ExecutionTime = auditLogInfo.ExecutionTime,
            HttpMethod = auditLogInfo.HttpMethod,
            TenantId = auditLogInfo.TenantId,
            TenantName = auditLogInfo.TenantName,
            UserId = auditLogInfo.UserId,
            UserName = auditLogInfo.UserName,
            ClientIpAddress = auditLogInfo.ClientIpAddress,
            HttpStatusCode = auditLogInfo.HttpStatusCode,
            ImpersonatorTenantId = auditLogInfo.ImpersonatorTenantId,
            ImpersonatorTenantName = auditLogInfo.ImpersonatorTenantName,
            ImpersonatorUserId = auditLogInfo.ImpersonatorUserId,
            ImpersonatorUserName = auditLogInfo.ImpersonatorUserName
        });
    }

    public Task<AuditLogInfo> CovertToAuditLogInfoAsync(AuditLogInfoEto auditLogInfoEto)
    {
        return Task.FromResult(new AuditLogInfo()
        {
            Actions = MapActionEtosToEntities(auditLogInfoEto.Actions),
            // Exceptions = {  },
            // EntityChanges = {  },
            Comments = new List<string>(eventData.Comments),
            Url = eventData.Url,
            ApplicationName = eventData.ApplicationName,
            BrowserInfo = eventData.BrowserInfo,
            ClientId = eventData.ClientId,
            ClientName = eventData.ClientName,
            CorrelationId = eventData.CorrelationId,
            ExecutionDuration = eventData.ExecutionDuration,
            ExecutionTime = eventData.ExecutionTime,
            HttpMethod = eventData.HttpMethod,
            TenantId = eventData.TenantId,
            TenantName = eventData.TenantName,
            UserId = eventData.UserId,
            UserName = eventData.UserName,
            ClientIpAddress = eventData.ClientIpAddress,
            HttpStatusCode = eventData.HttpStatusCode,
            ImpersonatorTenantId = eventData.ImpersonatorTenantId,
            ImpersonatorTenantName = eventData.ImpersonatorTenantName,
            ImpersonatorUserId = eventData.ImpersonatorUserId,
            ImpersonatorUserName = eventData.ImpersonatorUserName
        };)
    }


    private List<AuditLogActionInfoEto> MapActionsToEtoList(IEnumerable<AuditLogActionInfo> auditInfoActions)
    {
        return auditInfoActions.Select(action => MapActionToEto(action)).ToList();
    }

    private AuditLogActionInfoEto MapActionToEto(AuditLogActionInfo action)
    {
        return new AuditLogActionInfoEto {
            Parameters = action.Parameters,
            ExecutionDuration = action.ExecutionDuration,
            ExecutionTime = action.ExecutionTime,
            MethodName = action.MethodName,
            ServiceName = action.ServiceName
        };
    }

    private List<EntityChangeInfoEto> MapEntityChangesToEtoList(IEnumerable<EntityChangeInfo> auditInfoEntityChanges)
    {
        return auditInfoEntityChanges.Select(entityChange => MapEntityChangesToEto(entityChange)).ToList();
    }

    private EntityChangeInfoEto MapEntityChangesToEto(EntityChangeInfo entityChange)
    {
        return new EntityChangeInfoEto {
            ChangeTime = entityChange.ChangeTime,
            ChangeType = entityChange.ChangeType,
            EntityId = entityChange.EntityId,
            EntityTenantId = entityChange.EntityTenantId,
            EntityTypeFullName = entityChange.EntityTypeFullName,
            PropertyChanges = MapPropertyChangesToEtoList(entityChange.PropertyChanges)
        };
    }

    private List<EntityPropertyChangeInfoEto> MapPropertyChangesToEtoList(
        IEnumerable<EntityPropertyChangeInfo> entityChangePropertyChanges)
    {
        return entityChangePropertyChanges.Select(propertyChange => MapPropertyChangesToEto(propertyChange)).ToList();
    }

    private EntityPropertyChangeInfoEto MapPropertyChangesToEto(EntityPropertyChangeInfo propertyChange)
    {
        return new EntityPropertyChangeInfoEto {
            NewValue = propertyChange.NewValue,
            OriginalValue = propertyChange.OriginalValue,
            PropertyName = propertyChange.PropertyName,
            PropertyTypeFullName = propertyChange.PropertyTypeFullName
        };
    }
}