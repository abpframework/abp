using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Text.Formatting;

namespace Volo.Abp.AspNetCore.MultiTenancy;

//TODO: Create a better domain format. We can accept regex for example.

public class DomainTenantResolveContributor : HttpTenantResolveContributorBase
{
    public const string ContributorName = "Domain";

    public override string Name => ContributorName;

    private static readonly string[] ProtocolPrefixes = { "http://", "https://" };

    private readonly string _domainFormat;

    public DomainTenantResolveContributor(string domainFormat)
    {
        _domainFormat = domainFormat.RemovePreFix(ProtocolPrefixes);
    }

    protected override Task<string?> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
    {
        if (!httpContext.Request.Host.HasValue)
        {
            return Task.FromResult<string?>(null);
        }

        var hostName = httpContext.Request.Host.Value.RemovePreFix(ProtocolPrefixes);

        context.Handled = true;

        var extractResult = FormattedStringValueExtracter.Extract(hostName, _domainFormat, ignoreCase: true);
        if (!extractResult.IsMatch)
        {
            return Task.FromResult<string?>(null);
        }

        // Extract also matches a host that merely contains the domain format, like "acme.abp.io.example.com" for the
        // format "{0}.abp.io". The host has to be that format filled with the extracted values, or the same host with
        // its port appended, because the format is not required to contain the port.
        var formattedHost = extractResult.Matches
            .Aggregate(_domainFormat, (current, nameValue) => current.Replace($"{{{nameValue.Name}}}", nameValue.Value));

        var hostFitsDomainFormat =
            hostName.Equals(formattedHost, StringComparison.OrdinalIgnoreCase) ||
            httpContext.Request.Host.Host.Equals(formattedHost, StringComparison.OrdinalIgnoreCase);

        return Task.FromResult(hostFitsDomainFormat ? extractResult.Matches[0].Value : null);
    }
}
