using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public interface IObsClient
    {
        Task CreateContainer(string containerName, string containerLocation);

        Task PutObjectAsync(string containerName, string blobName, Stream blobStream);

        Task<Stream> GetObjectAsync(string containerName, string blobName);

        Task DeleteObjectAsync(string containerName, string blobName);

        Task DeleteContainerAsync(string containerName);

        Task<bool> DoesContainerExistAsync(string containerName);

        Task<bool> DoesBlobExistAsync(string containerName, string blobName);

        Task<List<string>> GetObjectNamesAsync(string containerName);
    }
}