using System;
using System.Net.Http;

namespace Volo.Abp.IdentityModel
{
    public class IdentityModelHttpRequestMessageOptions
    {
        public Action<HttpRequestMessage> ConfigureHttpRequestMessage { get; set; }
    }
}
