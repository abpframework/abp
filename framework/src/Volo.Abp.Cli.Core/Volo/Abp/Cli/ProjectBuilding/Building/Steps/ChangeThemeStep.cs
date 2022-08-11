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

    protected void ChangeToBasicTheme(ProjectBuildContext context)
    {
        var defaultThemeName = context.BuildArgs.TemplateName is AppTemplate.TemplateName or AppNoLayersTemplate.TemplateName
            ? "LeptonXLite" 
            : "LeptonX";
        
        #region MVC Projects

        ChangeThemeToBasicForMvcProjects(context, defaultThemeName);

        #endregion

        #region MyCompanyName.MyProjectName.Blazor
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            $"Volo.Abp.AspNetCore.Components.WebAssembly.{defaultThemeName}Theme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.csproj"
            );
        
        ChangeNamespaceAndKeyword(
            context,
            "/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            $"Volo.Abp.AspNetCore.Components.WebAssembly.{defaultThemeName}Theme",
            "Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme",
            $"AbpAspNetCoreComponentsWebAssembly{defaultThemeName}ThemeModule",
            "AbpAspNetCoreComponentsWebAssemblyBasicThemeModule"
        );

        ChangeNamespace(
            context,
            "/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            $"Volo.Abp.AspNetCore.Components.Web.{defaultThemeName}Theme.Themes.{defaultThemeName}",
            "Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic"
        );
        
        #endregion

        #region Blazor.Server Projects

        ChangeThemeToBasicForBlazorServerProjects(context, defaultThemeName);
        
        #endregion

        #region Angular

        var angularPackageName = context.BuildArgs.TemplateName is AppTemplate.TemplateName or AppNoLayersTemplate.TemplateName
            ? "@abp/ng.theme.lepton-x" 
            : "@volosoft/abp.ng.theme.lepton-x";
        
        ReplaceImportPackage(
            context, 
            "/angular/src/app/app.module.ts",
            angularPackageName, 
            "@abp/ng.theme.basic"
        );

        RemoveLinesByStatement(
            context,
            "/angular/src/app/app.module.ts",
            "SideMenuLayoutModule"
        );

        ReplaceMethodNames(
            context,
            "/angular/src/app/app.module.ts",
            "ThemeLeptonXModule",
            "ThemeBasicModule"
        );
        
        RemoveLinesByStatement(
            context,
            "/angular/angular.json",
            "node_modules/bootstrap-icons/font/bootstrap-icons.css"
        );

        #endregion
    }

    protected void ChangeToLeptonTheme(ProjectBuildContext context)
    {
        #region Common 
        
        RenameLeptonXFolders(context, folderName: "Lepton");
        AddLeptonThemeManagementReferenceToProjects(context);
        
        #endregion

        #region MVC Projects

        ChangeThemeToLeptonForMvcProjects(context);

        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme\Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components",
            "Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components",
            "AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule",
            "AbpAspNetCoreComponentsWebAssemblyLeptonThemeModule"
        );

        ChangeNamespace(
            context,
            "/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme",
            "Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme"
        );

        #endregion

        #region MyCompanyName.MyProjectName.Blazor.Server && MyCompanyName.MyProjectName.Blazor.Server.Tiered 

        ChangeThemeToLeptonForBlazorServerProjects(context);

        #endregion

        #region Angular

        ReplaceImportPackage(
            context, 
            "/angular/src/app/app.module.ts",
            "@volosoft/abp.ng.theme.lepton-x", 
            "@volo/abp.ng.theme.lepton"
        );

        RemoveLinesByStatement(
            context,
            "/angular/src/app/app.module.ts",
            "SideMenuLayoutModule"
        );

        ReplaceMethodNames(
            context,
            "/angular/src/app/app.module.ts",
            "ThemeLeptonXModule",
            "ThemeLeptonModule"
        );
        
        RemoveLinesByStatement(
            context,
            "/angular/angular.json",
            "node_modules/bootstrap-icons/font/bootstrap-icons.css"
        );

        #endregion

        #region MyCompanyName.MyProjectName.Mvc && MyCompanyName.MyProjectName.Mvc.Mongo

        var projectNames = new[] {"Web", "HttpApi", "Application"};
        ConfigureLeptonManagementPackagesForNoLayersMvc(context, @"/MyCompanyName.MyProjectName.Mvc/MyCompanyName.MyProjectName.csproj", projectNames);

        #endregion

        #region MyCompanyName.MyProjectName.Blazor.Server && MyCompanyName.MyProjectName.Blazor.Server.Mongo - (app-nolayers)

        ChangeThemeToLeptonForNoLayersBlazorServerProjects(context);
        
        #endregion
    }

    private void ConfigureLeptonManagementPackagesForNoLayersMvc(ProjectBuildContext context, string targetProjectPath, string[] projectNames)
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

    private void ChangeThemeToLeptonForMvcProjects(ProjectBuildContext context)
    {
        var projectNames = new[] 
        {
            ".Web", ".HttpApi.Host", ".AuthServer", 
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
        
            ChangeNamespaceAndKeyword(
                context,
                moduleFile.Name,
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
                "LeptonXThemeBundles.Styles.Global",
                "LeptonThemeBundles.Styles.Global"
            );
        
            ChangeNamespaceAndKeyword(
                context,
                moduleFile.Name,
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
                "AbpAspNetCoreMvcUiLeptonXThemeModule",
                "AbpAspNetCoreMvcUiLeptonThemeModule"
            );
        
            RemoveLinesByStatement(
                context,
                moduleFile.Name,
                "Volo.Abp.LeptonX.Shared;"
            );
        }
    }
    
    private void AddLeptonThemeManagementReferenceToProjects(ProjectBuildContext context)
    {
        var projects = new Dictionary<string, string> 
        {
            {"Domain", "MyCompanyName.MyProjectName.Domain.csproj"},
            {"Domain.Shared", "MyCompanyName.MyProjectName.Domain.Shared.csproj"},
            {"Application", "MyCompanyName.MyProjectName.Application.csproj"},
            {"Application.Contracts", "MyCompanyName.MyProjectName.Application.Contracts.csproj"},
            {"Blazor.WebAssembly", "MyCompanyName.MyProjectName.Blazor.csproj"},
            {"Blazor.Server", "MyCompanyName.MyProjectName.Blazor.Server.csproj"},
            {"HttpApi", "MyCompanyName.MyProjectName.HttpApi.csproj"},
            {"HttpApi.Client", "MyCompanyName.MyProjectName.HttpApi.Client.csproj"},
            {"Web.Host", "MyCompanyName.MyProjectName.Web.Host.csproj"},
            {"Web", "MyCompanyName.MyProjectName.Web.csproj"},
        };

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
    
    private void AddLeptonThemeManagementReference(ProjectBuildContext context, KeyValuePair<string, string> projectInfo) 
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

    private void AddModuleDependency(FileEntry moduleFile, string projectName, string dependency, bool underManagementFolder = true)
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

    protected void ReplacePackageReferenceWithProjectReference(        
        ProjectBuildContext context, 
        string targetProjectFilePath,
        string packageReference,
        string projectReference)
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
        
        lines[lineIndex] = lines[lineIndex].Replace(lines[lineIndex], $"\t<ProjectReference Include=\"{projectReference}\" />");
        file.SetLines(lines);
    }

    protected void ChangeNamespaceAndKeyword(
        ProjectBuildContext context,
        string targetModuleFilePath,
        string oldNamespace,
        string newNamespace,
        string oldKeyword,
        string newKeyword)
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
            if (lines[i].Contains($"using {oldNamespace}"))
            {
                lines[i] = lines[i].Replace($"using {oldNamespace}", $"using {newNamespace}");
            }
            else if (lines[i].Contains(oldKeyword))
            {
                lines[i] = lines[i].Replace(oldKeyword, newKeyword);
            }
        }

        file.SetLines(lines);
    }

    protected void ChangeNamespace(
        ProjectBuildContext context,
        string targetModuleFilePath,
        string oldNamespace,
        string newNamespace)
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
            if (lines[i].Contains($"using {oldNamespace}"))
            {
                lines[i] = lines[i].Replace($"using {oldNamespace}", $"using {newNamespace}");
            }
        }

        file.SetLines(lines);
    }

    protected void ReplaceImportPackage(
        ProjectBuildContext context,
        string filePath,
        string oldImportPackage,
        string newImportPackage)
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

    protected void RemoveLinesByStatement(
        ProjectBuildContext context,        
        string filePath,
        string statement)
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

    protected void ReplaceMethodNames(
        ProjectBuildContext context,        
        string filePath,
        string oldMethodName,
        string newMethodName)
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
            if (lines[i].Contains(oldMethodName))
            {
                lines[i] = lines[i].Replace(oldMethodName, newMethodName);
            }
        }
        
        file.SetLines(lines);
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
        
        var fileName = splittedProjectFileName?.Last();
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
    
    private static void RenameLeptonXFolders(ProjectBuildContext context, string folderName)
    {
        var leptonXFiles = context.Files.Where(x => x.Name.Contains("LeptonX") && x.IsDirectory);
        foreach (var file in leptonXFiles)
        {
            new MoveFolderStep(file.Name, file.Name.Replace("LeptonX", folderName)).Execute(context);
        }
    }

    private void ChangeThemeToBasicForMvcProjects(ProjectBuildContext context, string defaultThemeName)
    {
        var projects = new Dictionary<string, string>
        {
            {".Web", "MyProjectNameWebModule"},
            {".HttpApi.Host", "MyProjectNameHttpApiHostModule"},
            {".AuthServer", "MyProjectNameAuthServerModule"},
            {"", "MyProjectNameWebModule"}
        };

        foreach (var project in projects)
        {
            ReplacePackageReferenceWithProjectReference(
                context,
                $"/MyCompanyName.MyProjectName{project.Key}/MyCompanyName.MyProjectName{project.Key}.csproj",
                $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}",
                @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
            );
        
            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName{project.Key}/{project.Value}.cs",
                $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
                $"AbpAspNetCoreMvcUi{defaultThemeName}ThemeModule",
                "AbpAspNetCoreMvcUiBasicThemeModule"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName{project.Key}/{project.Value}.cs",
                $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}.Bundling",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
                $"{defaultThemeName}ThemeBundles.Styles.Global",
                "BasicThemeBundles.Styles.Global"
            );
        }
    }
    
    private void ChangeThemeToBasicForBlazorServerProjects(ProjectBuildContext context, string defaultThemeName)
    {
        var projects = new Dictionary<string, string>
        {
            {"Blazor", "MyProjectNameBlazorModule"},
            {"Blazor.Server.Tiered", "MyProjectNameBlazorModule"},
            {"Blazor.Server", "MyProjectNameModule"},
            {"Blazor.Server.Mongo", "MyProjectNameModule"}
        };

        foreach (var project in projects)
        {
            ReplacePackageReferenceWithProjectReference(
                context,
                $"/MyCompanyName.MyProjectName.{project.Key}/MyCompanyName.MyProjectName.{project.Key}.csproj",
                $"Volo.Abp.AspNetCore.Components.Server.{defaultThemeName}Theme",
                @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
            );

            ReplacePackageReferenceWithProjectReference(
                context,
                $"/MyCompanyName.MyProjectName.{project.Key}/MyCompanyName.MyProjectName.{project.Key}.csproj",
                $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}",
                @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.Server.BasicTheme\Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{project.Key}/{project.Value}.cs",
                $"Volo.Abp.AspNetCore.Components.Server.{defaultThemeName}Theme",
                "Volo.Abp.AspNetCore.Components.Server.BasicTheme",
                $"AbpAspNetCoreComponentsServer{defaultThemeName}ThemeModule",
                "AbpAspNetCoreComponentsServerBasicThemeModule"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{project.Key}/{project.Value}.cs",
                $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
                $"AbpAspNetCoreMvcUi{defaultThemeName}ThemeModule",
                "AbpAspNetCoreMvcUiBasicThemeModule"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{project.Key}/{project.Value}.cs",
                $"Volo.Abp.AspNetCore.Components.Server.{defaultThemeName}Theme.Bundling",
                "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
                $"{defaultThemeName}ThemeBundles.Styles.Global",
                "BasicThemeBundles.Styles.Global"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{project.Key}/{project.Value}.cs",
                $"Volo.Abp.AspNetCore.Mvc.UI.Theme.{defaultThemeName}.Bundling",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
                $"Blazor{defaultThemeName}ThemeBundles.Styles.Global",
                "BlazorBasicThemeBundles.Styles.Global"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{project.Key}/Pages/_Host.cshtml",
                $"Volo.Abp.AspNetCore.Components.Web.{defaultThemeName}Theme.Themes.{defaultThemeName}",
                "Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic",
                $"Blazor{defaultThemeName}ThemeBundles.Styles.Global",
                "BlazorBasicThemeBundles.Styles.Global"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{project.Key}/Pages/_Host.cshtml",
                $"Volo.Abp.AspNetCore.Components.Server.{defaultThemeName}Theme.Bundling",
                "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
                $"Blazor{defaultThemeName}ThemeBundles.Scripts.Global",
                "BlazorBasicThemeBundles.Scripts.Global"
            );
        }
    }

    private void ChangeThemeToLeptonForBlazorServerProjects(ProjectBuildContext context)
    {
        var projectNames = new[] {"Blazor", "Blazor.Server.Tiered"};
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

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameBlazorModule.cs",
                "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
                "Volo.Abp.AspNetCore.Components.Server.LeptonTheme",
                "AbpAspNetCoreComponentsServerLeptonXThemeModule",
                "AbpAspNetCoreComponentsServerLeptonThemeModule"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameBlazorModule.cs",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
                "AbpAspNetCoreMvcUiLeptonXThemeModule",
                "AbpAspNetCoreMvcUiLeptonThemeModule"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameBlazorModule.cs",
                "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
                "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
                "LeptonXThemeBundles.Styles.Global",
                "LeptonThemeBundles.Styles.Global"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameBlazorModule.cs",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
                "BlazorLeptonXThemeBundles.Styles.Global",
                "BlazorLeptonThemeBundles.Styles.Global"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/Pages/_Host.cshtml",
                "Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components",
                "Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components",
                "BlazorLeptonXThemeBundles.Styles.Global",
                "BlazorLeptonThemeBundles.Styles.Global"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/Pages/_Host.cshtml",
                "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
                "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
                "BlazorLeptonXThemeBundles.Scripts.Global",
                "BlazorLeptonThemeBundles.Scripts.Global"
            );

            RemoveLinesByStatement(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameBlazorModule.cs",
                "Volo.Abp.LeptonX.Shared;"
            );
        }
    }

    private void ChangeThemeToLeptonForNoLayersBlazorServerProjects(ProjectBuildContext context)
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

            ConfigureLeptonManagementPackagesForNoLayersMvc(context,
                $@"/MyCompanyName.MyProjectName.{projectName}/MyCompanyName.MyProjectName.{projectName}.csproj",
                blazorServerProjects);

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameModule.cs",
                "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
                "Volo.Abp.AspNetCore.Components.Server.LeptonTheme",
                "AbpAspNetCoreComponentsServerLeptonXThemeModule",
                "AbpAspNetCoreComponentsServerLeptonThemeModule"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameModule.cs",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
                "AbpAspNetCoreMvcUiLeptonXThemeModule",
                "AbpAspNetCoreMvcUiLeptonThemeModule"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameModule.cs",
                "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
                "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
                "LeptonXThemeBundles.Styles.Global",
                "LeptonThemeBundles.Styles.Global"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/MyProjectNameModule.cs",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
                "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
                "BlazorLeptonXThemeBundles.Styles.Global",
                "BlazorLeptonThemeBundles.Styles.Global"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/Pages/_Host.cshtml",
                "Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components",
                "Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components",
                "BlazorLeptonXThemeBundles.Styles.Global",
                "BlazorLeptonThemeBundles.Styles.Global"
            );

            ChangeNamespaceAndKeyword(
                context,
                $"/MyCompanyName.MyProjectName.{projectName}/Pages/_Host.cshtml",
                "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
                "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
                "BlazorLeptonXThemeBundles.Scripts.Global",
                "BlazorLeptonThemeBundles.Scripts.Global"
            );
        }
    }
}