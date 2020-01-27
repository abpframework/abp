using System.Collections.Generic;

namespace Volo.Docs.HtmlConverting
{
    public class PartialTemplateDto
    {
        public string Name { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}