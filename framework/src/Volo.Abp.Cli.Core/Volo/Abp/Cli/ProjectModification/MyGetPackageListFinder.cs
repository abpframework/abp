using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Volo.Abp.Cli.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectModification;

public class MyGetPackageListFinder : ISingletonDependency
{
    public ILogger<MyGetPackageListFinder> Logger { get; set; }

    private MyGetApiResponse _response;
    private readonly CliHttpClientFactory _cliHttpClientFactory;
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public MyGetPackageListFinder(CliHttpClientFactory cliHttpClientFactory,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        _cliHttpClientFactory = cliHttpClientFactory;
        CancellationTokenProvider = cancellationTokenProvider;
        Logger = NullLogger<MyGetPackageListFinder>.Instance;
    }

    public async Task<MyGetApiResponse> GetPackagesAsync()
    {
        if (_response != null)
        {
            return _response;
        }

        try
        {
            var client = _cliHttpClientFactory.CreateClient();

            using (var responseMessage = await client.GetAsync(
                $"{CliUrls.WwwAbpIo}api/myget/packages/",
                _cliHttpClientFactory.GetCancellationToken(TimeSpan.FromMinutes(10))))
            {
                _response = JsonConvert.DeserializeObject<MyGetApiResponse>(
                    Encoding.Default.GetString(await responseMessage.Content.ReadAsByteArrayAsync())
                );
            }
        }
        catch (Exception ex)
        {
            Logger.LogError("Unable to get latest preview version. Error: " + ex.Message);
            throw;
        }

        return _response;
    }

}
