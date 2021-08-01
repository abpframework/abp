using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using SR = OpenIddict.Abstractions.OpenIddictResources;

namespace Volo.Abp.OpenIddict.Scopes
{
    /// <inheritdoc/>
    public class InMemoryOpenIddictScopeStore : OpenIddictScopeStoreBase
    {
        protected ConcurrentDictionary<Guid, OpenIddictScope> Scopes => MemoryStore.Scopes;

        protected IOpenIddictMemoryStore MemoryStore { get; }

        public InMemoryOpenIddictScopeStore(
            IOpenIddictMemoryStore memoryStore,
            IGuidGenerator guidGenerator) : base(guidGenerator)
        {
            MemoryStore = memoryStore;
        }

        /// <inheritdoc/>
        public override ValueTask<long> CountAsync(CancellationToken cancellationToken)
            => new(Scopes.Count);

        /// <inheritdoc/>
        public override ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictScope>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return new(query(Scopes.Values.AsQueryable()).LongCount());
        }

        /// <inheritdoc/>
        public override ValueTask CreateAsync(OpenIddictScope scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (scope.Id == default)
            {
                EntityHelper.TrySetId(
                    scope,
                    () => GuidGenerator.Create(),
                    true
                );
            }

            Scopes.TryAdd(scope.Id, scope);
            return default;
        }

        /// <inheritdoc/>
        public override ValueTask UpdateAsync(OpenIddictScope scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.ConcurrencyStamp = Guid.NewGuid().ToString();

            Scopes[scope.Id] = scope;

            return default;
        }

        /// <inheritdoc/>
        public override ValueTask DeleteAsync(OpenIddictScope scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            Scopes.TryRemove(scope.Id, out var _);

            return default;
        }

        /// <inheritdoc/>
        public override ValueTask<OpenIddictScope> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
            }

            var key = ConvertIdentifierFromString(identifier);

            Scopes.TryGetValue(key, out var scope);

            return new(scope);
        }

        /// <inheritdoc/>
        public override ValueTask<OpenIddictScope> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0202), nameof(name));
            }

            return new(Scopes.Values.FirstOrDefault(scope => scope.Name == name));
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictScope> FindByNamesAsync(
            ImmutableArray<string> names,
            CancellationToken cancellationToken)
        {
            if (names.Any(name => name.IsNullOrWhiteSpace()))
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0203), nameof(names));
            }

            return (from scope in Scopes.Values
                    where Enumerable.Contains(names, scope.Name)
                    select scope).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictScope> FindByResourceAsync(
            string resource,
            CancellationToken cancellationToken)
        {
            if (resource.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(SR.GetResourceString(SR.ID0062), nameof(resource));
            }

            return ExecuteAsync(cancellationToken);

            async IAsyncEnumerable<OpenIddictScope> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var scopes = (from scope in Scopes.Values
                              where scope.Resources!.Contains(resource)
                              select scope).AsAsyncEnumerable();

                await foreach (var scope in scopes)
                {
                    var resources = await GetResourcesAsync(scope, cancellationToken);
                    if (resources.Contains(resource, StringComparer.Ordinal))
                    {
                        yield return scope;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query,
            TState state,
            CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return new(query(Scopes.Values.AsQueryable(), state).FirstOrDefault());
        }

        /// <inheritdoc/>
        public override IAsyncEnumerable<OpenIddictScope> ListAsync(
            int? count,
            int? offset,
            CancellationToken cancellationToken)
        {
            var query = Scopes.Values.OrderBy(scope => scope.Id!).AsEnumerable();

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
            Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query,
            TState state, CancellationToken cancellationToken)
        {
            Check.NotNull(query, nameof(query));

            return query(Scopes.Values.AsQueryable(), state).AsAsyncEnumerable();
        }
    }
}
