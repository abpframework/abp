using System;
using Volo.Utils.SolutionTemplating.Files;

namespace Volo.Utils.SolutionTemplating.Building.Steps
{
    public class SwitchEntityFrameworkCoreToMongoDbStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            ChangeProjectReference(context);
            ChangeWebModuleUsage(context);
            ChangeConnectionString(context);
        }

        private void ChangeProjectReference(ProjectBuildContext context)
        {
            var file = GetFile(context, "/src/MyCompanyName.MyProjectName.Web/MyCompanyName.MyProjectName.Web.csproj");

            file.NormalizeLineEndings();

            var lines = file.GetLines();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("ProjectReference") && lines[i].Contains("MyCompanyName.MyProjectName.EntityFrameworkCore"))
                {
                    lines[i] = lines[i].Replace("EntityFrameworkCore", "MongoDB");
                    file.SetLines(lines);
                    return;
                }
            }

            throw new ApplicationException("Could not find the 'Default' connection string in appsettings.json file!");
        }

        private void ChangeWebModuleUsage(ProjectBuildContext context)
        {
            var file = GetFile(context, "/src/MyCompanyName.MyProjectName.Web/MyProjectNameWebModule.cs");

            file.NormalizeLineEndings();

            file.RemoveTemplateCodeIfNot("EntityFrameworkCore");

            var lines = file.GetLines();

            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("using MyCompanyName.MyProjectName.EntityFrameworkCore"))
                {
                    lines[i] = "using MyCompanyName.MyProjectName.MongoDb;";
                }
                else if (lines[i].Contains("MyProjectNameEntityFrameworkCoreModule"))
                {
                    lines[i] = lines[i].Replace("MyProjectNameEntityFrameworkCoreModule", "MyProjectNameMongoDbModule");
                }
            }

            file.SetLines(lines);
        }

        private void ChangeConnectionString(ProjectBuildContext context)
        {
            var file = GetFile(context, "/src/MyCompanyName.MyProjectName.Web/appsettings.json");

            file.NormalizeLineEndings();

            var lines = file.GetLines();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Default") && lines[i].Contains("Database"))
                {
                    lines[i] = "    \"Default\": \"mongodb://localhost:27017|MyProjectName\"";
                    file.SetLines(lines);
                    return;
                }
            }

            throw new ApplicationException("Could not find the 'Default' connection string in appsettings.json file!");
        }
    }
}