using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Text.Formatting;
using Volo.ExtensionMethods;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class DomainTenantResolver : HttpTenantResolverBase
    {
        private readonly string _domainFormat;

        public DomainTenantResolver(string domainFormat)
        {
            _domainFormat = domainFormat.RemovePreFix("http://", "https://");
        }

        protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            var hostName = httpContext.Request.Host.Host.RemovePreFix("http://", "https://");
            var extractResult = FormattedStringValueExtracter.Extract(hostName, _domainFormat, true);

            if (!extractResult.IsMatch)
            {
                return null;
            }

            return extractResult.Matches[0].Value;
        }
    }
}