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
using OpenIddict.Abstractions;
using Volo.Abp.Guids;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpOpenIddictApplicationStore : AbpOpenIddictStoreBase<IOpenIddictApplicationRepository>, IOpenIddictApplicationStore<OpenIddictApplicationModel>
{
    protected IOpenIddictTokenRepository TokenRepository { get; }

    public AbpOpenIddictApplicationStore(
        IOpenIddictApplicationRepository repository,
        IUnitOfWorkManager unitOfWorkManager,
        IOpenIddictTokenRepository tokenRepository,
        IGuidGenerator guidGenerator)
        : base(repository, unitOfWorkManager, guidGenerator)
    {
        TokenRepository = tokenRepository;
    }

    public async ValueTask<long> CountAsync(CancellationToken cancellationToken)
    {
        return await Repository.GetCountAsync(cancellationToken);
    }

    public ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictApplicationModel>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public async ValueTask CreateAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        await Repository.InsertAsync(application.ToEntity(), autoSave: true, cancellationToken: cancellationToken);
    }

    public async ValueTask DeleteAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true, isolationLevel: IsolationLevel.RepeatableRead))
        {
            await TokenRepository.DeleteManyByApplicationIdAsync(application.Id, cancellationToken: cancellationToken);

            await Repository.DeleteAsync(application.Id, cancellationToken: cancellationToken);

            await uow.CompleteAsync(cancellationToken);
        }
    }

    public async ValueTask<OpenIddictApplicationModel> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return (await Repository.FindAsync(ConvertIdentifierFromString(identifier),  cancellationToken: cancellationToken)).ToModel();
    }

    public async ValueTask<OpenIddictApplicationModel> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return (await Repository.FindByClientIdAsync(identifier, cancellationToken: cancellationToken)).ToModel();
    }

    public async IAsyncEnumerable<OpenIddictApplicationModel> FindByPostLogoutRedirectUriAsync(string address, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(address, nameof(address));

        var applications = await Repository.FindByPostLogoutRedirectUriAsync(address, cancellationToken);

        foreach (var application in applications)
        {
            var addresses = await GetPostLogoutRedirectUrisAsync(application.ToModel(), cancellationToken);
            if (addresses.Contains(address, StringComparer.Ordinal))
            {
                yield return application.ToModel();
            }
        }
    }

    public async IAsyncEnumerable<OpenIddictApplicationModel> FindByRedirectUriAsync(string address, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(address, nameof(address));

        var applications = await Repository.FindByRedirectUriAsync(address, cancellationToken);
        foreach (var application in applications)
        {
            var addresses = await GetRedirectUrisAsync(application.ToModel(), cancellationToken);
            if (addresses.Contains(address, StringComparer.Ordinal))
            {
                yield return application.ToModel();
            }
        }
    }

    public ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictApplicationModel>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public ValueTask<string> GetClientIdAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ClientId);
    }

    public ValueTask<string> GetClientSecretAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ClientSecret);
    }

    public ValueTask<string> GetClientTypeAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));
        return new ValueTask<string>(application.Type);
    }

    public ValueTask<string> GetConsentTypeAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.ConsentType);
    }

    public ValueTask<string> GetDisplayNameAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(application.DisplayName);
    }

    public ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
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

    public ValueTask<string> GetIdAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        return new ValueTask<string>(ConvertIdentifierToString(application.Id));
    }

    public ValueTask<ImmutableArray<string>> GetPermissionsAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
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

    public ValueTask<ImmutableArray<string>> GetPostLogoutRedirectUrisAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
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

    public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
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

    public ValueTask<ImmutableArray<string>> GetRedirectUrisAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
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

    public ValueTask<ImmutableArray<string>> GetRequirementsAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
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

    public ValueTask<OpenIddictApplicationModel> InstantiateAsync(CancellationToken cancellationToken)
    {
        return new ValueTask<OpenIddictApplicationModel>(new OpenIddictApplicationModel
        {
            Id = GuidGenerator.Create()
        });
    }

    public async IAsyncEnumerable<OpenIddictApplicationModel> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var applications = await Repository.ListAsync(count, offset, cancellationToken);
        foreach (var application in applications)
        {
            yield return application.ToModel();
        }
    }

    public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictApplicationModel>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
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

        application.Type = type;
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

    public virtual ValueTask SetPostLogoutRedirectUrisAsync(OpenIddictApplicationModel application, ImmutableArray<string> addresses,
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

    public virtual ValueTask SetRedirectUrisAsync(OpenIddictApplicationModel application, ImmutableArray<string> addresses,
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

    public virtual async ValueTask UpdateAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        var entity = await Repository.GetAsync(application.Id, cancellationToken: cancellationToken);

        await Repository.UpdateAsync(application.ToEntity(entity), autoSave: true, cancellationToken: cancellationToken);
    }
}
