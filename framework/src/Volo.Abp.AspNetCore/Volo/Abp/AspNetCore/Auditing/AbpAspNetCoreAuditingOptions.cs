using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Auditing
{
    public class AbpAspNetCoreAuditingOptions
    {
        /// <summary>
        /// This is used to disable the <see cref="AbpAuditingMiddleware"/>,
        /// app.UseAuditing(), for the specified URLs.
        /// <see cref="AbpAuditingMiddleware"/> will be disabled for URLs
        /// starting with an ignored URL.  
        /// </summary>
        public List<string> IgnoredUrls { get; } = new List<string>();
    }
}