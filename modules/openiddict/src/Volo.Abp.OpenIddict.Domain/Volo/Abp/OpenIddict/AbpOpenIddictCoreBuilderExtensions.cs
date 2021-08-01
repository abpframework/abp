using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict
{
    public static class AbpOpenIddictCoreBuilderExtensions
    {
        public static OpenIddictCoreBuilder AddAbpOpenIddictCore(this OpenIddictBuilder builder)
        {
            var coreBuilder = builder.AddCore();

            coreBuilder.SetDefaultApplicationEntity<OpenIddictApplication>();
            coreBuilder.SetDefaultAuthorizationEntity<OpenIddictAuthorization>();
            coreBuilder.SetDefaultScopeEntity<OpenIddictScope>();
            coreBuilder.SetDefaultTokenEntity<OpenIddictToken>();

            return coreBuilder;
        }

        public static OpenIddictCoreBuilder TryAddAbpMemoryStore(this OpenIddictCoreBuilder coreBuilder)
        {
            if (!coreBuilder.Services.IsAdded<IOpenIddictApplicationStore<OpenIddictApplication>>())
            {
                coreBuilder.AddApplicationStore<InMemoryOpenIddictApplicationStore>(ServiceLifetime.Singleton);
            }
            if (!coreBuilder.Services.IsAdded<IOpenIddictAuthorizationStore<OpenIddictAuthorization>>())
            {
                coreBuilder.AddAuthorizationStore<InMemoryOpenIddictAuthorizationStore>(ServiceLifetime.Singleton);
            }
            if (!coreBuilder.Services.IsAdded<IOpenIddictScopeStore<OpenIddictScope>>())
            {
                coreBuilder.AddScopeStore<InMemoryOpenIddictScopeStore>(ServiceLifetime.Singleton);
            }
            if (!coreBuilder.Services.IsAdded<IOpenIddictTokenStore<OpenIddictToken>>())
            {
                coreBuilder.AddTokenStore<InMemoryOpenIddictTokenStore>(ServiceLifetime.Singleton);
            }
            return coreBuilder;
        }

        public static OpenIddictCoreBuilder AddAbpMemoryStore(this OpenIddictCoreBuilder coreBuilder)
        {
            coreBuilder
                .AddApplicationStore<InMemoryOpenIddictApplicationStore>(ServiceLifetime.Singleton)
                .AddAuthorizationStore<InMemoryOpenIddictAuthorizationStore>(ServiceLifetime.Singleton)
                .AddScopeStore<InMemoryOpenIddictScopeStore>(ServiceLifetime.Singleton)
                .AddTokenStore<InMemoryOpenIddictTokenStore>(ServiceLifetime.Singleton);
            return coreBuilder;
        }

        public static OpenIddictCoreBuilder AddAbpStore(this OpenIddictCoreBuilder coreBuilder)
        {
            coreBuilder
                .AddApplicationStore<AbpOpenIddictApplicationStore>(ServiceLifetime.Transient)
                .AddAuthorizationStore<AbpOpenIddictAuthorizationStore>(ServiceLifetime.Transient)
                .AddScopeStore<AbpOpenIddictScopeStore>(ServiceLifetime.Transient)
                .AddTokenStore<AbpOpenIddictTokenStore>(ServiceLifetime.Transient);
            return coreBuilder;
        }
    }
}
