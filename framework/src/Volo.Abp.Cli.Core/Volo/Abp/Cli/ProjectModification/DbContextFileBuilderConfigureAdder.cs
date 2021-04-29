using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class DbContextFileBuilderConfigureAdder : ITransientDependency
    {
        public ILogger<DbContextFileBuilderConfigureAdder> Logger { get; set; }

        protected UsingStatementAdder UsingStatementAdder { get; }

        public DbContextFileBuilderConfigureAdder(UsingStatementAdder usingStatementAdder)
        {
            UsingStatementAdder = usingStatementAdder;
            Logger = NullLogger<DbContextFileBuilderConfigureAdder>.Instance;
        }

        public bool Add(string path, string moduleConfiguration)
        {
            var file = File.ReadAllText(path);

            var parsedModuleConfiguration = moduleConfiguration.Split(", ");

            var namespaces = parsedModuleConfiguration.Select(GetNamespace);
            var configurationLines = parsedModuleConfiguration.Select(GetLineToAdd);

            var indexToInsert = FindIndexToInsert(file);

            if (indexToInsert <= 0 || indexToInsert >= file.Length)
            {
                Logger.LogWarning($"\"OnModelCreating(ModelBuilder builder)\" method couldn't be found in {path}");
                return false;
            }

            foreach (var configurationLine in configurationLines)
            {
                if (file.Contains(configurationLine))
                {
                    continue;
                }

                file = file.Insert(indexToInsert, "    " + configurationLine + Environment.NewLine + "        ");
            }

            foreach (var namespaceOfConfiguration in namespaces)
            {
                file = UsingStatementAdder.Add(file, namespaceOfConfiguration);
            }

            File.WriteAllText(path, file);
            return true;
        }

        protected int FindIndexToInsert(string file)
        {
            var indexOfMethodDeclaration = file.IndexOf("OnModelCreating(", StringComparison.Ordinal);
            var indexOfOpeningBracket =
                indexOfMethodDeclaration + file.Substring(indexOfMethodDeclaration).IndexOf('{');

            var stack = 1;
            var index = indexOfOpeningBracket;

            while (stack > 0)
            {
                index++;

                if (index >= file.Length)
                {
                    break;
                }

                if (file[index] == '{')
                {
                    stack++;
                }
                else if (file[index] == '}')
                {
                    stack--;
                }
            }

            return index;
        }

        protected string GetLineToAdd(string moduleConfiguration)
        {
            return "builder." + moduleConfiguration.Split(':')[1] + "();";
        }

        protected string GetNamespace(string moduleConfiguration)
        {
            return string.Join(".", moduleConfiguration.Split(':')[0]);
        }
    }
}
