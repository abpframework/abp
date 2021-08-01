using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.Linq;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using static OpenIddict.Abstractions.OpenIddictConstants;
using SR = OpenIddict.Abstractions.OpenIddictResources;

namespace Volo.Abp.OpenIddict.Tokens
{
    /// <inheritdoc/>
    public class InMemoryOpenIddictTokenStore : OpenIddictTokenStoreBase
    {
        protected ConcurrentDictionary<Guid, OpenIddictApplication> Applications => MemoryStore.Applications;

        protected ConcurrentDictionary<Guid, OpenIddictAuthorization> Authorizations => MemoryStore.Authorizations;

        protected ConcurrentDictionary<Guid, OpenIddictToken> Tokens => MemoryStore.Tokens;

        protected IOpenIddictMemoryStore MemoryStore { get; }

        protected OpenIddictCleanupOptions Options { get; }

        public InMemoryOpenIddictTokenStore(
            IOpenIddictMemoryStore memoryStore,
            IGuidGenerator guidGenerator,
            IOptions<OpenIddictCleanupOptions> options) : base(guidGenerator)
        {
            MemoryStore = memoryStore;
            Options = options.Value;
        }

        /// <inheritdoc/>
        public override ValueTask<long> CountAsync(CancellationToken cancellationToken)
            => new(Tokens.Count);

        /// <inheritdoc/>
        public override ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictToken>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return new(query(Tokens.Values.AsQueryable()).LongCount());
        }

        /// <inheritdoc/>
        public override ValueTask CreateAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (token.Id == default)
            {
                EntityHelper.TrySetId(
                    token,
                    () => GuidGenerator.Create(),
                    true
                );
            }

            Tokens.TryAdd(token.Id, token);
            return default;
        }

        /// <inheritdoc/>
        public override ValueTask UpdateAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.ConcurrencyStamp = Guid.NewGuid().ToString();

            Tokens[token.Id] = token;

            return default;
        }

        /// <inheritdoc/>
        public override ValueTask DeleteAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            Tokens.TryRemove(token.Id, out var _);

            return default;
        }

        /// <inheritdoc/>
        public override ValueTask PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
        {
            List<Exception> exceptions = null;

            // Note: to avoid sending too many queries, the maximum number of elements
            // that can be removed by a single call to PruneAsync() is deliberately limited.
            for (var index = 0; index < Options.CleanupLoopCount; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var date = threshold.UtcDateTime;

                var tokens =
                    (from token in Tokens.Values
                     where token.CreationDate < date
                     where (token.Status != Statuses.Inactive && token.Status != Statuses.Valid) ||
                           (token.AuthorizationId != null && Authorizations[token.AuthorizationId.Value].Status != Statuses.Valid) ||
                            token.ExpirationDate < DateTime.UtcNow
                     orderby token.Id
                     select token).Take(Options.CleanupBatchSize).ToList();

                if (tokens.Count == 0)
                {
                    break;
                }

                try
                {
                    tokens.ForEach(x => Tokens.TryRemove(x.Id, out var _));
                }
                catch (Exception exception)
                {
                    exceptions ??= new List<Exception>(capacity: 1);
                    exceptions.Add(exception);
                }
            }

            if (exceptions is not null)
            {
                throw new AggregateException(SR.GetResourceString(SR.ID0249), exceptions);
            }

            return default;
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictToken> FindAsync(
            string subject,
            string client,
            CancellationToken cancellationToken)
        {
            if (subject.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
            }

            if (client.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0124), nameof(client));
            }

            var key = ConvertIdentifierFromString(client);

            return (from token in Tokens.Values
                    where token.Subject == subject
                    join application in Applications.Values on token.ApplicationId equals application.Id
                    where application.Id!.Equals(key)
                    select token).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictToken> FindAsync(
            string subject,
            string client,
            string status,
            CancellationToken cancellationToken)
        {
            if (subject.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
            }

            if (client.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0124), nameof(client));
            }

            if (status.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0199), nameof(status));
            }

            var key = ConvertIdentifierFromString(client);

            return (from token in Tokens.Values
                    where token.Subject == subject &&
                          token.Status == status
                    join application in Applications.Values on token.ApplicationId equals application.Id
                    where application.Id!.Equals(key)
                    select token).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictToken> FindAsync(
            string subject,
            string client,
            string status,
            string type,
            CancellationToken cancellationToken)
        {
            if (subject.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
            }

            if (client.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0124), nameof(client));
            }

            if (status.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0199), nameof(status));
            }

            if (type.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0200), nameof(type));
            }

            var key = ConvertIdentifierFromString(client);

            return (from token in Tokens.Values
                    where token.Subject == subject &&
                          token.Status == status &&
                          token.Type == type
                    join application in Applications.Values on token.ApplicationId equals application.Id
                    where application.Id!.Equals(key)
                    select token).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictToken> FindByApplicationIdAsync(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            var key = ConvertIdentifierFromString(identifier);

            return (from token in Tokens.Values
                    join application in Applications.Values on token.ApplicationId equals application.Id
                    where application.Id!.Equals(key)
                    select token).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictToken> FindByAuthorizationIdAsync(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            var key = ConvertIdentifierFromString(identifier);

            return (from token in Tokens.Values
                    join authorization in Authorizations.Values on token.AuthorizationId equals authorization.Id
                    where authorization.Id!.Equals(key)
                    select token).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override ValueTask<OpenIddictToken> FindByIdAsync(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            var key = ConvertIdentifierFromString(identifier);

            Tokens.TryGetValue(key, out var token);

            return new(token);
        }

        /// <inheritdoc/>
        public override ValueTask<OpenIddictToken> FindByReferenceIdAsync(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            return new((from token in Tokens.Values
                        where token.ReferenceId == identifier
                        select token).FirstOrDefault());
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictToken> FindBySubjectAsync(
            string subject,
            CancellationToken cancellationToken)
        {
            if (subject.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
            }

            return (from token in Tokens.Values
                    where token.Subject == subject
                    select token).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return new(query(Tokens.Values.AsQueryable(), state).FirstOrDefault());
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictToken> ListAsync(
            int? count,
            int? offset,
            CancellationToken cancellationToken)
        {
            var query = Tokens.Values.AsEnumerable();

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
            Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return query(Tokens.Values.AsQueryable(), state).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override ValueTask SetApplicationIdAsync(OpenIddictToken token,
            string identifier,
            CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (!identifier.IsNullOrWhiteSpace())
            {
                var key = ConvertIdentifierFromString(identifier);

                if (!Applications.TryGetValue(key, out var application))
                {
                    throw new InvalidOperationException(SR.GetResourceString(SR.ID0250));
                }

                token.SetApplicationId(application.Id);
            }
            else
            {
                token.SetApplicationId(null);
            }

            return default;
        }

        /// <inheritdoc/>
        public override ValueTask SetAuthorizationIdAsync(OpenIddictToken token,
            string identifier,
            CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (!identifier.IsNullOrWhiteSpace())
            {
                var key = ConvertIdentifierFromString(identifier);

                if (!Authorizations.TryGetValue(key, out var authorization))
                {
                    throw new InvalidOperationException(SR.GetResourceString(SR.ID0251));
                }

                token.SetAuthorizationId(authorization.Id);
            }
            else
            {
                token.SetAuthorizationId(null);
            }

            return default;
        }
    }
}
