using System;
using System.Diagnostics;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class EfCoreMigrationAdder : ITransientDependency
    {
        public void AddMigration(string csprojFile, string module, bool updateDatabase = true)
        {
            var moduleName = ParseModuleName(module);
            var migrationName = "Added_" + moduleName + "_Module" + GetUniquePostFix();

            var process = Process.Start("CMD.exe", "/C cd \"" + csprojFile + "\" & dotnet ef migrations add " + migrationName);
            process.WaitForExit();

            if (updateDatabase)
            {
                UpdateDatabase(csprojFile);
            }
        }

        protected void UpdateDatabase(string csprojFile)
        {
            var process = Process.Start("CMD.exe", "/C cd \"" + csprojFile + "\" & dotnet ef database update");
            process.WaitForExit();
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
    }
}
