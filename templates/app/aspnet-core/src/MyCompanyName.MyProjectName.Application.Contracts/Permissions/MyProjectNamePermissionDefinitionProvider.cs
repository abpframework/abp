using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MyCompanyName.MyProjectName.Permissions;

public class MyProjectNamePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MyProjectNamePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MyProjectNamePermissions.MyPermission1, L("Permission:MyPermission1"));

        var booksPermission = myGroup.AddPermission(MyProjectNamePermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(MyProjectNamePermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(MyProjectNamePermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(MyProjectNamePermissions.Books.Delete, L("Permission:Books.Delete"));
        booksPermission.AddChild(MyProjectNamePermissions.Books.ManagePermissions, L("Permission:Books.ManagePermissions"));

        AddBookStoreResourcePermissions(context);

    }

    protected virtual void AddBookStoreResourcePermissions(IPermissionDefinitionContext context)
    {
        context.AddResourcePermission(
            MyProjectNamePermissions.Books.Resources.ChangeName,
            MyProjectNamePermissions.Books.Resources.Name,
            MyProjectNamePermissions.Books.ManagePermissions,
            L("Change Book Name")
        );

        context.AddResourcePermission(
            MyProjectNamePermissions.Books.Resources.ChangeType,
            MyProjectNamePermissions.Books.Resources.Name,
            MyProjectNamePermissions.Books.ManagePermissions,
            L("Change Book Type")
        );

        context.AddResourcePermission(
            MyProjectNamePermissions.Books.Resources.ChangePrice,
            MyProjectNamePermissions.Books.Resources.Name,
            MyProjectNamePermissions.Books.ManagePermissions,
            L("Change Book Price")
        );

        context.AddResourcePermission(
            MyProjectNamePermissions.Books.Resources.ChangeAuthor,
            MyProjectNamePermissions.Books.Resources.Name,
            MyProjectNamePermissions.Books.ManagePermissions,
            L("Change Book Author")
        );
    }



    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MyProjectNameResource>(name);
    }
}
