using System;
using System.Collections.Generic;

namespace Volo.Abp.Storage
{
    public interface IBlobDescriptor
    {
        string ContentType { get; set; }

        string ContentMd5 { get; }

        string ETag { get; }

        long Length { get; }

        DateTimeOffset? LastModified { get; }

        IDictionary<string, string> Metadata { get; }
    }
}