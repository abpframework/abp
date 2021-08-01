using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using SR = OpenIddict.Abstractions.OpenIddictResources;

namespace Volo.Abp.OpenIddict.Authorizations
{
    /// <inheritdoc/>
    public class AbpOpenIddictAuthorizationStore : OpenIddictAuthorizationStoreBase
    {
        protected IOpenIddictAuthorizationRepository AuthorizationRepository { get; }

        protected OpenIddictCleanupOptions Options { get; }

        public AbpOpenIddictAuthorizationStore(
            IGuidGenerator guidGenerator,
            IOpenIddictAuthorizationRepository openIddictAuthorizationRepository,
            IOptions<OpenIddictCleanupOptions> options)
            : base(guidGenerator)
        {
            AuthorizationRepository = openIddictAuthorizationRepository;
            Options = options.Value;
        }

        /// <inheritdoc/>
        public override async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            return await AuthorizationRepository.LongCountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public override async ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictAuthorization>, IQueryable<TResult>> query,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            var queryable = await AuthorizationRepository.GetQueryableAsync();
            return await AuthorizationRepository.AsyncExecuter
                .LongCountAsync(query(queryable), cancellationToken);
        }

        /// <inheritdoc/>
        [UnitOfWork]
        public override async ValueTask CreateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            await AuthorizationRepository.InsertAsync(authorization, true, cancellationToken);
        }

        /// <inheritdoc/>
        [UnitOfWork]
        public override async ValueTask UpdateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            await AuthorizationRepository.UpdateAsync(authorization, true, cancellationToken);
        }

        /// <inheritdoc/>
        [UnitOfWork]
        public override async ValueTask DeleteAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            await AuthorizationRepository.DeleteAsync(authorization, true, cancellationToken);
        }

        /// <inheritdoc/>
        [UnitOfWork]
        public override async ValueTask PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
        {
            List<Exception> exceptions = null;

            for (var index = 0; index < Options.CleanupLoopCount; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var date = threshold.UtcDateTime;

                var authorizations = await AuthorizationRepository.GetPruneListAsync(date, Options.CleanupBatchSize, cancellationToken);

                if (authorizations.Count == 0)
                {
                    break;
                }

                try
                {
                    await AuthorizationRepository.DeleteManyAsync(authorizations);
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

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var authorizations = (await AuthorizationRepository.FindAsync(subject, key, cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var authorization in authorizations)
                {
                    yield return authorization;
                }
            }
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

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var authorizations = (await AuthorizationRepository.FindAsync(subject, key, status, cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var authorization in authorizations)
                {
                    yield return authorization;
                }
            }
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

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var authorizations = (await AuthorizationRepository.FindAsync(subject, key, status, type, cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var authorization in authorizations)
                {
                    yield return authorization;
                }
            }
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

                var authorizations = (await AuthorizationRepository.FindAsync(subject, key, status, type, cancellationToken))
                    .AsAsyncEnumerable();

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

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var authorizations = (await AuthorizationRepository.FindByApplicationIdAsync(key, cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var authorization in authorizations)
                {
                    yield return authorization;
                }
            }
        }

        /// <inheritdoc/>
        public override async ValueTask<OpenIddictAuthorization> FindByIdAsync(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            var key = ConvertIdentifierFromString(identifier);

            return await AuthorizationRepository.FindAsync(key);
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

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var authorizations = (await AuthorizationRepository.FindBySubjectAsync(subject, cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var authorization in authorizations)
                {
                    yield return authorization;
                }
            }
        }

        /// <inheritdoc/>
        public override async ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            var queryable = await AuthorizationRepository.GetQueryableAsync();
            return await AuthorizationRepository.AsyncExecuter
                .FirstOrDefaultAsync(query(queryable, state), cancellationToken);
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictAuthorization> ListAsync(
            int? count,
            int? offset,
            CancellationToken cancellationToken)
        {
            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var authorizations = (await AuthorizationRepository
                    .GetListAsync(count, offset, cancellationToken))
                    .AsAsyncEnumerable();

                await foreach (var authorization in authorizations)
                {
                    yield return authorization;
                }
            }
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<TResult> ListAsync<TState, TResult>(
            Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<TResult> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var queryable = await AuthorizationRepository.GetQueryableAsync();

                var results = (await AuthorizationRepository.AsyncExecuter
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
