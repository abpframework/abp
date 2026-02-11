namespace Volo.Abp.DependencyInjection;

public class OnServiceActivatedContext : IOnServiceActivatedContext
{
    public object Instance { get; set; }

    public OnServiceActivatedContext(object instance)
    {
        Instance = instance;
    }
}
