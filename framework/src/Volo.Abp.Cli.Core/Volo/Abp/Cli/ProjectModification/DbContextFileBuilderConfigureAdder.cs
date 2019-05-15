using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class DbContextFileBuilderConfigureAdder : ITransientDependency
    {
        public void Add(string path, string moduleConfiguration)
        {
            var file = File.ReadAllText(path);

            var indexToInsert = FindIndexToInsert(file);
            var stringToAdd = GetLineToAdd(moduleConfiguration);

            stringToAdd = "    " + stringToAdd + Environment.NewLine;

            file = file.Insert(indexToInsert, stringToAdd);

            File.WriteAllText(path, file);
        }

        protected int FindIndexToInsert(string file)
        {
            var indexOfMethodDeclaration = file.IndexOf("OnModelCreating(", StringComparison.Ordinal);
            var indexOfOpeningBracket = indexOfMethodDeclaration + file.Substring(indexOfMethodDeclaration).IndexOf('{');

            var stack = 1;
            var index = indexOfOpeningBracket;

            while (stack > 0 || index < file.Length)
            {
                index++;
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
            return "builder.Configure" + moduleConfiguration + "();";
        }
    }
}
