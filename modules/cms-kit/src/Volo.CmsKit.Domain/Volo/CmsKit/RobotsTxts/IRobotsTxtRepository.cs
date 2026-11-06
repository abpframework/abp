using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.RobotsTxts;

public interface IRobotsTxtRepository : IBasicRepository<RobotsTxt, Guid>
{
    /// <summary>
    /// Finds the robots.txt of the current tenant (or the host, if there is no current tenant).
    /// </summary>
    Task<RobotsTxt> FindAsync(CancellationToken cancellationToken = default);
}
