namespace Volo.Abp.Cli.ProjectBuilding.Analyticses
{
    public class CliAnalyticsCollectInputDto
    {
        public string Tool { get; set; }

        public string TemplateName { get; set; }

        public string TemplateVersion { get; set; }

        public string Command { get; set; }

        public string DatabaseProvider { get; set; }

        public string ProjectName { get; set; }

        public bool? IsTiered { get; set; }

        public string UiFramework { get; set; }

        public string Options { get; set; }

        public string Language { get; set; }

        public string Ip { get; set; }
    }
}