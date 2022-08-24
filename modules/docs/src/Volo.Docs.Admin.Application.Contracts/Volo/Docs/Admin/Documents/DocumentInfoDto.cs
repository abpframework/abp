using System;

namespace Volo.Docs.Admin.Documents;

public class DocumentInfoDto
{
    public string Version { get; set; }
    public string Format { get; set; }
    public string LanguageCode { get; set; }
    public Guid ProjectId { get; set; }
}