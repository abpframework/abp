using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.DynamicProxying
{
    public interface IApplicationApiDescriptionModelCache
    {
        Task<ApplicationApiDescriptionModel> GetAsync(string baseUrl, CancellationToken cancellationToken = default(CancellationToken));
    }
}