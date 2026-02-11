using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace Volo.Abp.Emailing;

[Serializable]
public class AdditionalEmailSendingArgs
{
    public List<string>? CC { get; set; }

    public List<EmailAttachment>? Attachments { get; set; }

    public ExtraPropertyDictionary? ExtraProperties { get; set; }
}
