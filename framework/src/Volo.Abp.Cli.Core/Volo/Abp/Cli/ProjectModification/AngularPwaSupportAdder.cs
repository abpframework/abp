using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class AngularPwaSupportAdder : ITransientDependency
{
    protected ICmdHelper CmdHelper { get; }
    protected PackageJsonFileFinder PackageJsonFileFinder { get; }

    public AngularPwaSupportAdder(
        ICmdHelper cmdHelper,
        PackageJsonFileFinder packageJsonFileFinder)
    {
        CmdHelper = cmdHelper;
        PackageJsonFileFinder = packageJsonFileFinder;
    }

    public virtual void AddPwaSupport(string rootDirectory)
    {
        var fileList = PackageJsonFileFinder.Find(rootDirectory).Where(x => File.Exists(x.RemovePostFix("package.json") + "angular.json")).ToList();

        fileList.ForEach(AddPwaSupportToProject);
    }

    protected virtual void AddPwaSupportToProject(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath).EnsureEndsWith(Path.DirectorySeparatorChar);
        var pwaMajorVersion = GetAngularCliMajorVersion(filePath);

        // "ng" is usually not on the PATH and node_modules may not be installed yet, so the Angular CLI is run through npx.
        CmdHelper.RunCmd(
            $"npx -y -p @angular/cli@{pwaMajorVersion} ng add @angular/pwa@{pwaMajorVersion} --skip-confirmation",
            out var exitCode,
            workingDirectory: directory
        );

        if (exitCode != 0)
        {
            throw new AbpException(
                $"Failed to add Angular PWA support to '{filePath}'. 'ng add @angular/pwa@{pwaMajorVersion}' exited with code {exitCode}."
            );
        }

        var angularJsonFilePath = Path.Combine(directory, "angular.json");
        var angularJson = File.ReadAllText(angularJsonFilePath);
        var updatedAngularJson = AddPublicFolderToBuildAssets(angularJson, directory);

        if (updatedAngularJson != angularJson)
        {
            File.WriteAllText(angularJsonFilePath, updatedAngularJson);
        }
    }

    protected virtual string GetAngularCliMajorVersion(string packageJsonFilePath)
    {
        var packageJson = JObject.Parse(File.ReadAllText(packageJsonFilePath));

        var cliVersion =
            (packageJson["devDependencies"]?["@angular/cli"] ?? packageJson["dependencies"]?["@angular/cli"])?.ToString();

        var match = cliVersion == null ? null : Regex.Match(cliVersion, @"\d+");
        if (match == null || !match.Success)
        {
            throw new AbpException(
                $"Could not determine the @angular/cli version from '{packageJsonFilePath}', so the matching @angular/pwa version cannot be selected."
            );
        }

        return match.Value;
    }

    // "ng add @angular/pwa" puts the manifest and the icons into the "public" folder of the project,
    // which is not copied to the build output unless the build assets include it.
    protected virtual string AddPublicFolderToBuildAssets(string angularJson, string workspaceDirectory)
    {
        var json = JObject.Parse(angularJson);
        var changed = false;

        foreach (var project in json["projects"]?.Children<JProperty>() ?? Enumerable.Empty<JProperty>())
        {
            if (project.Value.SelectToken("architect.build.options.assets") is not JArray assets)
            {
                continue;
            }

            var projectRoot = project.Value["root"]?.ToString().Replace('\\', '/').TrimEnd('/');
            var publicFolder = projectRoot.IsNullOrEmpty() ? "public" : projectRoot + "/public";

            if (!Directory.Exists(Path.Combine(workspaceDirectory, publicFolder)) ||
                assets.OfType<JObject>().Any(x => x["input"]?.ToString().Replace('\\', '/').TrimEnd('/') == publicFolder))
            {
                continue;
            }

            assets.Add(new JObject
            {
                ["glob"] = "**/*",
                ["input"] = publicFolder
            });
            changed = true;
        }

        if (!changed)
        {
            return angularJson;
        }

        var newLine = angularJson.Contains("\r\n") ? "\r\n" : "\n";
        var result = json.ToString(Formatting.Indented).Replace("\r\n", "\n").Replace("\n", newLine);
        return angularJson.EndsWith("\n") ? result + newLine : result;
    }
}
