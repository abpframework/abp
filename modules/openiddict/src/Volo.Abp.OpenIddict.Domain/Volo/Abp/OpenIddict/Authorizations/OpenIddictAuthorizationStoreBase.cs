using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using SR = OpenIddict.Abstractions.OpenIddictResources;

namespace Volo.Abp.OpenIddict.Authorizations
{
    /// <inheritdoc/>
    public abstract class OpenIddictAuthorizationStoreBase : IOpenIddictAuthorizationStore<OpenIddictAuthorization>
    {
        protected IGuidGenerator GuidGenerator { get; }

        protected OpenIddictAuthorizationStoreBase(
            IGuidGenerator guidGenerator)
        {
            GuidGenerator = guidGenerator;
        }

        /// <inheritdoc/>
        public abstract ValueTask<long> CountAsync(CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictAuthorization>, IQueryable<TResult>> query, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask CreateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask UpdateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask DeleteAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictAuthorization> FindAsync(
            string subject, string client, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictAuthorization> FindAsync(
            string subject, string client,
            string status, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictAuthorization> FindAsync(
            string subject, string client,
            string status, string type, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictAuthorization> FindAsync(
            string subject, string client,
            string status, string type,
            ImmutableArray<string> scopes, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictAuthorization> FindByApplicationIdAsync(
            string identifier, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<OpenIddictAuthorization> FindByIdAsync(
            string identifier, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictAuthorization> FindBySubjectAsync(
            string subject, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask<string> GetApplicationIdAsync(
            OpenIddictAuthorization authorization,
            CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            return new(authorization.ApplicationId?.ToString());
        }

        /// <inheritdoc/>
        public abstract ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query,
            TState state, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask<DateTimeOffset?> GetCreationDateAsync(
            OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            if (authorization.CreationDate == null)
            {
                return new ValueTask<DateTimeOffset?>(result: null);
            }

            return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(authorization.CreationDate.Value, DateTimeKind.Utc));
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetIdAsync(OpenIddictAuthorization authorization,
            CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            return new ValueTask<string>(authorization.Id.ToString());
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(
            OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            if (authorization.Properties.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
            }

            return new(authorization.Properties.ToImmutableDictionary());
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableArray<string>> GetScopesAsync(OpenIddictAuthorization authorization,
            CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            if (authorization.Scopes.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            return new(authorization.Scopes.ToImmutableArray());
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetStatusAsync(
            OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            return new ValueTask<string>(authorization.Status);
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetSubjectAsync(
            OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            return new ValueTask<string>(authorization.Subject);
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetTypeAsync(
            OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            return new ValueTask<string>(authorization.Type);
        }

        /// <inheritdoc/>
        public virtual ValueTask<OpenIddictAuthorization> InstantiateAsync(CancellationToken cancellationToken)
        {
            try
            {
                return new ValueTask<OpenIddictAuthorization>(new OpenIddictAuthorization(GuidGenerator.Create()));
            }

            catch (MemberAccessException exception)
            {
                return new ValueTask<OpenIddictAuthorization>(Task.FromException<OpenIddictAuthorization>(
                    new InvalidOperationException(SR.GetResourceString(SR.ID0242), exception)));
            }
        }

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictAuthorization> ListAsync(
            int? count, int? offset, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<TResult> ListAsync<TState, TResult>(
            Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query,
            TState state, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask SetApplicationIdAsync(
            OpenIddictAuthorization authorization,
            string identifier,
            CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            if (!identifier.IsNullOrWhiteSpace())
            {
                var key = ConvertIdentifierFromString(identifier);

                authorization.SetApplicationId(key);
            }
            else
            {
                authorization.SetApplicationId(null);
            }

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetCreationDateAsync(OpenIddictAuthorization authorization,
            DateTimeOffset? date, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            authorization.SetCreationDate(date?.UtcDateTime);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetPropertiesAsync(OpenIddictAuthorization authorization,
            ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            authorization.SetProperties(new Dictionary<string, JsonElement>(properties));

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetScopesAsync(OpenIddictAuthorization authorization,
            ImmutableArray<string> scopes, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            authorization.SetScopes(new HashSet<string>(scopes));

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetStatusAsync(OpenIddictAuthorization authorization,
            string status, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            authorization.SetStatus(status);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetSubjectAsync(OpenIddictAuthorization authorization,
            string subject, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            authorization.SetSubject(subject);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetTypeAsync(OpenIddictAuthorization authorization,
            string type, CancellationToken cancellationToken)
        {
            Check.NotNull(authorization, nameof(authorization));

            authorization.SetType(type);

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
