using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Applications
{
    public interface IOpenIddictApplicationRepository : IRepository<OpenIddictApplication, Guid>
    {
        Task<OpenIddictApplication> FindByClientIdAsync(
            string clientId,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictApplication>> FindByPostLogoutRedirectUriAsync(
            string address,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictApplication>> FindByRedirectUriAsync(
            string address,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictApplication>> GetListAsync(
            int? maxResultCount,
            int? skipCount,
            CancellationToken cancellationToken = default);
    }
}