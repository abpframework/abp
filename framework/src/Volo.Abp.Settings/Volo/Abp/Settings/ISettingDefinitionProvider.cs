namespace Volo.Abp.Settings
{
    public interface ISettingDefinitionProvider
    {
        void Define(ISettingDefinitionContext context);
    }
}