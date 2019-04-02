using Volo.Abp.Settings;

namespace BaseManagement
{
    /* These setting definitions will be visible to clients that has a BaseManagement.Application.Contracts
     * reference. Settings those should be hidden from clients should be defined in the BaseManagement.Application
     * package.
     */
    public class BaseManagementSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    BaseManagementSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}