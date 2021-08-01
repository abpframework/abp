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

namespace Volo.Abp.OpenIddict.Applications
{
    /// <inheritdoc/>
    public abstract class OpenIddictApplicationStoreBase : IOpenIddictApplicationStore<OpenIddictApplication>
    {
        protected IGuidGenerator GuidGenerator { get; }

        protected OpenIddictApplicationStoreBase(
            IGuidGenerator guidGenerator)
        {
            GuidGenerator = guidGenerator;
        }

        /// <inheritdoc/>
        public abstract ValueTask<long> CountAsync(CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<long> CountAsync<TResult>(
            Func<IQueryable<OpenIddictApplication>, IQueryable<TResult>> query, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask CreateAsync(OpenIddictApplication application, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask UpdateAsync(OpenIddictApplication application, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask DeleteAsync(OpenIddictApplication application, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<OpenIddictApplication> FindByClientIdAsync(
            string identifier,
            CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<OpenIddictApplication> FindByIdAsync(
            string identifier,
            CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictApplication> FindByPostLogoutRedirectUriAsync(
            string address,
            CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictApplication> FindByRedirectUriAsync(
            string address,
            CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract ValueTask<TResult> GetAsync<TState, TResult>(
            Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query,
            TState state, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask<string> GetClientIdAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            return new ValueTask<string>(application.ClientId);
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetClientSecretAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            return new ValueTask<string>(application.ClientSecret);
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetClientTypeAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            return new ValueTask<string>(application.Type);
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetConsentTypeAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            return new ValueTask<string>(application.ConsentType);
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetDisplayNameAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            return new ValueTask<string>(application.DisplayName);
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            if (application.DisplayNames.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
            }

            return new(application.DisplayNames.ToImmutableDictionary());
        }

        /// <inheritdoc/>
        public virtual ValueTask<string> GetIdAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            return new ValueTask<string>(application.Id.ToString());
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableArray<string>> GetPermissionsAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            if (application.Permissions.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }
            return new(application.Permissions.ToImmutableArray());
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableArray<string>> GetPostLogoutRedirectUrisAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            if (application.PostLogoutRedirectUris.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            return new(application.PostLogoutRedirectUris.ToImmutableArray());
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            if (application.Properties.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
            }

            return new(application.Properties.ToImmutableDictionary());
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableArray<string>> GetRedirectUrisAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            if (application.RedirectUris.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            return new(application.RedirectUris.ToImmutableArray());
        }

        /// <inheritdoc/>
        public virtual ValueTask<ImmutableArray<string>> GetRequirementsAsync(
            OpenIddictApplication application,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            if (application.Requirements.IsNullOrEmpty())
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            return new(application.Requirements.ToImmutableArray());
        }

        /// <inheritdoc/>
        public virtual ValueTask<OpenIddictApplication> InstantiateAsync(CancellationToken cancellationToken)
        {
            try
            {
                return new ValueTask<OpenIddictApplication>(
                    new OpenIddictApplication(GuidGenerator.Create(), GuidGenerator.Create().ToString())
                    );
            }
            catch (MemberAccessException exception)
            {
                return new ValueTask<OpenIddictApplication>(Task.FromException<OpenIddictApplication>(
                    new InvalidOperationException(SR.GetResourceString(SR.ID0240), exception)));
            }
        }

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<OpenIddictApplication> ListAsync(
            int? count, int? offset, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract IAsyncEnumerable<TResult> ListAsync<TState, TResult>(
            Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query,
            TState state, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public virtual ValueTask SetClientIdAsync(
            OpenIddictApplication application,
            string identifier,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetClientId(identifier);
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetClientSecretAsync(
            OpenIddictApplication application,
            string secret,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetClientSecret(secret);
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetClientTypeAsync(
            OpenIddictApplication application,
            string type,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetClientType(type);
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetConsentTypeAsync(
            OpenIddictApplication application,
            string type,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetConsentType(type);
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetDisplayNameAsync(
            OpenIddictApplication application,
            string name,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetDisplayName(name);
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetDisplayNamesAsync(
            OpenIddictApplication application,
            ImmutableDictionary<CultureInfo, string> names,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetDisplayNames(new Dictionary<CultureInfo, string>(names));
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetPermissionsAsync(
            OpenIddictApplication application,
            ImmutableArray<string> permissions,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetPermissions(new HashSet<string>(permissions));
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetPostLogoutRedirectUrisAsync(
            OpenIddictApplication application,
            ImmutableArray<string> addresses,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetPostLogoutRedirectUris(new HashSet<string>(addresses));
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetPropertiesAsync(
            OpenIddictApplication application,
            ImmutableDictionary<string, JsonElement> properties,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetProperties(new Dictionary<string, JsonElement>(properties));
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetRedirectUrisAsync(
            OpenIddictApplication application,
            ImmutableArray<string> addresses,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetRedirectUris(new HashSet<string>(addresses));
            return default;
        }

        /// <inheritdoc/>
        public virtual ValueTask SetRequirementsAsync(
            OpenIddictApplication application,
            ImmutableArray<string> requirements,
            CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));

            application.SetRequirements(new HashSet<string>(requirements));
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