using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Uow;
using SR = OpenIddict.Abstractions.OpenIddictResources;

namespace Volo.Abp.OpenIddict.Applications
{
    /// <inheritdoc/>
    public class AbpOpenIddictApplicationStore : OpenIddictApplicationStoreBase
    {
        protected IOpenIddictApplicationRepository ApplicationRepository { get; }
        protected IOpenIddictAuthorizationRepository OpenIddictAuthorizationRepository { get; }
        protected IOpenIddictTokenRepository OpenIddictTokenRepository { get; }

        public AbpOpenIddictApplicationStore(
            IGuidGenerator guidGenerator,
            IOpenIddictApplicationRepository openIddictApplicationRepository,
            IOpenIddictAuthorizationRepository openIddictAuthorizationRepository,
            IOpenIddictTokenRepository openIddictTokenRepository)
            : base(guidGenerator)
        {
            ApplicationRepository = openIddictApplicationRepository;
            OpenIddictAuthorizationRepository = openIddictAuthorizationRepository;
            OpenIddictTokenRepository = openIddictTokenRepository;
        }

        /// <inheritdoc/>
        public override async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            return await ApplicationRepository.LongCountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public override async ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictApplication>, IQueryable<TResult>> query,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            var queryable = await ApplicationRepository.GetQueryableAsync();
            return await ApplicationRepository.AsyncExecuter
                .LongCountAsync(query(queryable), cancellationToken);
        }

        /// <inheritdoc/>
        [UnitOfWork]
        public override async ValueTask CreateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            await ApplicationRepository.InsertAsync(application, true, cancellationToken);
        }

        /// <inheritdoc/>
        [UnitOfWork]
        public override async ValueTask UpdateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            await ApplicationRepository.UpdateAsync(application);
        }

        /// <inheritdoc/>
        [UnitOfWork]
        public override async ValueTask DeleteAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            await ApplicationRepository.DeleteAsync(application, true, cancellationToken);
        }

        /// <inheritdoc/>
        public override async ValueTask<OpenIddictApplication> FindByClientIdAsync(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            return await ApplicationRepository.FindByClientIdAsync(identifier, cancellationToken);
        }

        /// <inheritdoc/>
        public override async ValueTask<OpenIddictApplication> FindByIdAsync(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            var key = ConvertIdentifierFromString(identifier);
            return await ApplicationRepository.FindAsync(key, false, cancellationToken);
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictApplication> FindByPostLogoutRedirectUriAsync(
            string address,
            CancellationToken cancellationToken)
        {
            if (address.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0143), nameof(address));
            }

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictApplication> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var applications = (await ApplicationRepository
                    .FindByPostLogoutRedirectUriAsync(address, cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var application in applications)
                {
                    var addresses = await GetPostLogoutRedirectUrisAsync(application, cancellationToken);
                    if (addresses.Contains(address, StringComparer.Ordinal))
                    {
                        yield return application;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictApplication> FindByRedirectUriAsync(
            string address,
            CancellationToken cancellationToken)
        {
            if (address.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0143), nameof(address));
            }

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictApplication> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var applications = (await ApplicationRepository
                    .FindByRedirectUriAsync(address, cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var application in applications)
                {
                    var addresses = await GetRedirectUrisAsync(application, cancellationToken);
                    if (addresses.Contains(address, StringComparer.Ordinal))
                    {
                        yield return application;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override async ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            var queryable = await ApplicationRepository.GetQueryableAsync();
            return await ApplicationRepository.AsyncExecuter
                .FirstOrDefaultAsync(query(queryable, state), cancellationToken);
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictApplication> ListAsync(
            int? count,
            int? offset,
            CancellationToken cancellationToken)
        {
            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictApplication> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var applications = (await ApplicationRepository
                    .GetListAsync(count, offset, cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var application in applications)
                {
                    yield return application;
                }
            }
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<TResult> ListAsync<TState, TResult>(
            Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<TResult> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var queryable = await ApplicationRepository.GetQueryableAsync();

                var results = (await ApplicationRepository.AsyncExecuter
                    .ToListAsync(query(queryable, state), cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var result in results)
                {
                    yield return result;
                }
            }
        }
    }
}
