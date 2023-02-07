using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

public class ApplicationConfigurationCache : ISingletonDependency
{
    protected ApplicationConfigurationDto Configuration { get; set; }

    public event Action ApplicationConfigurationChanged;

    public virtual ApplicationConfigurationDto Get()
    {
        return Configuration;
    }

    public void Set(ApplicationConfigurationDto configuration)
    {
        Configuration = configuration;
        ApplicationConfigurationChanged?.Invoke();
    }
}