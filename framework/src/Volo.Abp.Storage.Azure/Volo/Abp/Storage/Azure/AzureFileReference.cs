using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Volo.Abp.Storage.Azure
{
    public class AzureFileReference : IFileReference
    {
        private Lazy<AzureFileProperties> _propertiesLazy;
        private bool _withMetadata;

        public AzureFileReference(string path, ICloudBlob cloudBlob, bool withMetadata)
        {
            Path = path;
            CloudBlob = cloudBlob;
            _withMetadata = withMetadata;
            _propertiesLazy = new Lazy<AzureFileProperties>(() =>
            {
                if (withMetadata && cloudBlob.Metadata != null && cloudBlob.Properties != null)
                {
                    return new AzureFileProperties(cloudBlob);
                }

                throw new InvalidOperationException("Metadata are not loaded, please use withMetadata option");
            });
        }

        public AzureFileReference(ICloudBlob cloudBlob, bool withMetadata) :
            this(cloudBlob.Name, cloudBlob, withMetadata)
        {
        }

        public string Path { get; }

        public IFileProperties Properties => _propertiesLazy.Value;

        public string PublicUrl => CloudBlob.Uri.ToString();

        public ICloudBlob CloudBlob { get; }

        public Task DeleteAsync()
        {
            return CloudBlob.DeleteAsync();
        }

        public async ValueTask<Stream> ReadAsync()
        {
            return await ReadInMemoryAsync();
        }

        public async ValueTask<MemoryStream> ReadInMemoryAsync()
        {
            var memoryStream = new MemoryStream();
            await CloudBlob.DownloadRangeToStreamAsync(memoryStream, null, null);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public Task UpdateAsync(Stream stream)
        {
            return CloudBlob.UploadFromStreamAsync(stream);
        }

        public async Task ReadToStreamAsync(Stream targetStream)
        {
            await CloudBlob.DownloadRangeToStreamAsync(targetStream, null, null);
        }

        public async ValueTask<string> ReadAllTextAsync()
        {
            using (var reader = new StreamReader(await CloudBlob.OpenReadAsync(AccessCondition.GenerateEmptyCondition(), new BlobRequestOptions(), new OperationContext())))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public async ValueTask<byte[]> ReadAllBytesAsync()
        {
            return (await ReadInMemoryAsync()).ToArray();
        }

        public Task SavePropertiesAsync()
        {
            return _propertiesLazy.Value.SaveAsync();
        }

        public ValueTask<string> GetSharedAccessSignature(ISharedAccessPolicy policy)
        {
            var adHocPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessStartTime = policy.StartTime,
                SharedAccessExpiryTime = policy.ExpiryTime,
                Permissions = AbpAzureStore.FromGenericToAzure(policy.Permissions),
            };

            return new ValueTask<string>(CloudBlob.GetSharedAccessSignature(adHocPolicy));
        }

        public async Task FetchProperties()
        {
            if (_withMetadata)
            {
                return;
            }

            await CloudBlob.FetchAttributesAsync();

            _propertiesLazy = new Lazy<AzureFileProperties>(() => new AzureFileProperties(CloudBlob));
            _withMetadata = true;
        }
    }
}
