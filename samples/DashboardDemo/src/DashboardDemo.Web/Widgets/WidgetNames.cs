using Volo.Abp.Reflection;

namespace DashboardDemo.Widgets
{
    public static class WidgetNames
    {
        public const string MyWidget = "MyWidget";
        public const string DemoStatistics = "DemoStatistics";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(WidgetNames));
        }
    }
}
