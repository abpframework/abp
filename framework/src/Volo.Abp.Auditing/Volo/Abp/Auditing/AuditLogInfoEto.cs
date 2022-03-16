using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;

namespace Volo.Abp.Auditing;

[EventName("abp.audit_log.create")]
[Serializable]
public class AuditLogInfoEto
{
    public string ApplicationName { get; set; }

    public Guid? UserId { get; set; }

    public string UserName { get; set; }

    public Guid? TenantId { get; set; }

    public string TenantName { get; set; }

    public Guid? ImpersonatorUserId { get; set; }

    public Guid? ImpersonatorTenantId { get; set; }

    public string ImpersonatorUserName { get; set; }

    public string ImpersonatorTenantName { get; set; }

    public DateTime ExecutionTime { get; set; }

    public int ExecutionDuration { get; set; }

    public string ClientId { get; set; }

    public string CorrelationId { get; set; }

    public string ClientIpAddress { get; set; }

    public string ClientName { get; set; }

    public string BrowserInfo { get; set; }

    public string HttpMethod { get; set; }

    public int? HttpStatusCode { get; set; }

    public string Url { get; set; }

    public List<AuditLogActionInfoEto> Actions { get; set; } = new();

    public List<Exception> Exceptions { get; set; }

    public List<EntityChangeInfoEto> EntityChanges { get; set; } = new();

    public List<string> Comments { get; set; }
}

[Serializable]
public class AuditLogActionInfoEto
{
    public string ServiceName { get; set; }

    public string MethodName { get; set; }

    public string Parameters { get; set; }

    public DateTime ExecutionTime { get; set; }

    public int ExecutionDuration { get; set; }
}

[Serializable]
public class EntityChangeInfoEto
{
    public DateTime ChangeTime { get; set; }

    public EntityChangeType ChangeType { get; set; }
    public Guid? EntityTenantId { get; set; }

    public string EntityId { get; set; }

    public string EntityTypeFullName { get; set; }

    public List<EntityPropertyChangeInfoEto> PropertyChanges { get; set; }
}

[Serializable]
public class EntityPropertyChangeInfoEto
{
    public string NewValue { get; set; }

    public string OriginalValue { get; set; }

    public string PropertyName { get; set; }

    public string PropertyTypeFullName { get; set; }
}