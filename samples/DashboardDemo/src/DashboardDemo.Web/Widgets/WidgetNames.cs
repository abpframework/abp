using Volo.Abp.Reflection;

namespace DashboardDemo.Widgets
{
    public static class WidgetNames
    {
        public const string MyWidget = "MyWidget";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(WidgetNames));
        }
    }
}
