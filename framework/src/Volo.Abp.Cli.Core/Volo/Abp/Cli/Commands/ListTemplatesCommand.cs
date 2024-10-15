using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands.Templates;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.Commands;

public class ListTemplatesCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "list-templates";

    private readonly CliHttpClientFactory _cliHttpClientFactory;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly IRemoteServiceExceptionHandler _remoteServiceExceptionHandler;

    public ILogger<ListTemplatesCommand> Logger { get; set; }

    public ListTemplatesCommand(
        CliHttpClientFactory cliHttpClientFactory,
        IJsonSerializer jsonSerializer,
        ICancellationTokenProvider cancellationTokenProvider,
        IRemoteServiceExceptionHandler remoteServiceExceptionHandler)
    {
        _cliHttpClientFactory = cliHttpClientFactory;
        _jsonSerializer = jsonSerializer;
        _cancellationTokenProvider = cancellationTokenProvider;
        _remoteServiceExceptionHandler = remoteServiceExceptionHandler;

        Logger = NullLogger<ListTemplatesCommand>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var templateNameSpaceLength = 26;
        var templateDisplayNameSpaceLength = 32;

        var templates = await GetTemplatesAsync();

        var output = new StringBuilder(Environment.NewLine + Environment.NewLine);

        output.AppendLine(
            $"{"Template".PadRight(templateNameSpaceLength)}" +
            $"{"Template Name".PadRight(templateDisplayNameSpaceLength)}" +
            "Document Url");

        output.AppendLine(
            $"{"--------".PadRight(templateNameSpaceLength)}" +
            $"{"-------------".PadRight(templateDisplayNameSpaceLength)}" +
            "------------");

        output.AppendLine();

        foreach (var template in templates)
        {
            output.AppendLine(
                $"{template.Name?.PadRight(templateNameSpaceLength)}" +
                $"{template.DisplayName?.PadRight(templateDisplayNameSpaceLength)}" +
                $"{template.DocumentUrl}");
        }

        Logger.LogInformation(output.ToString());
    }

    private async Task<List<TemplateInfo>> GetTemplatesAsync()
    {
        var client = _cliHttpClientFactory.CreateClient();

        using (var responseMessage = await client.GetAsync(
                   $"{CliUrls.WwwAbpIo}api/download/templates/",
                   _cancellationTokenProvider.Token
               ))
        {
            await _remoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(responseMessage);
            var result = await responseMessage.Content.ReadAsStringAsync();
            return _jsonSerializer.Deserialize<List<TemplateInfo>>(result);
        }
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp list-templates");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://abp.io/docs/latest/cli");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Lists available templates to be created.";
    }
}