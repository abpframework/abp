using Volo.Abp.Reflection;

namespace Volo.CmsKit.Permissions
{
    public class CmsKitAdminPermissions
    {
        public const string GroupName = "CmsKit";
        public static class Tags
        {
            public const string Default = GroupName + ".Tags";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class Contents
        {
            public const string Default = GroupName + ".Contents";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }
}
