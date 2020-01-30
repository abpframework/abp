namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public class AppProTemplate : AppTemplateBase
    {
        /// <summary>
        /// "app-pro".
        /// </summary>
        public const string TemplateName = "app-pro";

        public AppProTemplate()
            : base(TemplateName)
        {
            DocumentUrl = "https://docs.abp.io"+ "/en/commercial/latest";
        }
    }
}