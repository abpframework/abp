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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpOpenIddictApplicationStore : AbpOpenIddictStoreBase<IOpenIddictApplicationRepository>, IAbpOpenIdApplicationStore
{
    protected IOpenIddictTokenRepository TokenRepository { get; }

    public AbpOpenIddictApplicationStore(
        IOpenIddictApplicationRepository repository,
        IUnitOfWorkManager unitOfWorkManager,
        IOpenIddictTokenRepository tokenRepository,
        IGuidGenerator guidGenerator,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IOpenIddictDbConcurrencyExceptionHandler concurrencyExceptionHandler,
        IOptions<AbpOpenIddictStoreOptions> storeOptions)
        : base(repository, unitOfWorkManager, guidGenerator, identifierConverter, concurrencyExceptionHandler, storeOptions)
    {
        TokenRepository = tokenRepository;
    }

    public virtual async ValueTask<long> CountAsync(CancellationToken cancellationToken)
    {
        return await Repository.GetCountAsync(cancellationToken);
    }

    public virtual ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictApplicationModel>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public virtual async ValueTask CreateAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        await Repository.InsertAsync(application.ToEntity(), autoSave: true, cancellationToken: cancellationToken);

        application = (await Repository.FindAsync(application.Id, cancellationToken: cancellationToken)).ToModel();
    }

    public virtual async ValueTask DeleteAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        try
        {
            using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true, isolationLevel: StoreOptions.Value.DeleteIsolationLevel))
            {
                await TokenRepository.DeleteManyByApplicationIdAsync(application.Id, cancellationToken: cancellationToken);

                await Repository.DeleteAsync(application.Id, cancellationToken: cancellationToken);

                await uow.CompleteAsync(cancellationToken);
            }
        }
        catch (AbpDbConcurrencyException e)
        {
            Logger.LogException(e);
            await ConcurrencyExceptionHandler.HandleAsync(e);
            throw new OpenIddictExceptions.ConcurrencyException(e.Message, e.InnerException);
        }
    }

    public virtual async ValueTask<OpenIddictApplicationModel> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return (await Repository.FindAsync(ConvertIdentifierFromString(identifier),  cancellationToken: cancellationToken)).ToModel();
    }

    public virtual async ValueTask<OpenIddictApplicationModel> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return (await Repository.FindByClientIdAsync(identifier, cancellationToken: cancellationToken)).ToModel();
    }

    public virtual async IAsyncEnumerable<OpenIddictApplicationModel> FindByPostLogoutRedirectUriAsync(string uris, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(uris, nameof(uris));

        var applications = await Repository.FindByPostLogoutRedirectUriAsync(uris, cancellationToken);

        foreach (var application in applications)
        {
            var addresses = await GetPostLogoutRedirectUrisAsync(application.ToModel(), cancellationToken);
            if (addresses.Contains(uris, StringComparer.Ordinal))
            {
                yield return application.ToModel();
            }
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictApplicationModel> FindByRedirectUriAsync(string uri, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(uri, nameof(uri));

        var applications = await Repository.FindByRedirectUriAsync(uri, cancellationToken);
        foreach (var application in applications)
        {
            var uris = await GetRedirectUrisAsync(application.ToModel(), cancellationToken);
            if (uris.Contains(uri, StringComparer.Ordinal))
            {
                yield return application.ToModel();
            }
        }
    }

    public virtual ValueTask<string> GetApplicationTypeAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ApplicationType);
    }

    public virtual ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictApplicationModel>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public virtual ValueTask<string> GetClientIdAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ClientId);
    }

    public virtual ValueTask<string> GetClientSecretAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ClientSecret);
    }

    public virtual ValueTask<string> GetClientTypeAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));
        return new ValueTask<string>(application.ClientType);
    }

    public virtual ValueTask<string> GetConsentTypeAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ConsentType);
    }

    public virtual ValueTask<string> GetDisplayNameAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.DisplayName);
    }

    public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.DisplayNames))
        {
            return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
        }

        using (var document = JsonDocument.Parse(application.DisplayNames))
        {
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

            return new ValueTask<ImmutableDictionary<CultureInfo, string>>(builder.ToImmutable());
        }
    }

    public virtual ValueTask<string> GetIdAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(ConvertIdentifierToString(application.Id));
    }

    public virtual ValueTask<JsonWebKeySet> GetJsonWebKeySetAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<JsonWebKeySet>(application.JsonWebKeySet);
    }

    public virtual ValueTask<ImmutableArray<string>> GetPermissionsAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.Permissions))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        using (var document = JsonDocument.Parse(application.Permissions))
        {
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

            return new ValueTask<ImmutableArray<string>>(builder.ToImmutable());
        }
    }

    public virtual ValueTask<ImmutableArray<string>> GetPostLogoutRedirectUrisAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.PostLogoutRedirectUris))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        using (var document = JsonDocument.Parse(application.PostLogoutRedirectUris))
        {
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

            return new ValueTask<ImmutableArray<string>>(builder.ToImmutable());
        };
    }

    public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.Properties))
        {
            return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
        }

        using (var document = JsonDocument.Parse(application.Properties))
        {
            var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                builder[property.Name] = property.Value.Clone();
            }
            return new ValueTask<ImmutableDictionary<string, JsonElement>>(builder.ToImmutable());
        }
    }

    public virtual ValueTask<ImmutableArray<string>> GetRedirectUrisAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.RedirectUris))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        using (var document = JsonDocument.Parse(application.RedirectUris))
        {
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

            return new ValueTask<ImmutableArray<string>>( builder.ToImmutable());
        }
    }

    public virtual ValueTask<ImmutableArray<string>> GetRequirementsAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.Requirements))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        using (var document = JsonDocument.Parse(application.Requirements))
        {
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

            return new ValueTask<ImmutableArray<string>>(builder.ToImmutable());
        }
    }

    public virtual ValueTask<ImmutableDictionary<string, string>> GetSettingsAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (string.IsNullOrEmpty(application.Settings))
        {
            return new ValueTask<ImmutableDictionary<string, string>>(ImmutableDictionary.Create<string, string>());
        }

        using (var document = JsonDocument.Parse(application.Settings))
        {
            var builder = ImmutableDictionary.CreateBuilder<string, string>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                var value = property.Value.GetString();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                builder[property.Name] = value;
            }

            return new ValueTask<ImmutableDictionary<string, string>>(builder.ToImmutable());
        }
    }

    public virtual ValueTask<OpenIddictApplicationModel> InstantiateAsync(CancellationToken cancellationToken)
    {
        return new ValueTask<OpenIddictApplicationModel>(new OpenIddictApplicationModel
        {
            Id = GuidGenerator.Create()
        });
    }

    public virtual async IAsyncEnumerable<OpenIddictApplicationModel> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var applications = await Repository.ListAsync(count, offset, cancellationToken);
        foreach (var application in applications)
        {
            yield return application.ToModel();
        }
    }

    public virtual IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictApplicationModel>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public virtual ValueTask SetApplicationTypeAsync(OpenIddictApplicationModel application, string type, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.ApplicationType = type;
        return default;
    }

    public virtual ValueTask SetClientIdAsync(OpenIddictApplicationModel application, string identifier, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.ClientId = identifier;
        return default;
    }

    public virtual ValueTask SetClientSecretAsync(OpenIddictApplicationModel application, string secret, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.ClientSecret = secret;
        return default;
    }

    public virtual ValueTask SetClientTypeAsync(OpenIddictApplicationModel application, string type, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.ClientType = type;
        return default;
    }

    public virtual ValueTask SetConsentTypeAsync(OpenIddictApplicationModel application, string type, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.ConsentType = type;
        return default;
    }

    public virtual ValueTask SetDisplayNameAsync(OpenIddictApplicationModel application, string name, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.DisplayName = name;
        return default;
    }

    public virtual ValueTask SetDisplayNamesAsync(OpenIddictApplicationModel application, ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken)
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

    public virtual ValueTask SetJsonWebKeySetAsync(OpenIddictApplicationModel application, JsonWebKeySet set, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        application.JsonWebKeySet = set;
        return default;
    }

    public virtual ValueTask SetPermissionsAsync(OpenIddictApplicationModel application, ImmutableArray<string> permissions,
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

    public virtual ValueTask SetPostLogoutRedirectUrisAsync(OpenIddictApplicationModel application, ImmutableArray<string> uris,
        CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (uris.IsDefaultOrEmpty)
        {
            application.PostLogoutRedirectUris = null;
            return default;
        }

        application.PostLogoutRedirectUris = WriteStream(writer =>
        {
            writer.WriteStartArray();
            foreach (var uri in uris)
            {
                writer.WriteStringValue(uri);
            }
            writer.WriteEndArray();
        });

        return default;
    }

    public virtual ValueTask SetPropertiesAsync(OpenIddictApplicationModel application, ImmutableDictionary<string, JsonElement> properties,
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

    public virtual ValueTask SetRedirectUrisAsync(OpenIddictApplicationModel application, ImmutableArray<string> uris,
        CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (uris.IsDefaultOrEmpty)
        {
            application.RedirectUris = null;
            return default;
        }

        application.RedirectUris = WriteStream(writer =>
        {
            writer.WriteStartArray();
            foreach (var uri in uris)
            {
                writer.WriteStringValue(uri);
            }
            writer.WriteEndArray();
        });

        return default;
    }

    public virtual ValueTask SetRequirementsAsync(OpenIddictApplicationModel application, ImmutableArray<string> requirements,
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

    public virtual ValueTask SetSettingsAsync(OpenIddictApplicationModel application, ImmutableDictionary<string, string> settings, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        if (settings.IsEmpty)
        {
            application.Settings = null;
            return default;
        }

        application.Settings = WriteStream(writer =>
        {
            writer.WriteStartObject();
            foreach (var setting in settings)
            {
                writer.WritePropertyName(setting.Key);
                writer.WriteStringValue(setting.Value);
            }
            writer.WriteEndObject();
        });

        return default;
    }

    public virtual async ValueTask UpdateAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        var entity = await Repository.GetAsync(application.Id, cancellationToken: cancellationToken);

        try
        {
            await Repository.UpdateAsync(application.ToEntity(entity), autoSave: true, cancellationToken: cancellationToken);
        }
        catch (AbpDbConcurrencyException e)
        {
            Logger.LogException(e);
            await ConcurrencyExceptionHandler.HandleAsync(e);
            throw new OpenIddictExceptions.ConcurrencyException(e.Message, e.InnerException);
        }

        application = (await Repository.FindAsync(entity.Id, cancellationToken: cancellationToken)).ToModel();
    }

    public virtual ValueTask<string> GetClientUriAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken = default)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ClientUri);
    }

    public virtual ValueTask<string> GetLogoUriAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken = default)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.LogoUri);
    }
}
