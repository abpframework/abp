namespace Abp.FeatureManagement
{
    public class FeatureManagementPermissions
    {
        public const string GroupName = "FeatureManagement";

        public static string[] GetAll()
        {
            return new[]
            {
                GroupName
            };
        }
    }
}