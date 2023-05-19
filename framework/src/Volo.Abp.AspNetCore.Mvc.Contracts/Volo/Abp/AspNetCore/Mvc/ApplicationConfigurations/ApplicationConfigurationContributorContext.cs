using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationConfigurationContributorContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public ApplicationConfigurationDto ApplicationConfiguration { get; }

    public ApplicationConfigurationContributorContext(IServiceProvider serviceProvider, ApplicationConfigurationDto applicationConfiguration)
    {
        ServiceProvider = serviceProvider;
        ApplicationConfiguration = applicationConfiguration;
    }
}
