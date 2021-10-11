using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public class AppTemplateChangeDbMigratorPortSettingsStep : ProjectBuildPipelineStep
    {
        public string IdentityServerPort { get; }

        /// <param name="identityServerPort"></param>
        public AppTemplateChangeDbMigratorPortSettingsStep(
            string identityServerPort)
        {
            IdentityServerPort = identityServerPort;
        }

        public override void Execute(ProjectBuildContext context)
        {
            context
                .GetFile("/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator/appsettings.json")
                .ReplaceText("44305", IdentityServerPort);
        }
    }
}
