using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Tokens;
using static OpenIddict.Abstractions.OpenIddictConstants;
using SR = OpenIddict.Abstractions.OpenIddictResources;

namespace Volo.Abp.OpenIddict.Authorizations
{
    /// <inheritdoc/>
    public class InMemoryOpenIddictAuthorizationStore : OpenIddictAuthorizationStoreBase
    {
        protected ConcurrentDictionary<Guid, OpenIddictApplication> Applications => MemoryStore.Applications;

        protected ConcurrentDictionary<Guid, OpenIddictAuthorization> Authorizations => MemoryStore.Authorizations;

        protected ConcurrentDictionary<Guid, OpenIddictToken> Tokens => MemoryStore.Tokens;

        protected IOpenIddictMemoryStore MemoryStore { get; }

        protected OpenIddictCleanupOptions Options { get; }

        public InMemoryOpenIddictAuthorizationStore(
            IOpenIddictMemoryStore memoryStore,
            IGuidGenerator guidGenerator,
            IOptions<OpenIddictCleanupOptions> options) : base(guidGenerator)
        {
            MemoryStore = memoryStore;
            Options = options.Value;
        }

        /// <inheritdoc/>
        public override ValueTask<long> CountAsync(CancellationToken cancellationToken)
            => new(Authorizations.Count);

        /// <inheritdoc/>
        public override ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictAuthorization>, IQueryable<TResult>> query,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return new(query(Authorizations.Values.AsQueryable()).LongCount());
        }

        /// <inheritdoc/>
        public override ValueTask CreateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            if (authorization.Id == default)
            {
                EntityHelper.TrySetId(
                    authorization,
                    () => GuidGenerator.Create(),
                    true
                );
            }

            Authorizations.TryAdd(authorization.Id, authorization);
            return default;
        }

        /// <inheritdoc/>
        public override ValueTask UpdateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            authorization.ConcurrencyStamp = Guid.NewGuid().ToString();

            Authorizations[authorization.Id] = authorization;

            return default;
        }

        /// <inheritdoc/>
        public override ValueTask DeleteAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            Authorizations.TryRemove(authorization.Id, out var _);

            var tokens = Tokens.Values
                .Where(token => token.ApplicationId == authorization.Id)
                .ToList();
            tokens.ForEach(t => Tokens.TryRemove(t.Id, out var _));

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

                var authorizations =
                         (from authorization in Authorizations.Values
                          where authorization.CreationDate < date
                          where authorization.Status != Statuses.Valid ||
                               (authorization.Type == AuthorizationTypes.AdHoc && !Tokens.Values.Any(x => x.AuthorizationId == authorization.Id))
                          orderby authorization.Id
                          select authorization).Take(Options.CleanupBatchSize).ToList();

                if (authorizations.Count == 0)
                {
                    break;
                }

                try
                {
                    authorizations.ForEach(a => Authorizations.TryRemove(a.Id, out var _));
                }

                catch (Exception exception)
                {
                    exceptions ??= new List<Exception>(capacity: 1);
                    exceptions.Add(exception);
                }
            }

            if (exceptions is not null)
            {
                throw new AggregateException(SR.GetResourceString(SR.ID0243), exceptions);
            }

            return default;
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictAuthorization> FindAsync(
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

            return (from authorization in Authorizations.Values
                    where authorization.Subject == subject
                    join application in Applications.Values on authorization.ApplicationId equals application.Id
                    where application.Id!.Equals(key)
                    select authorization).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictAuthorization> FindAsync(
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

            return (from authorization in Authorizations.Values
                    where authorization.Subject == subject && authorization.Status == status
                    join application in Applications.Values on authorization.ApplicationId equals application.Id
                    where application.Id!.Equals(key)
                    select authorization).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictAuthorization> FindAsync(
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

            return (from authorization in Authorizations.Values
                    where authorization.Subject == subject &&
                          authorization.Status == status &&
                          authorization.Type == type
                    join application in Applications.Values on authorization.ApplicationId equals application.Id
                    where application.Id!.Equals(key)
                    select authorization).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictAuthorization> FindAsync(
            string subject,
            string client,
            string status,
            string type,
            ImmutableArray<string> scopes,
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

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var key = ConvertIdentifierFromString(client);

                var authorizations = (from authorization in Authorizations.Values
                                      where authorization.Subject == subject &&
                                            authorization.Status == status &&
                                            authorization.Type == type
                                      join application in Applications.Values on authorization.ApplicationId equals application.Id
                                      where application.Id!.Equals(key)
                                      select authorization).AsAsyncEnumerable();

                await foreach (var authorization in authorizations)
                {
                    if (new HashSet<string>(await GetScopesAsync(authorization, cancellationToken), StringComparer.Ordinal).IsSupersetOf(scopes))
                    {
                        yield return authorization;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictAuthorization> FindByApplicationIdAsync(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            var key = ConvertIdentifierFromString(identifier);

            return (from authorization in Authorizations.Values
                    join application in Applications.Values on authorization.ApplicationId equals application.Id
                    where application.Id!.Equals(key)
                    select authorization).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override ValueTask<OpenIddictAuthorization> FindByIdAsync(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            var key = ConvertIdentifierFromString(identifier);

            Authorizations.TryGetValue(key, out var authorization);

            return new(authorization);
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictAuthorization> FindBySubjectAsync(
            string subject,
            CancellationToken cancellationToken)
        {
            if (subject.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
            }

            return (from authorization in Authorizations.Values
                    where authorization.Subject == subject
                    select authorization).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override ValueTask<string> GetApplicationIdAsync(
            OpenIddictAuthorization authorization,
            CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            if (!Applications.ContainsKey(authorization.ApplicationId.Value))
            {
                return new((string)null);
            }

            return new(authorization.ApplicationId?.ToString());
        }

        /// <inheritdoc/>
        public override ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return new(query(Authorizations.Values.AsQueryable(), state).FirstOrDefault());
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictAuthorization> ListAsync(
            int? count,
            int? offset,
            CancellationToken cancellationToken)
        {
            var query = Authorizations.Values.OrderBy(authorization => authorization.Id!).AsEnumerable();

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
            Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return query(Authorizations.Values.AsQueryable(), state).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override ValueTask SetApplicationIdAsync(
            OpenIddictAuthorization authorization,
            string identifier,
            CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            if (!string.IsNullOrEmpty(identifier))
            {
                var key = ConvertIdentifierFromString(identifier);

                if (!Applications.TryGetValue(key, out var application))
                {
                    throw new InvalidOperationException(SR.GetResourceString(SR.ID0244));
                }

                authorization.SetApplicationId(application.Id);
            }
            else
            {
                authorization.SetApplicationId(null);
            }

            return default;
        }
    }
}
