namespace Volo.Abp.Settings
{
    public interface ISettingDefinitionManager
    {
        SettingDefinition Get(string name);
    }
}