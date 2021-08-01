using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.OpenIddict
{
    public class AbpOpenIddictMemoryStore : IOpenIddictMemoryStore, ISingletonDependency
    {
        private static readonly MethodInfo ObfuscateClientSecretAsyncMethod =
            typeof(OpenIddictApplicationManager<OpenIddictApplication>)
                .GetMethod("ObfuscateClientSecretAsync", BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly Lazy<ConcurrentDictionary<Guid, OpenIddictApplication>> _lazyApplications;
        private readonly Lazy<ConcurrentDictionary<Guid, OpenIddictScope>> _lazyScopes;

        public ConcurrentDictionary<Guid, OpenIddictApplication> Applications => _lazyApplications.Value;

        public ConcurrentDictionary<Guid, OpenIddictAuthorization> Authorizations { get; } = new();

        public ConcurrentDictionary<Guid, OpenIddictScope> Scopes => _lazyScopes.Value;

        public ConcurrentDictionary<Guid, OpenIddictToken> Tokens { get; } = new();

        protected IConfiguration Configuration { get; }

        protected IGuidGenerator GuidGenerator { get; }

        protected AbpOpenIddictOptions OpenIddictOptions { get; }

        protected IClock Clock { get; }

        protected IAbpLazyServiceProvider LazyServiceProvider { get; }

        public AbpOpenIddictMemoryStore(
            IConfiguration configuration,
            IGuidGenerator guidGenerator,
            IClock clock,
            IOptions<AbpOpenIddictOptions> openIddictOptions,
            IAbpLazyServiceProvider lazyServiceProvider)
        {
            Configuration = configuration;
            GuidGenerator = guidGenerator;
            Clock = clock;
            OpenIddictOptions = openIddictOptions.Value;
            LazyServiceProvider = lazyServiceProvider;

            _lazyApplications = new(SeedApplicationsData, true);
            _lazyScopes = new(SeedScopesData, true);
        }

        protected virtual ConcurrentDictionary<Guid, OpenIddictApplication> SeedApplicationsData()
        {
            var applications = new ConcurrentDictionary<Guid, OpenIddictApplication>();

            foreach (var item in OpenIddictOptions.Applications)
            {
                var application = new OpenIddictApplication(GuidGenerator.Create(), item.ClientId);
                application.CreationTime = Clock.Now;
                application.ConcurrencyStamp = Guid.NewGuid().ToString();
                application.SetClientId(item.ClientId);
                if (!item.ClientSecret.IsNullOrWhiteSpace())
                {
                    application.SetClientSecret(ObfuscateClientSecret(item.ClientSecret));
                }

                application.SetClientType(item.Type);
                application.SetConsentType(item.ConsentType);
                application.SetDisplayName(item.DisplayName);
                application.SetPermissions(item.Permissions);
                application.SetPostLogoutRedirectUris(new HashSet<string>(item.PostLogoutRedirectUris.Select(x => x.AbsoluteUri).ToList()));
                application.SetRedirectUris(new HashSet<string>(item.RedirectUris.Select(x => x.AbsoluteUri).ToList()));
                application.SetRequirements(item.Requirements);
                application.SetDisplayNames(item.DisplayNames);
                application.SetProperties(item.Properties);
                applications.TryAdd(application.Id, application);
            }

            return applications;
        }

        protected virtual ConcurrentDictionary<Guid, OpenIddictScope> SeedScopesData()
        {
            var scopes = new ConcurrentDictionary<Guid, OpenIddictScope>();

            foreach (var item in OpenIddictOptions.Scopes)
            {
                var scope = new OpenIddictScope(GuidGenerator.Create());
                scope.ConcurrencyStamp = Guid.NewGuid().ToString();
                scope.SetName(item.Name);
                scope.SetDescription(item.Description);
                scope.SetDisplayName(item.DisplayName);
                scope.SetResources(item.Resources);
                scope.SetDisplayNames(item.DisplayNames);
                scope.SetDescriptions(item.Descriptions);
                scope.SetProperties(item.Properties);
                scopes.TryAdd(scope.Id, scope);
            }

            return scopes;
        }

        //OpenIddict.Core
        protected virtual string ObfuscateClientSecret(string secret)
        {
            var applicationManager = LazyServiceProvider.LazyGetRequiredService<IOpenIddictApplicationManager>();

            var hashSecret = AsyncHelper.RunSync(async () =>
                await (ValueTask<string>)ObfuscateClientSecretAsyncMethod
                    .Invoke(applicationManager, new object[] { secret, CancellationToken.None }));

            return hashSecret;
        }
    }
}
