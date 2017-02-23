using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.Application.Services.Dtos;

namespace Volo.Abp.Identity
{
    public class HttpProxyUserAppService : IUserAppService
    {
        private readonly AbpIdentityHttpProxyOptions _options;

        public HttpProxyUserAppService(IOptionsSnapshot<AbpIdentityHttpProxyOptions> options)
        {
            _options = options.Value;
        }

        public async Task<ListResultDto<IdentityUserDto>> GetAll()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_options.ApiUrlBase + "api/identity/users");
                if (!response.IsSuccessStatusCode)
                {
                    throw new AbpException("Remote service returns error!");
                }

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ListResultDto<IdentityUserDto>>(
                    content,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }
        }

        public async Task<IdentityUserDto> Get(Guid id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_options.ApiUrlBase + "api/identity/users/" + id);
                if (!response.IsSuccessStatusCode)
                {
                    throw new AbpException("Remote service returns error!");
                }

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IdentityUserDto>(
                    content,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }
        }
    }
}
