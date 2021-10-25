using System;
using System.Net.Http;
using System.Threading;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.Http
{
    public class CliHttpClientFactory : ISingletonDependency
    {
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(2);

        private readonly IHttpClientFactory _clientFactory;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public CliHttpClientFactory(IHttpClientFactory clientFactory,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            _clientFactory = clientFactory;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public HttpClient CreateClient(bool needsAuthentication = true, TimeSpan? timeout = null)
        {
            var httpClient = _clientFactory.CreateClient(CliConsts.HttpClientName);
            httpClient.Timeout = timeout ?? DefaultTimeout;

            if (needsAuthentication)
            {
                httpClient.AddAbpAuthenticationToken();
            }

            return httpClient;
        }

        public CancellationToken GetCancellationToken(TimeSpan? timeout = null)
        {
            if (timeout == null)
            {
                if (_cancellationTokenProvider == null)
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    cancellationTokenSource.CancelAfter(DefaultTimeout);
                    return cancellationTokenSource.Token;
                }
                else
                {
                    return _cancellationTokenProvider.Token;
                }
            }
            else
            {
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(Convert.ToInt32(timeout.Value.TotalMilliseconds));
                return cancellationTokenSource.Token;
            }
        }
    }
}