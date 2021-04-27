using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;
using Volo.Abp.IO;

namespace Volo.Abp.Cli.Auth
{
    public class AuthService : ITransientDependency
    {
        protected IIdentityModelAuthenticationService AuthenticationService { get; }
        protected ILogger<NewCommand> Logger { get; }
        protected CliHttpClientFactory CliHttpClientFactory { get; }
        
        public AuthService(
            IIdentityModelAuthenticationService authenticationService, 
            ILogger<NewCommand> logger,
            CliHttpClientFactory cliHttpClientFactory
        )
        {
            AuthenticationService = authenticationService;
            Logger = logger;
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
            string accessToken = "";
            if (File.Exists(CliPaths.AccessToken))
            {
                accessToken = File.ReadAllText(CliPaths.AccessToken);
                FileHelper.DeleteIfExists(CliPaths.AccessToken);
            }
            
            if (File.Exists(CliPaths.Lic))
            {
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    await LogoutAsync(accessToken);
                }
                
                FileHelper.DeleteIfExists(CliPaths.Lic);
            }
        }

        private async Task LogoutAsync(string accessToken)
        {
            try
            {
                var client = CliHttpClientFactory.CreateClient();
                var data = JsonSerializer.Serialize(new { token = accessToken });
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(CliConsts.LogoutUrl, content, CancellationToken.None))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Logger.LogWarning($"Cannot logout! Status Code: '{response.StatusCode}'");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Cannot logout. {e.Message}");
            }
        }

        public static bool IsLoggedIn()
        {
            return File.Exists(CliPaths.AccessToken);
        }
    }
}
