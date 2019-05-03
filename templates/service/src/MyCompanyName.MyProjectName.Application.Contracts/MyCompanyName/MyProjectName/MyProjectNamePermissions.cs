using Volo.Abp.Reflection;

namespace MyCompanyName.MyProjectName
{
    public class MyProjectNamePermissions
    {
        public const string GroupName = "MyProjectName";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(MyProjectNamePermissions));
        }
    }
}