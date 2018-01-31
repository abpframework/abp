using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace AbpDesk.Web.Mvc.Controllers
{
    [Route("identity-server-test")]
    public class IdentityServerTestController : AbpController
    {
        private readonly IClientRepository _clientRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IResourceStore _resourceStore;

        public IdentityServerTestController(
            IClientRepository clientRepository, 
            IApiResourceRepository apiResourceRepository,
            IResourceStore resourceStore)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _resourceStore = resourceStore;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Content("Resources: " + (await _resourceStore.FindApiResourceAsync("api1")).Name);
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> CreateClient(string clientId)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync("api1");

            if (apiResource == null)
            {
                apiResource = new ApiResource(GuidGenerator.Create(), "api1");
                await _apiResourceRepository.InsertAsync(apiResource);
            }

            var client = new Client(GuidGenerator.Create(), clientId);

            client.AddGrantTypes(GrantTypes.ClientCredentials);
            client.AddSecret("secret".Sha256());
            client.AddScope("api1");

            await _clientRepository.InsertAsync(client);

            return Content("OK: " + client.Id);
        }
    }
}
