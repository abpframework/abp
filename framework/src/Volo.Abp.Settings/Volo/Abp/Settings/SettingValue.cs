using System;

namespace Volo.Abp.Settings;

[Serializable]
public class SettingValue : NameValue<string?>
{
    public SettingValue()
    {

    }

    public SettingValue(string name, string? value)
    {
        Name = name;
        Value = value;
    }
}
