using System;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Text.Formatting;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    //TODO: Create a better domain format. We can accept regex for example.

    public class DomainTenantResolveContributer : HttpTenantResolveContributerBase
    {
        private readonly string _domainFormat;

        public DomainTenantResolveContributer(string domainFormat)
        {
            _domainFormat = domainFormat.RemovePreFix("http://", "https://");
        }

        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            if (httpContext.Request?.Host == null)
            {
                return null;
            }

            var hostName = httpContext.Request.Host.Host.RemovePreFix("http://", "https://");
            var extractResult = FormattedStringValueExtracter.Extract(hostName, _domainFormat, ignoreCase: true);

            if (!extractResult.IsMatch)
            {
                return null;
            }

            return extractResult.Matches[0].Value;
        }
    }
}