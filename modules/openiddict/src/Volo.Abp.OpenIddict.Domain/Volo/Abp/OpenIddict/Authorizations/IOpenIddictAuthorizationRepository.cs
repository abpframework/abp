using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Authorizations
{
    public interface IOpenIddictAuthorizationRepository : IRepository<OpenIddictAuthorization, Guid>
    {
        Task<List<OpenIddictAuthorization>> FindAsync(
            string subject,
            Guid applicationId,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictAuthorization>> FindAsync(
            string subject,
            Guid applicationId,
            string status,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictAuthorization>> FindAsync(
            string subject,
            Guid applicationId,
            string status,
            string type,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(
            Guid applicationId,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictAuthorization>> FindBySubjectAsync(
            string subject,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictAuthorization>> GetListAsync(
            int? maxResultCount,
            int? skipCount,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictAuthorization>> GetPruneListAsync(
            DateTime date,
            int maxResultCount = 1_000,
            CancellationToken cancellationToken = default);
    }
}