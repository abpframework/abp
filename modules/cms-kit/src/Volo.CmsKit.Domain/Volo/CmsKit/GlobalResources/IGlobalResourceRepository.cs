using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.GlobalResources;

public interface IGlobalResourceRepository: IBasicRepository<GlobalResource, Guid>
{
    Task<GlobalResource> FindByName([NotNull] string name, CancellationToken cancellationToken = default);
}