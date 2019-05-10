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

            var csFiles = GetAllCsFilesUnderDirectory(Path.GetDirectoryName(csprojFilePath), new List<string>());

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

        protected virtual List<string> GetAllCsFilesUnderDirectory(string path, List<string> allCsFileList)
        {
            var directory = new DirectoryInfo(path);
            var files = directory.GetFiles("*.cs").Select(f => f.DirectoryName + "\\" + f.Name).ToList();

            foreach (var s in files)
            {
                allCsFileList.Add(s);
            }

            var directories = directory.GetDirectories().Select(d => path + "\\" + d.Name).ToList();

            foreach (var subDirectory in directories)
            {
                allCsFileList = GetAllCsFilesUnderDirectory(subDirectory, allCsFileList);
            }

            return allCsFileList;
        }
    }
}
