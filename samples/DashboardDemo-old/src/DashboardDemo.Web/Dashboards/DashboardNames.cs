using Volo.Abp.Reflection;

namespace DashboardDemo.Dashboards
{
    public static class DashboardNames
    {
        public const string MyDashboard = "MyDashboard";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(DashboardNames));
        }
    }
}
