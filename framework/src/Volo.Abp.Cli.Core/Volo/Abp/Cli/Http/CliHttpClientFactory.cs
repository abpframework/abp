using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Http
{
    public class CliHttpClientFactory : ISingletonDependency, IDisposable
    {
        private static CliHttpClient _authenticatedHttpClient;
        private static CliHttpClient _unauthenticatedHttpClient;

        public CliHttpClient CreateClient(bool needsAuthentication = true)
        {
            if (needsAuthentication)
            {
                return CreateAuthenticatedHttpClient();
            }

            return CreateUnAuthenticatedHttpClient();
        }

        private static CliHttpClient CreateAuthenticatedHttpClient()
        {
            if (_authenticatedHttpClient == null)
            {
                _authenticatedHttpClient = new CliHttpClient(setBearerToken: true);
            }

            return _authenticatedHttpClient;
        }

        private static CliHttpClient CreateUnAuthenticatedHttpClient()
        {
            if (_unauthenticatedHttpClient == null)
            {
                _unauthenticatedHttpClient = new CliHttpClient(setBearerToken: false);
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