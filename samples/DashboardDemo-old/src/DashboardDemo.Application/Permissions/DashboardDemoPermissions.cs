using System;
using Volo.Abp.Reflection;

namespace DashboardDemo.Permissions
{
    public static class DashboardDemoPermissions
    {
        public const string GroupName = "DashboardDemo";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public static string[] GetAll()
        {
            //Return an array of all permissions
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(DashboardDemoPermissions));
        }
    }
}