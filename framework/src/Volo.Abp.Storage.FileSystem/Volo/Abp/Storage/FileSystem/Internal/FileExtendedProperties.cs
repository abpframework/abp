using System.Collections.Generic;

namespace Volo.Abp.Storage.FileSystem.Internal
{
    public class FileExtendedProperties
    {
        public FileExtendedProperties()
        {
            Metadata = new Dictionary<string, string>();
        }

        public string ContentType { get; set; }

        public string ETag { get; set; }

        public string CacheControl { get; set; }

        public string ContentMd5 { get; set; }

        public IDictionary<string, string> Metadata { get; set; }
    }
}
