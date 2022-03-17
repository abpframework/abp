using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Volo.Abp.Auditing;

[EventName("abp.audit_log.create")]
[Serializable]
public class AuditLogInfoEto : EtoBase, IHasExtraProperties
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
    public ExtraPropertyDictionary ExtraProperties { get; set; }
}