using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Json;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class NugetPackageInfoProvider : INugetPackageInfoProvider, ITransientDependency
    {
        public IJsonSerializer JsonSerializer { get; }
        public ICancellationTokenProvider CancellationTokenProvider { get; }
        public IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }

        private readonly CliHttpClientFactory _cliHttpClientFactory;

        public NugetPackageInfoProvider(
            IJsonSerializer jsonSerializer,
            ICancellationTokenProvider cancellationTokenProvider,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            CliHttpClientFactory cliHttpClientFactory)
        {
            JsonSerializer = jsonSerializer;
            CancellationTokenProvider = cancellationTokenProvider;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            _cliHttpClientFactory = cliHttpClientFactory;
        }

        public async Task<NugetPackageInfo> GetAsync(string name)
        {
            var packageList = await GetPackageListInternalAsync();

            var package = packageList.FirstOrDefault(m => m.Name == name);

            if (package == null)
            {
                throw new Exception("Package is not found or downloadable!");
            }

            return package;
        }

        private async Task<List<NugetPackageInfo>> GetPackageListInternalAsync()
        {
            var client = _cliHttpClientFactory.CreateClient();

            using (var responseMessage = await client.GetAsync(
                $"{CliUrls.WwwAbpIo}api/download/nugetPackages/",
                CancellationTokenProvider.Token
            ))
            {
                await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(responseMessage);
                var result = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NugetPackageInfo>>(result);
            }
        }
    }
}
