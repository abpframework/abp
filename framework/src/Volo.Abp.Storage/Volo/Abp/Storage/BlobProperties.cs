using System.Collections.Generic;

namespace Volo.Abp.Storage
{
    public class BlobProperties
    {
        public static readonly BlobProperties Empty = new BlobProperties
        {
            Security = BlobSecurity.Private,
        };

        public BlobSecurity Security { get; set; }

        public string ContentType { get; set; }

        public string ContentDisposition { get; set; }

        public BlobProperties WithContentDispositionFilename(string filename)
        {
            ContentDisposition = $"attachment; filename=\"{filename}\"";
            return this;
        }

        public IDictionary<string, string> Metadata { get; set; }
    }
}