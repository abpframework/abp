namespace Volo.Abp.ObjectExtending.TestObjects;

public class ExtensibleTestPerson : ExtensibleObject
{
    public ExtensibleTestPerson()
    {

    }

    public ExtensibleTestPerson(bool setDefaultsForExtraProperties)
        : base(setDefaultsForExtraProperties)
    {

    }
}

public enum ExtensibleTestEnumProperty
{
    Value1,
    Value2
}
