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

namespace Volo.Abp.Cli.ProjectBuilding.Analyticses
{
    public class CliAnalyticsCollect : ICliAnalyticsCollect, ITransientDependency
    {
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger<CliAnalyticsCollect> _logger;

        public CliAnalyticsCollect(ICancellationTokenProvider cancellationTokenProvider,
            IJsonSerializer jsonSerializer)
        {
            _cancellationTokenProvider = cancellationTokenProvider;
            _jsonSerializer = jsonSerializer;
            _logger = NullLogger<CliAnalyticsCollect>.Instance;
        }

        public async Task CollectAsync(CliAnalyticsCollectInputDto input)
        {
            var postData = _jsonSerializer.Serialize(input);
            using (var client = new CliHttpClient())
            {
                var responseMessage = await client.PostAsync(
                    $"{CliUrls.WwwAbpIo}api/clianalytics/collect",
                    new StringContent(postData, Encoding.UTF8, MimeTypes.Application.Json),
                    _cancellationTokenProvider.Token
                );

                // TODO: Do not output logs, keep silent?
                if (!responseMessage.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Remote server returns error! HTTP status code: " +
                                           responseMessage.StatusCode);
                }
            }
        }
    }
}