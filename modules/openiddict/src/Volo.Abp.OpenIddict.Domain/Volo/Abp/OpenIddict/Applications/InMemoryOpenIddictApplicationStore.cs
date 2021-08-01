using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Tokens;
using SR = OpenIddict.Abstractions.OpenIddictResources;

namespace Volo.Abp.OpenIddict.Applications
{
    /// <inheritdoc/>
    public class InMemoryOpenIddictApplicationStore : OpenIddictApplicationStoreBase
    {
        protected ConcurrentDictionary<Guid, OpenIddictApplication> Applications => MemoryStore.Applications;

        protected ConcurrentDictionary<Guid, OpenIddictAuthorization> Authorizations => MemoryStore.Authorizations;

        protected ConcurrentDictionary<Guid, OpenIddictToken> Tokens => MemoryStore.Tokens;

        protected IOpenIddictMemoryStore MemoryStore { get; }

        public InMemoryOpenIddictApplicationStore(
            IOpenIddictMemoryStore memoryStore,
            IGuidGenerator guidGenerator) : base(guidGenerator)
        {
            MemoryStore = memoryStore;
        }

        /// <inheritdoc/>
        public override ValueTask<long> CountAsync(CancellationToken cancellationToken)
            => new(Applications.Count);

        /// <inheritdoc/>
        public override ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictApplication>, IQueryable<TResult>> query,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return new(query(Applications.Values.AsQueryable()).LongCount());
        }

        /// <inheritdoc/>
        public override ValueTask CreateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            if (Applications.Values.Any(x => x.ClientId == application.ClientId))
            {
                throw new AbpException($"Application ClientId '{application.ClientId}' exist.");
            }

            if (application.Id == default)
            {
                EntityHelper.TrySetId(
                    application,
                    () => GuidGenerator.Create(),
                    true
                );
            }

            Applications.TryAdd(application.Id, application);
            return default;
        }

        /// <inheritdoc/>
        public override ValueTask UpdateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.ConcurrencyStamp = Guid.NewGuid().ToString();

            Applications[application.Id] = application;

            return default;
        }

        /// <inheritdoc/>
        public override ValueTask DeleteAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            Applications.TryRemove(application.Id, out var _);

            var authorizations = Authorizations.Values
                .Where(application => application.ApplicationId == application.Id)
                .ToList();

            authorizations.ForEach(t => Authorizations.TryRemove(t.Id, out var _));

            var tokens = Tokens.Values
                .Where(token => token.ApplicationId == application.Id)
                .ToList();

            tokens.ForEach(x => Tokens.TryRemove(x.Id, out var _));

            return default;
        }

        /// <inheritdoc/>
        public override ValueTask<OpenIddictApplication> FindByClientIdAsync(
            string identifier, CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            return new(Applications.Values.FirstOrDefault(application => application.ClientId == identifier));
        }

        /// <inheritdoc/>
        public override ValueTask<OpenIddictApplication> FindByIdAsync(
            string identifier, CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            var key = ConvertIdentifierFromString(identifier);

            Applications.TryGetValue(key, out var application);

            return new(application);
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictApplication> FindByPostLogoutRedirectUriAsync(
            string address, CancellationToken cancellationToken)
        {
            if (address.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0143), nameof(address));
            }

            return ExecuteAsync();

            async IAsyncEnumerable<OpenIddictApplication> ExecuteAsync()
            {
                var applications = Applications.Values
                    .Where(application => application.PostLogoutRedirectUris!.Contains(address, StringComparer.Ordinal))
                    .AsAsyncEnumerable();

                await foreach (var application in applications)
                {
                    yield return application;
                }
            }
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictApplication> FindByRedirectUriAsync(
            string address, CancellationToken cancellationToken)
        {
            if (address.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0143), nameof(address));
            }

            return ExecuteAsync();

            async IAsyncEnumerable<OpenIddictApplication> ExecuteAsync()
            {
                var applications = Applications.Values
                    .Where(application => application.RedirectUris!.Contains(address, StringComparer.Ordinal))
                    .AsAsyncEnumerable();

                await foreach (var application in applications)
                {
                    yield return application;
                }
            }
        }

        /// <inheritdoc/>
        public override ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return new(query(Applications.Values.AsQueryable(), state).FirstOrDefault());
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictApplication> ListAsync(
            int? count,
            int? offset,
            CancellationToken cancellationToken)
        {
            var query = Applications.Values.OrderBy(application => application.Id!).AsEnumerable();

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<TResult> ListAsync<TState, TResult>(
            Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return query(Applications.Values.AsQueryable(), state).AsAsyncEnumerable();
        }
    }
}
