using System;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.IdentityModel;

namespace ConsoleClientDemo
{
    public class ClientDemoService : ITransientDependency
    {
        private readonly IIdentityUserAppService _userAppService;
        private readonly IIdentityModelHttpClientAuthenticator _authenticator;

        public ClientDemoService(
            IIdentityUserAppService userAppService,
            IIdentityModelHttpClientAuthenticator authenticator)
        {
            _userAppService = userAppService;
            _authenticator = authenticator;
        }

        public async Task RunAsync()
        {
            await TestWithHttpClient();
            await TestIdentityService();
        }

        /// <summary>
        /// Shows how to manually create an HttpClient and authenticate using the
        /// IIdentityModelHttpClientAuthenticator service.
        /// </summary>
        private async Task TestWithHttpClient()
        {
            Console.WriteLine("*** TestWithHttpClient ***");

            using (var client = new HttpClient())
            {
                await _authenticator.AuthenticateAsync(client);

                var response = await client.GetAsync("http://localhost:63568/Test");
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

        /// <summary>
        /// Shows how to use application service interfaces (IIdentityUserAppService in this sample)
        /// to call a remote service which is possible by the dynamic http client proxy system.
        /// No need to use IIdentityModelHttpClientAuthenticator since the dynamic http client proxy
        /// system internally uses it. You just inject a service (IIdentityUserAppService)
        /// and call a method (GetListAsync) like a local method.
        /// </summary>
        private async Task TestIdentityService()
        {
            var output = await _userAppService.GetListAsync(new GetIdentityUsersInput());

            Console.WriteLine("*** TestIdentityService ***");
            Console.WriteLine("Total user count: " + output.TotalCount);

            foreach (var user in output.Items)
            {
                Console.WriteLine($"- UserName={user.UserName}, Email={user.Email}, Name={user.Name}, Surname={user.Surname}");
            }
        }
    }
}