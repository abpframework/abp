using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;
using Volo.Abp.IO;

namespace Volo.Abp.Cli.Auth
{
    public class AuthService : ITransientDependency
    {
        protected IIdentityModelAuthenticationService AuthenticationService { get; }

        public AuthService(IIdentityModelAuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        public async Task LoginAsync(string userName, string password, string organizationName = null)
        {
            var configuration = new IdentityClientConfiguration(
                CliUrls.AccountAbpIo,
                "role email abpio abpio_www abpio_commercial", 
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

        public Task LogoutAsync()
        {
            FileHelper.DeleteIfExists(CliPaths.AccessToken);
            return Task.CompletedTask;
        }
    }
}
