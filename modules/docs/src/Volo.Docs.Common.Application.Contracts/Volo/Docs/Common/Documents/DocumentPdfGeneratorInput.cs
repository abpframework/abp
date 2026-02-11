using System;

namespace Volo.Docs.Common.Documents;

public class DocumentPdfGeneratorInput
{
    public Guid ProjectId { get; set; }
    
    public string Version { get; set; }
    
    public string LanguageCode { get; set; }
}