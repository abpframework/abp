using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class AbpModuleClassFinder : ITransientDependency
    {
        public virtual List<string> Find(string csprojFilePath)
        {
            var moduleFilePaths = new List<string>();

            var csFiles = new DirectoryInfo(Path.GetDirectoryName(csprojFilePath))
                .GetFiles("*Module.cs", SearchOption.AllDirectories) //TODO: Module assumption is not so good!
                .Select(f => f.FullName)
                .ToList();

            foreach (var csFile in csFiles)
            {
                if (IsDerivedFromAbpModule(csFile))
                {
                    moduleFilePaths.Add(csFile);
                }
            }

            return moduleFilePaths;
        }

        private bool IsDerivedFromAbpModule(string csFile)
        {
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(csFile)).GetRoot();
            var namespaceSyntax = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().First();
            var classDeclaration = (namespaceSyntax.DescendantNodes().OfType<ClassDeclarationSyntax>()).First();

            return classDeclaration.BaseList?.Types
                .Any(t => t.ToString().Contains("AbpModule")) ?? false;
        }
    }
}
