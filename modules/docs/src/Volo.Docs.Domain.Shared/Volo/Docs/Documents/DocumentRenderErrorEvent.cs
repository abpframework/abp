using System.Collections.Generic;

namespace Volo.Docs.Documents;

public class DocumentRenderErrorEvent
{
    public string Name { get; set; }
    
    public string ErrorMessage { get; set; }

    public string Url { get; set; }

    public Dictionary<string, string> UserPreferences { get; set; }
}