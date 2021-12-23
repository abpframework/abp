namespace Volo.Abp.Modularity;

public interface IAbpModule
{
    void ConfigureServices(ServiceConfigurationContext context);
}
