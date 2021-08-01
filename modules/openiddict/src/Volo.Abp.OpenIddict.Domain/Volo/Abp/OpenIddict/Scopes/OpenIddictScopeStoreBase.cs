using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using SR = OpenIddict.Abstractions.OpenIddictResources;

namespace Volo.Abp.OpenIddict.Scopes
{
    /// <inheritdoc/>
    public abstract class OpenIddictScopeStoreBase : IOpenIddictScopeStore<OpenIddictScope>
    {
        protected IGuidGenerator GuidGenerator { get; }

        protected OpenIddictScopeStoreBase(
            IGuidGenerator guidGenerator)
        {
            GuidGenerator = guidGenerator;
        }

        /// <inheritdoc/>
        public abstract ValueTask<long> CountAsync(CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictScope>, IQueryable<TResult>> query, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask CreateAsync(OpenIddictScope scope, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask UpdateAsync(OpenIddictScope scope, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask DeleteAsync(OpenIddictScope scope, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<OpenIddictScope> FindByIdAsync(string identifier, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<OpenIddictScope> FindByNameAsync(string name, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictScope> FindByNamesAsync(
            ImmutableArray<string> names, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictScope> FindByResourceAsync(
            string resource, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query,
            TState state, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask<string> GetDescriptionAsync(OpenIddictScope scope,
            CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            return new ValueTask<string>(scope.Description);
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDescriptionsAsync(
            OpenIddictScope scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (scope.Descriptions.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
            }

            return new(scope.Descriptions.ToImmutableDictionary());
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetDisplayNameAsync(OpenIddictScope scope,
            CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            return new ValueTask<string>(scope.DisplayName);
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(
            OpenIddictScope scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (scope.DisplayNames.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
            }

            return new(scope.DisplayNames.ToImmutableDictionary());
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetIdAsync(OpenIddictScope scope,
            CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            return new ValueTask<string>(scope.Id.ToString());
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetNameAsync(OpenIddictScope scope,
            CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            return new ValueTask<string>(scope.Name);
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(
            OpenIddictScope scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (scope.Properties.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
            }

            return new(scope.Properties.ToImmutableDictionary());
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableArray<string>> GetResourcesAsync(OpenIddictScope scope,
            CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (scope.Resources.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            return new(scope.Resources.ToImmutableArray());
        }

        /// <inheritdoc/>
        public virtual ValueTask<OpenIddictScope> InstantiateAsync(CancellationToken cancellationToken)
        {
            try
            {
                return new ValueTask<OpenIddictScope>(new OpenIddictScope(GuidGenerator.Create()));
            }
            catch (MemberAccessException exception)
            {
                return new ValueTask<OpenIddictScope>(Task.FromException<OpenIddictScope>(
                    new InvalidOperationException(SR.GetResourceString(SR.ID0246), exception)));
            }
        }

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictScope> ListAsync(
            int? count, int? offset, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<TResult> ListAsync<TState, TResult>(
            Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query,
            TState state, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask SetDescriptionAsync(OpenIddictScope scope,
            string description, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.SetDescription(description);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetDescriptionsAsync(OpenIddictScope scope,
            ImmutableDictionary<CultureInfo, string> descriptions, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.SetDescriptions(new Dictionary<CultureInfo, string>(descriptions));

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetDisplayNameAsync(OpenIddictScope scope,
            string name, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.SetDisplayName(name);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetDisplayNamesAsync(OpenIddictScope scope,
            ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.SetDisplayNames(new Dictionary<CultureInfo, string>(names));

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetNameAsync(OpenIddictScope scope, string name, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.SetName(name);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetPropertiesAsync(OpenIddictScope scope,
            ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.SetProperties(new Dictionary<string, JsonElement>(properties));

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetResourcesAsync(OpenIddictScope scope,
            ImmutableArray<string> resources, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.SetResources(new HashSet<string>(resources));

            return default;
        }

        protected virtual Guid ConvertIdentifierFromString(string identifier)
        {
            if (identifier.IsNullOrWhiteSpace())
            {
                return default;
            }

            return Guid.Parse(identifier);
        }
    }
}
