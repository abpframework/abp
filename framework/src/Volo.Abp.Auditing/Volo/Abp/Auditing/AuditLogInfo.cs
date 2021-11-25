using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Data;

namespace Volo.Abp.Auditing;

[Serializable]
public class AuditLogInfo : IHasExtraProperties
{
    public string ApplicationName { get; set; }

    public Guid? UserId { get; set; }

    public string UserName { get; set; }

    public Guid? TenantId { get; set; }

    public string TenantName { get; set; }

    public Guid? ImpersonatorUserId { get; set; }

    public Guid? ImpersonatorTenantId { get; set; }

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

    public List<AuditLogActionInfo> Actions { get; set; }

    public List<Exception> Exceptions { get; }

    public ExtraPropertyDictionary ExtraProperties { get; }

    public List<EntityChangeInfo> EntityChanges { get; }

    public List<string> Comments { get; set; }

    public AuditLogInfo()
    {
        Actions = new List<AuditLogActionInfo>();
        Exceptions = new List<Exception>();
        ExtraProperties = new ExtraPropertyDictionary();
        EntityChanges = new List<EntityChangeInfo>();
        Comments = new List<string>();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"AUDIT LOG: [{HttpStatusCode?.ToString() ?? "---"}: {(HttpMethod ?? "-------").PadRight(7)}] {Url}");
        sb.AppendLine($"- UserName - UserId                 : {UserName} - {UserId}");
        sb.AppendLine($"- ClientIpAddress        : {ClientIpAddress}");
        sb.AppendLine($"- ExecutionDuration      : {ExecutionDuration}");

        if (Actions.Any())
        {
            sb.AppendLine("- Actions:");
            foreach (var action in Actions)
            {
                sb.AppendLine($"  - {action.ServiceName}.{action.MethodName} ({action.ExecutionDuration} ms.)");
                sb.AppendLine($"    {action.Parameters}");
            }
        }

        if (Exceptions.Any())
        {
            sb.AppendLine("- Exceptions:");
            foreach (var exception in Exceptions)
            {
                sb.AppendLine($"  - {exception.Message}");
                sb.AppendLine($"    {exception}");
            }
        }

        if (EntityChanges.Any())
        {
            sb.AppendLine("- Entity Changes:");
            foreach (var entityChange in EntityChanges)
            {
                sb.AppendLine($"  - [{entityChange.ChangeType}] {entityChange.EntityTypeFullName}, Id = {entityChange.EntityId}");
                foreach (var propertyChange in entityChange.PropertyChanges)
                {
                    sb.AppendLine($"    {propertyChange.PropertyName}: {propertyChange.OriginalValue} -> {propertyChange.NewValue}");
                }
            }
        }

        return sb.ToString();
    }
}
