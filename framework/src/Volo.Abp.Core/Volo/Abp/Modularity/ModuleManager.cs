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
            LogListOfModules();

            foreach (var Contributor in _lifecycleContributors)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    Contributor.Initialize(context, module.Instance);
                }
            }

            _logger.LogInformation("Initialized all ABP modules.");
        }

        private void LogListOfModules()
        {
            _logger.LogInformation("Loaded ABP modules:");

            foreach (var module in _moduleContainer.Modules)
            {
                _logger.LogInformation("- " + module.Type.FullName);
            }
        }

        public void ShutdownModules(ApplicationShutdownContext context)
        {
            var modules = _moduleContainer.Modules.Reverse().ToList();

            foreach (var Contributor in _lifecycleContributors)
            {
                foreach (var module in modules)
                {
                    Contributor.Shutdown(context, module.Instance);
                }
            }
        }
    }
}