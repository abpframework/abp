using System;
using JetBrains.Annotations;
using Microsoft.Net.Http.Headers;
using Volo.Abp;

namespace Microsoft.AspNetCore.Http;

public static class AbpHttpRequestExtensions
{
    public static bool IsAjax([NotNull] this HttpRequest request)
    {
        Check.NotNull(request, nameof(request));

        return string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
               string.Equals(request.Headers[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal);
    }

    public static bool CanAccept([NotNull] this HttpRequest request, [NotNull] string contentType)
    {
        Check.NotNull(request, nameof(request));
        Check.NotNull(contentType, nameof(contentType));

        return request.Headers[HeaderNames.Accept].ToString().Contains(contentType, StringComparison.OrdinalIgnoreCase);
    }
}
