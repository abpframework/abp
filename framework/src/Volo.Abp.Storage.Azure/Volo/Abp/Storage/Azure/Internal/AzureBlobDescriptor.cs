using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Volo.Abp.Storage.Azure.Internal
{
    public class AzureBlobDescriptor : IBlobDescriptor
    {
        private const string DefaultCacheControl = "max-age=300, must-revalidate";
        private readonly ICloudBlob _cloudBlob;
        private readonly Dictionary<string, string> _decodedMetadata;

        public AzureBlobDescriptor(ICloudBlob cloudBlob)
        {
            _cloudBlob = cloudBlob;
            if (string.IsNullOrEmpty(_cloudBlob.Properties.CacheControl))
                _cloudBlob.Properties.CacheControl = DefaultCacheControl;

            _decodedMetadata = _cloudBlob.Metadata != null
                ? _cloudBlob.Metadata.ToDictionary(m => m.Key, m => WebUtility.HtmlDecode(m.Value))
                : new Dictionary<string, string>();
        }

        public string CacheControl
        {
            get => _cloudBlob.Properties.CacheControl;
            set => _cloudBlob.Properties.CacheControl = value;
        }

        public DateTimeOffset? LastModified => _cloudBlob.Properties.LastModified;

        public long Length => _cloudBlob.Properties.Length;

        public string ContentType
        {
            get => _cloudBlob.Properties.ContentType;
            set => _cloudBlob.Properties.ContentType = value;
        }

        public string ETag => _cloudBlob.Properties.ETag;

        public string ContentMd5 => _cloudBlob.Properties.ContentMD5;

        public IDictionary<string, string> Metadata => _decodedMetadata;

        internal async Task SaveAsync()
        {
            await _cloudBlob.SetPropertiesAsync();

            foreach (var meta in _decodedMetadata) _cloudBlob.Metadata[meta.Key] = WebUtility.HtmlEncode(meta.Value);

            await _cloudBlob.SetMetadataAsync();
        }
    }
}