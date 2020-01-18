using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Acme.BookStore.BookManagement.Books;
using Acme.BookStore.BookManagement.Samples;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;

namespace Acme.BookStore.BookManagement
{
    public class ClientDemoService : ITransientDependency
    {
        private readonly ISampleAppService _sampleAppService;
        private readonly IIdentityModelAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
        private readonly IBookAppService _bookAppService;

        public ClientDemoService(
            ISampleAppService sampleAppService, 
            IIdentityModelAuthenticationService authenticationService, 
            IConfiguration configuration,
            IBookAppService bookAppService)
        {
            _sampleAppService = sampleAppService;
            _authenticationService = authenticationService;
            _configuration = configuration;
            _bookAppService = bookAppService;
        }

        public async Task RunAsync()
        {
            await TestWithDynamicProxiesAsync();
            await TestWithHttpClientAndIdentityModelAuthenticationServiceAsync();
            await TestAllManuallyAsync();

            var result = await _bookAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            foreach (var bookDto in result.Items)
            {
                Console.WriteLine(bookDto.Name);
            }
        }

        /* Shows how to perform an HTTP request to the API using ABP's dynamic c# proxy
         * feature. It is just simple as calling a local service method.
         * Authorization and HTTP request details are handled by the ABP framework.
         */
        private async Task TestWithDynamicProxiesAsync()
        {
            Console.WriteLine();
            Console.WriteLine($"***** {nameof(TestWithDynamicProxiesAsync)} *****");

            var result = await _sampleAppService.GetAsync();
            Console.WriteLine("Result: " + result.Value);

            result = await _sampleAppService.GetAuthorizedAsync();
            Console.WriteLine("Result (authorized): " + result.Value);
        }

        /* Shows how to use HttpClient to perform a request to the HTTP API.
         * It uses ABP's IIdentityModelAuthenticationService to simplify obtaining access tokens.
         */
        private async Task TestWithHttpClientAndIdentityModelAuthenticationServiceAsync()
        {
            Console.WriteLine();
            Console.WriteLine($"***** {nameof(TestWithHttpClientAndIdentityModelAuthenticationServiceAsync)} *****");

            //Get access token using ABP's IIdentityModelAuthenticationService

            var accessToken = await _authenticationService.GetAccessTokenAsync(
                new IdentityClientConfiguration(
                    _configuration["IdentityClients:Default:Authority"],
                    _configuration["IdentityClients:Default:Scope"],
                    _configuration["IdentityClients:Default:ClientId"],
                    _configuration["IdentityClients:Default:ClientSecret"],
                    _configuration["IdentityClients:Default:GrantType"],
                    _configuration["IdentityClients:Default:UserName"],
                    _configuration["IdentityClients:Default:UserPassword"]
                )
            );

            //Perform the actual HTTP request

            using (var httpClient = new HttpClient())
            {
                httpClient.SetBearerToken(accessToken);

                var url = _configuration["RemoteServices:BookManagement:BaseUrl"] +
                          "api/BookManagement/sample/authorized";

                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseString = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: " + responseString);
                }
                else
                {
                    throw new Exception("Remote server returns error code: " + responseMessage.StatusCode);
                }
            }
        }

        /* Shows how to use HttpClient to perform a request to the HTTP API.
         * It obtains access token using IdentityServer's API. See its documentation:
         * https://identityserver4.readthedocs.io/en/latest/quickstarts/2_resource_owner_passwords.html
         */
        private async Task TestAllManuallyAsync()
        {
            Console.WriteLine();
            Console.WriteLine($"***** {nameof(TestAllManuallyAsync)} *****");

            //Obtain access token from the IDS4 server

            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_configuration["IdentityClients:Default:Authority"]);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _configuration["IdentityClients:Default:ClientId"],
                ClientSecret = _configuration["IdentityClients:Default:ClientSecret"],
                UserName = _configuration["IdentityClients:Default:UserName"],
                Password = _configuration["IdentityClients:Default:UserPassword"],
                Scope = _configuration["IdentityClients:Default:Scope"]
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            //Perform the actual HTTP request

            using (var httpClient = new HttpClient())
            {
                httpClient.SetBearerToken(tokenResponse.AccessToken);

                var url = _configuration["RemoteServices:BookManagement:BaseUrl"] +
                          "api/BookManagement/sample/authorized";

                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseString = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: " + responseString);
                }
                else
                {
                    throw new Exception("Remote server returns error code: " + responseMessage.StatusCode);
                }
            }
        }
    }
}