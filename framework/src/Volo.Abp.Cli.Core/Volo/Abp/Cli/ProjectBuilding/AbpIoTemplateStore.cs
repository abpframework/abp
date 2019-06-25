using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.IO;
using Volo.Abp.Json;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class AbpIoTemplateStore : ITemplateStore, ITransientDependency
    {
        public ILogger<AbpIoTemplateStore> Logger { get; set; }

        protected CliOptions Options { get; }

        protected IJsonSerializer JsonSerializer { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public AbpIoTemplateStore(
            IOptions<CliOptions> options,
            IJsonSerializer jsonSerializer,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            JsonSerializer = jsonSerializer;
            CancellationTokenProvider = cancellationTokenProvider;
            Options = options.Value;

            Logger = NullLogger<AbpIoTemplateStore>.Instance;
        }

        public async Task<TemplateFile> GetAsync(
            string name,
            DatabaseProvider databaseProvider,
            string projectName,
            string version = null)
        {
            if (version == null)
            {
                version = await GetLatestTemplateVersionAsync(name);
            }

            DirectoryHelper.CreateIfNotExists(CliPaths.TemplateCache);

            var localCacheFile = Path.Combine(CliPaths.TemplateCache, name + "-" + version + ".zip");
            if (Options.CacheTemplates && File.Exists(localCacheFile))
            {
                Logger.LogInformation("Using cached template: " + name + ", version: " + version);
                return new TemplateFile(File.ReadAllBytes(localCacheFile), version);
            }

            Logger.LogInformation("Downloading template: " + name + ", version: " + version);

            var fileContent = await DownloadTemplateFileContentAsync(
                new TemplateDownloadInputDto
                {
                    Name = name,
                    Version = version,
                    DatabaseProvider = databaseProvider.ToString(),
                    ProjectName = projectName,
                    Tool = Options.ToolName
                }
            );

            if (Options.CacheTemplates)
            {
                File.WriteAllBytes(localCacheFile, fileContent);
            }

            return new TemplateFile(fileContent, version);
        }

        private async Task<string> GetLatestTemplateVersionAsync(string name)
        {
            var postData = JsonSerializer.Serialize(new GetLatestTemplateVersionDto { Name = name });

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);

                AddAuthentication(client);

                var responseMessage = await client.PostAsync(
                    $"{CliUrls.WwwAbpIo}api/download/template/get-version/",
                    new StringContent(postData, Encoding.UTF8, MimeTypes.Application.Json),
                    CancellationTokenProvider.Token
                );

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception("Remote server returns error! HTTP status code: " + responseMessage.StatusCode);
                }

                var result = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GetLatestTemplateVersionResultDto>(result).Version;
            }
        }

        private async Task<byte[]> DownloadTemplateFileContentAsync(TemplateDownloadInputDto input)
        {
            var postData = JsonSerializer.Serialize(input);

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(3);

                AddAuthentication(client);

                var responseMessage = await client.PostAsync(
                    $"{CliUrls.WwwAbpIo}api/download/template/",
                    new StringContent(postData, Encoding.UTF8, MimeTypes.Application.Json),
                    CancellationTokenProvider.Token
                );

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception("Remote server returns error! HTTP status code: " + responseMessage.StatusCode);
                }

                return await responseMessage.Content.ReadAsByteArrayAsync();
            }
        }

        private static void AddAuthentication(HttpClient client)
        {
            if (File.Exists(CliPaths.AccessToken))
            {
                var accessToken = File.ReadAllText(CliPaths.AccessToken, Encoding.UTF8);
                if (!accessToken.IsNullOrEmpty())
                {
                    client.SetBearerToken(accessToken);
                }
            }
        }

        public class TemplateDownloadInputDto
        {
            public string Name { get; set; }

            public string Version { get; set; }

            public string DatabaseProvider { get; set; }

            public string ProjectName { get; set; }

            public string Tool { get; set; }
        }

        public class GetLatestTemplateVersionDto
        {
            public string Name { get; set; }
        }

        public class GetLatestTemplateVersionResultDto
        {
            public string Version { get; set; }
        }
    }
}