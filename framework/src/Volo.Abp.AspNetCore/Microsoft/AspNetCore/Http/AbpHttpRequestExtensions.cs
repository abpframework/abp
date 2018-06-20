using JetBrains.Annotations;
using Volo.Abp;

namespace Microsoft.AspNetCore.Http
{
    public static class AbpHttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public static bool IsAjax([NotNull]this HttpRequest request)
        {
            Check.NotNull(request, nameof(request));

            if (request.Headers == null)
            {
                return false;
            }

            return request.Headers[RequestedWithHeader] == XmlHttpRequest;
        }

        public static bool CanAccept([NotNull]this HttpRequest request, [NotNull] string contentType)
        {
            Check.NotNull(request, nameof(request));
            Check.NotNull(contentType, nameof(contentType));

            return request.Headers["Accept"].ToString().Contains(contentType);
        }
    }
}
