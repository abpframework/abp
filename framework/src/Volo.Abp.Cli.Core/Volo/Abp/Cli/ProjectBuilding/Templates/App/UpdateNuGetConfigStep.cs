using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public class UpdateNuGetConfigStep : ProjectBuildPipelineStep
    {
        private readonly string _nugetConfigFilePath;

        public UpdateNuGetConfigStep(string nugetConfigFilePath)
        {
            _nugetConfigFilePath = nugetConfigFilePath;
        }

        public override void Execute(ProjectBuildContext context)
        {
            var file = context.Files.FirstOrDefault(f => f.Name == _nugetConfigFilePath);
            if (file == null)
            {
                return;
            }

            var apiKey = context.BuildArgs.ExtraProperties.GetOrDefault("api-key");
            if (apiKey.IsNullOrEmpty())
            {
                return;
            }

            const string placeHolder = "<!-- {ABP_COMMERCIAL_NUGET_SOURCE} -->";
            var nugetSourceTag = $"<add key=\"ABP Commercial NuGet Source\" value=\"https://nuget.abp.io/{apiKey}/v3/index.json\" />";

            file.ReplaceText(placeHolder, nugetSourceTag);
        }
    }
}