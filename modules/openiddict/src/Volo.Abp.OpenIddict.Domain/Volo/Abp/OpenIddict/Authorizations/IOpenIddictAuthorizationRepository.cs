using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Authorizations;

public interface IOpenIddictAuthorizationRepository : IBasicRepository<OpenIddictAuthorization, Guid>
{
    Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, string type, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default);

    Task<OpenIddictAuthorization> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> GetPruneListAsync(DateTime date, int count, CancellationToken cancellationToken = default);
}
