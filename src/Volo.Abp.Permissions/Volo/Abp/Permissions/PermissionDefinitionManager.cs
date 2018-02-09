using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public class PermissionDefinitionManager : IPermissionDefinitionManager, ISingletonDependency
    {
        protected Lazy<List<IPermissionDefinitionProvider>> Providers { get; }

        protected Lazy<IDictionary<string, PermissionDefinition>> PermissionDefinitions { get; }

        protected PermissionOptions Options { get; }

        private readonly IServiceProvider _serviceProvider;

        public PermissionDefinitionManager(
            IOptions<PermissionOptions> options,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Options = options.Value;

            Providers = new Lazy<List<IPermissionDefinitionProvider>>(CreatePermissionProviders, true);
            PermissionDefinitions = new Lazy<IDictionary<string, PermissionDefinition>>(CreatePermissionDefinitions, true);
        }

        public virtual PermissionDefinition Get(string name)
        {
            Check.NotNull(name, nameof(name));

            var permission = GetOrNull(name);

            if (permission == null)
            {
                throw new AbpException("Undefined permission: " + name);
            }

            return permission;
        }

        public virtual IReadOnlyList<PermissionDefinition> GetAll()
        {
            return PermissionDefinitions.Value.Values.ToImmutableList();
        }

        public virtual PermissionDefinition GetOrNull(string name)
        {
            return PermissionDefinitions.Value.GetOrDefault(name);
        }

        protected virtual List<IPermissionDefinitionProvider> CreatePermissionProviders()
        {
            return Options
                .DefinitionProviders
                .Select(p => _serviceProvider.GetRequiredService(p) as IPermissionDefinitionProvider)
                .ToList();
        }

        protected virtual IDictionary<string, PermissionDefinition> CreatePermissionDefinitions()
        {
            var permissions = new Dictionary<string, PermissionDefinition>();

            foreach (var provider in Providers.Value)
            {
                provider.Define(new PermissionDefinitionContext(permissions));
            }

            return permissions;
        }
    }
}