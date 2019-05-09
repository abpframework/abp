using System;
using System.IO;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ProjectModification
{
    public class ModuleClassDependcyAdder : ITransientDependency
    {
        public virtual void Add(string path, string module)
        {
            ParseModuleNameAndNameSpace(module, out var nameSpace, out var moduleName);

            var file = File.ReadAllText(path);

            var indexOfEndingPublicClass = file.IndexOf("public class", StringComparison.Ordinal);

            var dependsOnAttribute = "[DependsOn(" + moduleName + ")]" + Environment.NewLine + "    ";

            file = file.Insert(indexOfEndingPublicClass, dependsOnAttribute);
            file = file.Insert(0, GetUsingStatement(nameSpace) + Environment.NewLine); //TODO: Add as the last item in the using list!

            File.WriteAllText(path, file);
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

        protected virtual string GetUsingStatement(string nameSpace)
        {
            return "using " + nameSpace + ";";
        }
    }
}
