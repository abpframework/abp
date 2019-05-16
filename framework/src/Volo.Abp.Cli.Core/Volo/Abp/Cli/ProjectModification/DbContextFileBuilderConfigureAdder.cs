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
        protected UsingStatementAdder UsingStatementAdder { get; }
        public ILogger<DbContextFileBuilderConfigureAdder> Logger { get; set; }

        public DbContextFileBuilderConfigureAdder(UsingStatementAdder usingStatementAdder)
        {
            UsingStatementAdder = usingStatementAdder;
            Logger = NullLogger<DbContextFileBuilderConfigureAdder>.Instance;
        }

        public void Add(string path, string moduleConfiguration)
        {
            var file = File.ReadAllText(path);

            file = UsingStatementAdder.Add(file, GetNamespace(moduleConfiguration));

            var stringToAdd = GetLineToAdd(moduleConfiguration);
            if (!file.Contains(stringToAdd))
            {
                var indexToInsert = FindIndexToInsert(file);

                if (indexToInsert <= 0 || indexToInsert >= file.Length)
                {
                    Logger.LogWarning($"\"OnModelCreating(ModelBuilder builder)\" method couldn't be found in {path}");
                    return;
                }
                file = file.Insert(indexToInsert, "    " + stringToAdd + Environment.NewLine + "        ");
            }


            File.WriteAllText(path, file);
        }

        protected int FindIndexToInsert(string file)
        {
            var indexOfMethodDeclaration = file.IndexOf("OnModelCreating(", StringComparison.Ordinal);
            var indexOfOpeningBracket = indexOfMethodDeclaration + file.Substring(indexOfMethodDeclaration).IndexOf('{');

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
            return "builder." + moduleConfiguration.Split('.').Last() + "();";
        }

        protected string GetNamespace(string moduleConfiguration)
        {
            return string.Join(".", moduleConfiguration.Split('.').Reverse().Skip(2));
        }
    }
}
