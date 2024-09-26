using System;

namespace Volo.Abp.Http;

public static class UrlHelpers
{
    private const string WildcardSubdomain = "*.";

    public static bool IsSubdomainOf(string subdomain, string domain)
    {
        if (Uri.TryCreate(subdomain, UriKind.Absolute, out var subdomainUri) &&
            Uri.TryCreate(domain.Replace(WildcardSubdomain, string.Empty), UriKind.Absolute, out var domainUri))
        {
            return domainUri == subdomainUri || IsSubdomainOf(subdomainUri, domainUri);
        }

        return false;
    }

    public static bool IsSubdomainOf(Uri subdomain, Uri domain)
    {
        return subdomain.IsAbsoluteUri
               && domain.IsAbsoluteUri
               && subdomain.Scheme == domain.Scheme
               && subdomain.Port == domain.Port
               && subdomain.Host.EndsWith($".{domain.Host}", StringComparison.Ordinal);
    }
}
