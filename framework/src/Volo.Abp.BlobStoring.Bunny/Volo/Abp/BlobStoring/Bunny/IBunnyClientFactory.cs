using System.Threading.Tasks;
using BunnyCDN.Net.Storage;

namespace Volo.Abp.BlobStoring.Bunny;

public interface IBunnyClientFactory
{
    Task<BunnyCDNStorage> CreateAsync(string accessKey, string containerName, string region = "de");

    Task EnsureStorageZoneExistsAsync(string accessKey, string containerName, string region = "de", bool createIfNotExists = false);
}
