using System;
using System.Runtime.Serialization;
using Volo.Abp;

namespace Volo.Docs
{
    [Serializable]
    public class DocumentNotFoundException : BusinessException
    {
        public string DocumentUrl { get; set; }

        public DocumentNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public DocumentNotFoundException(string documentUrl)
        {
            DocumentUrl = documentUrl;
        }
    }
}
