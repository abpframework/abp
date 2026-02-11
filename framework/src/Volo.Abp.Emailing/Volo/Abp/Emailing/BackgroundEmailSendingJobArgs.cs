using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Emailing;

[Serializable]
public class BackgroundEmailSendingJobArgs : IMultiTenant
{
    public Guid? TenantId { get; set; }

    public string? From { get; set; }

    public string To { get; set; } = default!;

    public string? Subject { get; set; }

    public string? Body { get; set; }

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool IsBodyHtml { get; set; } = true;

    public AdditionalEmailSendingArgs? AdditionalEmailSendingArgs { get; set; }
}
