using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding;

public class ProjectBuildArgs
{
    [NotNull]
    public SolutionName SolutionName { get; }

    [CanBeNull]
    public string TemplateName { get; set; }

    [CanBeNull]
    public string Version { get; set; }

    public DatabaseProvider DatabaseProvider { get; set; }

    public DatabaseManagementSystem DatabaseManagementSystem { get; set; }

    public UiFramework UiFramework { get; set; }

    public MobileApp? MobileApp { get; set; }

    public bool PublicWebSite { get; set; }

    [CanBeNull]
    public string AbpGitHubLocalRepositoryPath { get; set; }

    [CanBeNull]
    public string VoloGitHubLocalRepositoryPath { get; set; }

    [CanBeNull]
    public string TemplateSource { get; set; }

    [CanBeNull]
    public string ConnectionString { get; set; }

    [NotNull]
    public string OutputFolder { get; set; }

    [NotNull]
    public Dictionary<string, string> ExtraProperties { get; set; }

    public ProjectBuildArgs(
        [NotNull] SolutionName solutionName,
        [CanBeNull] string templateName = null,
        [CanBeNull] string version = null,
        string outputFolder = null,
        DatabaseProvider databaseProvider = DatabaseProvider.NotSpecified,
        DatabaseManagementSystem databaseManagementSystem = DatabaseManagementSystem.NotSpecified,
        UiFramework uiFramework = UiFramework.NotSpecified,
        MobileApp? mobileApp = null,
        bool publicWebSite = false,
        [CanBeNull] string abpGitHubLocalRepositoryPath = null,
        [CanBeNull] string voloGitHubLocalRepositoryPath = null,
        [CanBeNull] string templateSource = null,
        Dictionary<string, string> extraProperties = null,
        [CanBeNull] string connectionString = null)
    {
        SolutionName = Check.NotNull(solutionName, nameof(solutionName));
        TemplateName = templateName;
        Version = version;
        OutputFolder = outputFolder;
        DatabaseProvider = databaseProvider;
        DatabaseManagementSystem = databaseManagementSystem;
        UiFramework = uiFramework;
        MobileApp = mobileApp;
        PublicWebSite = publicWebSite;
        AbpGitHubLocalRepositoryPath = abpGitHubLocalRepositoryPath;
        VoloGitHubLocalRepositoryPath = voloGitHubLocalRepositoryPath;
        TemplateSource = templateSource;
        ExtraProperties = extraProperties ?? new Dictionary<string, string>();
        ConnectionString = connectionString;
    }
}
