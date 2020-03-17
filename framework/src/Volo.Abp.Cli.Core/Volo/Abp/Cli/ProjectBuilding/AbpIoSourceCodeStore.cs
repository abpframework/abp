using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.IO;
using Volo.Abp.Json;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class AbpIoSourceCodeStore : ISourceCodeStore, ITransientDependency
    {
        public ILogger<AbpIoSourceCodeStore> Logger { get; set; }

        protected AbpCliOptions Options { get; }

        protected IJsonSerializer JsonSerializer { get; }

        protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public AbpIoSourceCodeStore(
            IOptions<AbpCliOptions> options,
            IJsonSerializer jsonSerializer,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            JsonSerializer = jsonSerializer;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            CancellationTokenProvider = cancellationTokenProvider;
            Options = options.Value;

            Logger = NullLogger<AbpIoSourceCodeStore>.Instance;
        }

        public async Task<TemplateFile> GetAsync(
            string name,
            string type,
            string version = null,
            string templateSource = null)
        {

            var latestVersion = await GetLatestSourceCodeVersionAsync(name, type);
            if (version == null)
            {
                version = latestVersion;
            }

            var nugetVersion = (await GetTemplateNugetVersionAsync(name, type, version)) ?? version;

            DirectoryHelper.CreateIfNotExists(CliPaths.TemplateCache);

            if (!string.IsNullOrWhiteSpace(templateSource) && !IsNetworkSource(templateSource))
            {
                Logger.LogInformation("Using local " + type + ": " + name + ", version: " + version);
                return new TemplateFile(File.ReadAllBytes(Path.Combine(templateSource, name + "-" + version + ".zip")), version, latestVersion, nugetVersion);
            }

            var localCacheFile = Path.Combine(CliPaths.TemplateCache, name + "-" + version + ".zip");
            if (Options.CacheTemplates && File.Exists(localCacheFile) && templateSource.IsNullOrWhiteSpace())
            {
                Logger.LogInformation("Using cached " + type + ": " + name + ", version: " + version);
                return new TemplateFile(File.ReadAllBytes(localCacheFile), version, latestVersion, nugetVersion);
            }

            Logger.LogInformation("Downloading " + type + ": " + name + ", version: " + version);

            var fileContent = await DownloadSourceCodeContentAsync(
                new SourceCodeDownloadInputDto
                {
                    Name = name,
                    Type = type,
                    TemplateSource = templateSource,
                    Version = version
                }
            );

            if (Options.CacheTemplates && templateSource.IsNullOrWhiteSpace())
            {
                File.WriteAllBytes(localCacheFile, fileContent);
            }

            return new TemplateFile(fileContent, version, latestVersion, nugetVersion);

        }

        private async Task<string> GetLatestSourceCodeVersionAsync(string name, string type)
        {
            using (var client = new CliHttpClient())
            {
                var response = await client.PostAsync(
                    $"{CliUrls.WwwAbpIo}api/download/{type}/get-version/",
                    new StringContent(
                        JsonSerializer.Serialize(
                            new GetLatestSourceCodeVersionDto { Name = name }
                        ),
                        Encoding.UTF8,
                        MimeTypes.Application.Json
                    ),
                    CancellationTokenProvider.Token
                );

                await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);

                var result = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<GetVersionResultDto>(result).Version;
            }
        }

        private async Task<string> GetTemplateNugetVersionAsync(string name, string type, string version)
        {
            try
            {
                using (var client = new CliHttpClient(TimeSpan.FromMinutes(10)))
                {
                    var response = await client.PostAsync(
                        $"{CliUrls.WwwAbpIo}api/download/{type}/get-nuget-version/",
                        new StringContent(
                            JsonSerializer.Serialize(
                                new GetTemplateNugetVersionDto { Name = name, Version = version }
                            ),
                            Encoding.UTF8,
                            MimeTypes.Application.Json
                        ),
                        CancellationTokenProvider.Token
                    );

                    await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);

                    var result = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<GetVersionResultDto>(result).Version;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<byte[]> DownloadSourceCodeContentAsync(SourceCodeDownloadInputDto input)
        {
            var postData = JsonSerializer.Serialize(input);

            using (var client = new CliHttpClient(TimeSpan.FromMinutes(10)))
            {
                HttpResponseMessage responseMessage;

                if (input.TemplateSource.IsNullOrWhiteSpace())
                {
                    responseMessage = await client.PostAsync(
                        $"{CliUrls.WwwAbpIo}api/download/{input.Type}/",
                        new StringContent(postData, Encoding.UTF8, MimeTypes.Application.Json),
                        CancellationTokenProvider.Token
                    );
                }
                else
                {
                    responseMessage = await client.GetAsync(input.TemplateSource, CancellationTokenProvider.Token);
                }

                await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(responseMessage);

                return await responseMessage.Content.ReadAsByteArrayAsync();
            }
        }

        private bool IsNetworkSource(string source)
        {
            return source.ToLower().StartsWith("http");
        }

        public class SourceCodeDownloadInputDto
        {
            public string Name { get; set; }

            public string Version { get; set; }

            public string Type { get; set; }

            public string TemplateSource { get; set; }
        }

        public class GetLatestSourceCodeVersionDto
        {
            public string Name { get; set; }
        }

        public class GetTemplateNugetVersionDto
        {
            public string Name { get; set; }

            public string Version { get; set; }
        }

        public class GetVersionResultDto
        {
            public string Version { get; set; }
        }
    }
}