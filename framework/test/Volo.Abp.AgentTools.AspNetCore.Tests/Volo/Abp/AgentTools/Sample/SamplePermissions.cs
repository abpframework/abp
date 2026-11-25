using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.AgentTools.Sample;

public static class SamplePermissions
{
    public const string GroupName = "AgentToolsSample";

    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Reports
    {
        public const string Default = GroupName + ".Reports";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}

public class SamplePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(SamplePermissions.GroupName);
        var books = group.AddPermission(SamplePermissions.Books.Default);
        books.AddChild(SamplePermissions.Books.Create);
        books.AddChild(SamplePermissions.Books.Delete);
        var reports = group.AddPermission(SamplePermissions.Reports.Default);
        reports.AddChild(SamplePermissions.Reports.Create);
        reports.AddChild(SamplePermissions.Reports.Delete);
    }
}
