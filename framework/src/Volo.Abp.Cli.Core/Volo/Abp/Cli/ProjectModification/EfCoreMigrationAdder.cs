using System;
using System.IO;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class EfCoreMigrationAdder : ITransientDependency
    {
        public void AddMigration(string csprojFile, string module, string startupProject, bool updateDatabase = true)
        {
            var moduleName = ParseModuleName(module);
            var migrationName = "Added_" + moduleName + "_Module" + GetUniquePostFix();

            CmdHelper.RunCmd("cd \"" + Path.GetDirectoryName(csprojFile) + "\" & dotnet ef migrations add " + migrationName + GetStartupProjectOption(startupProject));

            if (updateDatabase)
            {
                UpdateDatabase(csprojFile, startupProject);
            }
        }

        protected void UpdateDatabase(string csprojFile, string startupProject)
        {
            CmdHelper.RunCmd("cd \"" + Path.GetDirectoryName(csprojFile) + "\" & dotnet ef database update" + GetStartupProjectOption(startupProject));
        }

        protected virtual string ParseModuleName(string fullModuleName)
        {
            var words = fullModuleName?.Split('.');

            if (words == null || words.Length <= 1)
            {
                return "";
            }

            return words[words.Length - 1];
        }

        protected virtual string GetUniquePostFix()
        {
            return "_" + new Random().Next(1,99999);
        }

        protected virtual string GetStartupProjectOption(string startupProject)
        {
            return startupProject.IsNullOrWhiteSpace() ? "" : $" -s {startupProject}";
        }
    }
}
