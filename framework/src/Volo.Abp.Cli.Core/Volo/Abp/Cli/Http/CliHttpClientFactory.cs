using System;
using System.Threading;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.Http
{
    public class CliHttpClientFactory : ISingletonDependency, IDisposable
    {
        private static CliHttpClient _authenticatedHttpClient;
        private static CliHttpClient _unauthenticatedHttpClient;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public CliHttpClientFactory(ICancellationTokenProvider cancellationTokenProvider)
        {
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public CliHttpClient CreateClient(bool needsAuthentication = true, TimeSpan? timeout = null)
        {
            if (needsAuthentication)
            {
                return CreateAuthenticatedHttpClient(timeout);
            }

            return CreateUnAuthenticatedHttpClient(timeout);
        }

        public CancellationToken GetCancellationToken(TimeSpan? timeout = null)
        {
            if (timeout == null)
            {
                if (_cancellationTokenProvider == null)
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    cancellationTokenSource.CancelAfter(CliHttpClient.DefaultTimeout);
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

        private static CliHttpClient CreateAuthenticatedHttpClient(TimeSpan? timeout = null)
        {
            if (_authenticatedHttpClient == null)
            {
                _authenticatedHttpClient = new CliHttpClient(setBearerToken: true)
                {
                    Timeout = System.Threading.Timeout.InfiniteTimeSpan
                };
            }

            return _authenticatedHttpClient;
        }

        private static CliHttpClient CreateUnAuthenticatedHttpClient(TimeSpan? timeout = null)
        {
            if (_unauthenticatedHttpClient == null)
            {
                _unauthenticatedHttpClient = new CliHttpClient(setBearerToken: false)
                {
                    Timeout = System.Threading.Timeout.InfiniteTimeSpan
                };
            }

            return _unauthenticatedHttpClient;
        }

        public void Dispose()
        {
            _authenticatedHttpClient?.Dispose();
            _unauthenticatedHttpClient?.Dispose();
        }
    }
}