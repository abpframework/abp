using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.Auth
{
    public class AuthService : ITransientDependency
    {
        protected IIdentityModelAuthenticationService AuthenticationService { get; }
        protected ILogger<NewCommand> Logger { get; }
        protected CliHttpClientFactory CliHttpClientFactory { get; }
        public ICancellationTokenProvider CancellationTokenProvider { get; }

        public AuthService(
            IIdentityModelAuthenticationService authenticationService,
            ILogger<NewCommand> logger,
            ICancellationTokenProvider cancellationTokenProvider,
            CliHttpClientFactory cliHttpClientFactory
        )
        {
            AuthenticationService = authenticationService;
            Logger = logger;
            CancellationTokenProvider = cancellationTokenProvider;
            CliHttpClientFactory = cliHttpClientFactory;
        }

        public async Task LoginAsync(string userName, string password, string organizationName = null)
        {
            var configuration = new IdentityClientConfiguration(
                CliUrls.AccountAbpIo,
                "role email abpio abpio_www abpio_commercial offline_access",
                "abp-cli",
                "1q2w3e*",
                OidcConstants.GrantTypes.Password,
                userName,
                password
            );

            if (!organizationName.IsNullOrWhiteSpace())
            {
                configuration["[o]abp-organization-name"] = organizationName;
            }

            var accessToken = await AuthenticationService.GetAccessTokenAsync(configuration);

            File.WriteAllText(CliPaths.AccessToken, accessToken, Encoding.UTF8);
        }

        public async Task LogoutAsync()
        {
            string accessToken = null;
            if (File.Exists(CliPaths.AccessToken))
            {
                accessToken = File.ReadAllText(CliPaths.AccessToken);
                File.Delete(CliPaths.AccessToken);
            }

            if (File.Exists(CliPaths.Lic))
            {
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    await LogoutAsync(accessToken);
                }

                File.Delete(CliPaths.Lic);
            }
        }

        private async Task LogoutAsync(string accessToken)
        {
            try
            {
                var client = CliHttpClientFactory.CreateClient();
                var content = new StringContent(
                    JsonSerializer.Serialize(new {token = accessToken}),
                    Encoding.UTF8, "application/json"
                );

                using (var response = await client.PostAsync(CliConsts.LogoutUrl, content, CancellationTokenProvider.Token))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Logger.LogWarning(
                            $"Cannot logout from remote service! Response: {response.StatusCode}-{response.ReasonPhrase}"
                        );
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Error occured while logging out from remote service. {e.Message}");
            }
        }

        public static bool IsLoggedIn()
        {
            return File.Exists(CliPaths.AccessToken);
        }
    }
}