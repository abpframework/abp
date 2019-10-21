using System;
using System.Threading.Tasks;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.Licensing
{
    public class AbpIoApiKeyService : IApiKeyService, ITransientDependency
    {
        protected IJsonSerializer JsonSerializer { get; }

        protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }

        public AbpIoApiKeyService(IJsonSerializer jsonSerializer, IRemoteServiceExceptionHandler remoteServiceExceptionHandler)
        {
            JsonSerializer = jsonSerializer;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
        }

        public async Task<DeveloperApiKeyResult> GetApiKeyOrNullAsync()
        {
            using (var client = new CliHttpClient())
            {
                var url = $"{CliUrls.WwwAbpIo}api/license/api-key";

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"ERROR: Remote server returns '{response.StatusCode}'");
                }

                await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DeveloperApiKeyResult>(responseContent);
            }
        }
    }
}