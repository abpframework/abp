using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Volo.Abp.Storage.Azure.Internal
{
    public class AzureBlobReference : IBlobReference
    {
        private Lazy<AzureBlobDescriptor> _descriptorLazy;
        private bool _withMetadata;

        public AzureBlobReference(string path, ICloudBlob cloudBlob, bool withMetadata)
        {
            Path = path;
            CloudBlob = cloudBlob;
            _withMetadata = withMetadata;
            _descriptorLazy = new Lazy<AzureBlobDescriptor>(() =>
            {
                if (withMetadata && cloudBlob.Metadata != null && cloudBlob.Properties != null)
                    return new AzureBlobDescriptor(cloudBlob);

                throw new InvalidOperationException("Metadata are not loaded, please use withMetadata option");
            });
        }

        public AzureBlobReference(ICloudBlob cloudBlob, bool withMetadata) :
            this(cloudBlob.Name, cloudBlob, withMetadata)
        {
        }

        public ICloudBlob CloudBlob { get; }

        public string Path { get; }

        public IBlobDescriptor BlobDescriptor => _descriptorLazy.Value;

        public string GetPublicBlobUrl => CloudBlob.Uri.ToString();

        public Task DeleteBlobAsync()
        {
            return CloudBlob.DeleteAsync();
        }

        public async ValueTask<Stream> ReadBlobAsync()
        {
            return await ReadInMemoryAsync();
        }

        public Task UpdateBlobAsync(Stream stream)
        {
            return CloudBlob.UploadFromStreamAsync(stream);
        }

        public async Task ReadBlobToStreamAsync(Stream targetStream)
        {
            await CloudBlob.DownloadRangeToStreamAsync(targetStream, null, null);
        }

        public async ValueTask<string> ReadBlobTextAsync()
        {
            using (var reader = new StreamReader(await CloudBlob.OpenReadAsync(AccessCondition.GenerateEmptyCondition(),
                new BlobRequestOptions(), new OperationContext())))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public async ValueTask<byte[]> ReadBlobBytesAsync()
        {
            return (await ReadInMemoryAsync()).ToArray();
        }

        public Task SaveBlobDescriptorAsync()
        {
            return _descriptorLazy.Value.SaveAsync();
        }

        public ValueTask<string> GetBlobSasUrl(ISharedAccessPolicy policy)
        {
            var adHocPolicy = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = policy.StartTime,
                SharedAccessExpiryTime = policy.ExpiryTime,
                Permissions = AbpAzureStore.FromGenericToAzure(policy.Permissions)
            };

            return new ValueTask<string>(CloudBlob.GetSharedAccessSignature(adHocPolicy));
        }

        public async Task FetchBlobProperties()
        {
            if (_withMetadata) return;

            await CloudBlob.FetchAttributesAsync();

            _descriptorLazy = new Lazy<AzureBlobDescriptor>(() => new AzureBlobDescriptor(CloudBlob));
            _withMetadata = true;
        }

        public async ValueTask<MemoryStream> ReadInMemoryAsync()
        {
            var memoryStream = new MemoryStream();
            await CloudBlob.DownloadRangeToStreamAsync(memoryStream, null, null);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}