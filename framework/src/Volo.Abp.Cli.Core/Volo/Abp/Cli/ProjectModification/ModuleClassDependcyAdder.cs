using System;
using System.IO;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class ModuleClassDependcyAdder : ITransientDependency
    {
        protected UsingStatementAdder UsingStatementAdder { get; }

        public ModuleClassDependcyAdder(UsingStatementAdder usingStatementAdder)
        {
            UsingStatementAdder = usingStatementAdder;
        }

        public virtual void Add(string path, string module)
        {
            ParseModuleNameAndNameSpace(module, out var nameSpace, out var moduleName);

            var file = File.ReadAllText(path);

            file = UsingStatementAdder.Add(file, nameSpace);

            if (!file.Contains(moduleName) )
            {
                file = InsertDependsOnAttribute(file, moduleName);
            }

            File.WriteAllText(path, file);
        }

        protected virtual string InsertDependsOnAttribute(string file, string moduleName)
        {
            var indexOfPublicClassDeclaration = GetIndexOfWhereDependsOnWillBeAdded(file);
            var dependsOnAttribute = GetDependsOnAttribute(moduleName);

            return file.Insert(indexOfPublicClassDeclaration, dependsOnAttribute);
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
            return "[DependsOn(typeof(" + moduleName + "))]" + Environment.NewLine + "    ";
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
