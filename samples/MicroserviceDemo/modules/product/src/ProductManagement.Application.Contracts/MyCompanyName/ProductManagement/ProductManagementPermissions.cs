namespace ProductManagement
{
    public class ProductManagementPermissions
    {
        public const string GroupName = "ProductManagement";

        public static string[] GetAll()
        {
            return new[]
            {
                GroupName
            };
        }
    }
}