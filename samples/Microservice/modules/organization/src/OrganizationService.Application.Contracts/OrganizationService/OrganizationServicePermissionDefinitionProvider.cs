using OrganizationService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace OrganizationService
{
    public class OrganizationServicePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var baseManagementGroup = context.AddGroup(OrganizationServicePermissions.GroupName, L("Permission:OrganizationService"));

            var baseTypes = baseManagementGroup.AddPermission(OrganizationServicePermissions.AbpOrganizations.Default, L("Permission:AbpOrganizations"));
            baseTypes.AddChild(OrganizationServicePermissions.AbpOrganizations.Update, L("Permission:AbpOrganizations:Edit"));
            baseTypes.AddChild(OrganizationServicePermissions.AbpOrganizations.Delete, L("Permission:AbpOrganizations:Delete"));
            baseTypes.AddChild(OrganizationServicePermissions.AbpOrganizations.Create, L("Permission:AbpOrganizations:Create"));

   
        }

        private static LocalizableString L(string name)
        {
             return LocalizableString.Create<OrganizationServiceResource>(name);
        }
    }
}