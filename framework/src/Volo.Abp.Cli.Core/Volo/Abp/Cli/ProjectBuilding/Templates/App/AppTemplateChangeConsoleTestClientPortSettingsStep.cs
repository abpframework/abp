using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public class AppTemplateChangeConsoleTestClientPortSettingsStep : ProjectBuildPipelineStep
    {
        public string RemoteServicePort { get; }
        public string IdentityServerPort { get; }

        /// <param name="remoteServicePort"></param>
        /// <param name="identityServerPort">Assumed same as the <paramref name="remoteServicePort"/> if leaved as null.</param>
        public AppTemplateChangeConsoleTestClientPortSettingsStep(
            string remoteServicePort, 
            string identityServerPort = null)
        {
            RemoteServicePort = remoteServicePort;
            IdentityServerPort = identityServerPort ?? remoteServicePort;
        }

        public override void Execute(ProjectBuildContext context)
        {
            context
                .GetFile("/aspnet-core/test/MyCompanyName.MyProjectName.HttpApi.Client.ConsoleTestApp/appsettings.json")
                .ReplaceText("44300", RemoteServicePort)
                .ReplaceText("44301", IdentityServerPort);
        }
    }
}
