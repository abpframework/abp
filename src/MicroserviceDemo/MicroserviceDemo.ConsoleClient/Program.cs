using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace MicroserviceDemo.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunDemo().Wait();
            Console.ReadLine();
        }

        private static async Task RunDemo()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://abp-test-authserver.com:54307");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("multi-tenancy-api");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://abp-test-tenancy.com:63877/api/multi-tenancy/tenant?SkipCount=0&MaxResultCount=100");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }
    }
}
