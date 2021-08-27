using System.Threading.Tasks;

namespace Volo.Abp.Studio.Nuget
{
    public static class TemplateNugetPackageInfoProvider
    {
        public static string GetNugetPackageName(string template)
        {
            switch (template)
            {
                case "app":
                    return "Cotur.Basic.Template"; // todo: replace with real template!
                default:
                    return null;
            }
        }
    }
}
