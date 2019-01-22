using ProductManagement;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Identity;
using Volo.Abp.IdentityModel;

namespace ConsoleClientDemo
{
    public class ClientDemoService : ITransientDependency
    {
        private readonly IIdentityUserAppService _userAppService;
        private readonly IProductAppService _productAppService;
        private readonly IIdentityModelHttpClientAuthenticator _authenticator;
        private readonly RemoteServiceOptions _remoteServiceOptions;

        public ClientDemoService(
            IIdentityUserAppService userAppService,
            IProductAppService productAppService,
            IIdentityModelHttpClientAuthenticator authenticator, 
            IOptions<RemoteServiceOptions> remoteServiceOptions)
        {
            _userAppService = userAppService;
            _authenticator = authenticator;
            _remoteServiceOptions = remoteServiceOptions.Value;
            _productAppService = productAppService;
        }

        public async Task RunAsync()
        {
            await TestWithHttpClient();
            await TestIdentityService();
            await TestProductService();
        }

        /// <summary>
        /// Shows how to manually create an HttpClient and authenticate using the
        /// IIdentityModelHttpClientAuthenticator service.
        /// </summary>
        private async Task TestWithHttpClient()
        {
            Console.WriteLine();
            Console.WriteLine("*** TestWithHttpClient ************************************");

            try
            {
                using (var client = new HttpClient())
                {
                    await _authenticator.AuthenticateAsync(client);

                    var response = await client.GetAsync(_remoteServiceOptions.RemoteServices.Default.BaseUrl.EnsureEndsWith('/') + "Test/Index");
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
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            Console.WriteLine();
            Console.WriteLine("*** TestIdentityService ************************************");

            try
            {
                var output = await _userAppService.GetListAsync(new GetIdentityUsersInput());

                Console.WriteLine("Total user count: " + output.TotalCount);

                foreach (var user in output.Items)
                {
                    Console.WriteLine($"- UserName={user.UserName}, Email={user.Email}, Name={user.Name}, Surname={user.Surname}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Shows how to use application service interfaces (IProductAppService in this sample)
        /// to call a remote service which is possible by the dynamic http client proxy system.
        /// No need to use IIdentityModelHttpClientAuthenticator since the dynamic http client proxy
        /// system internally uses it. You just inject a service (IProductAppService)
        /// and call a method (GetListAsync) like a local method.
        /// </summary>
        private async Task TestProductService()
        {
            Console.WriteLine();
            Console.WriteLine("*** TestProductService ************************************");

            try
            {
                var output = await _productAppService.GetListAsync();
                Console.WriteLine("Total product count: " + output.Items.Count);

                foreach (var product in output.Items)
                {
                    Console.WriteLine($"- Code={product.Code}, Name={product.Name}, Price={product.Price}, StockCount={product.StockCount}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}