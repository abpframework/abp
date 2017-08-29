using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public class ModuleManager : IModuleManager, ISingletonDependency
    {
        private readonly IModuleContainer _moduleContainer;
        private readonly IEnumerable<IModuleLifecycleContributer> _lifecycleContributers;
        private readonly ILogger<ModuleManager> _logger;

        public ModuleManager(
            IModuleContainer moduleContainer,
            IEnumerable<IModuleLifecycleContributer> lifecycleContributers,
            ILogger<ModuleManager> logger)
        {
            _moduleContainer = moduleContainer;
            _lifecycleContributers = lifecycleContributers;
            _logger = logger;
        }

        public void InitializeModules(ApplicationInitializationContext context)
        {
            LogListOfModules();

            foreach (var contributer in _lifecycleContributers)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    contributer.Initialize(context, module.Instance);
                }
            }

            _logger.LogInformation("Initialized all modules.");
        }

        private void LogListOfModules()
        {
            _logger.LogInformation("Loaded modules:");

            foreach (var module in _moduleContainer.Modules)
            {
                _logger.LogInformation("- " + module.Type.FullName);
            }
        }

        public void ShutdownModules(ApplicationShutdownContext context)
        {
            foreach (var contributer in _lifecycleContributers)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    contributer.Shutdown(context, module.Instance);
                }
            }
        }
    }
}