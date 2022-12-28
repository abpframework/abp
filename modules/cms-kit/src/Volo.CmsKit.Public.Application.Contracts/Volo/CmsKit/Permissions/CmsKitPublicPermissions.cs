namespace Volo.CmsKit.Permissions;

public static class CmsKitPublicPermissions
{
    public const string GroupName = "CmsKitPublic";

    public static class Comments
    {
        public const string Default = GroupName + ".Comments";
        public const string DeleteAll = Default + ".DeleteAll";
    }
}
