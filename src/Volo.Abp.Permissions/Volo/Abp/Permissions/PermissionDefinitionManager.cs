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
        protected List<IPermissionDefinitionProvider> Providers => _lazyProviders.Value;
        private readonly Lazy<List<IPermissionDefinitionProvider>> _lazyProviders;

        protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;
        private readonly Lazy<IDictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;

        protected PermissionOptions Options { get; }

        private readonly IServiceProvider _serviceProvider;

        public PermissionDefinitionManager(
            IOptions<PermissionOptions> options,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Options = options.Value;

            _lazyProviders = new Lazy<List<IPermissionDefinitionProvider>>(CreatePermissionProviders, true);
            _lazyPermissionDefinitions = new Lazy<IDictionary<string, PermissionDefinition>>(CreatePermissionDefinitions, true);
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
            return PermissionDefinitions.Values.ToImmutableList();
        }

        public virtual PermissionDefinition GetOrNull(string name)
        {
            return PermissionDefinitions.GetOrDefault(name);
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
            var context = new PermissionDefinitionContext();

            foreach (var provider in Providers)
            {
                provider.Define(context);
                context.HandleNewChildren();
            }

            return context.Permissions;
        }
    }
}