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
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Tokens;

//https://github.com/abpframework/abp/pull/12094
[ExposeServices(
    typeof(IOpenIddictTokenStore<OpenIddictToken>),
    typeof(OpenIddictTokenStore)
)]
public class OpenIddictTokenStore : OpenIddictStoreBase<IOpenIddictTokenRepository>, IOpenIddictTokenStore<OpenIddictToken>, IScopedDependency
{
    protected IOpenIddictApplicationRepository ApplicationRepository { get; }
    protected IOpenIddictAuthorizationRepository AuthorizationRepository { get; }

    public OpenIddictTokenStore(
        IOpenIddictTokenRepository repository,
        IUnitOfWorkManager unitOfWorkManager,
        IMemoryCache cache,
        IOpenIddictApplicationRepository applicationRepository,
        IOpenIddictAuthorizationRepository authorizationRepository)
        : base(repository, unitOfWorkManager, cache)
    {
        ApplicationRepository = applicationRepository;
        AuthorizationRepository = authorizationRepository;
    }

    public virtual async ValueTask<long> CountAsync(CancellationToken cancellationToken)
    {
        return await Repository.GetCountAsync(cancellationToken);
    }

    public virtual async ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictToken>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        return await Repository.CountAsync(query, cancellationToken);
    }

    public virtual async ValueTask CreateAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        await Repository.InsertAsync(token, autoSave: true, cancellationToken: cancellationToken);
    }

    public virtual async ValueTask DeleteAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        await Repository.DeleteAsync(token, autoSave: true, cancellationToken: cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictToken> FindAsync(string subject, string client, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));

        var tokens = await Repository.FindAsync(subject, ConvertIdentifierFromString(client), cancellationToken);
        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictToken> FindAsync(string subject, string client, string status, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));

        var tokens = await Repository.FindAsync(subject, ConvertIdentifierFromString(client), status, cancellationToken);
        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictToken> FindAsync(string subject, string client, string status, string type, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));
        Check.NotNullOrEmpty(type, nameof(type));

        var tokens = await Repository.FindAsync(subject, ConvertIdentifierFromString(client), status, type, cancellationToken);
        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictToken> FindByApplicationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        var tokens = await Repository.FindByApplicationIdAsync(ConvertIdentifierFromString(identifier), cancellationToken);
        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictToken> FindByAuthorizationIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        var tokens = await Repository.FindByAuthorizationIdAsync(ConvertIdentifierFromString(identifier), cancellationToken);
        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual async ValueTask<OpenIddictToken> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return await Repository.FindByIdAsync(ConvertIdentifierFromString(identifier), cancellationToken);
    }

    public virtual async ValueTask<OpenIddictToken> FindByReferenceIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return await Repository.FindByReferenceIdAsync(identifier, cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictToken> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));

        var tokens = await Repository.FindBySubjectAsync(subject, cancellationToken);
        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual ValueTask<string> GetApplicationIdAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        return new ValueTask<string>(token.ApplicationId.HasValue
            ? ConvertIdentifierToString(token.ApplicationId.Value)
            : null);
    }

    public virtual async ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        return  await Repository.GetAsync(query, state, cancellationToken);
    }

    public virtual ValueTask<string> GetAuthorizationIdAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        return new ValueTask<string>(token.AuthorizationId.HasValue
            ? ConvertIdentifierToString(token.AuthorizationId.Value)
            : null);
    }

    public virtual ValueTask<DateTimeOffset?> GetCreationDateAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        if (token.CreationDate is null)
        {
            return new ValueTask<DateTimeOffset?>(result: null);
        }

        return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(token.CreationDate.Value, DateTimeKind.Utc));
    }

    public virtual ValueTask<DateTimeOffset?> GetExpirationDateAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        if (token.ExpirationDate is null)
        {
            return new ValueTask<DateTimeOffset?>(result: null);
        }

        return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(token.ExpirationDate.Value, DateTimeKind.Utc));
    }

    public virtual ValueTask<string> GetIdAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        return new ValueTask<string>(ConvertIdentifierToString(token.Id));
    }

    public virtual ValueTask<string> GetPayloadAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        return new ValueTask<string>(token.Payload);
    }

    public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        if (string.IsNullOrEmpty(token.Properties))
        {
            return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
        }

        // Note: parsing the stringified properties is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("d0509397-1bbf-40e7-97e1-5e6d7bc2536c", "\x1e", token.Properties);
        var properties = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(token.Properties);
            var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                builder[property.Name] = property.Value.Clone();
            }

            return builder.ToImmutable();
        });

        return new ValueTask<ImmutableDictionary<string, JsonElement>>(properties);
    }

    public virtual ValueTask<DateTimeOffset?> GetRedemptionDateAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        if (token.RedemptionDate is null)
        {
            return new ValueTask<DateTimeOffset?>(result: null);
        }

        return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(token.RedemptionDate.Value, DateTimeKind.Utc));
    }

    public virtual ValueTask<string> GetReferenceIdAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        return new ValueTask<string>(token.ReferenceId);
    }

    public virtual ValueTask<string> GetStatusAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        return new ValueTask<string>(token.Status);
    }

    public virtual ValueTask<string> GetSubjectAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        return new ValueTask<string>(token.Subject);
    }

    public virtual ValueTask<string> GetTypeAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        return new ValueTask<string>(token.Type);
    }

    public virtual ValueTask<OpenIddictToken> InstantiateAsync(CancellationToken cancellationToken)
    {
        try
        {
            return new ValueTask<OpenIddictToken>(Activator.CreateInstance<OpenIddictToken>());
        }
        catch (MemberAccessException exception)
        {
            return new ValueTask<OpenIddictToken>(Task.FromException<OpenIddictToken>(exception));
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictToken> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var tokens = await Repository.ListAsync(count, offset, cancellationToken);
        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual async IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        var tokens = await Repository.ListAsync(query, state, cancellationToken);
        foreach (var token in tokens)
        {
            yield return token;
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

                var tokens = await Repository.GetPruneListAsync(date, 1_000, cancellationToken);
                if (!tokens.Any())
                {
                    break;
                }

                await Repository.DeleteManyAsync(tokens, autoSave: true, cancellationToken: cancellationToken);
                await uow.CompleteAsync(cancellationToken);
            }
        }
    }

    public virtual async ValueTask SetApplicationIdAsync(OpenIddictToken token, string identifier, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        if (!string.IsNullOrEmpty(identifier))
        {
            var application = await ApplicationRepository.GetAsync(ConvertIdentifierFromString(identifier), cancellationToken: cancellationToken);
            token.ApplicationId = application.Id;
        }
        else
        {
            token.ApplicationId = null;
        }
    }

    public virtual async ValueTask SetAuthorizationIdAsync(OpenIddictToken token, string identifier, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        if (!string.IsNullOrEmpty(identifier))
        {
            var authorization = await AuthorizationRepository.GetAsync(ConvertIdentifierFromString(identifier), cancellationToken: cancellationToken);
            token.AuthorizationId = authorization.Id;
        }
        else
        {
            token.AuthorizationId = null;
        }
    }

    public virtual ValueTask SetCreationDateAsync(OpenIddictToken token, DateTimeOffset? date, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        token.CreationDate = date?.UtcDateTime;

        return default;
    }

    public virtual ValueTask SetExpirationDateAsync(OpenIddictToken token, DateTimeOffset? date, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        token.ExpirationDate = date?.UtcDateTime;

        return default;
    }

    public virtual ValueTask SetPayloadAsync(OpenIddictToken token, string payload, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        token.Payload = payload;

        return default;
    }

    public virtual ValueTask SetPropertiesAsync(OpenIddictToken token, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        if (properties is null || properties.IsEmpty)
        {
            token.Properties = null;
            return default;
        }

        token.Properties  = WriteStream(writer =>
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

    public virtual ValueTask SetRedemptionDateAsync(OpenIddictToken token, DateTimeOffset? date, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        token.RedemptionDate = date?.UtcDateTime;

        return default;
    }

    public virtual ValueTask SetReferenceIdAsync(OpenIddictToken token, string identifier, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        token.ReferenceId = identifier;

        return default;
    }

    public virtual ValueTask SetStatusAsync(OpenIddictToken token, string status, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        token.Status = status;

        return default;
    }

    public virtual ValueTask SetSubjectAsync(OpenIddictToken token, string subject, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        token.Subject = subject;

        return default;
    }

    public virtual ValueTask SetTypeAsync(OpenIddictToken token, string type, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        token.Type = type;

        return default;
    }

    public virtual async ValueTask UpdateAsync(OpenIddictToken token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        await Repository.UpdateAsync(token, autoSave: true, cancellationToken: cancellationToken);
    }
}
