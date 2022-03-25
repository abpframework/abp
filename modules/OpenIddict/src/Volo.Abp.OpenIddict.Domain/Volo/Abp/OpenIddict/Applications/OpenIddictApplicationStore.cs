using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Applications;

public class OpenIddictApplicationStore : OpenIddictStoreBase<IOpenIddictApplicationRepository>, IOpenIddictApplicationStore<OpenIddictApplication>, IScopedDependency
{
    public OpenIddictApplicationStore(
        IOpenIddictApplicationRepository repository,
        IUnitOfWorkManager unitOfWorkManager,
        IMemoryCache cache)
        : base(repository, unitOfWorkManager, cache)
    {

    }

    public async ValueTask<long> CountAsync(CancellationToken cancellationToken)
    {
        return await Repository.GetCountAsync(cancellationToken);
    }

    public async ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictApplication>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        return await Repository.CountAsync(query, cancellationToken);
    }

    public async ValueTask CreateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        await Repository.InsertAsync(application, cancellationToken: cancellationToken);
    }

    public async ValueTask DeleteAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        await Repository.DeleteAsync(application, cancellationToken: cancellationToken);
    }

    public async ValueTask<OpenIddictApplication> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return await Repository.FindAsync(ConvertIdentifierFromString(identifier), cancellationToken: cancellationToken);
    }

    public async ValueTask<OpenIddictApplication> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return await Repository.FindByClientIdAsync(identifier, cancellationToken: cancellationToken);
    }

    public async IAsyncEnumerable<OpenIddictApplication> FindByPostLogoutRedirectUriAsync(string address, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(address, nameof(address));

        var applications = await Repository.FindByPostLogoutRedirectUriAsync(address, cancellationToken);

        foreach (var application in applications)
        {
            var addresses = await GetPostLogoutRedirectUrisAsync(application, cancellationToken);
            if (addresses.Contains(address, StringComparer.Ordinal))
            {
                yield return application;
            }
        }
    }

    public async IAsyncEnumerable<OpenIddictApplication> FindByRedirectUriAsync(string address, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(address, nameof(address));

        var applications = await Repository.FindByRedirectUriAsync(address, cancellationToken);
        foreach (var application in applications)
        {
            var addresses = await GetRedirectUrisAsync(application, cancellationToken);
            if (addresses.Contains(address, StringComparer.Ordinal))
            {
                yield return application;
            }
        }
    }

    public async ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        return await Repository.GetAsync(query, state, cancellationToken);
    }

    public ValueTask<string> GetClientIdAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ClientId);
    }

    public ValueTask<string> GetClientSecretAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ClientSecret);
    }

    public ValueTask<string> GetClientTypeAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));
        return new ValueTask<string>(application.Type);
    }

    public ValueTask<string> GetConsentTypeAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ConsentType);
    }

    public ValueTask<string> GetDisplayNameAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.DisplayName);
    }

    public ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.DisplayNames))
        {
            return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
        }

        // Note: parsing the stringified display names is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("7762c378-c113-4564-b14b-1402b3949aaa", "\x1e", application.DisplayNames);
        var names = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(application.DisplayNames);
            var builder = ImmutableDictionary.CreateBuilder<CultureInfo, string>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                var value = property.Value.GetString();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                builder[CultureInfo.GetCultureInfo(property.Name)] = value;
            }

            return builder.ToImmutable();
        });

        return new ValueTask<ImmutableDictionary<CultureInfo, string>>(names);
    }

    public ValueTask<string> GetIdAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(ConvertIdentifierToString(application.Id));
    }

    public ValueTask<ImmutableArray<string>> GetPermissionsAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.Permissions))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        // Note: parsing the stringified permissions is an expensive operation.
        // To mitigate that, the resulting array is stored in the memory cache.
        var key = string.Concat("0347e0aa-3a26-410a-97e8-a83bdeb21a1f", "\x1e", application.Permissions);
        var permissions = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(application.Permissions);
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

        return new ValueTask<ImmutableArray<string>>(permissions);
    }

    public ValueTask<ImmutableArray<string>> GetPostLogoutRedirectUrisAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.PostLogoutRedirectUris))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        // Note: parsing the stringified addresses is an expensive operation.
        // To mitigate that, the resulting array is stored in the memory cache.
        var key = string.Concat("fb14dfb9-9216-4b77-bfa9-7e85f8201ff4", "\x1e", application.PostLogoutRedirectUris);
        var addresses = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(application.PostLogoutRedirectUris);
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

        return new ValueTask<ImmutableArray<string>>(addresses);
    }

    public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.Properties))
        {
            return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
        }

        // Note: parsing the stringified properties is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("2e3e9680-5654-48d8-a27d-b8bb4f0f1d50", "\x1e", application.Properties);
        var properties = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(application.Properties);
            var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                builder[property.Name] = property.Value.Clone();
            }

            return builder.ToImmutable();
        });

        return new ValueTask<ImmutableDictionary<string, JsonElement>>(properties);
    }

    public ValueTask<ImmutableArray<string>> GetRedirectUrisAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.RedirectUris))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        // Note: parsing the stringified addresses is an expensive operation.
        // To mitigate that, the resulting array is stored in the memory cache.
        var key = string.Concat("851d6f08-2ee0-4452-bbe5-ab864611ecaa", "\x1e", application.RedirectUris);
        var addresses = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(application.RedirectUris);
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

        return new ValueTask<ImmutableArray<string>>(addresses);
    }

    public ValueTask<ImmutableArray<string>> GetRequirementsAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.Requirements))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        // Note: parsing the stringified requirements is an expensive operation.
        // To mitigate that, the resulting array is stored in the memory cache.
        var key = string.Concat("b4808a89-8969-4512-895f-a909c62a8995", "\x1e", application.Requirements);
        var requirements = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(application.Requirements);
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

        return new ValueTask<ImmutableArray<string>>(requirements);
    }

    public ValueTask<OpenIddictApplication> InstantiateAsync(CancellationToken cancellationToken)
    {
        try
        {
            return new ValueTask<OpenIddictApplication>(Activator.CreateInstance<OpenIddictApplication>());
        }
        catch (MemberAccessException exception)
        {
            return new ValueTask<OpenIddictApplication>(Task.FromException<OpenIddictApplication>(exception));
        }
    }

    public async IAsyncEnumerable<OpenIddictApplication> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var applications = await Repository.ListAsync(count, offset, cancellationToken);
        foreach (var application in applications)
        {
            yield return application;
        }
    }

    public async IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        var applications = await Repository.ListAsync(query, state, cancellationToken);
        foreach (var application in applications)
        {
            yield return application;
        }
    }

    public virtual ValueTask SetClientIdAsync(OpenIddictApplication application, string identifier, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.ClientId = identifier;
        return default;
    }

    public virtual ValueTask SetClientSecretAsync(OpenIddictApplication application, string secret, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.ClientSecret = secret;
        return default;
    }

    public virtual ValueTask SetClientTypeAsync(OpenIddictApplication application, string type, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.Type = type;
        return default;
    }

    public virtual ValueTask SetConsentTypeAsync(OpenIddictApplication application, string type, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.ConsentType = type;
        return default;
    }

    public virtual ValueTask SetDisplayNameAsync(OpenIddictApplication application, string name, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.DisplayName = name;
        return default;
    }

    public virtual ValueTask SetDisplayNamesAsync(OpenIddictApplication application, ImmutableDictionary<CultureInfo, string> names,
        CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (names is null || names.IsEmpty)
        {
            application.DisplayNames = null;
            return default;
        }

        application.DisplayNames = WriteStream(writer =>
        {
            writer.WriteStartObject();
            foreach (var pair in names)
            {
                writer.WritePropertyName(pair.Key.Name);
                writer.WriteStringValue(pair.Value);
            }
            writer.WriteEndObject();
        });

        return default;
    }

    public virtual ValueTask SetPermissionsAsync(OpenIddictApplication application, ImmutableArray<string> permissions,
        CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (permissions.IsDefaultOrEmpty)
        {
            application.Permissions = null;
            return default;
        }

        application.Permissions = WriteStream(writer =>
        {
            writer.WriteStartArray();
            foreach (var permission in permissions)
            {
                writer.WriteStringValue(permission);
            }
            writer.WriteEndArray();
        });

        return default;
    }

    public virtual ValueTask SetPostLogoutRedirectUrisAsync(OpenIddictApplication application, ImmutableArray<string> addresses,
        CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (addresses.IsDefaultOrEmpty)
        {
            application.PostLogoutRedirectUris = null;
            return default;
        }

        application.PostLogoutRedirectUris = WriteStream(writer =>
        {
            writer.WriteStartArray();
            foreach (var address in addresses)
            {
                writer.WriteStringValue(address);
            }
            writer.WriteEndArray();
        });

        return default;
    }

    public virtual ValueTask SetPropertiesAsync(OpenIddictApplication application, ImmutableDictionary<string, JsonElement> properties,
        CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (properties is null || properties.IsEmpty)
        {
            application.Properties = null;
            return default;
        }

        application.Properties = WriteStream(writer =>
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

    public virtual ValueTask SetRedirectUrisAsync(OpenIddictApplication application, ImmutableArray<string> addresses,
        CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (addresses.IsDefaultOrEmpty)
        {
            application.RedirectUris = null;
            return default;
        }

        application.RedirectUris = WriteStream(writer =>
        {
            writer.WriteStartArray();
            foreach (var address in addresses)
            {
                writer.WriteStringValue(address);
            }
            writer.WriteEndArray();
        });

        return default;
    }

    public virtual ValueTask SetRequirementsAsync(OpenIddictApplication application, ImmutableArray<string> requirements,
        CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (requirements.IsDefaultOrEmpty)
        {
            application.Requirements = null;
            return default;
        }

        application.Requirements = WriteStream(writer =>
        {
            writer.WriteStartArray();
            foreach (var requirement in requirements)
            {
                writer.WriteStringValue(requirement);
            }
            writer.WriteEndArray();
        });

        return default;
    }

    public virtual async ValueTask UpdateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        await Repository.UpdateAsync(application, autoSave: true, cancellationToken: cancellationToken);
    }
}
