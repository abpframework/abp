using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Uow;

public class AbpAspNetCoreUnitOfWorkOptions
{
    /// <summary>
    /// This is used to disable the <see cref="AbpUnitOfWorkMiddleware"/>,
    /// app.UseUnitOfWork(), for the specified URLs.
    /// <see cref="AbpUnitOfWorkMiddleware"/> will be disabled for URLs
    /// starting with an ignored URL.  
    /// </summary>
    public List<string> IgnoredUrls { get; } = new List<string>();
}
