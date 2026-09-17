namespace Volo.Abp.Domain.Entities.Events.Distributed;

public class AbpDistributedEntityEventOptions
{
    public IAutoEntityDistributedEventSelectorList AutoEventSelectors { get; }

    public IAutoEntityDistributedEventSelectorList IgnoredEventSelectors { get; }

    public EtoMappingDictionary EtoMappings { get; set; }

    public AbpDistributedEntityEventOptions()
    {
        AutoEventSelectors = new AutoEntityDistributedEventSelectorList();
        IgnoredEventSelectors = new AutoEntityDistributedEventSelectorList();
        EtoMappings = new EtoMappingDictionary();
    }
}
