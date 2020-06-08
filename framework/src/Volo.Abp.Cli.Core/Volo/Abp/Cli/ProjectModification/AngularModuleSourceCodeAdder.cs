using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class AngularModuleSourceCodeAdder : ITransientDependency
    {
        public async Task AddAsync(string solutionFilePath, string angularPath)
        {
            var angularProjectsPath = Path.Combine(angularPath, "projects");

            var projects = await CopyAndGetNamesOfAngularProjectsAsync(solutionFilePath, angularProjectsPath);

            if (!projects.Any())
            {
                return;
            }

            await ReplaceProjectNamesAndCopySourCodeRequirementsToProjectsAsync(angularProjectsPath, projects);

            await RemoveRedundantFilesAsync(angularProjectsPath, projects);

            await AddPathsToTsConfigAsync(angularPath, angularProjectsPath, projects);

            await AddProjectToAngularJsonAsync(angularPath, projects);
        }

        private async Task AddProjectToAngularJsonAsync(string angularPath, List<string> projects)
        {
            var angularJsonFilePath = Path.Combine(angularPath, "angular.json");
            var fileContent = File.ReadAllText(angularJsonFilePath);

            var json = JObject.Parse(fileContent);

            var projectsJobject = (JObject)json["projects"];

            foreach (var project in projects)
            {
                projectsJobject.Add(project, new JObject(
                    new JProperty("projectType", "library"),
                    new JProperty("root", $"projects/{project}"),
                    new JProperty("sourceRoot", $"projects/{project}/src"),
                    new JProperty("prefix", "lib"),
                    new JProperty("architect", new JObject(
                        new JProperty("build", new JObject(
                            new JProperty("builder", "@angular-devkit/build-ng-package:build"),
                            new JProperty("options", new JObject(
                                new JProperty("tsConfig", $"projects/{project}/tsconfig.lib.json"),
                                new JProperty("project", $"projects/{project}/ng-package.json")
                                )),
                            new JProperty("configurations", new JObject(
                                new JProperty("production", new JObject(
                                    new JProperty("tsConfig", $"projects/{project}/tsconfig.lib.prod.json")))
                            )))),
                        new JProperty("test", new JObject(
                            new JProperty("builder", "@angular-devkit/build-angular:karma"),
                            new JProperty("options", new JObject(
                                new JProperty("main", $"projects/{project}/src/test.ts"),
                                new JProperty("tsConfig", $"projects/{project}/tsconfig.spec.json"),
                                new JProperty("karmaConfig", $"projects/{project}/karma.conf.js")
                                )
                            ))),
                        new JProperty("lint", new JObject(
                            new JProperty("builder", "@angular-devkit/build-angular:tslint"),
                            new JProperty("options", new JObject(
                                new JProperty("tsConfig", new JArray(new JValue($"projects/{project}/tsconfig.lib.json"), new JValue($"projects/{project}/tsconfig.spec.json"))),
                                new JProperty("exclude", new JArray(new JValue("**/node_modules/**")))
                                ))
                            ))
                        ))
                    ));
            }

            File.WriteAllText(angularJsonFilePath, json.ToString(Formatting.Indented));
        }

        private async Task AddPathsToTsConfigAsync(string angularPath, string angularProjectsPath, List<string> projects)
        {
            var tsConfigPath = Path.Combine(angularPath, "tsconfig.json");
            var fileContent = File.ReadAllText(tsConfigPath);

            var tsConfigAsJson = JObject.Parse(fileContent);

            var compilerOptions = (JObject)tsConfigAsJson["compilerOptions"];

            foreach (var project in projects)
            {
                var projectPackageName = await GetProjectPackageNameAsync(angularProjectsPath, project);

                var property = new JProperty($"{projectPackageName}",
                    new JArray(new object[] { $"projects/{project}/src/public-api.ts" })
                    );
                var property2 = new JProperty($"{projectPackageName}/*",
                    new JArray(new object[] { $"projects/{project}/src/lib/*" })
                    );

                if (compilerOptions["paths"] == null)
                {

                    compilerOptions.Add("paths", new JObject());
                }

                ((JObject)compilerOptions["paths"]).Add(property);
                ((JObject)compilerOptions["paths"]).Add(property2);
            }

            File.WriteAllText(tsConfigPath, tsConfigAsJson.ToString(Formatting.Indented));
        }

        private async Task<string> GetProjectPackageNameAsync(string angularProjectsPath, string project)
        {
            var packageJsonPath = Path.Combine(angularProjectsPath, project, "package.json");

            var fileContent = File.ReadAllText(packageJsonPath);

            return (string)JObject.Parse(fileContent)["name"];
        }

        private async Task RemoveRedundantFilesAsync(string angularProjectsPath, List<string> projects)
        {
            foreach (var project in projects)
            {
                var jestConfigPath = Path.Combine(angularProjectsPath, project, "jest.config.js");

                if (File.Exists(jestConfigPath))
                {
                    File.Delete(jestConfigPath);
                }

                var testPath = Path.Combine(angularProjectsPath, project, "src", "test.ts");

                if (File.Exists(testPath))
                {
                    File.Delete(testPath);
                }
            }
        }

        private async Task ReplaceProjectNamesAndCopySourCodeRequirementsToProjectsAsync(string angularProjectsPath, List<string> projects)
        {
            foreach (var project in projects)
            {
                var sourceCodeReqFolder = Path.Combine(angularProjectsPath, project, "source-code-requirements");

                Directory.CreateDirectory(sourceCodeReqFolder);

                var filesUnderSourceCodeReqFolder = Directory.GetFiles(sourceCodeReqFolder);

                foreach (var fileUnderSourceCodeReqFolder in filesUnderSourceCodeReqFolder)
                {
                    var newDest = Path.Combine(angularProjectsPath, project, Path.GetFileName(fileUnderSourceCodeReqFolder));
                    File.Move(fileUnderSourceCodeReqFolder, newDest);

                    var fileContent = File.ReadAllText(newDest);
                    fileContent = fileContent.Replace("{{project-name}}", project);
                    fileContent = fileContent.Replace("{{library-name-kebab-case}}", project);
                    File.WriteAllText(newDest, fileContent);
                }
            }
        }

        private async Task<List<string>> CopyAndGetNamesOfAngularProjectsAsync(string solutionFilePath, string angularProjectsPath)
        {
            var projects = new List<string>();

            if (!Directory.Exists(angularProjectsPath))
            {
                Directory.CreateDirectory(angularProjectsPath);
            }

            var angularPathsInDownloadedSourceCode = Directory.GetDirectories(Path.Combine(Path.Combine(Path.GetDirectoryName(solutionFilePath), "modules"))).Select(p => Path.Combine(p, "angular"))
                .Where(Directory.Exists);


            foreach (var folder in angularPathsInDownloadedSourceCode)
            {
                var projectsInFolder = Directory.GetDirectories(folder);
                foreach (var projectInFolder in projectsInFolder)
                {
                    var projectName = Path.GetFileName(projectInFolder.TrimEnd('\\').TrimEnd('/'));

                    var destDirName = Path.Combine(angularProjectsPath, projectName);
                    if (Directory.Exists(destDirName))
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
    }
}
