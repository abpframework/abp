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
            string version,
            DatabaseProvider databaseProvider,
            string projectName)
        {
            var localCacheFolder = Path.Combine(CliPaths.TemplateCache, version);
            DirectoryHelper.CreateIfNotExists(localCacheFolder);

            var localCacheFile = Path.Combine(localCacheFolder, name + ".zip");
            if (File.Exists(localCacheFile))
            {
                Logger.LogInformation("Using cached template: " + name + ", version: " + version);
                return new TemplateFile(File.ReadAllBytes(localCacheFile));
            }

            Logger.LogInformation("Downloading template: " + name + ", version: " + version);

            using (var client = new System.Net.Http.HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(3);

                if (File.Exists(CliPaths.AccessToken))
                {
                    var accessToken = File.ReadAllText(CliPaths.AccessToken, Encoding.UTF8);
                    if (!accessToken.IsNullOrEmpty())
                    {
                        client.SetBearerToken(accessToken);
                    }
                }

                var serializedPostDataAsString = JsonSerializer.Serialize(new
                {
                    name = name,
                    version = version,
                    databaseProvider = databaseProvider,
                    projectName = projectName
                });

                var downloadUrl = Options.AbpIoWwwUrlRoot + "api/download/template/";
                var responseMessage = await client.PostAsync(
                    downloadUrl,
                    new StringContent(serializedPostDataAsString, Encoding.UTF8, MimeTypes.Application.Json),
                    CancellationTokenProvider.Token
                );

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception("Remote server returns error! HTTP status code: " + responseMessage.StatusCode);
                }

                var fileContent = await responseMessage.Content.ReadAsByteArrayAsync();

                File.WriteAllBytes(localCacheFile, fileContent);
                return new TemplateFile(fileContent);
            }
        }
    }
}