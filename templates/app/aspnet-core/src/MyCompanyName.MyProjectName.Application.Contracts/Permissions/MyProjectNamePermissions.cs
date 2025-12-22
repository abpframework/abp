namespace MyCompanyName.MyProjectName.Permissions;

public static class MyProjectNamePermissions
{
    public const string GroupName = "MyProjectName";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string ManagePermissions = Default + ".ManagePermissions";

        public static class Resources
        {
            public const string Name = "MyCompanyName.MyProjectName.Books.Book";
            public const string ChangeName = Name + ".ChangeName";
            public const string ChangeType = Name + ".ChangeType";
            public const string ChangeAuthor = Name + ".ChangeAuthor";
            public const string ChangePrice = Name + ".ChangePrice";
        }
    }
}
