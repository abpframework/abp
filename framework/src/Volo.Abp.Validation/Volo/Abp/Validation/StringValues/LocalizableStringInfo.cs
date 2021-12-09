namespace Volo.Abp.Validation.StringValues;

public class LocalizableStringInfo
{
    public string ResourceName { get; }

    public string Name { get; }

    public LocalizableStringInfo(string resourceName, string name)
    {
        ResourceName = resourceName;
        Name = name;
    }
}
