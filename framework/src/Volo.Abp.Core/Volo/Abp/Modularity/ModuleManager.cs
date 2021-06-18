using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public class ModuleManager : IModuleManager, ISingletonDependency
    {
        private readonly IModuleContainer _moduleContainer;
        private readonly IEnumerable<IModuleLifecycleContributor> _lifecycleContributors;
        private readonly ILogger<ModuleManager> _logger;

        public ModuleManager(
            IModuleContainer moduleContainer,
            ILogger<ModuleManager> logger,
            IOptions<AbpModuleLifecycleOptions> options,
            IServiceProvider serviceProvider)
        {
            _moduleContainer = moduleContainer;
            _logger = logger;

            _lifecycleContributors = options.Value
                .Contributors
                .Select(serviceProvider.GetRequiredService)
                .Cast<IModuleLifecycleContributor>()
                .ToArray();
        }

        public void InitializeModules(ApplicationInitializationContext context)
        {
            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    try
                    {
                        contributor.Initialize(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new AbpInitializationException($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }

            _logger.LogInformation("Initialized all ABP modules.");
        }

        public void ShutdownModules(ApplicationShutdownContext context)
        {
            var modules = _moduleContainer.Modules.Reverse().ToList();

            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in modules)
                {
                    try
                    {
                        contributor.Shutdown(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new AbpShutdownException($"An error occurred during the shutdown {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }
        }
    }
}