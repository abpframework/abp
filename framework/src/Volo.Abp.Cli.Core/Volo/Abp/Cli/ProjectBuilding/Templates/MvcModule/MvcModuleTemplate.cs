﻿using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule
{
    public class MvcModuleTemplate : TemplateInfo
    {
        /// <summary>
        /// "mvc-module".
        /// </summary>
        public const string TemplateName = "mvc-module";

        public MvcModuleTemplate()
            : base(TemplateName)
        {
            DocumentUrl = "https://docs.abp.io/en/abp/latest/Startup-Templates/Mvc-Module";
        }

        public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            var steps = new List<ProjectBuildPipelineStep>();

            DeleteUnrelatedProjects(context, steps);

            steps.Add(new TemplateRandomSslPortStep(new List<string>
            {
                "https://localhost:44300",
                "https://localhost:44301",
                "https://localhost:44302",
                "https://localhost:44303"
            }));

            return steps;
        }

        private void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey("no-ui"))
            {
                steps.Add(new RemoveProjectFromSolutionStep(
                    "MyCompanyName.MyProjectName.Web"
                ));

                steps.Add(new RemoveProjectFromSolutionStep(
                    "MyCompanyName.MyProjectName.Web.Host",
                    projectFolderPath: "/host/MyCompanyName.MyProjectName.Web.Host"
                ));

                steps.Add(new RemoveProjectFromSolutionStep(
                    "MyCompanyName.MyProjectName.Web.Unified",
                    projectFolderPath: "/host/MyCompanyName.MyProjectName.Web.Unified"
                ));
            }
        }
    }
}
