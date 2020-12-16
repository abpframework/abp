namespace Volo.Abp.Cli.ProjectBuilding.Templates.Console
{
    public class ConsoleTemplate　: ConsoleTemplateBase
    {
        /// <summary>
        /// "console".
        /// </summary>
        public const string TemplateName = "console";

        public ConsoleTemplate()
            : base(TemplateName)
        {
            DocumentUrl = CliConsts.DocsLink + "/en/abp/latest/Startup-Templates/Console";
        }
    }
}
