using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.ApiVersioning;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Versioning
{
    public class HttpContextRequestedApiVersion : IRequestedApiVersion, ITransientDependency
    {
        public string Current => _httpContextAccessor.HttpContext?.GetRequestedApiVersion().ToString();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextRequestedApiVersion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}