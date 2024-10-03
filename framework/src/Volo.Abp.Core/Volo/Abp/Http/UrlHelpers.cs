using System;

namespace Volo.Abp.Http;

public static class UrlHelpers
{
    private const string WildcardSubdomain = "*.";

    /// <summary>
    /// Check if the subdomain is a subdomain of the domain.
    /// The Uri must be absolute URI and the scheme, port, and host must be the same.
    /// </summary>
    public static bool IsSubdomainOf(string subdomain, string domain)
    {
        if (Uri.TryCreate(subdomain, UriKind.Absolute, out var subdomainUri) &&
            Uri.TryCreate(domain.Replace(WildcardSubdomain, string.Empty), UriKind.Absolute, out var domainUri))
        {
            return domainUri == subdomainUri || IsSubdomainOf(subdomainUri, domainUri);
        }

        return false;
    }

    /// <summary>
    /// Check if the subdomain is a subdomain of the domain.
    /// The Uri must be absolute URI and the scheme, port, and host must be the same.
    /// </summary>
    public static bool IsSubdomainOf(Uri subdomain, Uri domain)
    {
        return subdomain.IsAbsoluteUri
               && domain.IsAbsoluteUri
               && subdomain.Scheme == domain.Scheme
               && subdomain.Port == domain.Port
               && subdomain.Host.EndsWith($".{domain.Host}", StringComparison.Ordinal);
    }
}
