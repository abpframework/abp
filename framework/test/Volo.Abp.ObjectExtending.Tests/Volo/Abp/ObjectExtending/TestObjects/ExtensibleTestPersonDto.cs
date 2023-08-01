namespace Volo.Abp.ObjectExtending.TestObjects;

public class ExtensibleTestPersonDto : ExtensibleObject
{
    public void SetExtraPropertiesAsNull()
    {
        ExtraProperties = null;
    }
}
