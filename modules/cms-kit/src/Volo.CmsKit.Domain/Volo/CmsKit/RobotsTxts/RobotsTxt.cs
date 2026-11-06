using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.RobotsTxts;

public class RobotsTxt : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual string Content { get; private set; }

    public virtual Guid? TenantId { get; protected set; }

    protected RobotsTxt()
    {
    }

    internal RobotsTxt(
        Guid id,
        [CanBeNull] string content,
        Guid? tenantId = null) : base(id)
    {
        SetContent(content);
        TenantId = tenantId;
    }

    public virtual void SetContent([CanBeNull] string content)
    {
        content = NormalizeLineEndings(content ?? string.Empty);
        Check.Length(content, nameof(content), RobotsTxtConsts.MaxContentLength);

        Content = content;
    }

    protected static string NormalizeLineEndings(string content)
    {
        return content.Replace("\r\n", "\n").Replace("\r", "\n");
    }
}
