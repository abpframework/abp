namespace Volo.Abp.Domain.Entities.Events.Distributed;

public class AbpDistributedEntityEventOptions
{
    public IAutoEntityDistributedEventSelectorList AutoEventSelectors { get; }

    public EtoMappingDictionary EtoMappings { get; set; }

    public AbpDistributedEntityEventOptions()
    {
        AutoEventSelectors = new AutoEntityDistributedEventSelectorList();
        EtoMappings = new EtoMappingDictionary();
    }
}
