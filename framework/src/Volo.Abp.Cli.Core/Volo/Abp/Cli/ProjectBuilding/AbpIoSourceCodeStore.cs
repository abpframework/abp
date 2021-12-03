using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Console;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;
using Volo.Abp.Cli.ProjectBuilding.Templates.Wpf;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.IO;
using Volo.Abp.Json;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectBuilding;

public class AbpIoSourceCodeStore : ISourceCodeStore, ITransientDependency
{
    public ILogger<AbpIoSourceCodeStore> Logger { get; set; }

    protected AbpCliOptions Options { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    private readonly CliHttpClientFactory _cliHttpClientFactory;

    public AbpIoSourceCodeStore(
        IOptions<AbpCliOptions> options,
        IJsonSerializer jsonSerializer,
        IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
        ICancellationTokenProvider cancellationTokenProvider,
        CliHttpClientFactory cliHttpClientFactory)
    {
        JsonSerializer = jsonSerializer;
        RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
        CancellationTokenProvider = cancellationTokenProvider;
        _cliHttpClientFactory = cliHttpClientFactory;
        Options = options.Value;

        Logger = NullLogger<AbpIoSourceCodeStore>.Instance;
    }

    public async Task<TemplateFile> GetAsync(
        string name,
        string type,
        string version = null,
        string templateSource = null,
        bool includePreReleases = false)
    {
        DirectoryHelper.CreateIfNotExists(CliPaths.TemplateCache);
        var latestVersion = version ?? await GetLatestSourceCodeVersionAsync(name, type, null, includePreReleases);

        if (version == null)
        {
            if (latestVersion == null)
            {
                Logger.LogWarning("The remote service is currently unavailable, please specify the version.");
                Logger.LogWarning(string.Empty);
                Logger.LogWarning("Find the following template in your cache directory: ");
                Logger.LogWarning("\tTemplate Name\tVersion");

                var templateList = GetLocalTemplates();
                foreach (var cacheFile in templateList)
                {
                    Logger.LogWarning($"\t{cacheFile.TemplateName}\t\t{cacheFile.Version}");
                }

                Logger.LogWarning(string.Empty);
                throw new CliUsageException("Use command: abp new Acme.BookStore -v version");
            }

            version = latestVersion;
        }

        if (await GetTemplateNugetVersionAsync(name, type, version) == null)
        {
            throw new Exception("There is no version found with given version: " + version);
        }

        if (!string.IsNullOrWhiteSpace(templateSource) && !IsNetworkSource(templateSource))
        {
            Logger.LogInformation("Using local " + type + ": " + name + ", version: " + version);
            return new TemplateFile(File.ReadAllBytes(Path.Combine(templateSource, name + "-" + version + ".zip")),
                version, latestVersion, version);
        }

        var localCacheFile = Path.Combine(CliPaths.TemplateCache, name.Replace("/", ".") + "-" + version + ".zip");

#if DEBUG
        if (File.Exists(localCacheFile))
        {
            return new TemplateFile(File.ReadAllBytes(localCacheFile), version, latestVersion, version);
        }
#endif

        if (Options.CacheTemplates && File.Exists(localCacheFile) && templateSource.IsNullOrWhiteSpace())
        {
            Logger.LogInformation("Using cached " + type + ": " + name + ", version: " + version);
            return new TemplateFile(File.ReadAllBytes(localCacheFile), version, latestVersion, version);
        }

        Logger.LogInformation("Downloading " + type + ": " + name + ", version: " + version);

        var fileContent = await DownloadSourceCodeContentAsync(
            new SourceCodeDownloadInputDto
            {
                Name = name,
                Type = type,
                TemplateSource = templateSource,
                Version = version,
                IncludePreReleases = includePreReleases
            }
        );

        if (Options.CacheTemplates && templateSource.IsNullOrWhiteSpace())
        {
            File.WriteAllBytes(localCacheFile, fileContent);
        }

        return new TemplateFile(fileContent, version, latestVersion, version);
    }

    private async Task<string> GetLatestSourceCodeVersionAsync(string name, string type, string url = null,
        bool includePreReleases = false)
    {
        if (url == null)
        {
            url = $"{CliUrls.WwwAbpIo}api/download/{type}/get-version/";
        }

        try
        {
            var client = _cliHttpClientFactory.CreateClient();
            var stringContent = new StringContent(
                JsonSerializer.Serialize(new GetLatestSourceCodeVersionDto
                    {Name = name, IncludePreReleases = includePreReleases}),
                Encoding.UTF8,
                MimeTypes.Application.Json
            );

            using (var response = await client.PostAsync(url, stringContent,
                _cliHttpClientFactory.GetCancellationToken(TimeSpan.FromMinutes(10))))
            {
                await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GetVersionResultDto>(result).Version;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured while getting the latest version from {0} : {1}", url, ex.Message);
            return null;
        }
    }

    private async Task<string> GetTemplateNugetVersionAsync(string name, string type, string version)
    {
        if (type != SourceCodeTypes.Template)
        {
            return null;
        }

        try
        {
            var url = $"{CliUrls.WwwAbpIo}api/download/{type}/get-nuget-version/";
            var client = _cliHttpClientFactory.CreateClient();

            var stringContent = new StringContent(
                JsonSerializer.Serialize(new GetTemplateNugetVersionDto {Name = name, Version = version}),
                Encoding.UTF8,
                MimeTypes.Application.Json
            );

            using (var response = await client.PostAsync(url, stringContent,
                _cliHttpClientFactory.GetCancellationToken(TimeSpan.FromMinutes(10))))
            {
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
        var url = $"{CliUrls.WwwAbpIo}api/download/{input.Type}/";

        HttpResponseMessage responseMessage = null;

        try
        {
            var client = _cliHttpClientFactory.CreateClient(timeout: TimeSpan.FromMinutes(5));

            if (input.TemplateSource.IsNullOrWhiteSpace())
            {
                responseMessage = await client.PostAsync(
                    url,
                    new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, MimeTypes.Application.Json),
                    _cliHttpClientFactory.GetCancellationToken(TimeSpan.FromMinutes(10))
                );
            }
            else
            {
                responseMessage = await client.GetAsync(input.TemplateSource,
                    _cliHttpClientFactory.GetCancellationToken());
            }

            await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(responseMessage);
            var resultAsBytes = await responseMessage.Content.ReadAsByteArrayAsync();
            responseMessage.Dispose();

            return resultAsBytes;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured while downloading source-code from {0} : {1}{2}{3}", url,
                responseMessage?.ToString(), Environment.NewLine, ex.Message);
            throw;
        }
    }

    private static bool IsNetworkSource(string source)
    {
        return source.ToLower().StartsWith("http");
    }

    private List<(string TemplateName, string Version)> GetLocalTemplates()
    {
        var templateList = new List<(string TemplateName, string Version)>();

        var stringBuilder = new StringBuilder();
        foreach (var cacheFile in Directory.GetFiles(CliPaths.TemplateCache))
        {
            stringBuilder.AppendLine(cacheFile);
        }

        var matches = Regex.Matches(stringBuilder.ToString(),
            $"({AppTemplate.TemplateName}|{AppProTemplate.TemplateName}|{ModuleTemplate.TemplateName}|{ModuleProTemplate.TemplateName}|{ConsoleTemplate.TemplateName}|{WpfTemplate.TemplateName})-(.+).zip");
        foreach (Match match in matches)
        {
            templateList.Add((match.Groups[1].Value, match.Groups[2].Value));
        }

        return templateList;
    }

    public class SourceCodeDownloadInputDto
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Type { get; set; }

        public string TemplateSource { get; set; }

        public bool IncludePreReleases { get; set; }
    }

    public class GetLatestSourceCodeVersionDto
    {
        public string Name { get; set; }

        public bool IncludePreReleases { get; set; }
    }

    public class GetTemplateNugetVersionDto
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public bool IncludePreReleases { get; set; }
    }

    public class GetVersionResultDto
    {
        public string Version { get; set; }
    }
}
