﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class AngularModuleSourceCodeAdder : ITransientDependency
    {
        public ILogger<SolutionModuleAdder> Logger { get; set; }

        public AngularModuleSourceCodeAdder()
        {
            Logger = NullLogger<SolutionModuleAdder>.Instance;
        }

        public async Task AddAsync(string solutionFilePath, string angularPath)
        {
            try
            {
                var angularProjectsPath = Path.Combine(angularPath, "projects");

                var projects = await CopyAndGetNamesOfAngularProjectsAsync(solutionFilePath, angularProjectsPath);

                if (!projects.Any())
                {
                    return;
                }

                await AddPathsToTsConfigAsync(angularPath, angularProjectsPath, projects);

                await AddProjectToAngularJsonAsync(angularPath, projects);
            }
            catch (Exception e)
            {
                Logger.LogError("Unable to add angular source code: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        private async Task AddProjectToAngularJsonAsync(string angularPath, List<string> projects)
        {
            var angularJsonFilePath = Path.Combine(angularPath, "angular.json");
            var fileContent = File.ReadAllText(angularJsonFilePath);

            var json = JObject.Parse(fileContent);

            var projectsJobject = (JObject) json["projects"];

            foreach (var project in projects)
            {
                projectsJobject.Add(project, new JObject(
                    new JProperty("projectType", "library"),
                    new JProperty("root", $"projects/{project}"),
                    new JProperty("sourceRoot", $"projects/{project}/src"),
                    new JProperty("prefix", "abp"),
                    new JProperty("architect", new JObject(
                        new JProperty("build", new JObject(
                            new JProperty("builder", "@angular-devkit/build-ng-packagr:build"),
                            new JProperty("options", new JObject(
                                new JProperty("tsConfig", $"projects/{project}/tsconfig.lib.json"),
                                new JProperty("project", $"projects/{project}/ng-package.json")
                            )),
                            new JProperty("configurations", new JObject(
                                new JProperty("production", new JObject(
                                    new JProperty("tsConfig", $"projects/{project}/tsconfig.lib.prod.json")))
                            )))),
                        new JProperty("test", new JObject(
                            new JProperty("builder", "@angular-builders/jest:run"),
                            new JProperty("options", new JObject(
                                    new JProperty("coverage", true),
                                    new JProperty("passWithNoTests", true)
                                )
                            ))),
                        new JProperty("lint", new JObject(
                            new JProperty("builder", "@angular-devkit/build-angular:tslint"),
                            new JProperty("options", new JObject(
                                new JProperty("tsConfig",
                                    new JArray(new JValue($"projects/{project}/tsconfig.lib.json"),
                                        new JValue($"projects/{project}/tsconfig.spec.json"))),
                                new JProperty("exclude", new JArray(new JValue("**/node_modules/**")))
                            ))
                        ))
                    ))
                ));
            }

            File.WriteAllText(angularJsonFilePath, json.ToString(Formatting.Indented));
        }

        private async Task AddPathsToTsConfigAsync(string angularPath, string angularProjectsPath,
            List<string> projects)
        {
            var tsConfigPath = Path.Combine(angularPath, "tsconfig.json");
            var fileContent = File.ReadAllText(tsConfigPath);
            var tsConfigAsJson = JObject.Parse(fileContent);
            var compilerOptions = (JObject) tsConfigAsJson["compilerOptions"];

            foreach (var project in projects)
            {
                var projectPackageName = await GetProjectPackageNameAsync(angularProjectsPath, project);

                var publicApis = Directory.GetFiles(Path.Combine(angularProjectsPath, project), "*public-api.ts", SearchOption.AllDirectories)
                    .Where(p => !p.Contains("\\node_modules\\"))
                    .Select(p => p.RemovePreFix(angularPath).Replace("\\", "/").RemovePreFix("/"));

                if (compilerOptions["paths"] == null)
                {
                    compilerOptions.Add("paths", new JObject());
                }

                foreach (var publicApi in publicApis)
                {
                    var subFolderName = publicApi.RemovePreFix($"projects/{project}/").Split("/")[0];

                    if (subFolderName == "src")
                    {
                        subFolderName = "";
                    }
                    else
                    {
                        subFolderName = $"/{subFolderName}";
                    }

                    if (compilerOptions["paths"][$"{projectPackageName}{subFolderName}"] != null)
                    {
                        continue;
                    }

                    ((JObject) compilerOptions["paths"]).Add(
                        new JProperty($"{projectPackageName}{subFolderName}",
                            new JArray(new object[] {publicApi})
                        )
                    );
                }
            }

            File.WriteAllText(tsConfigPath, tsConfigAsJson.ToString(Formatting.Indented));
        }

        private async Task<List<string>> CopyAndGetNamesOfAngularProjectsAsync(string solutionFilePath,
            string angularProjectsPath)
        {
            var projects = new List<string>();

            if (!Directory.Exists(angularProjectsPath))
            {
                Directory.CreateDirectory(angularProjectsPath);
            }

            var angularPathsInDownloadedSourceCode = Directory
                .GetDirectories(Path.Combine(Path.Combine(Path.GetDirectoryName(solutionFilePath), "modules")))
                .Select(p => Path.Combine(p, "angular"))
                .Where(Directory.Exists);

            foreach (var folder in angularPathsInDownloadedSourceCode)
            {
                var projectsInFolder = Directory.GetDirectories(folder);

                if (projectsInFolder.Length == 1 && Path.GetFileName(projectsInFolder[0]) == "projects")
                {
                    var foldersUnderProject = Directory.GetDirectories(Path.Combine(folder, "projects"));
                    foreach (var folderUnderProject in foldersUnderProject)
                    {
                        if (Directory.Exists(Path.Combine(folder, Path.GetFileName(folderUnderProject))))
                        {
                            continue;
                        }
                        Directory.Move(folderUnderProject, Path.Combine(folder, Path.GetFileName(folderUnderProject)));
                    }
                    projectsInFolder = Directory.GetDirectories(folder);
                }

                foreach (var projectInFolder in projectsInFolder)
                {
                    var projectName = Path.GetFileName(projectInFolder.TrimEnd('\\').TrimEnd('/'));

                    var destDirName = Path.Combine(angularProjectsPath, projectName);
                    if (projectName == "projects" || Directory.Exists(destDirName))
                    {
                        Directory.Delete(projectInFolder, true);
                        continue;
                    }

                    projects.Add(projectName);

                    Directory.Move(projectInFolder, destDirName);
                }
            }

            return projects;
        }

        private async Task<string> GetProjectPackageNameAsync(string angularProjectsPath, string project)
        {
            var packageJsonPath = Path.Combine(angularProjectsPath, project, "package.json");

            var fileContent = File.ReadAllText(packageJsonPath);

            return (string) JObject.Parse(fileContent)["name"];
        }
    }
}
