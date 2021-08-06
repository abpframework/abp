using System;

namespace Volo.Docs.Documents
{
    public class DocumentWithoutDetails
    {
        public Guid Id { get; set; }

        public virtual string Version { get; set; }

        public virtual string LanguageCode { get; set; }
        
        public virtual string Format { get; set; }
    }
}