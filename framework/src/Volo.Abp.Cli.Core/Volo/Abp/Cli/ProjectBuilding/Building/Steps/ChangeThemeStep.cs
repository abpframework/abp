using System.Collections.Generic;
using System.Linq;

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
        }
    }

    protected void ChangeToBasicTheme(ProjectBuildContext context)
    {
        #region MyCompanyName.MyProjectName.Web

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyCompanyName.MyProjectName.Web.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );

        #endregion
        
        #region MyCompanyName.MyProjectName.HttpApi.Host

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyCompanyName.MyProjectName.HttpApi.Host.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyProjectNameHttpApiHostModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyProjectNameHttpApiHostModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );

        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.csproj"
            );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme",
            "AbpAspNetCoreComponentsWebAssemblyLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsWebAssemblyBasicThemeModule"
        );

        ChangeNamespace(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite",
            "Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic"
        );
        
        #endregion

        #region MyCompanyName.MyProjectName.Blazor.Server
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
            );

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.Server.BasicTheme\Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme",
            "AbpAspNetCoreComponentsServerLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsServerBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
            );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite",
            "Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "BlazorLeptonXLiteThemeBundles.Scripts.Global",
            "BlazorBasicThemeBundles.Scripts.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor.Server.Tiered

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyCompanyName.MyProjectName.Blazor.Server.Tiered.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyCompanyName.MyProjectName.Blazor.Server.Tiered.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.Server.BasicTheme\Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme",
            "AbpAspNetCoreComponentsServerLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsServerBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
                
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite",
            "Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "BlazorLeptonXLiteThemeBundles.Scripts.Global",
            "BlazorBasicThemeBundles.Scripts.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.AuthServer

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/MyCompanyName.MyProjectName.AuthServer.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/MyProjectNameAuthServerModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/MyProjectNameAuthServerModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Web.Mvc
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyCompanyName.MyProjectName.Web.Mvc.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
            );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Web.Mvc.Mongo

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc.Mongo/MyCompanyName.MyProjectName.Web.Mvc.Mongo.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc.Mongo/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc.Mongo/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor.Server - (app-nolayers)
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme",
            "AbpAspNetCoreComponentsServerLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsServerBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor.Server.Mongo

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyCompanyName.MyProjectName.Blazor.Server.Mongo.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyCompanyName.MyProjectName.Blazor.Server.Mongo.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.Server.BasicTheme\Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme",
            "AbpAspNetCoreComponentsServerLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsServerBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        #endregion

        #region Angular

        ReplaceImportPackage(
            context, 
            "/angular/src/app/app.module.ts",
            "@abp/ng.theme.lepton-x", 
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

        #endregion
    }
    
    protected void ReplacePackageReferenceWithProjectReference(        
        ProjectBuildContext context, 
        string targetProjectFilePath,
        string packageReference,
        string projectReference)
    {
        var file = context.FindFile(targetProjectFilePath);
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
        var file = context.FindFile(targetModuleFilePath);

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
                lines[i] = $"using {newNamespace};";
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
        var file = context.FindFile(targetModuleFilePath);

        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains($"using {oldNamespace};"))
            {
                lines[i] = $"using {newNamespace};";
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
        var file = context.FindFile(filePath);

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
        var file = context.FindFile(filePath);

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
        var file = context.FindFile(filePath);

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
}