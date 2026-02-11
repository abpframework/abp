using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging;

public class AuditLogExcelFile : CreationAuditedEntity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    
    public virtual string FileName { get; protected set; }
    
    protected AuditLogExcelFile()
    {
    }

    public AuditLogExcelFile(
        Guid id,
        string fileName,
        Guid? tenantId = null,
        Guid? creatorId = null) : base(id)
    {
        FileName = Check.NotNullOrWhiteSpace(fileName, nameof(fileName), AuditLogExcelFileConsts.MaxFileNameLength);
        TenantId = tenantId;
        CreatorId = creatorId;
    }
}