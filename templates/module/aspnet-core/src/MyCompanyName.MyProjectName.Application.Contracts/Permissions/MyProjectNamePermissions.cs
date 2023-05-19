using Volo.Abp.Reflection;

namespace MyCompanyName.MyProjectName.Permissions;

public class MyProjectNamePermissions
{
    public const string GroupName = "MyProjectName";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(MyProjectNamePermissions));
    }
}
