using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Tokens;

public interface IOpenIddictTokenRepository : IBasicRepository<OpenIddictToken, Guid>
{
    Task DeleteManyByApplicationIdAsync(Guid applicationId, bool autoSave = false, CancellationToken cancellationToken = default);

    Task DeleteManyByAuthorizationIdAsync(Guid authorizationId, bool autoSave = false, CancellationToken cancellationToken = default);

    Task DeleteManyByAuthorizationIdsAsync(Guid[] authorizationIds, bool autoSave = false, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid? client, string status, string type, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(Guid authorizationId, CancellationToken cancellationToken = default);

    Task<OpenIddictToken> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<OpenIddictToken> FindByReferenceIdAsync(string referenceId, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default);

    Task<long> PruneAsync(DateTime date, CancellationToken cancellationToken = default);

    ValueTask<long> RevokeAsync(string subject, Guid? applicationId, string status, string type, CancellationToken cancellationToken = default);

    ValueTask<long> RevokeByAuthorizationIdAsync(Guid id, CancellationToken cancellationToken = default);

    ValueTask<long> RevokeByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default);

    ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken = default);
}
