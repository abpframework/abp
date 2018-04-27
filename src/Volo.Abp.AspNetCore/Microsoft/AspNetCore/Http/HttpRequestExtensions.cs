using Volo.Abp;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public static bool IsAjax(this HttpRequest request)
        {
            Check.NotNull(request, nameof(request));

            if (request.Headers == null)
            {
                return false;
            }

            return request.Headers[RequestedWithHeader] == XmlHttpRequest;
        }
    }
}
