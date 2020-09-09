using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Docs.Documents
{
    public class DocumentSearchOutput
    {
        public DocumentSearchOutput()
        {
            Highlight = new List<string>();
        }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Version { get; set; }

        public string LanguageCode { get; set; }

        public List<string> Highlight { get; set; }
    }
}
