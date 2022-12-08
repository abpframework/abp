﻿using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppTemplateChangeConsoleTestClientPortSettingsStep : ProjectBuildPipelineStep
{
    public string RemoteServicePort { get; }
    public string AuthServerPort { get; }

    /// <param name="remoteServicePort"></param>
    /// <param name="authServerPort">Assumed same as the <paramref name="remoteServicePort"/> if leaved as null.</param>
    public AppTemplateChangeConsoleTestClientPortSettingsStep(
        string remoteServicePort,
        string authServerPort = null)
    {
        RemoteServicePort = remoteServicePort;
        AuthServerPort = authServerPort ?? remoteServicePort;
    }

    public override void Execute(ProjectBuildContext context)
    {
        context
            .GetFile("/aspnet-core/test/MyCompanyName.MyProjectName.HttpApi.Client.ConsoleTestApp/appsettings.json")
            .ReplaceText("44300", RemoteServicePort)
            .ReplaceText("44301", AuthServerPort);
    }
}
