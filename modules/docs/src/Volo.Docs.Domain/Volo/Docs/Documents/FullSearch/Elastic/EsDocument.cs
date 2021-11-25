using System;
using System.Collections.Generic;

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public class EsDocument
    {
        public EsDocument()
        {
            Highlight = new List<string>();
        }

        public string Id { get; set; }

        public string ProjectId { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Version { get; set; }

        public string LanguageCode { get; set; }

        public string Content { get; set; }

        public List<string> Highlight { get; set; }
    }
}
