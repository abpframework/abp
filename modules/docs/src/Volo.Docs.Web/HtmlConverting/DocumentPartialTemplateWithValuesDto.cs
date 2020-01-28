using System.Collections.Generic;

namespace Volo.Docs.HtmlConverting
{
    public class DocumentPartialTemplateWithValuesDto
    {
        public string Name { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}