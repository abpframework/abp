namespace Volo.Abp.Cli.ProjectBuilding.Building
{
    public static class UiFrameworkExtensions
    {
        public static string ToFrameworkName(this UiFramework uiFramework)
        {
            switch (uiFramework)
            {
                case UiFramework.Mvc: return "mvc";
                case UiFramework.Angular: return "angular";
                case UiFramework.NotSpecified: return "NotSpecified";
                default: return "NotSpecified";
            }
        }
    }
}