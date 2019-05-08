using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Volo.Abp.ProjectModification
{
    public class AbpModuleFinder
    {
        public List<string> Find(string csprojFilePath)
        {
            var modules = new List<string>();

            var csFiles = GetAllCsFilesUnderDirectory(Path.GetDirectoryName(csprojFilePath), new List<string>());

            foreach (var csFile in csFiles)
            {
                try
                {
                    var root = CSharpSyntaxTree.ParseText(File.ReadAllText(csFile)).GetRoot();

                    var namespaceSyntaxs = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

                    foreach (var namespaceSyntax in namespaceSyntaxs)
                    {
                        var classDeclarationSyntaxs = namespaceSyntax.DescendantNodes().OfType<ClassDeclarationSyntax>();
                        var syntaxsArray = classDeclarationSyntaxs as ClassDeclarationSyntax[] ?? classDeclarationSyntaxs.ToArray();
                        
                        foreach (var classDeclaration in syntaxsArray)
                        {
                            var classDerivedFromAbpModule = classDeclaration.BaseList?.Types.FirstOrDefault(t => t.ToString().Contains("AbpModule"));

                            if (classDerivedFromAbpModule != null)
                            {
                                modules.Add(csFile);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //ignored!
                }
            }

            return modules;
        }

        private static List<string> GetAllCsFilesUnderDirectory(string path, List<string> allCsFileList)
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
