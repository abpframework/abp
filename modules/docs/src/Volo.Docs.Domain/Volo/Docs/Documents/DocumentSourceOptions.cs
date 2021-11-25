using System;
using System.Collections.Generic;

namespace Volo.Docs.Documents
{
    public class DocumentSourceOptions
    {
        public Dictionary<string, Type> Sources { get; set; }

        public DocumentSourceOptions()
        {
            Sources = new Dictionary<string, Type>();
        }
    }
}
