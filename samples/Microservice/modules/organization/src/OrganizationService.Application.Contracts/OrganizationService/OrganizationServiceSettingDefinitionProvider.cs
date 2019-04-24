using Volo.Abp.Settings;

namespace OrganizationService
{
    /* These setting definitions will be visible to clients that has a OrganizationService.Application.Contracts
     * reference. Settings those should be hidden from clients should be defined in the OrganizationService.Application
     * package.
     */
    public class AbpOrganizationettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    AbpOrganizationettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}