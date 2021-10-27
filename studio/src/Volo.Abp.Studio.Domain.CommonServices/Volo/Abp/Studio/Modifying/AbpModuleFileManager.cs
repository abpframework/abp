using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Studio.Packages.Modifying
{
    public class AbpModuleFileManager : IAbpModuleFileManager, ITransientDependency
    {
        public IFileSystem FileSystem { get; }

        public AbpModuleFileManager(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        public async Task AddDependency(string filePath, string moduleToAdd)
        {
            var fileContent = await FileSystem.File.ReadAllTextAsync(filePath);
            var moduleName = moduleToAdd.Split(".").Last();
            var moduleNamespace = moduleToAdd.RemovePostFix("." + moduleName);
            var usingStatement = $"using {moduleNamespace};";
            var dependsOnStart = $"DependsOn(";

            if (fileContent.Contains($"typeof({moduleName})"))
            {
                // module already added
                return;
            }

            if (!fileContent.NormalizeLineEndings().SplitToLines().Any(l=> l.Trim().StartsWith("namespace ") && l.Contains(moduleNamespace)) &&
                !fileContent.Contains(usingStatement)
                )
            {
                fileContent = usingStatement + Environment.NewLine + fileContent;
            }

            if (!fileContent.Contains(dependsOnStart))
            {
                var fileLines = fileContent.NormalizeLineEndings().SplitToLines().ToList();
                fileLines.InsertBefore(l=> l.Contains("public") && l.Contains("class"), $"    [DependsOn(typeof({moduleName}))]");
                fileContent = fileLines.JoinAsString(Environment.NewLine);
            }
            else
            {
                var fileLines = fileContent.NormalizeLineEndings().SplitToLines().ToList();
                var dependsOnStartLine = fileLines.First(l=> l.Contains(dependsOnStart));
                var dependsOnStartLineModified = dependsOnStartLine.Replace(dependsOnStart,
                    dependsOnStart + Environment.NewLine +
                    $"        typeof({moduleName}),"
                );
                fileContent = fileContent.Replace(dependsOnStartLine, dependsOnStartLineModified);
            }

            FileSystem.File.WriteAllTextAsync(filePath, fileContent);
        }

        public async Task<string> ExtractModuleNameWithNamespace(string filePath)
        {
            var fileContent = await FileSystem.File.ReadAllTextAsync(filePath);

            var fileLines = fileContent.NormalizeLineEndings().SplitToLines();

            var lineOfClassName = fileLines.First(l=> l.Contains("public") && l.Contains("class"));

            var moduleName = lineOfClassName.Split(":")[0].Trim().Split(" ").Last();

            var lineOfNamespace = fileLines.First(l=> l.Trim().StartsWith("namespace"));

            var moduleNamespace = lineOfNamespace.Split(" ").Skip(1).First();

            return moduleNamespace + "." + moduleName;
        }
    }
}
