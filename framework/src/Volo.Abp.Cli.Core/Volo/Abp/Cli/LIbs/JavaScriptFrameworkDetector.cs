using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Volo.Abp.Cli.LIbs;

public static class JavaScriptFrameworkDetector
{
    /// <summary>
    /// Detects the JavaScript framework of a project from the dependencies in its package.json.
    /// Returns <see cref="JavaScriptFrameworkType.None"/> when the file doesn't exist or uses none of the known frameworks.
    /// Throws when the file is not a valid JSON document.
    /// </summary>
    public static JavaScriptFrameworkType Detect(string packageJsonFilePath)
    {
        if (!File.Exists(packageJsonFilePath))
        {
            return JavaScriptFrameworkType.None;
        }

        var packageJson = JObject.Parse(File.ReadAllText(packageJsonFilePath));

        var dependencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var dependencyListName in new[] { "dependencies", "devDependencies" })
        {
            if (packageJson[dependencyListName] is JObject dependencyList)
            {
                foreach (var dependency in dependencyList.Properties())
                {
                    dependencies.Add(dependency.Name);
                }
            }
        }

        // React Native and Next.js projects also depend on "react", so they are checked first.
        if (dependencies.Contains("react-native"))
        {
            return JavaScriptFrameworkType.ReactNative;
        }

        if (dependencies.Contains("next"))
        {
            return JavaScriptFrameworkType.NextJs;
        }

        if (dependencies.Contains("vue"))
        {
            return JavaScriptFrameworkType.Vue;
        }

        if (dependencies.Contains("react"))
        {
            return JavaScriptFrameworkType.React;
        }

        return JavaScriptFrameworkType.None;
    }
}
