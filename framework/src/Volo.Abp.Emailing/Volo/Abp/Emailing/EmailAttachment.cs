using System;

namespace Volo.Abp.Emailing;

[Serializable]
public class EmailAttachment
{
    public string? Name { get; set; }

    public byte[]? File { get; set; }
}
