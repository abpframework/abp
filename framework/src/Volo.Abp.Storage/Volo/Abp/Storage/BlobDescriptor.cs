using System;
using System.Collections.Generic;

namespace Volo.Abp.Storage
{
    public class BlobDescriptor
    {
        public string ContentType { get; set; }

        public string ContentMD5 { get; set; }

        public string ContentDisposition { get; set; }

        public string ETag { get; set; }

        public long Length { get; set; }

        public DateTimeOffset? LastModified { get; set; }

        public BlobSecurity Security { get; set; }

        public string Name { get; set; }

        public string Container { get; set; }

        public string Url { get; set; }

        public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}