using System;
using System.Collections.Generic;

namespace Volo.Docs.Documents
{
    public class DocumentStoreOptions
    {
        public Dictionary<string, Type> Stores { get; set; }

        public DocumentStoreOptions()
        {
            Stores = new Dictionary<string, Type>();
        }
    }
}
