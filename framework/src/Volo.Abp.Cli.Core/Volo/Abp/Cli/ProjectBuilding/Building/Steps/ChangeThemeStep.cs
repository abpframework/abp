using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.Utils;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class ChangeThemeStep : ProjectBuildPipelineStep
{
    private const string Basic = "Basic";
    private const string LeptonXLite = "LeptonXLite";
    private const string LeptonX = "LeptonX";
    private const string Lepton = "Lepton";

    public override void Execute(ProjectBuildContext context)
    {
        if (!context.BuildArgs.Theme.HasValue)
        {
            return;
        }

        switch (context.BuildArgs.Theme)
        {
            case Theme.Basic:
                ChangeToBasicTheme(context);
                break;
            case Theme.Lepton:
                ChangeToLeptonTheme(context);
                break;
        }
    }

    protected virtual void ChangeToBasicTheme(ProjectBuildContext context)
    {
        var defaultThemeName = context.BuildArgs.TemplateName is AppTemplate.TemplateName or AppNoLayersTemplate.TemplateName
            ? LeptonXLite : LeptonX;
        
        new RemoveFilesStep($"/Themes/{defaultThemeName}").Execute(context);
        
        ChangeThemeToBasicForMvcProjects(context, defaultThemeName);
        ChangeThemeToBasicForBlazorProjects(context, defaultThemeName);
        ChangeThemeToBasicForBlazorServerProjects(context, defaultThemeName);
        ChangeThemeToBasicForBlazorWebAppProjects(context, defaultThemeName);
        ChangeThemeForAngularProjects(context, defaultThemeName, Basic, GetAngularPackageName(context.BuildArgs.Theme!.Value), GetAngularPackageName(Theme.Basic));
    }

    protected virtual void ChangeToLeptonTheme(ProjectBuildContext context)
    {
        //common
        RenameFolders(context, oldFolderName: LeptonX , newFolderName: Lepton);
        AddLeptonThemeManagementReferenceToProjects(context);

        ChangeThemeToLeptonForMvcProjects(context);
        ChangeThemeToLeptonForBlazorProjects(context);
        ChangeThemeToLeptonForBlazorServerProjects(context);
        ChangeThemeToLeptonForBlazorWebAppProjects(context);
        ChangeThemeForAngularProjects(context, oldThemeName: LeptonX, Lepton, GetAngularPackageName(Theme.LeptonX), GetAngularPackageName(Theme.Lepton));

        ConfigureLeptonManagementPackagesForNoLayersMvc(context, "/MyCompanyName.MyProjectName.Mvc/MyCompanyName.MyProjectName.csproj", new[] { "Web", "HttpApi", "Application" });
        ChangeThemeToLeptonForNoLayersBlazorServerProjects(context);
        ChangeThemeToLeptonForMauiBlazorProjects(context);
    }

    private static string GetAngularPackageName(Theme theme)
    {
        return theme switch
        {
            Theme.LeptonX => "@volosoft/abp.ng.theme.lepton-x",
            Theme.LeptonXLite => "@abp/ng.theme.lepton-x",
            Theme.Lepton => "@volo/abp.ng.theme.lepton",
            Theme.Basic => "@abp/ng.theme.basic",
            _ => string.Empty
        };
    }

    private static void ChangeThemeToBasicForBlazorProjects(ProjectBuildContext context, string defaultThemeName)
    {
        ReplacePackageReferenceWithProjectReference(
            context,
            "MyCompanyName.MyProjectName.Blazor.csproj",
            $"Volo.Abp.AspNetCore.Components.WebAssembly.{defaultThemeName}Theme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.csproj"
        );

        ReplacePackageReferenceWithProjectReference(
            context,
            "MyCompanyName.MyProjectName.Blazor.Client.csproj",
            $"Volo.Abp.AspNetCore.Components.WebAssembly.{defaultThemeName}Theme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.csproj"
        );

        ReplaceAllKeywords(
            context,
            "MyProjectNameBlazorModule.cs",
            $"{defaultThemeName}Theme.Components",
            "BasicTheme.Themes.Basic"
        );

        ReplaceAllKeywords(
            context,
            "MyProjectNameBlazorClientModule.cs",
            $"{defaultThemeName}Theme.Components",
            "BasicTheme.Themes.Basic"
        );

        ReplaceAllKeywords(
            context,
            "MyProjectNameBlazorModule.cs",
            defaultThemeName,
            Basic
        );

        ReplaceAllKeywords(
            context,
            "MyProjectNameBlazorClientModule.cs",
            defaultThemeName,
            Basic
        );

        ReplaceAllKeywords(
            context,
            "Routes.razor",
            defaultThemeName,
            Basic
        );

        ReplaceAllKeywords(
            context,
            "App.razor",
            defaultThemeName,
            Basic
        );

        ReplacePackageReferenceWithProjectReference(
            context,
            "MyCompanyName.MyProjectName.Host.csproj",
            $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );

        ReplaceAllKeywords(
            context,
            "MyProjectNameHostModule.cs",
            defaultThemeName,
            Basic
        );
    }

    private static void ChangeThemeForAngularProjects(ProjectBuildContext context, string oldThemeName, string newThemeName, string oldPackageName, string newPackageName)
    {
        if (context.BuildArgs.UiFramework != UiFramework.Angular)
        {
            return;
        }

        ReplaceImportPackage(
            context,
            "/angular/src/app/app.module.ts",
            oldPackageName,
            newPackageName
        );

        RemoveLinesByStatement(
            context,
            "/angular/src/app/app.module.ts",
            "SideMenuLayoutModule"
        );

        ReplaceAllKeywords(
            context,
            "/angular/src/app/app.module.ts",
            $"Theme{oldThemeName}Module",
            $"Theme{newThemeName}Module"
        );

        if (oldThemeName != LeptonX)
        {
            return;
        }

        ReplaceAllKeywords(
            context,
            "/angular/src/app/app.module.ts",
            "HttpErrorComponent, ",
            ""
        );

        ChangeModuleImportBetweenStatements(
            context,
            "/angular/src/app/app.module.ts",
            "ThemeSharedModule.forRoot",
            "AccountAdminConfigModule.forRoot",
            "ThemeSharedModule.forRoot(),"
        );
    }

    private static void ChangeThemeToLeptonForBlazorProjects(ProjectBuildContext context)
    {
        ReplacePackageReferenceWithProjectReference(
            context,
            "/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme\Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme.csproj"
        );

        ReplaceAllKeywords(
            context,
            "/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            LeptonX,
            Lepton
        );

        ReplacePackageReferenceWithProjectReference(
            context,
            "/MyCompanyName.MyProjectName.Host/MyCompanyName.MyProjectName.Host.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
        );

        ReplaceAllKeywords(
            context,
            "/MyCompanyName.MyProjectName.Host/MyProjectNameHostModule.cs",
            LeptonX,
            Lepton
        );
    }

    private static void ConfigureLeptonManagementPackagesForNoLayersMvc(ProjectBuildContext context, string targetProjectPath, IEnumerable<string> projectNames)
    {
        var file = context.Files.FirstOrDefault(f => !f.Name.Contains("Test") && f.Name.Contains(targetProjectPath) && f.Name.Contains(".csproj"));
        if (file == null)
        {
            return;
        }

        foreach (var projectName in projectNames)
        {
            var moduleFile = ConvertProjectFileToModuleFile(context, file);
            if (moduleFile == null)
            {
                continue;
            }

            AddProjectReference(file, $@"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.{projectName}\Volo.Abp.LeptonTheme.Management.{projectName}.csproj");
            AddModuleDependency(moduleFile, projectName, $"LeptonThemeManagement{ConvertProjectNameToModuleName($"{projectName}")}Module");
        }
    }

    private static void ChangeThemeToLeptonForMvcProjects(ProjectBuildContext context)
    {
        var projectNames = new[]
        {
            ".Web", ".HttpApi.Host", ".AuthServer", ".Web.Public", ".Web.Public.Host",
            "" //for app-nolayers-mvc
        };

        foreach (var projectName in projectNames)
        {
            var projectPath = $"/MyCompanyName.MyProjectName{projectName}/MyCompanyName.MyProjectName{projectName}.csproj";
            var projectFile = context.Files.FirstOrDefault(x => x.Name.Contains(projectPath));
            if (projectFile == null)
            {
                continue;
            }

            var moduleFile = ConvertProjectFileToModuleFile(context, projectFile);
            if (moduleFile == null)
            {
                continue;
            }

            ReplacePackageReferenceWithProjectReference(
                context,
                projectFile.Name,
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
                @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
            );

            ReplaceAllKeywords(
                context,
                moduleFile.Name,
                LeptonX,
                Lepton
            );

            RemoveLinesByStatement(
                context,
                moduleFile.Name,
                "Volo.Abp.LeptonX.Shared;"
            );

            AddNamespaces(
                context,
                moduleFile.Name,
                "using Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton;",
                "using Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling;"
            );
        }
    }

    private static void AddLeptonThemeManagementReferenceToProjects(ProjectBuildContext context)
    {
        var projects = new Dictionary<string, string>
        {
            {"Domain", "MyCompanyName.MyProjectName.Domain.csproj"},
            {"Domain.Shared", "MyCompanyName.MyProjectName.Domain.Shared.csproj"},
            {"Application", "MyCompanyName.MyProjectName.Application.csproj"},
            {"Application.Contracts", "MyCompanyName.MyProjectName.Application.Contracts.csproj"},
            {"HttpApi", "MyCompanyName.MyProjectName.HttpApi.csproj"},
            {"HttpApi.Client", "MyCompanyName.MyProjectName.HttpApi.Client.csproj"}
        };

        AddUiProjectToProjects(projects, context);

        foreach (var project in projects)
        {
            AddLeptonThemeManagementReference(context, project);
        }

        var microserviceServiceProjects = new Dictionary<string, string>
        {
            {"Domain", "MyCompanyName.MyProjectName.AdministrationService.Domain.csproj"},
            {"Domain.Shared", "MyCompanyName.MyProjectName.AdministrationService.Domain.Shared.csproj"},
            {"Application", "MyCompanyName.MyProjectName.AdministrationService.Application.csproj"},
            {"Application.Contracts", "MyCompanyName.MyProjectName.AdministrationService.Application.Contracts.csproj"},
            {"HttpApi", "MyCompanyName.MyProjectName.AdministrationService.HttpApi.csproj"},
            {"HttpApi.Client", "MyCompanyName.MyProjectName.AdministrationService.HttpApi.Client.csproj"},
            {"Web", "MyCompanyName.MyProjectName.AdministrationService.Web.csproj"}
        };

        foreach (var microserviceServiceProject in microserviceServiceProjects)
        {
            AddLeptonThemeManagementReference(context, microserviceServiceProject);
        }
    }

    private static void AddUiProjectToProjects(Dictionary<string, string> projects, ProjectBuildContext context)
    {
        if (projects.IsNullOrEmpty())
        {
            return;
        }

        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.Mvc:
                projects["Web.Host"] = "MyCompanyName.MyProjectName.Web.Host.csproj";
                projects["Web"] = "MyCompanyName.MyProjectName.Web.csproj";
                break;
            case UiFramework.Blazor:
                projects["Blazor.WebAssembly"] = "MyCompanyName.MyProjectName.Blazor.csproj";
                break;
            case UiFramework.BlazorServer:
                projects["Blazor.Server"] = "MyCompanyName.MyProjectName.Blazor.csproj";
                break;
            case UiFramework.BlazorWebApp:
                projects["Blazor.WebApp"] = "MyCompanyName.MyProjectName.Blazor.csproj";
                break;
        }
    }

    private static void AddLeptonThemeManagementReference(ProjectBuildContext context, KeyValuePair<string, string> projectInfo)
    {
        var reference = $@"..\..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.{projectInfo.Key}\Volo.Abp.LeptonTheme.Management.{projectInfo.Key}.csproj";
        var projectFile = context.Files.FirstOrDefault(f => !f.Name.Contains("Test") && f.Name.Contains(projectInfo.Value) && f.Name.Contains(".csproj"));
        if (projectFile is null)
        {
            return;
        }

        var moduleFile = ConvertProjectFileToModuleFile(context, projectFile);
        if (moduleFile == null)
        {
            return;
        }

        AddProjectReference(projectFile, reference);

        AddModuleDependency(moduleFile, projectInfo.Key, $"LeptonThemeManagement{ConvertProjectNameToModuleName(projectInfo.Key)}Module",
            underManagementFolder: projectInfo.Key != "HttpApi");
    }

    private static void AddModuleDependency(FileEntry moduleFile, string projectName, string dependency, bool underManagementFolder = true)
    {
        var projectNames = new[] { "Blazor", "Blazor.Server", "Blazor.WebAssembly" };

        var lines = moduleFile.GetLines();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("namespace MyCompanyName.MyProjectName"))
            {
                lines[i - 1] = lines[i - 1] + ("using Volo.Abp.LeptonTheme" + (underManagementFolder ? ".Management." : ".") + (projectNames.Any(p => p == projectName) ? projectName : "")).TrimEnd('.').EnsureEndsWith(';') + Environment.NewLine;
            }

            if (lines[i].Contains("public class MyProjectName") && lines[i-1].Contains(")]"))
            {
                lines[i - 2] = lines[i - 2] + "," + Environment.NewLine + $"\ttypeof({dependency})";
            }
        }

        moduleFile.SetLines(lines);
    }

    private static void ReplacePackageReferenceWithProjectReference(ProjectBuildContext context, string targetProjectFilePath, string packageReference, string projectReference)
    {
        var file = context.Files.FirstOrDefault(x => x.Name.Contains(targetProjectFilePath));
        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        var lineIndex = lines.FindIndex(line => line.Contains("PackageReference") && line.Contains(packageReference));
        if (lineIndex == -1)
        {
            return;
        }

        lines[lineIndex] = lines[lineIndex].Replace(lines[lineIndex], $"\t\t<ProjectReference Include=\"{projectReference}\" />\n");
        file.SetLines(lines);
    }

    private static void ReplaceImportPackage(ProjectBuildContext context, string filePath, string oldImportPackage, string newImportPackage)
    {
        var file = context.Files.FirstOrDefault(x => x.Name.Contains(filePath));
        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        var lineIndex = lines.FindIndex(line => line.Contains($"from '{oldImportPackage}'"));
        if (lineIndex == -1)
        {
            return;
        }

        lines[lineIndex] = lines[lineIndex].Replace(oldImportPackage, newImportPackage);
        file.SetLines(lines);
    }

    private static void RemoveLinesByStatement(ProjectBuildContext context, string filePath, string statement)
    {
        var file = context.Files.FirstOrDefault(x => x.Name.Contains(filePath));
        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(statement))
            {
                lines[i] = null;
            }
        }

        file.SetLines(lines.Where(x => x != null));
    }

    private static void ChangeModuleImportBetweenStatements(ProjectBuildContext context, string filePath, string firstStatement, string lastStatement, string newStatement)
    {
        var file = context.Files.FirstOrDefault(x => x.Name.Contains(filePath));
        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        var firstLineIndex = lines.FindIndex(line => line.Contains(firstStatement));
        var lastLineIndex = lines.FindIndex(line => line.Contains(lastStatement));

        if(firstLineIndex == -1 || lastLineIndex == -1)
        {
            return;
        }

        lines[firstLineIndex] = newStatement;

        for (var i = firstLineIndex + 1; i <= lastLineIndex; i++)
        {
            lines[i] = null;
        }

        file.SetLines(lines.Where(x => x != null));
    }

    private static void AddProjectReference(FileEntry file, string reference)
    {
        if (!file.Name.Contains(".csproj"))
        {
            return;
        }

        var doc = new XmlDocument() { PreserveWhitespace = true };
        using (var stream = StreamHelper.GenerateStreamFromString(file.Content))
        {
            doc.Load(stream);

            var itemGroupNodes = doc.SelectNodes("/Project/ItemGroup");
            XmlNode itemGroupNode = null;

            if (itemGroupNodes == null || itemGroupNodes.Count < 1)
            {
                var projectNodes = doc.SelectNodes("/Project");
                var projectNode = projectNodes![0];

                itemGroupNode = doc.CreateElement("ItemGroup");
                projectNode.AppendChild(itemGroupNode);
            }
            else
            {
                for (var i = 0; i < itemGroupNodes.Count; i++)
                {
                    if (itemGroupNode is not null)
                    {
                        break;
                    }
                    for (var j = 0; j < itemGroupNodes[i].ChildNodes.Count; j++)
                    {
                        if (itemGroupNodes[i].ChildNodes[j].Name != "ProjectReference" || itemGroupNodes[i].ChildNodes[j].NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }

                        itemGroupNode = itemGroupNodes[i];
                        break;
                    }
                }
            }

            itemGroupNode ??= itemGroupNodes[0];

            var packageReferenceNode = doc.CreateElement("ProjectReference");

            var includeAttr = doc.CreateAttribute("Include");
            includeAttr.Value = reference;
            packageReferenceNode.Attributes.Append(includeAttr);

            itemGroupNode.AppendChild(doc.CreateTextNode("\t"));
            itemGroupNode.AppendChild(packageReferenceNode);
            itemGroupNode.AppendChild(doc.CreateTextNode(Environment.NewLine));
            file.SetContent(doc.OuterXml);
        }
    }

    private static FileEntry ConvertProjectFileToModuleFile(ProjectBuildContext context, FileEntry projectFile)
    {
        var splittedProjectFileName = projectFile.Name.RemovePostFix("/").Split("/");

        splittedProjectFileName = splittedProjectFileName.Take(splittedProjectFileName.Length - 1).ToArray();

        var fileName = splittedProjectFileName.Last();
        if (fileName == null)
        {
            return null;
        }

        fileName = fileName
            .Replace("MyCompanyName.", "")
            .Replace(".csproj", "Module")
            .Replace(".", "");

        return context.Files.FirstOrDefault(f => f.Name.Contains(splittedProjectFileName.Last() + "/" + fileName) && f.Name.EndsWith("Module.cs"));
    }

    private static string ConvertProjectNameToModuleName(string moduleName)
    {
        return moduleName.Replace(".", "");
    }

    private static void RenameFolders(ProjectBuildContext context, string oldFolderName, string newFolderName)
    {
        foreach (var file in context.Files.Where(x => x.Name.Contains(oldFolderName) && x.IsDirectory))
        {
            new MoveFolderStep(file.Name, file.Name.Replace(oldFolderName, newFolderName)).Execute(context);
        }
    }

    private static void ReplaceAllKeywords(ProjectBuildContext context, string targetModuleFilePath, string oldKeyword, string newKeyword)
    {
        var file = context.Files.FirstOrDefault(x => x.Name.Contains(targetModuleFilePath));
        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();

        for (var i = 0; i < lines.Length; i++)
        {
            if (!lines[i].Contains(oldKeyword))
            {
                continue;
            }

            lines[i] = lines[i].Replace(oldKeyword, newKeyword);
        }

        file.SetLines(lines);
    }

    private static void AddNamespaces(ProjectBuildContext context, string targetModuleFilePath, params string[] namespaces)
    {
        var file = context.Files.FirstOrDefault(x => x.Name.Contains(targetModuleFilePath));
        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines().ToList();

        foreach (var @namespace in namespaces)
        {
            lines.AddFirst(@namespace);
        }
        
        file.SetLines(lines);
    }

    private static void ChangeThemeToBasicForMvcProjects(ProjectBuildContext context, string defaultThemeName)
    {
        var projectNames = new List<string>
        {
            ".Web", ".AuthServer", ".Web.Public", ".Web.Public.Host",
            "" //for app-nolayers-mvc
        };
        
        if(!context.Symbols.Contains("tiered"))
        {
            projectNames.Add(".HttpApi.Host");
        }

        foreach (var projectName in projectNames)
        {
            var projectPath = $"/MyCompanyName.MyProjectName{projectName}/MyCompanyName.MyProjectName{projectName}.csproj";
            var projectFile = context.Files.FirstOrDefault(x => x.Name.Contains(projectPath));
            if (projectFile == null)
            {
                continue;
            }

            var moduleFile = ConvertProjectFileToModuleFile(context, projectFile);
            if (moduleFile == null)
            {
                continue;
            }

            ReplacePackageReferenceWithProjectReference(
                context,
                projectFile.Name,
                $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}",
                @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
            );

            ReplaceAllKeywords(
                context,
                moduleFile.Name,
                defaultThemeName,
                Basic
            );

            AddNamespaces(
                context,
                moduleFile.Name,
                "using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;",
                "using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;"
            );
        }
    }

    private static void ChangeThemeToBasicForBlazorServerProjects(ProjectBuildContext context, string defaultThemeName)
    {
        var projects = new Dictionary<string, string>
        {
            {".Blazor", "MyProjectNameBlazorModule"},
            {".Blazor.Server.Tiered", "MyProjectNameBlazorModule"},
            {".Blazor.Server", "MyProjectNameModule"},
            {"Blazor.Server.Mongo", "MyProjectNameModule"},
            {"", ""} //for app-nolayers blazor-server
        };

        foreach (var project in projects)
        {
            ReplacePackageReferenceWithProjectReference(
                context,
                $"MyCompanyName.MyProjectName{project.Key}.csproj",
                $"Volo.Abp.AspNetCore.Components.Server.{defaultThemeName}",
                @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.Server.BasicTheme\Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj"
            );

            ReplacePackageReferenceWithProjectReference(
                context,
                $"MyCompanyName.MyProjectName{project.Key}.csproj",
                $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}",
                @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
            );

            ReplaceAllKeywords(
                context,
                $"_Host.cshtml",
                $"{defaultThemeName}Theme.Components",
                "BasicTheme.Themes.Basic" 
            );

            ReplaceAllKeywords(
                context,
                $"/MyCompanyName.MyProjectName{project.Key}/{project.Value}.cs",
                defaultThemeName,
                Basic + "Theme"
            );

            ReplaceAllKeywords(
                context,
                $"_Host.cshtml",
                defaultThemeName,
                Basic
            );

            ReplaceAllKeywords(
                context,
                "Routes.razor",
                defaultThemeName,
                Basic
            );

            ReplaceAllKeywords(
                context,
                "App.razor",
                defaultThemeName,
                Basic
            );
        }
    }

    private static void ChangeThemeToBasicForBlazorWebAppProjects(ProjectBuildContext context, string defaultThemeName)
    {
        var projects = new Dictionary<string, string>
        {
            {".Blazor.WebApp", "MyProjectNameBlazorModule"},
            {".Blazor.WebApp.Client", "MyProjectNameBlazorClientModule"}
        };

        foreach (var project in projects)
        {
            ReplacePackageReferenceWithProjectReference(
                context,
                $"MyCompanyName.MyProjectName{project.Key}.csproj",
                $"Volo.Abp.AspNetCore.Components.Server.{defaultThemeName}",
                @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.Server.BasicTheme\Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj"
            );

            ReplacePackageReferenceWithProjectReference(
                context,
                $"MyCompanyName.MyProjectName{project.Key}.csproj",
                $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}",
                @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
            );

            ReplaceAllKeywords(
                context,
                $"{project.Value}.cs",
                defaultThemeName,
                Basic + "Theme"
            );

            ReplaceAllKeywords(
                context,
                "Routes.razor",
                defaultThemeName,
                Basic
            );

            ReplaceAllKeywords(
                context,
                "App.razor",
                defaultThemeName,
                Basic
            );
        }
    }

    private static void ChangeThemeToLeptonForBlazorServerProjects(ProjectBuildContext context)
    {
        var projectNames = new[] { "Blazor", "Blazor.Server.Tiered" };

        foreach (var projectName in projectNames)
        {
            ReplacePackageReferenceWithProjectReference(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyCompanyName.MyProjectName.{projectName}.csproj",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
                @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
            );

            ReplacePackageReferenceWithProjectReference(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyCompanyName.MyProjectName.{projectName}.csproj",
                "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
                @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.Server.LeptonTheme\Volo.Abp.AspNetCore.Components.Server.LeptonTheme.csproj"
            );

            ReplaceAllKeywords(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameBlazorModule.cs",
                LeptonX,
                Lepton
            );

            ReplaceAllKeywords(
                context,
                $"/Pages/_Host.cshtml",
                LeptonX,
                Lepton
            );

            RemoveLinesByStatement(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameBlazorModule.cs",
                "Volo.Abp.LeptonX.Shared;"
            );
        }
    }

    private static void ChangeThemeToLeptonForBlazorWebAppProjects(ProjectBuildContext context)
    {
        var projectNames = new[] { "Blazor.WebApp", "Blazor.WebApp.Client", "Blazor.WebApp.Tiered",  "Blazor.WebApp.Tiered.Client" };

        foreach (var projectName in projectNames)
        {
            ReplacePackageReferenceWithProjectReference(
                context,
                $"MyCompanyName.MyProjectName.{projectName}.csproj",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
                @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
            );

            ReplacePackageReferenceWithProjectReference(
                context,
                $"MyCompanyName.MyProjectName.{projectName}.csproj",
                "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
                @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.Server.LeptonTheme\Volo.Abp.AspNetCore.Components.Server.LeptonTheme.csproj"
            );

            ReplaceAllKeywords(
                context,
                $"MyProjectNameBlazorModule.cs",
                LeptonX,
                Lepton
            );

            ReplaceAllKeywords(
                context,
                $"MyProjectNameBlazorClientModule.cs",
                LeptonX,
                Lepton
            );

            ReplaceAllKeywords(
                context,
                "Routes.razor",
                LeptonX,
                Lepton
            );

            ReplaceAllKeywords(
                context,
                "App.razor",
                LeptonX,
                Lepton
            );

            RemoveLinesByStatement(
                context,
                $"MyProjectNameBlazorModule.cs",
                "Volo.Abp.LeptonX.Shared;"
            );

            RemoveLinesByStatement(
                context,
                $"MyProjectNameBlazorClientModule.cs",
                "Volo.Abp.LeptonX.Shared;"
            );
        }
    }

    private static void ChangeThemeToLeptonForNoLayersBlazorServerProjects(ProjectBuildContext context)
    {
        var blazorServerProjects = new[] { "Blazor.Server", "HttpApi", "Application" };
        var projectNames = new[] { "Blazor.Server", "Blazor.Server.Mongo" };

        foreach (var projectName in projectNames)
        {
            ReplacePackageReferenceWithProjectReference(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyCompanyName.MyProjectName.{projectName}.csproj",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
                @"..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
            );

            ReplacePackageReferenceWithProjectReference(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyCompanyName.MyProjectName.{projectName}.csproj",
                "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
                @"..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.Server.LeptonTheme\Volo.Abp.AspNetCore.Components.Server.LeptonTheme.csproj"
            );

            ConfigureLeptonManagementPackagesForNoLayersMvc(
                context,
                $@"/MyCompanyName.MyProjectName.{projectName}/MyCompanyName.MyProjectName.{projectName}.csproj",
                blazorServerProjects
            );

            ReplaceAllKeywords(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameModule.cs",
                LeptonX,
                Lepton
            );

            ReplaceAllKeywords(
                context,
                $"/Pages/_Host.cshtml",
                LeptonX,
                Lepton
            );
        }
    }

    private static void ChangeThemeToLeptonForMauiBlazorProjects(ProjectBuildContext context)
    {
        ReplacePackageReferenceWithProjectReference(
            context,
            "/MyCompanyName.MyProjectName.MauiBlazor/MyCompanyName.MyProjectName.MauiBlazor.csproj",
            "Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonTheme\Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonTheme.csproj"
        );

        ReplaceAllKeywords(
            context,
            "/MyCompanyName.MyProjectName.MauiBlazor/MainPage.xaml",
            "clr-namespace:Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components;assembly=Volo.Abp.AspNetCore.Components.Web.LeptonXTheme",
            "clr-namespace:Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components;assembly=Volo.Abp.AspNetCore.Components.Web.LeptonTheme");

        ReplaceAllKeywords(
            context,
            "/MyCompanyName.MyProjectName.MauiBlazor/MainPage.xaml",
            "leptonXTheme",
            "leptonTheme");

        ReplaceAllKeywords(
            context,
            "/MyCompanyName.MyProjectName.MauiBlazor/Pages/Account/Login.razor",
            "Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme.Components.AccountLayout",
            "Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonTheme.Components.AccountLayout");

        ReplaceAllKeywords(
            context,
            "/MyCompanyName.MyProjectName.MauiBlazor/Pages/Account/RedirectToLogout.razor",
            "LeptonXResource",
            "LeptonThemeManagementResource");

        ReplaceAllKeywords(
            context,
            "/MyCompanyName.MyProjectName.MauiBlazor/Pages/Account/RedirectToLogout.razor",
            "Volo.Abp.LeptonX.Shared.Localization",
            "Volo.Abp.LeptonTheme.Management.Localization");

        ReplaceAllKeywords(
            context,
            "/MyCompanyName.MyProjectName.MauiBlazor/MyProjectNameMauiBlazorModule.cs",
            LeptonX,
            Lepton
        );
    }
}
