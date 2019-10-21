using System;
using System.IO;
using System.Net.Http;
using System.Text;
using IdentityModel.Client;

namespace Volo.Abp.Cli.Http
{
    public class CliHttpClient : HttpClient
    {
        public CliHttpClient(TimeSpan? timeout = null) : base(new CliHttpClientHandler())
        {
            Timeout = timeout ?? TimeSpan.FromMinutes(1);

            AddAuthentication(this);
        }

        private static void AddAuthentication(HttpClient client)
        {
            if (File.Exists(CliPaths.AccessToken))
            {
                var accessToken = File.ReadAllText(CliPaths.AccessToken, Encoding.UTF8);
                if (!accessToken.IsNullOrEmpty())
                {
                    client.SetBearerToken(accessToken);
                }
            }
        }
    }
}