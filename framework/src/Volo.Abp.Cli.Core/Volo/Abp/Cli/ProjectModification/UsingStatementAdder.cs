using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class UsingStatementAdder : ITransientDependency
    {
        public string Add(string fileContent, string nameSpace)
        {
            if (fileContent.Contains($" {nameSpace};"))
            {
                return fileContent;
            }

            var index = GetIndexOfTheEndOfTheLastUsingStatement(fileContent);

            if (index < 0 || index >= fileContent.Length)
            {
                index = 0;
            }

            var usingStatement = GetUsingStatement(nameSpace);

            return fileContent.Insert(index, usingStatement);
        }

        protected virtual string GetUsingStatement(string nameSpace)
        {
            return Environment.NewLine + "using " + nameSpace + ";";
        }

        protected virtual int GetIndexOfTheEndOfTheLastUsingStatement(string fileContent)
        {
            var indexOfNamespaceDeclaration = fileContent.IndexOf("namespace", StringComparison.Ordinal);

            if (indexOfNamespaceDeclaration < 0)
            {
                return 0;
            }

            fileContent = fileContent.Substring(0, indexOfNamespaceDeclaration);

            var indexOfTheStartOfLastUsingStatement =
                fileContent.LastIndexOf("using ", StringComparison.Ordinal);

            if (indexOfTheStartOfLastUsingStatement < 0)
            {
                return 0;
            }

            var indexOfFirstSemiColonAfterLastUsingStatement =
                fileContent.Substring(indexOfTheStartOfLastUsingStatement).IndexOf(';');

            if (indexOfFirstSemiColonAfterLastUsingStatement < 0)
            {
                return 0;
            }

            return indexOfTheStartOfLastUsingStatement
                   + indexOfFirstSemiColonAfterLastUsingStatement + 1;
        }
    }
}
