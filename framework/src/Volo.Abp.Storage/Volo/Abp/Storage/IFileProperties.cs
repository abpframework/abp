using System;
using System.Collections.Generic;

namespace Volo.Abp.Storage
{
    public interface IFileProperties
    {
        DateTimeOffset? LastModified { get; }

        long Length { get; }

        string ContentType { get; set; }

        string ETag { get; }

        string CacheControl { get; set; }

        string ContentMd5 { get; }

        IDictionary<string, string> Metadata { get; }
    }
}
