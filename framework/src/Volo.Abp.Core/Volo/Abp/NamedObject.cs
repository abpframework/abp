namespace Volo.Abp;

public class NamedObject
{
    public string Name { get; }

    public NamedObject(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }
}