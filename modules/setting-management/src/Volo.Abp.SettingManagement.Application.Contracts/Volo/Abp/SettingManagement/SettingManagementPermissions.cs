using Volo.Abp.Reflection;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementPermissions
    {
        public const string GroupName = "SettingManagement";

        public const string Emailing = GroupName + ".Emailing";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(SettingManagementPermissions));
        }
    }
}
