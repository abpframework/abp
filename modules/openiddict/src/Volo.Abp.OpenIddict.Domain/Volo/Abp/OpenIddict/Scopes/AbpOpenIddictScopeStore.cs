using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

namespace Volo.Abp.OpenIddict.Scopes;

public class AbpOpenIddictScopeStore : AbpOpenIddictStoreBase<IOpenIddictScopeRepository>, IOpenIddictScopeStore<OpenIddictScope>, IScopedDependency
{
   public AbpOpenIddictScopeStore(
        IOpenIddictScopeRepository repository,
        IUnitOfWorkManager unitOfWorkManager,
        IMemoryCache cache)
        : base(repository, unitOfWorkManager, cache)
    {

    }

   public virtual async ValueTask<long> CountAsync(CancellationToken cancellationToken)
   {
       return await Repository.GetCountAsync(cancellationToken);
   }

   public virtual async ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictScope>, IQueryable<TResult>> query, CancellationToken cancellationToken)
   {
       Check.NotNull(query, nameof(query));

       return await Repository.CountAsync(query, cancellationToken);
   }

   public virtual async ValueTask CreateAsync(OpenIddictScope scope, CancellationToken cancellationToken)
   {
       Check.NotNull(scope, nameof(scope));

       await Repository.InsertAsync(scope, autoSave: true, cancellationToken: cancellationToken);
   }

   public virtual async ValueTask DeleteAsync(OpenIddictScope scope, CancellationToken cancellationToken)
   {
       Check.NotNull(scope, nameof(scope));

       await Repository.DeleteAsync(scope, autoSave: true, cancellationToken: cancellationToken);
   }

   public virtual async ValueTask<OpenIddictScope> FindByIdAsync(string identifier, CancellationToken cancellationToken)
   {
       Check.NotNullOrEmpty(identifier, nameof(identifier));

       return await Repository.FindByIdAsync(ConvertIdentifierFromString(identifier), cancellationToken);
   }

    public virtual async ValueTask<OpenIddictScope> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(name, nameof(name));

        return await Repository.FindByNameAsync(name, cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictScope> FindByNamesAsync(ImmutableArray<string> names, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(names, nameof(names));
        foreach (var name in names)
        {
            Check.NotNullOrEmpty(name, nameof(name));
        }

        var scopes = await Repository.FindByNamesAsync(names.ToArray(), cancellationToken);
        foreach (var scope in scopes)
        {
            yield return scope;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictScope> FindByResourceAsync(string resource, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(resource, nameof(resource));

        var scopes = await Repository.FindByResourceAsync(resource, cancellationToken);
        foreach (var scope in scopes)
        {
            var resources = await GetResourcesAsync(scope, cancellationToken);
            if (resources.Contains(resource, StringComparer.Ordinal))
            {
                yield return scope;
            }
        }
    }

    public virtual async ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        return await Repository.GetAsync(query, state, cancellationToken);
    }

    public virtual  ValueTask<string> GetDescriptionAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        return new ValueTask<string>(scope.Description);
    }

    public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDescriptionsAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (string.IsNullOrEmpty(scope.Descriptions))
        {
            return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
        }

        // Note: parsing the stringified descriptions is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("42891062-8f69-43ba-9111-db7e8ded2553", "\x1e", scope.Descriptions);
        var descriptions = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(scope.Descriptions);
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

        return new ValueTask<ImmutableDictionary<CultureInfo, string>>(descriptions);
    }

    public virtual ValueTask<string> GetDisplayNameAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        return new ValueTask<string>(scope.DisplayName);
    }

    public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (string.IsNullOrEmpty(scope.DisplayNames))
        {
            return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
        }

        // Note: parsing the stringified display names is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("e17d437b-bdd2-43f3-974e-46d524f4bae1", "\x1e", scope.DisplayNames);
        var names = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(scope.DisplayNames);
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

    public virtual ValueTask<string> GetIdAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        return new ValueTask<string>(ConvertIdentifierToString(scope.Id));
    }

    public virtual ValueTask<string> GetNameAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        return new ValueTask<string>(scope.Name);
    }

    public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (string.IsNullOrEmpty(scope.Properties))
        {
            return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
        }

        // Note: parsing the stringified properties is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("78d8dfdd-3870-442e-b62e-dc9bf6eaeff7", "\x1e", scope.Properties);
        var properties = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(scope.Properties);
            var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                builder[property.Name] = property.Value.Clone();
            }

            return builder.ToImmutable();
        });

        return new ValueTask<ImmutableDictionary<string, JsonElement>>(properties);
    }

    public virtual ValueTask<ImmutableArray<string>> GetResourcesAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        if (string.IsNullOrEmpty(scope.Resources))
        {
            return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
        }

        // Note: parsing the stringified resources is an expensive operation.
        // To mitigate that, the resulting array is stored in the memory cache.
        var key = string.Concat("b6148250-aede-4fb9-a621-07c9bcf238c3", "\x1e", scope.Resources);
        var resources = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(scope.Resources);
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

        return new ValueTask<ImmutableArray<string>>(resources);
    }

    public virtual ValueTask<OpenIddictScope> InstantiateAsync(CancellationToken cancellationToken)
    {
        try
        {
            return new ValueTask<OpenIddictScope>(Activator.CreateInstance<OpenIddictScope>());
        }
        catch (MemberAccessException exception)
        {
            return new ValueTask<OpenIddictScope>(Task.FromException<OpenIddictScope>(exception));
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictScope> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var scopes = await Repository.ListAsync(count, offset, cancellationToken);
        foreach (var scope in scopes)
        {
            yield return scope;
        }
    }

    public async IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query, TState state, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        var scopes = await Repository.ListAsync(query, state, cancellationToken);
        foreach (var scope in scopes)
        {
            yield return scope;
        }
    }

    public virtual ValueTask SetDescriptionAsync(OpenIddictScope scope, string description, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        scope.Description = description;

        return default;
    }

    public virtual ValueTask SetDescriptionsAsync(OpenIddictScope scope, ImmutableDictionary<CultureInfo, string> descriptions, CancellationToken cancellationToken)
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

    public virtual ValueTask SetDisplayNameAsync(OpenIddictScope scope, string name, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        scope.DisplayName = name;

        return default;
    }

    public virtual ValueTask SetDisplayNamesAsync(OpenIddictScope scope, ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken)
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

    public virtual ValueTask SetNameAsync(OpenIddictScope scope, string name, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        scope.Name = name;

        return default;
    }

    public virtual ValueTask SetPropertiesAsync(OpenIddictScope scope, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
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

    public virtual ValueTask SetResourcesAsync(OpenIddictScope scope, ImmutableArray<string> resources, CancellationToken cancellationToken)
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

    public virtual async ValueTask UpdateAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        await Repository.UpdateAsync(scope, autoSave: true, cancellationToken: cancellationToken);
    }
}
