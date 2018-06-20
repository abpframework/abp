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
        private readonly IEnumerable<IModuleLifecycleContributer> _lifecycleContributers;
        private readonly ILogger<ModuleManager> _logger;

        public ModuleManager(
            IModuleContainer moduleContainer,
            ILogger<ModuleManager> logger,
            IOptions<ModuleLifecycleOptions> options,
            IServiceProvider serviceProvider)
        {
            _moduleContainer = moduleContainer;
            _logger = logger;

            _lifecycleContributers = options.Value
                .Contributers
                .Select(serviceProvider.GetRequiredService)
                .Cast<IModuleLifecycleContributer>()
                .ToArray();
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