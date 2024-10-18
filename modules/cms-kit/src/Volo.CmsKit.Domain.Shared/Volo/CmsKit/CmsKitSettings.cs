namespace Volo.CmsKit;

public static class CmsKitSettings
{
    private const string GroupName = "CmsKit";
    
    public static class Comments
    {
        private const string Default = GroupName + ".Comments";
        public const string RequireApprovement = Default + ".RequireApprovement";
    } 
}
