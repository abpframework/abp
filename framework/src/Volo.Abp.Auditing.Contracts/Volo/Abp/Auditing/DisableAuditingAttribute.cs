using System;

namespace Volo.Abp.Auditing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisableAuditingAttribute : Attribute
{
    /// <summary>
    /// When set to <c>false</c>, changes to this entity property will not update audit properties (like <see cref="AuditLogInfo.LastModificationTime"/>).
    /// </summary>
    public bool UpdateModificationProps { get; set; } = true;

    /// <summary>
    /// When set to <c>false</c>, changes to this entity property will not publish entity change events (<see cref="EntityUpdatedEvent"/>).
    /// </summary>
    public bool PublishEntityEvent { get; set; } = true;
}
