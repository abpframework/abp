using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Logging;
using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public class ModuleManager : IModuleManager, ISingletonDependency
    {
        public IReadOnlyList<AbpModuleDescriptor> Modules => _modules.ToImmutableList();

        private readonly List<AbpModuleDescriptor> _modules;
        private readonly IEnumerable<IModuleLifecycleContributer> _lifecycleContributers;
        private readonly ILogger<ModuleManager> _logger;

        public ModuleManager(
            AbpApplication application,
            IEnumerable<IModuleLifecycleContributer> lifecycleContributers,
            ILogger<ModuleManager> logger)
        {
            _lifecycleContributers = lifecycleContributers;
            _logger = logger;
            _modules = application.Modules.ToList();
        }

        public void InitializeModules(ApplicationInitializationContext context)
        {
            LogListOfModules();

            foreach (var contributer in _lifecycleContributers)
            {
                foreach (var module in Modules)
                {
                    contributer.Initialize(context, module.Instance);
                }
            }

            _logger.LogInformation("Initialized all modules.");
        }

        private void LogListOfModules()
        {
            _logger.LogInformation("Loaded modules:");

            foreach (var module in Modules)
            {
                _logger.LogInformation("- " + module.Type.FullName);
            }
        }

        public void ShutdownModules(ApplicationShutdownContext context)
        {
            foreach (var contributer in _lifecycleContributers)
            {
                foreach (var module in Modules)
                {
                    contributer.Shutdown(context, module.Instance);
                }
            }
        }
    }
}