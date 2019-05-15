using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class DerivedClassFinder : ITransientDependency
    {
        public virtual List<string> Find(string csprojFilePath, string baseClass)
        {
            var moduleFilePaths = new List<string>();

            var csFiles = new DirectoryInfo(Path.GetDirectoryName(csprojFilePath))
                .GetFiles("*.cs", SearchOption.AllDirectories)
                .Select(f => f.FullName)
                .ToList();

            foreach (var csFile in csFiles)
            {
                if (IsDerivedFromAbpModule(csFile, baseClass))
                {
                    moduleFilePaths.Add(csFile);
                }
            }

            return moduleFilePaths;
        }

        protected bool IsDerivedFromAbpModule(string csFile, string baseClass)
        {
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(csFile)).GetRoot();
            var namespaceSyntax = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().First();
            var classDeclaration = (namespaceSyntax.DescendantNodes().OfType<ClassDeclarationSyntax>()).First();

            return classDeclaration.BaseList?.Types
                .Any(t => t.ToString().Equals(baseClass)) ?? false;
        }
    }
}
