using Volo.Abp.Reflection;

namespace Volo.CmsKit.Permissions;

public class CmsKitAdminPermissions
{
    public const string GroupName = "CmsKit";

    public static class Comments
    {
        public const string Default = GroupName + ".Comments";
        public const string Delete = Default + ".Delete";
    }

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

    public static class Pages
    {
        public const string Default = GroupName + ".Pages";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Blogs
    {
        public const string Default = GroupName + ".Blogs";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Features = Default + ".Features";
    }

    public static class BlogPosts
    {
        public const string Default = GroupName + ".BlogPosts";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Menus
    {
        public const string Default = GroupName + ".Menus";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class GlobalResources
    {
        public const string Default = GroupName + ".Menus";
    }
}
