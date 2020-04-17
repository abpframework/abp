using System;
using System.IO;
using System.Net.Http;
using System.Text;
using IdentityModel.Client;
using Volo.Abp.Cli.Auth;

namespace Volo.Abp.Cli.Http
{
    public class CliHttpClient : HttpClient
    {
        public static TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(1);

        public CliHttpClient(TimeSpan? timeout = null) : base(new CliHttpClientHandler())
        {
            Timeout = timeout ?? DefaultTimeout;

            AddAuthentication(this);
        }

        public CliHttpClient(bool setBearerToken) : base(new CliHttpClientHandler())
        {
            Timeout = DefaultTimeout;

            if (setBearerToken)
            {
                AddAuthentication(this);
            }
        }

        private static void AddAuthentication(HttpClient client)
        {
            if (!AuthService.IsLoggedIn())
            {
                return;
            }

            var accessToken = File.ReadAllText(CliPaths.AccessToken, Encoding.UTF8);
            if (!accessToken.IsNullOrEmpty())
            {
                client.SetBearerToken(accessToken);
            }
        }
    }
}