namespace Volo.Abp.Settings
{
    public interface ISettingProvider
    {
        void Define(ISettingDefinitionContext context);
    }
}