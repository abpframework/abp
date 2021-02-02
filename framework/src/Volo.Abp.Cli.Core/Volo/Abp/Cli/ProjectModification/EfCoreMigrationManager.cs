using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class EfCoreMigrationManager : ITransientDependency
    {
        public ILogger<EfCoreMigrationManager> Logger { get; set; }

        public EfCoreMigrationManager()
        {
            Logger = NullLogger<EfCoreMigrationManager>.Instance;
        }

        public void AddMigration(string dbMigrationsCsprojFile, string module, string startupProject)
        {
            var moduleName = ParseModuleName(module);
            var migrationName = "Added_" + moduleName + "_Module" + GetUniquePostFix();

            CmdHelper.RunCmd("cd \"" + Path.GetDirectoryName(dbMigrationsCsprojFile) +
                             "\" && dotnet ef migrations add " + migrationName +
                             GetStartupProjectOption(startupProject));
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
            return "_" + new Random().Next(1, 99999);
        }

        protected virtual string GetStartupProjectOption(string startupProject)
        {
            return startupProject.IsNullOrWhiteSpace() ? "" : $" -s {startupProject}";
        }
    }
}
