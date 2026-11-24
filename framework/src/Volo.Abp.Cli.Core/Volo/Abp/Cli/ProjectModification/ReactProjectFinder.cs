using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Volo.Abp.Cli.LIbs;

namespace Volo.Abp.Cli.ProjectModification;

public static class ReactProjectFinder
{
    private static readonly string[] ExcludedDirectoryNames =
    {
        "node_modules",
        ".git",
        ".idea",
        ".vs",
        "_templates",
        "bin",
        "obj"
    };

    /// <summary>
    /// Returns the directories of the React projects in the given directory and its subdirectories.
    /// The folders of a React project are not searched: what is in them (workspace packages,
    /// downloaded source code) belongs to that project.
    /// </summary>
    public static List<string> Find(string rootDirectory)
    {
        var reactProjectDirectories = new List<string>();
        var directoriesToSearch = new Stack<string>();
        directoriesToSearch.Push(rootDirectory);

        while (directoriesToSearch.Count > 0)
        {
            var directory = directoriesToSearch.Pop();

            if (IsReactProject(directory))
            {
                reactProjectDirectories.Add(directory);
                continue;
            }

            foreach (var subDirectory in Directory.GetDirectories(directory))
            {
                if (!ExcludedDirectoryNames.Contains(Path.GetFileName(subDirectory), StringComparer.OrdinalIgnoreCase))
                {
                    directoriesToSearch.Push(subDirectory);
                }
            }
        }

        return reactProjectDirectories.OrderBy(d => d, StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <summary>
    /// A React project is a folder whose package.json depends on React and that is neither a .NET project
    /// nor an Angular project (the same rule the install-libs command uses).
    /// </summary>
    public static bool IsReactProject(string directory)
    {
        if (File.Exists(Path.Combine(directory, "angular.json")) ||
            Directory.GetFiles(directory, "*.csproj", SearchOption.TopDirectoryOnly).Any())
        {
            return false;
        }

        try
        {
            return JavaScriptFrameworkDetector.Detect(Path.Combine(directory, "package.json")) ==
                   JavaScriptFrameworkType.React;
        }
        catch (JsonException)
        {
            // A package.json that cannot be read cannot be recognized as a React project.
            return false;
        }
    }
}
