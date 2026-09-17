using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Volo.Docs.Documents;

public class DocumentParams
{
    [JsonPropertyName("parameters")]
    public List<DocumentParameter> Parameters { get; set; } = new();
    
    
    public class DocumentParameter
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
    
        [JsonPropertyName("values")]
        public Dictionary<string, string> Values { get; set; } 
    }
}