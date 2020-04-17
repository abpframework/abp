using ProductManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ProductManagement
{
    public class ProductManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var productManagementGroup = context.AddGroup(ProductManagementPermissions.GroupName, L("Permission:ProductManagement"));

            var products = productManagementGroup.AddPermission(ProductManagementPermissions.Products.Default, L("Permission:Products"));
            products.AddChild(ProductManagementPermissions.Products.Update, L("Permission:Edit"));
            products.AddChild(ProductManagementPermissions.Products.Delete, L("Permission:Delete"));
            products.AddChild(ProductManagementPermissions.Products.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ProductManagementResource>(name);
        }
    }
}