using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class DerivedClassFinder : ITransientDependency
{
    public ILogger<DerivedClassFinder> Logger { get; set; }

    public DerivedClassFinder()
    {
        Logger = NullLogger<DerivedClassFinder>.Instance;
    }

    public virtual List<string> Find(string csprojFilePath, string baseClass)
    {
        var moduleFilePaths = new List<string>();
        var csprojFileDirectory = Path.GetDirectoryName(csprojFilePath);
        var binFile = Path.Combine(csprojFileDirectory, "bin");
        var objFile = Path.Combine(csprojFileDirectory, "obj");

        var csFiles = new DirectoryInfo(csprojFileDirectory)
            .GetFiles("*.cs", SearchOption.AllDirectories)
            .Where(f => !f.FullName.StartsWith(binFile, StringComparison.OrdinalIgnoreCase) &&
                        !f.FullName.StartsWith(objFile, StringComparison.OrdinalIgnoreCase))
            .Select(f => f.FullName)
            .ToList();

        foreach (var csFile in csFiles)
        {
            try
            {
                if (IsDerived(csFile, baseClass))
                {
                    moduleFilePaths.Add(csFile);
                }
            }
            catch (Exception)
            {
                Logger.LogDebug($"Couldn't parse {csFile}.");
            }
        }

        return moduleFilePaths;
    }

    protected bool IsDerived(string csFile, string baseClass)
    {
        var csFileText = File.ReadAllText(csFile);
        if (!csFileText.Contains("class"))
        {
            return false;
        }

        var root = CSharpSyntaxTree.ParseText(csFileText).GetRoot();
        var namespaceSyntax = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
        var classDeclaration = (namespaceSyntax?.DescendantNodes().OfType<ClassDeclarationSyntax>())?.FirstOrDefault();

        if (classDeclaration == null)
        {
            classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
        }

        var baseTypeList = classDeclaration.BaseList?.Types.Select(t => t.ToString()).ToList();

        if (baseTypeList == null)
        {
            return false;
        }

        foreach (var baseType in baseTypeList)
        {
            if (baseType.Contains('<') && baseType.Substring(0, baseType.IndexOf('<')) == baseClass)
            {
                return true;
            }
            if (baseType == baseClass)
            {
                return true;
            }
        }

        return false;
    }
}
