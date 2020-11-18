using System.IO;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.Content
{
    internal class InternalRemoteStreamContent : IRemoteStreamContent
    {
        private readonly HttpContext _httpContext;
        
        public InternalRemoteStreamContent(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public string ContentType => _httpContext.Request.ContentType;

        public long? ContentLength => _httpContext.Request.ContentLength;

        public Stream GetStream()
        {
            return _httpContext.Request.Body;
        }
    }
}
