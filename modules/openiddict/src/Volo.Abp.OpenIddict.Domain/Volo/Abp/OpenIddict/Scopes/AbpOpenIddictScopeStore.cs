using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Scopes;

public class AbpOpenIddictScopeStore : AbpOpenIddictStoreBase<IOpenIddictScopeRepository>, IOpenIddictScopeStore<OpenIddictScopeModel>
{
    public AbpOpenIddictScopeStore(
        IOpenIddictScopeRepository repository,
        IUnitOfWorkManager unitOfWorkManager,
        IGuidGenerator guidGenerator)
        : base(repository, unitOfWorkManager, guidGenerator)
    {

    }

   public virtual async ValueTask<long> CountAsync(CancellationToken cancellationToken)
   {
       return await Repository.GetCountAsync(cancellationToken);
   }

   public virtual ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictScopeModel>, IQueryable<TResult>> query, CancellationToken cancellationToken)
   {
       throw new NotSupportedException();
   }

   public virtual async ValueTask CreateAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
   {
       Check.NotNull(scope, nameof(scope));

       await Repository.InsertAsync(scope.ToEntity(), autoSave: true, cancellationToken: cancellationToken);
   }

   public virtual async ValueTask DeleteAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
   {
       Check.NotNull(scope, nameof(scope));

       await Repository.DeleteAsync(scope.Id, autoSave: true, cancellationToken: cancellationToken);
   }

   public virtual async ValueTask<OpenIddictScopeModel> FindByIdAsync(string identifier, CancellationToken cancellationToken)
   {
       Check.NotNullOrEmpty(identifier, nameof(identifier));

       return (await Repository.FindByIdAsync(ConvertIdentifierFromString(identifier), cancellationToken)).ToModel();
   }

    public virtual async ValueTask<OpenIddictScopeModel> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(name, nameof(name));

        return (await Repository.FindByNameAsync(name, cancellationToken)).ToModel();
    }

    public virtual async IAsyncEnumerable<OpenIddictScopeModel> FindByNamesAsync(ImmutableArray<string> names, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(names, nameof(names));
        foreach (var name in names)
        {
            Check.NotNullOrEmpty(name, nameof(name));
        }

        var scopes = await Repository.FindByNamesAsync(names.ToArray(), cancellationToken);
        foreach (var scope in scopes)
        {
            yield return scope.ToModel();
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictScopeModel> FindByResourceAsync(string resource, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(resource, nameof(resource));

        var scopes = await Repository.FindByResourceAsync(resource, cancellationToken);
        foreach (var scope in scopes)
        {
            var resources = await GetResourcesAsync(scope.ToModel(), cancellationToken);
            if (resources.Contains(resource, StringComparer.Ordinal))
            {
                yield return scope.ToModel();
            }
        }
    }

    public virtual ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictScopeModel>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public virtual  ValueTask<string> GetDescriptionAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        return new ValueTask<string>(scope.Description);
    }

    public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDescriptionsAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (string.IsNullOrEmpty(scope.Descriptions))
        {
            return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
        }

        using (var document = JsonDocument.Parse(scope.Descriptions))
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

    public virtual ValueTask<string> GetDisplayNameAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        return new ValueTask<string>(scope.DisplayName);
    }

    public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (string.IsNullOrEmpty(scope.DisplayNames))
        {
            return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
        }

        using (var document = JsonDocument.Parse(scope.DisplayNames))
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

    public virtual ValueTask<string> GetIdAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        return new ValueTask<string>(ConvertIdentifierToString(scope.Id));
    }

    public virtual ValueTask<string> GetNameAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        return new ValueTask<string>(scope.Name);
    }

    public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (string.IsNullOrEmpty(scope.Properties))
        {
            return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
        }

        using (var document = JsonDocument.Parse(scope.Properties))
        {
            var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                builder[property.Name] = property.Value.Clone();
            }

            return new ValueTask<ImmutableDictionary<string, JsonElement>>(builder.ToImmutable());
        }
    }

    public virtual ValueTask<ImmutableArray<string>> GetResourcesAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (string.IsNullOrEmpty(scope.Resources))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        using (var document = JsonDocument.Parse(scope.Resources))
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

    public virtual ValueTask<OpenIddictScopeModel> InstantiateAsync(CancellationToken cancellationToken)
    {
        return new ValueTask<OpenIddictScopeModel>(new OpenIddictScopeModel
        {
            Id = GuidGenerator.Create()
        });
    }

    public virtual async IAsyncEnumerable<OpenIddictScopeModel> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var scopes = await Repository.ListAsync(count, offset, cancellationToken);
        foreach (var scope in scopes)
        {
            yield return scope.ToModel();
        }
    }

    public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictScopeModel>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public virtual ValueTask SetDescriptionAsync(OpenIddictScopeModel scope, string description, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        scope.Description = description;

        return default;
    }

    public virtual ValueTask SetDescriptionsAsync(OpenIddictScopeModel scope, ImmutableDictionary<CultureInfo, string> descriptions, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (descriptions is null || descriptions.IsEmpty)
        {
            scope.Descriptions = null;
            return default;
        }

        scope.Descriptions =WriteStream(writer =>
        {
            writer.WriteStartObject();
            foreach (var description in descriptions)
            {
                writer.WritePropertyName(description.Key.Name);
                writer.WriteStringValue(description.Value);
            }
            writer.WriteEndObject();
        });

        return default;
    }

    public virtual ValueTask SetDisplayNameAsync(OpenIddictScopeModel scope, string name, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        scope.DisplayName = name;

        return default;
    }

    public virtual ValueTask SetDisplayNamesAsync(OpenIddictScopeModel scope, ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (names is null || names.IsEmpty)
        {
            scope.DisplayNames = null;
            return default;
        }

        scope.DisplayNames =WriteStream(writer =>
        {
            writer.WriteStartObject();
            foreach (var name in names)
            {
                writer.WritePropertyName(name.Key.Name);
                writer.WriteStringValue(name.Value);
            }
            writer.WriteEndObject();
        });

        return default;
    }

    public virtual ValueTask SetNameAsync(OpenIddictScopeModel scope, string name, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        scope.Name = name;

        return default;
    }

    public virtual ValueTask SetPropertiesAsync(OpenIddictScopeModel scope, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (properties is null || properties.IsEmpty)
        {
            scope.Properties = null;
            return default;
        }

        scope.Properties =WriteStream(writer =>
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

    public virtual ValueTask SetResourcesAsync(OpenIddictScopeModel scope, ImmutableArray<string> resources, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (resources.IsDefaultOrEmpty)
        {
            scope.Resources = null;
            return default;
        }

        scope.Resources = WriteStream(writer =>
        {
            writer.WriteStartArray();
            foreach (var resource in resources)
            {
                writer.WriteStringValue(resource);
            }
            writer.WriteEndArray();
        });

        return default;
    }

    public virtual async ValueTask UpdateAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        var entity = await Repository.GetAsync(scope.Id, cancellationToken: cancellationToken);

        await Repository.UpdateAsync(scope.ToEntity(entity), autoSave: true, cancellationToken: cancellationToken);
    }
}
