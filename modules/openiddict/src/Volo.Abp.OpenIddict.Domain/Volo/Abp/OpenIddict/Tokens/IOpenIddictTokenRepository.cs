using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Tokens
{
    public interface IOpenIddictTokenRepository : IRepository<OpenIddictToken, Guid>
    {
        Task<List<OpenIddictToken>> FindAsync(
            string subject,
            Guid applicationId,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictToken>> FindAsync(
            string subject,
            Guid applicationId,
            string status,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictToken>> FindAsync(
            string subject,
            Guid applicationId,
            string status,
            string type,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictToken>> FindByApplicationIdAsync(
            Guid applicationId,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(
            Guid authorizationId,
            CancellationToken cancellationToken = default);

        Task<OpenIddictToken> FindByReferenceIdAsync(
            string referenceId,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictToken>> FindBySubjectAsync(
            string subject,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictToken>> GetPruneListAsync(
            DateTime date,
            int maxResultCount = 1_000,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictToken>> GetListAsync(
            int? maxResultCount,
            int? skipCount,
            CancellationToken cancellationToken = default);
    }
}