using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
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

        public IdentityServerTestController(IClientRepository clientRepository, IApiResourceRepository apiResourceRepository)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> CreateClient(string clientId)
        {
            var apiResource = (await _apiResourceRepository.GetListAsync()).FirstOrDefault(ar => ar.Name == "api1");

            if (apiResource == null)
            {
                apiResource = new ApiResource(GuidGenerator.Create(), "api1");
                await _apiResourceRepository.InsertAsync(apiResource);
            }

            var client = new Client(GuidGenerator.Create(), clientId);

            client.AddGrantTypes(GuidGenerator, GrantTypes.ClientCredentials);
            client.AddSecret(GuidGenerator, "secret".Sha256());
            client.AddAllowedScope(GuidGenerator, "api1"); //TODO: Why not with Id..?

            await _clientRepository.InsertAsync(client);

            return Content("OK: " + client.Id);
        }
    }
}
