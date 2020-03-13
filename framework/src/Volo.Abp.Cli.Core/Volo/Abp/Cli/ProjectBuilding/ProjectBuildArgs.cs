﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class ProjectBuildArgs
    {
        [NotNull]
        public SolutionName SolutionName { get; }

        [CanBeNull]
        public string TemplateName { get; set; }

        [CanBeNull]
        public string Version { get; set; }

        public DatabaseProvider DatabaseProvider { get; set; }

        public UiFramework UiFramework { get; set; }

        public MobileApp? MobileApp { get; set; }

        [CanBeNull]
        public string AbpGitHubLocalRepositoryPath { get; set; }

        [CanBeNull]
        public string TemplateSource { get; set; }

        [NotNull]
        public Dictionary<string, string> ExtraProperties { get; set; }

        public ProjectBuildArgs(
            [NotNull] SolutionName solutionName,
            [CanBeNull] string templateName = null,
            [CanBeNull] string version = null,
            DatabaseProvider databaseProvider = DatabaseProvider.NotSpecified,
            UiFramework uiFramework = UiFramework.NotSpecified,
            MobileApp? mobileApp = null,
            [CanBeNull] string abpGitHubLocalRepositoryPath = null,
            [CanBeNull] string templateSource = null,
            Dictionary<string, string> extraProperties = null)
        {
            SolutionName = Check.NotNull(solutionName, nameof(solutionName));
            TemplateName = templateName;
            Version = version;
            DatabaseProvider = databaseProvider;
            UiFramework = uiFramework;
            MobileApp = mobileApp;
            AbpGitHubLocalRepositoryPath = abpGitHubLocalRepositoryPath;
            TemplateSource = templateSource;
            ExtraProperties = extraProperties ?? new Dictionary<string, string>();
        }
    }
}