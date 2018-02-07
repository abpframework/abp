namespace Volo.Abp.Settings
{
    public class SettingDefinition
    {
        public string Name { get; }

        public string DefaultValue { get; set; }

        public SettingDefinition(string name, string defaultValue = null)
        {
            Name = name;

            DefaultValue = defaultValue;
        }
    }
}