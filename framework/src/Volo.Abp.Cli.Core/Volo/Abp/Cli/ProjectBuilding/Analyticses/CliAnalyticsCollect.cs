using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectBuilding.Analyticses;

public class CliAnalyticsCollect : ICliAnalyticsCollect, ITransientDependency
{
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ILogger<CliAnalyticsCollect> _logger;
    private readonly IRemoteServiceExceptionHandler _remoteServiceExceptionHandler;
    private readonly CliHttpClientFactory _cliHttpClientFactory;

    public CliAnalyticsCollect(
        ICancellationTokenProvider cancellationTokenProvider,
        IJsonSerializer jsonSerializer,
        IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
        CliHttpClientFactory cliHttpClientFactory)
    {
        _cancellationTokenProvider = cancellationTokenProvider;
        _jsonSerializer = jsonSerializer;
        _remoteServiceExceptionHandler = remoteServiceExceptionHandler;
        _cliHttpClientFactory = cliHttpClientFactory;
        _logger = NullLogger<CliAnalyticsCollect>.Instance;
    }

    public async Task CollectAsync(CliAnalyticsCollectInputDto input)
    {
        var postData = _jsonSerializer.Serialize(input);
        var url = $"{CliUrls.WwwAbpIo}api/clianalytics/collect";

        try
        {
            var client = _cliHttpClientFactory.CreateClient();

            var responseMessage = await client.PostAsync(
                url,
                new StringContent(postData, Encoding.UTF8, MimeTypes.Application.Json),
                _cancellationTokenProvider.Token
            );

            if (!responseMessage.IsSuccessStatusCode)
            {
                var exceptionMessage = "Remote server returns '" + (int)responseMessage.StatusCode + "-" + responseMessage.ReasonPhrase + "'. ";
                var remoteServiceErrorMessage = await _remoteServiceExceptionHandler.GetAbpRemoteServiceErrorAsync(responseMessage);

                if (remoteServiceErrorMessage != null)
                {
                    exceptionMessage += remoteServiceErrorMessage;
                }

                _logger.LogInformation(exceptionMessage);
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }
}
