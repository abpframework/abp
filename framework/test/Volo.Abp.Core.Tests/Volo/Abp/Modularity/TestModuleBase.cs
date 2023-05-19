using System.Threading.Tasks;

namespace Volo.Abp.Modularity;

public class TestModuleBase : AbpModule
{
    public bool PreConfigureServicesAsyncIsCalled { get; set; }
    public bool ConfigureServicesAsyncIsCalled { get; set; }
    public bool PostConfigureServicesAsyncIsCalled { get; set; }
    public bool OnPreApplicationInitializationIsCalled { get; set; }
    public bool OnPostApplicationInitializationIsCalled { get; set; }
    public bool OnApplicationInitializeAsyncIsCalled { get; set; }
    public bool OnApplicationShutdownAsyncIsCalled { get; set; }

    public bool PreConfigureServicesIsCalled { get; set; }
    public bool ConfigureServicesIsCalled { get; set; }
    public bool PostConfigureServicesIsCalled { get; set; }
    public bool OnPreApplicationInitializationAsyncIsCalled { get; set; }
    public bool OnPostApplicationInitializationAsyncIsCalled { get; set; }
    public bool OnApplicationInitializeIsCalled { get; set; }
    public bool OnApplicationShutdownIsCalled { get; set; }

    public override Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
        PreConfigureServicesAsyncIsCalled = true;
        return base.PreConfigureServicesAsync(context);
    }

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigureServicesIsCalled = true;
    }

    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        ConfigureServicesAsyncIsCalled = true;
        return base.ConfigureServicesAsync(context);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureServicesIsCalled = true;
    }

    public override Task PostConfigureServicesAsync(ServiceConfigurationContext context)
    {
        PostConfigureServicesAsyncIsCalled = true;
        return base.PostConfigureServicesAsync(context);
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        PostConfigureServicesIsCalled = true;
    }

    public override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        OnPreApplicationInitializationAsyncIsCalled = true;
        return base.OnPreApplicationInitializationAsync(context);
    }

    public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
    {
        OnPreApplicationInitializationIsCalled = true;
    }

    public override Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        OnPostApplicationInitializationAsyncIsCalled = true;
        return base.OnPostApplicationInitializationAsync(context);
    }

    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {
        OnPostApplicationInitializationIsCalled = true;
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        OnApplicationInitializeAsyncIsCalled = true;
        return base.OnApplicationInitializationAsync(context);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        OnApplicationInitializeIsCalled = true;
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        OnApplicationShutdownAsyncIsCalled = true;
        return base.OnApplicationShutdownAsync(context);
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        OnApplicationShutdownIsCalled = true;
    }
}
