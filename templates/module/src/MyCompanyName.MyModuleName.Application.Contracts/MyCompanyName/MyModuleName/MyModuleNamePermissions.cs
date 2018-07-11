namespace MyCompanyName.MyModuleName
{
    public class MyModuleNamePermissions
    {
        public const string GroupName = "MyModuleName";

        public static string[] GetAll()
        {
            return new[]
            {
                GroupName
            };
        }
    }
}