using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Authorizations;

[ExposeServices(
    typeof(IOpenIddictAuthorizationStore<OpenIddictAuthorization>),
    typeof(AbpOpenIddictAuthorizationStore)
)]
public class AbpOpenIddictAuthorizationStore : AbpOpenIddictStoreBase<IOpenIddictAuthorizationRepository>, IOpenIddictAuthorizationStore<OpenIddictAuthorization>, IScopedDependency
{
    protected IOpenIddictApplicationRepository ApplicationRepository { get; }
    protected IOpenIddictTokenRepository TokenRepository { get; }

    public AbpOpenIddictAuthorizationStore(
        IOpenIddictAuthorizationRepository repository,
        IUnitOfWorkManager unitOfWorkManager,
        IMemoryCache cache,
        IOpenIddictApplicationRepository applicationRepository,
        IOpenIddictTokenRepository tokenRepository)
        : base(repository, unitOfWorkManager, cache)
    {
        ApplicationRepository = applicationRepository;
        TokenRepository = tokenRepository;
    }

    public virtual async ValueTask<long> CountAsync(CancellationToken cancellationToken)
    {
        return await Repository.GetCountAsync(cancellationToken);
    }

    public virtual async ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictAuthorization>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        return await Repository.CountAsync(query, cancellationToken);
    }

    public virtual async ValueTask CreateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        await Repository.InsertAsync(authorization, autoSave: true, cancellationToken: cancellationToken);
    }

    public virtual async ValueTask DeleteAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true, isolationLevel: IsolationLevel.RepeatableRead))
        {
            if (authorization.Tokens.Any())
            {
                await TokenRepository.DeleteManyAsync(authorization.Tokens, cancellationToken: cancellationToken);
            }

            await Repository.DeleteAsync(authorization, cancellationToken: cancellationToken);

            await uow.CompleteAsync(cancellationToken);
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorization> FindAsync(string subject, string client, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));

        var authorizations = await Repository.FindAsync(subject, ConvertIdentifierFromString(client), includeDetails: true, cancellationToken);
        foreach (var authorization in authorizations)
        {
            yield return authorization;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorization> FindAsync(string subject, string client, string status, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));

        var authorizations = await Repository.FindAsync(subject, ConvertIdentifierFromString(client), status, includeDetails: true, cancellationToken);
        foreach (var authorization in authorizations)
        {
            yield return authorization;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorization> FindAsync(string subject, string client, string status, string type, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));
        Check.NotNullOrEmpty(type, nameof(type));

        var authorizations = await Repository.FindAsync(subject, ConvertIdentifierFromString(client), status, type, includeDetails: true, cancellationToken);
        foreach (var authorization in authorizations)
        {
            yield return authorization;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorization> FindAsync(string subject, string client, string status, string type, ImmutableArray<string> scopes, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));
        Check.NotNullOrEmpty(type, nameof(type));

        var authorizations = await Repository.FindAsync(subject, ConvertIdentifierFromString(client), status, type, includeDetails: true, cancellationToken);

        foreach (var authorization in authorizations)
        {
            if (new HashSet<string>(await GetScopesAsync(authorization, cancellationToken), StringComparer.Ordinal).IsSupersetOf(scopes))
            {
                yield return authorization;
            }
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorization> FindByApplicationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        var authorizations = await Repository.FindByApplicationIdAsync(ConvertIdentifierFromString(identifier), includeDetails: true, cancellationToken);
        foreach (var authorization in authorizations)
        {
            yield return authorization;
        }
    }

    public virtual async ValueTask<OpenIddictAuthorization> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return await Repository.FindByIdAsync(ConvertIdentifierFromString(identifier), includeDetails: true, cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorization> FindBySubjectAsync(string subject, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));

        var authorizations = await Repository.FindBySubjectAsync(subject, includeDetails: true, cancellationToken);
        foreach (var authorization in authorizations)
        {
            yield return authorization;
        }
    }

    public virtual ValueTask <string> GetApplicationIdAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        return new ValueTask<string>(authorization.ApplicationId.HasValue
            ? ConvertIdentifierToString(authorization.ApplicationId.Value)
            : null);
    }

    public virtual async ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        return await Repository.GetAsync(query, state, includeDetails: true, cancellationToken);
    }

    public virtual ValueTask <DateTimeOffset?> GetCreationDateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        return authorization.CreationDate is null
            ? new ValueTask<DateTimeOffset?>(result: null)
            : new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(authorization.CreationDate.Value, DateTimeKind.Utc));
    }

    public virtual ValueTask <string> GetIdAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        return new ValueTask<string>(ConvertIdentifierToString(authorization.Id));
    }

    public virtual ValueTask <ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        if (string.IsNullOrEmpty(authorization.Properties))
        {
            return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
        }

        // Note: parsing the stringified properties is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("68056e1a-dbcf-412b-9a6a-d791c7dbe726", "\x1e", authorization.Properties);
        var properties = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(authorization.Properties);
            var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                builder[property.Name] = property.Value.Clone();
            }

            return builder.ToImmutable();
        });

        return new ValueTask<ImmutableDictionary<string, JsonElement>>(properties);
    }

    public virtual ValueTask <ImmutableArray<string>> GetScopesAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        if (string.IsNullOrEmpty(authorization.Scopes))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        // Note: parsing the stringified scopes is an expensive operation.
        // To mitigate that, the resulting array is stored in the memory cache.
        var key = string.Concat("2ba4ab0f-e2ec-4d48-b3bd-28e2bb660c75", "\x1e", authorization.Scopes);
        var scopes = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(authorization.Scopes);
            var builder = ImmutableArray.CreateBuilder<string>(document.RootElement.GetArrayLength());

            foreach (var element in document.RootElement.EnumerateArray())
            {
                var value = element.GetString();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                builder.Add(value);
            }

            return builder.ToImmutable();
        });

        return new ValueTask<ImmutableArray<string>>(scopes);
    }

    public virtual ValueTask <string> GetStatusAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        return new ValueTask<string>(authorization.Status);
    }

    public virtual ValueTask <string> GetSubjectAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        return new ValueTask<string>(authorization.Subject);
    }

    public virtual ValueTask <string> GetTypeAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        return new ValueTask<string>(authorization.Type);
    }

    public virtual ValueTask <OpenIddictAuthorization> InstantiateAsync(CancellationToken cancellationToken)
    {
        try
        {
            return new ValueTask<OpenIddictAuthorization>(Activator.CreateInstance<OpenIddictAuthorization>());
        }
        catch (MemberAccessException exception)
        {
            return new ValueTask<OpenIddictAuthorization>(Task.FromException<OpenIddictAuthorization>(exception));
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorization> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var authorizations = await Repository.ListAsync(count, offset, includeDetails: true, cancellationToken);
        foreach (var authorization in authorizations)
        {
            yield return authorization;
        }
    }

    public virtual async IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var authorizations = await Repository.ListAsync(query, state, includeDetails: true, cancellationToken);
        foreach (var authorization in authorizations)
        {
            yield return authorization;
        }
    }

    public virtual async ValueTask PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
    {
        for (var index = 0; index < 1_000; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true, isolationLevel: IsolationLevel.RepeatableRead))
            {
                var date = threshold.UtcDateTime;

                var authorizations = await Repository.GetPruneListAsync(date, 1_000, includeDetails: false, cancellationToken);
                if (!authorizations.Any())
                {
                    break;
                }

                await Repository.DeleteManyAsync(authorizations, autoSave: true, cancellationToken: cancellationToken);
                await uow.CompleteAsync(cancellationToken);
            }
        }
    }

    public virtual async ValueTask SetApplicationIdAsync(OpenIddictAuthorization authorization, string identifier, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        if (!string.IsNullOrEmpty(identifier))
        {
            var application = await ApplicationRepository.GetAsync(ConvertIdentifierFromString(identifier), cancellationToken: cancellationToken);
            authorization.ApplicationId = application.Id;
        }
        else
        {
            authorization.ApplicationId = null;
        }
    }

    public virtual ValueTask SetCreationDateAsync(OpenIddictAuthorization authorization, DateTimeOffset? date, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        authorization.CreationDate = date?.UtcDateTime;

        return default;
    }

    public virtual ValueTask SetPropertiesAsync(OpenIddictAuthorization authorization, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
    {
        if (properties is null || properties.IsEmpty)
        {
            authorization.Properties = null;
            return default;
        }

        authorization.Properties = WriteStream(writer =>
        {
            writer.WriteStartObject();
            foreach (var property in properties)
            {
                writer.WritePropertyName(property.Key);
                property.Value.WriteTo(writer);
            }
            writer.WriteEndObject();
        });

        return default;
    }

    public virtual ValueTask SetScopesAsync(OpenIddictAuthorization authorization, ImmutableArray<string> scopes, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        if (scopes.IsDefaultOrEmpty)
        {
            authorization.Scopes = null;
            return default;
        }

        authorization.Scopes = WriteStream(writer =>
        {
            writer.WriteStartArray();
            foreach (var scope in scopes)
            {
                writer.WriteStringValue(scope);
            }
            writer.WriteEndArray();
        });

        return default;
    }

    public virtual ValueTask SetStatusAsync(OpenIddictAuthorization authorization, string status, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        authorization.Status = status;

        return default;
    }

    public virtual ValueTask SetSubjectAsync(OpenIddictAuthorization authorization, string subject, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        authorization.Subject = subject;

        return default;
    }

    public virtual ValueTask SetTypeAsync(OpenIddictAuthorization authorization, string type, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        authorization.Type = type;

        return default;
    }

    public virtual async ValueTask UpdateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        await Repository.UpdateAsync(authorization, autoSave: true, cancellationToken: cancellationToken);
    }
}
