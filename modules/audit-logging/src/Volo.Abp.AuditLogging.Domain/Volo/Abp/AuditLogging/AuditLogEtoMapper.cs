using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AuditLogging;

public class AuditLogEtoMapper : IAuditLogEtoMapper, ITransientDependency
{
    public Task<AuditLogInfoEto> MapToAuditLogInfoEtoAsync(AuditLogInfo auditLogInfo)
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

    public Task<AuditLogInfo> MapToAuditLogInfoAsync(AuditLogInfoEto auditLogInfoEto)
    {
        return Task.FromResult(new AuditLogInfo {
            Actions = MapActionEtosToEntities(auditLogInfoEto.Actions),
            EntityChanges = MapEntityChangeEtosToEntities(auditLogInfoEto.EntityChanges),
            Exceptions = auditLogInfoEto.Exceptions,
            ExtraProperties = auditLogInfoEto.ExtraProperties,
            Comments = new List<string>(auditLogInfoEto.Comments),
            Url = auditLogInfoEto.Url,
            ApplicationName = auditLogInfoEto.ApplicationName,
            BrowserInfo = auditLogInfoEto.BrowserInfo,
            ClientId = auditLogInfoEto.ClientId,
            ClientName = auditLogInfoEto.ClientName,
            CorrelationId = auditLogInfoEto.CorrelationId,
            ExecutionDuration = auditLogInfoEto.ExecutionDuration,
            ExecutionTime = auditLogInfoEto.ExecutionTime,
            HttpMethod = auditLogInfoEto.HttpMethod,
            TenantId = auditLogInfoEto.TenantId,
            TenantName = auditLogInfoEto.TenantName,
            UserId = auditLogInfoEto.UserId,
            UserName = auditLogInfoEto.UserName,
            ClientIpAddress = auditLogInfoEto.ClientIpAddress,
            HttpStatusCode = auditLogInfoEto.HttpStatusCode,
            ImpersonatorTenantId = auditLogInfoEto.ImpersonatorTenantId,
            ImpersonatorTenantName = auditLogInfoEto.ImpersonatorTenantName,
            ImpersonatorUserId = auditLogInfoEto.ImpersonatorUserId,
            ImpersonatorUserName = auditLogInfoEto.ImpersonatorUserName
        });
    }

    private List<EntityChangeInfo> MapEntityChangeEtosToEntities(IEnumerable<EntityChangeInfoEto> entityChanges)
    {
        return entityChanges.Select(entityChange => MapEntityChangeEtoToEntity(entityChange)).ToList();
    }

    private EntityChangeInfo MapEntityChangeEtoToEntity(EntityChangeInfoEto entityChange)
    {
        return new EntityChangeInfo() {
            ChangeTime = entityChange.ChangeTime,
            ChangeType = entityChange.ChangeType,
            EntityId = entityChange.EntityId,
            EntityTenantId = entityChange.EntityTenantId,
            EntityTypeFullName = entityChange.EntityTypeFullName,
            ExtraProperties = entityChange.ExtraProperties,
            PropertyChanges = MapPropertyChangeEtosToEntities(entityChange.PropertyChanges)
        };
    }

    private List<EntityPropertyChangeInfo> MapPropertyChangeEtosToEntities(IEnumerable<EntityPropertyChangeInfoEto> propertyChanges)
    {
        return propertyChanges.Select(propertyChange => MapPropertyChangeEtoToEntity(propertyChange)).ToList();
    }

    private EntityPropertyChangeInfo MapPropertyChangeEtoToEntity(EntityPropertyChangeInfoEto propertyChange)
    {
        return new EntityPropertyChangeInfo {
            NewValue = propertyChange.NewValue,
            OriginalValue = propertyChange.OriginalValue,
            PropertyName = propertyChange.PropertyName,
            PropertyTypeFullName = propertyChange.PropertyTypeFullName
        };
    }

    private List<AuditLogActionInfo> MapActionEtosToEntities(IEnumerable<AuditLogActionInfoEto> actions)
    {
        return actions.Select(action => MapActionEtoToEntity(action)).ToList();
    }

    private AuditLogActionInfo MapActionEtoToEntity(AuditLogActionInfoEto actionEto)
    {
        return new AuditLogActionInfo {
            Parameters = actionEto.Parameters,
            ExecutionDuration = actionEto.ExecutionDuration,
            ExecutionTime = actionEto.ExecutionTime,
            MethodName = actionEto.MethodName,
            ServiceName = actionEto.ServiceName,
            ExtraProperties = actionEto.ExtraProperties
        };
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
            ServiceName = action.ServiceName,
            ExtraProperties = action.ExtraProperties
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
            ExtraProperties = entityChange.ExtraProperties,
            PropertyChanges = MapPropertyChangesToEtoList(entityChange.PropertyChanges)
        };
    }

    private List<EntityPropertyChangeInfoEto> MapPropertyChangesToEtoList(IEnumerable<EntityPropertyChangeInfo> entityChangePropertyChanges)
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