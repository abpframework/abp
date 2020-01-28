using System.Collections.Generic;

namespace Volo.Docs.Documents
{
    public class DocumentTemplateDto
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public List<string> Parameters { get; set; }
    }
}