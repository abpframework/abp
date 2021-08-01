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

namespace Volo.Abp.OpenIddict.Tokens
{
    /// <inheritdoc/>
    public abstract class OpenIddictTokenStoreBase : IOpenIddictTokenStore<OpenIddictToken>
    {
        protected IGuidGenerator GuidGenerator { get; }

        protected OpenIddictTokenStoreBase(
            IGuidGenerator guidGenerator)
        {
            GuidGenerator = guidGenerator;
        }

        /// <inheritdoc/>
        public abstract ValueTask<long> CountAsync(CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictToken>, IQueryable<TResult>> query, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask CreateAsync(OpenIddictToken token, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask DeleteAsync(OpenIddictToken token, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictToken> FindAsync(
            string subject, string client, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictToken> FindAsync(
            string subject, string client,
            string status, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictToken> FindAsync(
            string subject, string client,
            string status, string type, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictToken> FindByApplicationIdAsync(
            string identifier, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictToken> FindByAuthorizationIdAsync(
            string identifier, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<OpenIddictToken> FindByIdAsync(
            string identifier, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<OpenIddictToken> FindByReferenceIdAsync(
            string identifier, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictToken> FindBySubjectAsync(
            string subject, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask<string> GetApplicationIdAsync(
            OpenIddictToken token, CancellationToken cancellationToken)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return new(token.ApplicationId?.ToString());
        }

        /// <inheritdoc/>
        public abstract ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query,
            TState state, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask<string> GetAuthorizationIdAsync(
            OpenIddictToken token,
            CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new(token.AuthorizationId?.ToString());
        }

        /// <inheritdoc/>
        public virtual ValueTask<DateTimeOffset?> GetCreationDateAsync(
            OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (token.CreationDate is null)
            {
                return new ValueTask<DateTimeOffset?>(result: null);
            }

            return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(token.CreationDate.Value, DateTimeKind.Utc));
        }

        /// <inheritdoc/>
        public virtual ValueTask<DateTimeOffset?> GetExpirationDateAsync(
            OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (token.ExpirationDate is null)
            {
                return new ValueTask<DateTimeOffset?>(result: null);
            }

            return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(token.ExpirationDate.Value, DateTimeKind.Utc));
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetIdAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string>(token.Id.ToString());
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetPayloadAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string>(token.Payload);
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(
            OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (token.Properties.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
            }

            return new(token.Properties.ToImmutableDictionary());
        }

        /// <inheritdoc/>
        public virtual ValueTask<DateTimeOffset?> GetRedemptionDateAsync(OpenIddictToken token,
            CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (token.RedemptionDate is null)
            {
                return new ValueTask<DateTimeOffset?>(result: null);
            }

            return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(token.RedemptionDate.Value, DateTimeKind.Utc));
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetReferenceIdAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string>(token.ReferenceId);
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetStatusAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string>(token.Status);
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetSubjectAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string>(token.Subject);
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetTypeAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string>(token.Type);
        }

        /// <inheritdoc/>
        public virtual ValueTask<OpenIddictToken> InstantiateAsync(CancellationToken cancellationToken)
        {
            try
            {
                return new ValueTask<OpenIddictToken>(new OpenIddictToken(GuidGenerator.Create()));
            }
            catch (MemberAccessException exception)
            {
                return new ValueTask<OpenIddictToken>(Task.FromException<OpenIddictToken>(
                    new InvalidOperationException(SR.GetResourceString(SR.ID0248), exception)));
            }
        }

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictToken> ListAsync(
            int? count, int? offset, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<TResult> ListAsync<TState, TResult>(
            Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query,
            TState state, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask SetApplicationIdAsync(OpenIddictToken token,
            string identifier, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (!identifier.IsNullOrWhiteSpace())
            {
                var key = ConvertIdentifierFromString(identifier);
                token.SetApplicationId(key);
            }
            else
            {
                token.SetApplicationId(null);
            }

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetAuthorizationIdAsync(OpenIddictToken token,
            string identifier, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (!identifier.IsNullOrWhiteSpace())
            {
                var key = ConvertIdentifierFromString(identifier);
                token.SetAuthorizationId(key);
            }
            else
            {
                token.SetAuthorizationId(null);
            }

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetCreationDateAsync(OpenIddictToken token,
            DateTimeOffset? date, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.SetCreationDate(date?.UtcDateTime);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetExpirationDateAsync(OpenIddictToken token,
            DateTimeOffset? date, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.SetExpirationDate(date?.UtcDateTime);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetPayloadAsync(OpenIddictToken token,
            string payload, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.SetPayload(payload);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetPropertiesAsync(OpenIddictToken token,
            ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.SetProperties(new Dictionary<string, JsonElement>(properties));

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetRedemptionDateAsync(OpenIddictToken token,
            DateTimeOffset? date, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.SetRedemptionDate(date?.UtcDateTime);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetReferenceIdAsync(OpenIddictToken token,
            string identifier, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.SetReferenceId(identifier);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetStatusAsync(OpenIddictToken token,
            string status, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.SetStatus(status);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetSubjectAsync(OpenIddictToken token,
            string subject, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.SetSubject(subject);

            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetTypeAsync(OpenIddictToken token,
            string type, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.SetType(type);

            return default;
        }

        /// <inheritdoc/>
        public abstract ValueTask UpdateAsync(OpenIddictToken token, CancellationToken cancellationToken);

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
