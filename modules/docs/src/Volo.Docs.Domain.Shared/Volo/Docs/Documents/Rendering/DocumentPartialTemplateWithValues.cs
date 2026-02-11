using System.Collections.Generic;

namespace Volo.Docs.Documents.Rendering;

public class DocumentPartialTemplateWithValues
{
    public string Path { get; set; }

    public Dictionary<string, string> Parameters { get; set; }
}