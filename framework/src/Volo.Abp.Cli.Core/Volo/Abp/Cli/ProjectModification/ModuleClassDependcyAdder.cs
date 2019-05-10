using System;
using System.IO;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class ModuleClassDependcyAdder : ITransientDependency
    {
        public virtual void Add(string path, string module)
        {
            ParseModuleNameAndNameSpace(module, out var nameSpace, out var moduleName);

            var file = File.ReadAllText(path);

            if (file.Contains(GetUsingStatement(nameSpace)) && file.Contains(moduleName))
            {
                return;
            }

            file = InsertDependsOnAttribute(file, moduleName);
            file = InsertUsingStatement(file, nameSpace);

            File.WriteAllText(path, file);
        }

        protected virtual string InsertDependsOnAttribute(string file, string moduleName)
        {
            var indexOfPublicClassDeclaration = GetIndexOfWhereDependsOnWillBeAdded(file);
            var dependsOnAttribute = GetDependsOnAttribute(moduleName);

            return file.Insert(indexOfPublicClassDeclaration, dependsOnAttribute);
        }

        protected virtual string InsertUsingStatement(string file, string nameSpace)
        {
            var indexOfTheEndOfTheLastUsingStatement = GetIndexOfTheEndOfTheLastUsingStatement(file);

            return file.Insert(indexOfTheEndOfTheLastUsingStatement, Environment.NewLine + GetUsingStatement(nameSpace));
        }

        protected virtual int GetIndexOfTheEndOfTheLastUsingStatement(string file)
        {
            var indexOfPublicClassDeclaration = GetIndexOfWhereDependsOnWillBeAdded(file);
            file = file.Substring(0, indexOfPublicClassDeclaration);

            var indexOfTheStartOfLastUsingStatement =
                file.LastIndexOf("using ", StringComparison.Ordinal);

            if (indexOfTheStartOfLastUsingStatement < 0)
            {
                return 0;
            }

            var indexOfFirstSemiColonAfterLastUsingStatement =
                file.Substring(indexOfTheStartOfLastUsingStatement).IndexOf(';');

            if (indexOfFirstSemiColonAfterLastUsingStatement < 0)
            {
                return 0;
            }

            return indexOfTheStartOfLastUsingStatement
                   + indexOfFirstSemiColonAfterLastUsingStatement + 1;
        }

        protected virtual int GetIndexOfWhereDependsOnWillBeAdded(string file)
        {
            var indexOfPublicClassDeclaration = file.IndexOf("public class", StringComparison.Ordinal);

            if (indexOfPublicClassDeclaration < 0)
            {
                throw new Exception("\"public class\" declaration not found!");
            }

            return indexOfPublicClassDeclaration;
        }

        protected virtual string GetDependsOnAttribute(string moduleName)
        {
            return "[DependsOn(" + moduleName + ")]" + Environment.NewLine + "    ";
        }

        protected virtual string GetUsingStatement(string nameSpace)
        {
            return "using " + nameSpace + ";";
        }

        protected virtual void ParseModuleNameAndNameSpace(string module, out string nameSpace, out string moduleName)
        {
            var words = module?.Split('.');

            if (words == null || words.Length <= 1)
            {
                nameSpace = null;
                moduleName = module;
                return;
            }

            moduleName = words[words.Length - 1];
            nameSpace = string.Join(".", words.Take(words.Length - 1));
        }
    }
}
