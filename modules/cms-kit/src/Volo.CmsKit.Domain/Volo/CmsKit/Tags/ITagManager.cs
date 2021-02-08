using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Tags
{
    public interface ITagManager : IDomainService
    {
        Task<Tag> InsertAsync(
            Guid id,
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        Task<Tag> UpdateAsync(
            Guid id,
            [NotNull] string name,
            CancellationToken cancellationToken = default);

        Task<Tag> GetOrAddAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
    }
}
