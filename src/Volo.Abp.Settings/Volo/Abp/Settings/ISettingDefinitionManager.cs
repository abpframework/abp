using JetBrains.Annotations;

namespace Volo.Abp.Settings
{
    public interface ISettingDefinitionManager
    {
        [NotNull]
        SettingDefinition Get([NotNull] string name);
    }
}